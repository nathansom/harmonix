using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class JobRepository : IJobRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public JobRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Job> GetJobByIdAsync(int id)
        {
            var result = await _context.Jobs!
            .Include(o => o.Organization!)
            .Include(o => o.Organization!.Photos!)
            .Include(j => j.JobPoster!)
            .Include(j => j.JobPoster!.Photos!)
            .SingleOrDefaultAsync(x => x.Id == id);

            return result!;
        }

        public async Task<PagedList<JobDto>> GetJobsByTitleAsync(JobParams jobParams, string title)
        {
            var query = _context.Jobs!.Where(t => t.Title!.ToLower().Contains(title.ToLower())).AsQueryable();

            if (jobParams.Title != null)
                query = query.Where(j => j.Title!.ToLower().Trim().Contains(jobParams.Title.ToLower().Trim())).AsQueryable();
            if (jobParams.JobType != null)
                query = query.Where(j => j.JobType!.ToLower().Trim().Contains(jobParams.JobType.ToLower().Trim())).AsQueryable();
            if (jobParams.PosterID != null && jobParams.PosterID > 0)
                query = query.Where(j => j.JobPoster!.Id == jobParams.PosterID).AsQueryable();
            if (jobParams.Genres != null)
                query = query.Where(j => j.Genres!.ToLower().Trim().Contains(jobParams.Genres.ToLower().Trim())).AsQueryable().AsQueryable();
            if (jobParams.SkillsRequired != null)
                query = query.Where(j => j.SkillsRequired!.ToLower().Trim().Contains(jobParams.SkillsRequired.ToLower().Trim())).AsQueryable().AsQueryable();
            if (jobParams.City != null)
                query = query.Where(j => j.City!.ToLower().Trim().Contains(jobParams.City.ToLower().Trim())).AsQueryable().AsQueryable();
            if (jobParams.ProvinceOrState != null)
                query = query.Where(j => j.ProvinceOrState!.ToLower().Trim().Contains(jobParams.ProvinceOrState.ToLower().Trim())).AsQueryable().AsQueryable();
            if (jobParams.Country != null)
                query = query.Where(j => j.Country!.ToLower().Trim().Contains(jobParams.Country.ToLower().Trim())).AsQueryable().AsQueryable();

            query = jobParams.OrderBy switch
            {
                "alphabetical" => query.OrderBy(o => o.Title),
                "deadline" => query.OrderByDescending(o => o.Deadline),
                "lastUpdated" => query.OrderByDescending(o => o.LastUpdated),
                _ => query.OrderByDescending(o => o.DateCreated)
            };

            return await PagedList<JobDto>.CreateAsync(
                query.ProjectTo<JobDto>(_mapper.ConfigurationProvider).AsNoTracking(),
                jobParams.PageNumber,
                jobParams.PageSize
            );
        }

        public async Task<PagedList<JobDto>> GetJobsAsync(JobParams jobParams)
        {
            var query = _context.Jobs!.AsQueryable();

            if (jobParams.Title != null)
                query = query.Where(u => u.Title!.ToLower().Trim().Contains(jobParams.Title.ToLower().Trim()));
            if (jobParams.JobType != null)
                query = query.Where(u => u.JobType!.ToLower().Trim().Contains(jobParams.JobType.ToLower().Trim()));
            if (jobParams.SelfPost)
                query = query.Where(u => u.JobPoster!.Id == jobParams.PosterID);
            if (jobParams.Genres != null)
                query = query.Where(j => j.Genres!.ToLower().Trim().Contains(jobParams.Genres.ToLower().Trim())).AsQueryable().AsQueryable();
            if (jobParams.SkillsRequired != null)
                query = query.Where(j => j.SkillsRequired!.ToLower().Trim().Contains(jobParams.SkillsRequired.ToLower().Trim())).AsQueryable().AsQueryable();
            if (jobParams.City != null)
                query = query.Where(j => j.City!.ToLower().Trim().Contains(jobParams.City.ToLower().Trim())).AsQueryable().AsQueryable();
            if (jobParams.ProvinceOrState != null)
                query = query.Where(j => j.ProvinceOrState!.ToLower().Trim().Contains(jobParams.ProvinceOrState.ToLower().Trim())).AsQueryable().AsQueryable();
            if (jobParams.Country != null)
                query = query.Where(j => j.Country!.ToLower().Trim().Contains(jobParams.Country.ToLower().Trim())).AsQueryable().AsQueryable();


            query = jobParams.OrderBy switch
            {
                "alphabetical" => query.OrderBy(o => o.Title),
                "deadline" => query.OrderByDescending(o => o.Deadline),
                "lastUpdated" => query.OrderByDescending(o => o.LastUpdated),
                _ => query.OrderByDescending(o => o.LastUpdated)
            };

            query = query
                        .Include(j => j.Organization!.Photos)
                        .Include(j => j.JobPoster!.Photos);

            return await PagedList<JobDto>.CreateAsync(
                query.ProjectTo<JobDto>(_mapper.ConfigurationProvider).AsNoTracking(),
                jobParams.PageNumber,
                jobParams.PageSize
            );
        }

        public Task<JobDto> GetMemberJobAsync()
        {
            //return await _context.Jobs.Where()
            throw new NotImplementedException();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(Job job)
        {
            _context.Entry(job).State = EntityState.Modified;
        }

        public void Add(Job job)
        {
            _context.Jobs!.Add(job);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            var result = await _context.Users
            .Include(p => p.Photos!).SingleOrDefaultAsync(x => x.UserName == username);

            return result!;
        }

        public async Task<PagedList<JobDto>> GetJobsByPosterIdAsync(JobParams jobParams, int id)
        {
            var jobs = _context.Jobs!.Where(j => j.JobPoster!.Id == id).AsQueryable();
            var query = jobs;

            if (jobParams.Title != null)
                query = query.Where(u => u.Title!.ToLower().Trim().Contains(jobParams.Title.ToLower().Trim()));
            if (jobParams.JobType != null)
                query = query.Where(u => u.JobType!.ToLower().Trim().Contains(jobParams.JobType.ToLower().Trim()));
            if (jobParams.Genres != null)
                query = query.Where(j => j.Genres!.ToLower().Trim().Contains(jobParams.Genres.ToLower().Trim())).AsQueryable().AsQueryable();
            if (jobParams.SkillsRequired != null)
                query = query.Where(j => j.SkillsRequired!.ToLower().Trim().Contains(jobParams.SkillsRequired.ToLower().Trim())).AsQueryable().AsQueryable();
            if (jobParams.City != null)
                query = query.Where(j => j.City!.ToLower().Trim().Contains(jobParams.City.ToLower().Trim())).AsQueryable().AsQueryable();
            if (jobParams.ProvinceOrState != null)
                query = query.Where(j => j.ProvinceOrState!.ToLower().Trim().Contains(jobParams.ProvinceOrState.ToLower().Trim())).AsQueryable().AsQueryable();
            if (jobParams.Country != null)
                query = query.Where(j => j.Country!.ToLower().Trim().Contains(jobParams.Country.ToLower().Trim())).AsQueryable().AsQueryable();

            query = jobParams.OrderBy switch
            {
                "alphabetical" => query.OrderBy(o => o.Title),
                "deadline" => query.OrderByDescending(o => o.Deadline),
                "lastUpdated" => query.OrderByDescending(o => o.LastUpdated),
                _ => query.OrderByDescending(o => o.LastUpdated)
            };

            return await PagedList<JobDto>.CreateAsync(
                query.ProjectTo<JobDto>(_mapper.ConfigurationProvider).AsNoTracking(),
                jobParams.PageNumber,
                jobParams.PageSize
            );
        }

        public Task<PagedList<JobDto>> GetMemberJobsAsync(JobParams jobParams)
        {
            throw new NotImplementedException();
        }

        public bool DeleteJobById(int id)
        {
            var job = _context.Jobs!.Where(j => j.Id == id)
                          .SingleOrDefault();

            var jobExist = job != null;

            if (jobExist)
            {
                _context.Jobs!.Remove(job!);
            }

            return jobExist;

        }
    }
}