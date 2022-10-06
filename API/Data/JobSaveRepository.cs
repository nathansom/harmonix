using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class JobSaveRepository : IJobSaveRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public JobSaveRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }
        public async Task<JobSave> GetSavedJob(int savedUserId, int jobId)
        {
            var result = await _context.SavedJobs!.FindAsync(jobId, savedUserId);
            return result!;
        }

        //list of jobs that the user has saved
        public async Task<PagedList<JobSaveDto>> GetSavedJobs(JobParams jobParams, int userId)
        {
            // var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var jobs = _context.Jobs!.AsQueryable();
            var savedjobs = _context.SavedJobs!.AsQueryable();

            //list of jobs currently logged in user has saved
            savedjobs = savedjobs.Where(user => user.SavedUserId == userId);
            jobs = savedjobs!.Select(save => save.SavedJob!);

            //   if (predicate == "savedBy")
            //   {

            //     savedjobs = savedjobs.Where(job => job.JobId == userId);
            //     users = savedjobs.Select(save => save.SavedUser);
            //  //   jobs = savedjobs.Where(job => job.JobId == userId)
            //   }

            var query = jobs.Select(job => new JobSaveDto
            {
                Id = job.Id,
                Title = job.Title,
                OrgId = job.Organization!.Id,
                JobPosterId = job.JobPoster!.Id,
                JobPosterName = job.JobPoster.UserName,
                LogoUrl = job.Organization.Photos != null || job.Organization != null
                ? job.Organization!.Photos!.FirstOrDefault(x => x.IsMain)!.Url
                : job.JobPoster!.Photos!.FirstOrDefault(x => x.IsMain)!.Url,
                Description = job.Description,
                Salary = job.Salary,
                City = job.City,
                ProvinceOrState = job.ProvinceOrState,
                Country = job.Country,
                Genres = job.Genres,
                JobType = job.JobType,
                SkillsRequired = job.SkillsRequired,
                ApplicationUrl = job.ApplicationUrl,
                DateCreated = job.DateCreated,
                Deadline = job.Deadline,
                LastUpdated = job.LastUpdated
            }).AsQueryable();

            if (jobParams.Title != null)
                query = query.Where(j => j.Title!.ToLower().Trim().Contains(jobParams.Title.ToLower().Trim())).AsQueryable();
            if (jobParams.JobType != null)
                query = query.Where(j => j.JobType!.ToLower().Trim().Contains(jobParams.JobType.ToLower().Trim())).AsQueryable();
            if (jobParams.PosterID != null && jobParams.PosterID > 0)
                query = query.Where(j => j.JobPosterId == jobParams.PosterID).AsQueryable();
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

            return await PagedList<JobSaveDto>.CreateAsync(query,
             jobParams.PageNumber, jobParams.PageSize);

        }

        //list of jobs that a user has saved
        public async Task<AppUser> GetUserWithSavedJobs(int userId)
        {
            var jobs = await _context.Users.Where(i => i.Id == userId)
            .Include(x => x.SavedJobs).FirstOrDefaultAsync();

            return jobs!;
        }
    }
}