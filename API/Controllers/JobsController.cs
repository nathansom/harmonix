using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Authorize]
    public class JobsController : BaseApiController
    { 
        private readonly IUserRepository _userRepository;       
        private readonly IJobRepository _jobRepository;
        private readonly IMapper _mapper;

    public JobsController(IUserRepository userRepository, IJobRepository jobRepository, IMapper mapper)
    {
        _jobRepository = jobRepository;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<JobDto>>> GetJobs(){
        var jobs = await _jobRepository.GetJobsAsync();
        var jobstoreturn = _mapper.Map<IEnumerable<JobDto>>(jobs);
        return Ok(jobstoreturn);
       // return Ok(await _context.Jobs.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<JobDto>> GetJobById(int id){
        var job = await _jobRepository.GetJobByIdAsync(id);
        return _mapper.Map<JobDto>(job);
    }

     [HttpGet("title/{title}")]
    public async Task<ActionResult<IEnumerable<JobDto>>> GetJobByTitle(string title){
        var job = await _jobRepository.GetJobByTitleAsync(title);
        var jobreturn = _mapper.Map<IEnumerable<JobDto>>(job);
        return Ok(jobreturn);
    }

     [HttpGet("poster/{id}")]
    public async Task<ActionResult<IEnumerable<JobDto>>> GetJobsByPosterId(int id){
        var job = await _jobRepository.GetJobsByPosterIdAsync(id);
        var jobreturn = _mapper.Map<IEnumerable<JobDto>>(job);
        return Ok(jobreturn);
    }

    //Update existing Job
    [AllowAnonymous]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateJob(JobUpdateDto jobUpdateDto,int id)
        {
            //var sourceUserId = User.GetUserId();

            var job = await _jobRepository.GetJobByIdAsync(id);

            _mapper.Map(jobUpdateDto, job);

            _jobRepository.Update(job);
            if (await _jobRepository.SaveAllAsync()) 
                return NoContent();

            return BadRequest("Failed to update user");
        }

    // Add a new Job
    [HttpPost("add")]

    public async Task<ActionResult<JobDto>> AddNewJobByPosterId(JobDto jobDto){
        var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
        var job = _mapper.Map<Job>(jobDto);
        job.JobPoster = user;

        _jobRepository.Add(job);

        if (await _jobRepository.SaveAllAsync()) 
            return NoContent();

        return BadRequest("Failed to add user");
    }

    }
}