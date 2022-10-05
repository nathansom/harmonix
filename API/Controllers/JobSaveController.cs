using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class JobSaveController : BaseApiController
    {
    private readonly IUserRepository _userRepository;
    private readonly IJobRepository _jobRepository;
    private readonly IJobSaveRepository _jobSaveRepository;
        public JobSaveController(IUserRepository userRepository,IJobRepository jobRepository, IJobSaveRepository jobSaveRepository)
        {
            _jobSaveRepository = jobSaveRepository;
            _jobRepository = jobRepository;
            _userRepository = userRepository;

        }

        //user to save job by id - api/jobSave/id
        [HttpPost("{id}")]
        public async Task<ActionResult> SaveJob(int id){
            var sourceUserId = User.GetUserId();
            var savedJob = await _jobRepository.GetJobByIdAsync(id);
            if(savedJob==null) return NotFound();
            
            var sourceUser = await _jobSaveRepository.GetUserWithSavedJobs(sourceUserId);

            // implement check - job poster cannot save their own posted job
            if(sourceUserId==savedJob.JobPoster.Id) return BadRequest("You cannot save the Job you posted.");

            var jobSaved = await _jobSaveRepository.GetSavedJob(sourceUserId, savedJob.Id);

            if(jobSaved != null) return BadRequest("This job has been saved by this user.");

            jobSaved = new JobSave
            {
                SavedJob = savedJob,
                JobId = savedJob.Id,
                SavedUser = sourceUser,
                SavedUserId = sourceUserId
                
            };

            sourceUser.SavedJobs.Add(jobSaved);
            if(await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to save Job.");

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobSaveDto>>> GetSavedJobs([FromQuery] JobParams jobParams)
        {
            var jobs = await _jobSaveRepository.GetSavedJobs(jobParams, User.GetUserId());
            
            Response.AddPaginationHeader(jobs.CurrentPage,
          jobs.PageSize, jobs.TotalCount, jobs.TotalPages);

            return Ok(jobs);
        }

        [HttpDelete("delete-savedJob/{id}")]
        public async Task<ActionResult> RemoveSavedJob(int id)
        {
            var sourceUserId = User.GetUserId();
            var savedJob = await _jobRepository.GetJobByIdAsync(id);

            var sourceUser = await _jobSaveRepository.GetUserWithSavedJobs(sourceUserId);

            if (savedJob == null) return NotFound();

            var jobSave = await _jobSaveRepository.GetSavedJob(sourceUserId, savedJob.Id);

            if (jobSave == null) return BadRequest("You already removed this job");

            
            sourceUser.SavedJobs.Remove(jobSave);

            if (await _jobRepository.SaveAllAsync())
            {
                return Ok();
            }

            return BadRequest("Failed to remove Job");

        }

        
    }
}