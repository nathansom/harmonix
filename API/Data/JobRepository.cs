using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
  public class JobRepository : IJobRepository
  {
    private readonly DataContext _context;

    public JobRepository(DataContext context){
      _context = context;
        
    }
    // public async Task<bool> DeleteJob(int id)
    // {
    //   throw new NotImplementedException();
    // }
 
    public async Task<Job> GetJobByIdAsync(int id)
    {
      return await _context.Jobs
      .Include(j => j.JobPoster)
      .SingleOrDefaultAsync(x => x.Id == id);
    }

     public async Task<IEnumerable<Job>> GetJobByTitleAsync(string title){
      var jobs = await _context.Jobs
        .Where(t=>t.Title.ToLower().Contains(title.ToLower()))
        .Include(j => j.JobPoster)
        .ToListAsync();
      return jobs;
    }

    public async Task<IEnumerable<Job>> GetJobsAsync()
    {
      return await _context.Jobs
      .Include(j => j.JobPoster)
      .ToListAsync();
    }

    public Task<JobDto> GetMemberJobAsync()
    {
      //return await _context.Jobs.Where()
      throw new NotImplementedException();
    }

    public Task<IEnumerable<JobDto>> GetMemberJobsAsync()
    {
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
            _context.Jobs.Add(job);
        }

    public async Task<AppUser> GetUserByUsernameAsync(string username)
    {
      return await _context.Users
      .Include(p=>p.Photos).SingleOrDefaultAsync(x=>x.UserName == username);
    }
    
    public async Task<IEnumerable<Job>> GetJobsByPosterIdAsync(int id){      
        var user = await _context.Users.Where(t=>t.Id==id)
        .Select(t=>t.UserName).SingleOrDefaultAsync();

      return  await _context.Jobs.Where(t=>t.JobPoster.UserName==user)
      .Include(j => j.JobPoster)
      .ToListAsync();
    }
  }
}
