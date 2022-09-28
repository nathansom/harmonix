using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.Interfaces
{
    public interface IJobRepository
    {
        void Update(Job job);
        void Add(Job job);
		Task<bool> SaveAllAsync();
		Task<IEnumerable<Job>> GetJobsAsync();
		Task<Job> GetJobByIdAsync(int id);
        Task<IEnumerable<Job>> GetJobByTitleAsync(string title);
		//Task<bool> DeleteJobAsync(int id);
        Task<IEnumerable<Job>> GetJobsByPosterIdAsync(int id);
        Task<IEnumerable<JobDto>> GetMemberJobsAsync();
        Task<JobDto> GetMemberJobAsync();
        Task<AppUser> GetUserByUsernameAsync(string username);
    }
}