using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class JobsController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IJobRepository _jobRepository;
        private readonly IMapper _mapper;
        private readonly IOrganizationRepository _organizationRepository;
        public UserManager<AppUser> _userManager { get; }

        public JobsController(UserManager<AppUser> userManager, IUserRepository userRepository, IJobRepository jobRepository, IOrganizationRepository organizationRepository, IMapper mapper)
        {
            _userManager = userManager;
            _organizationRepository = organizationRepository;
            _jobRepository = jobRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JobDto>>> GetJobs([FromQuery] JobParams jobParams)
        {
            jobParams.PosterID = User.GetUserId();

            var jobs = await _jobRepository.GetJobsAsync(jobParams);

            Response.AddPaginationHeader(jobs.CurrentPage, jobs.PageSize,
                jobs.TotalCount, jobs.TotalPages);

            return Ok(jobs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JobDto>> GetJobById(int id)
        {
            var job = await _jobRepository.GetJobByIdAsync(id);
            return _mapper.Map<JobDto>(job);
        }

        [HttpGet("title/{title}")]
        public async Task<ActionResult<IEnumerable<JobDto>>> GetJobsByTitle([FromQuery] JobParams jobParams, string title)
        {
            var jobs = await _jobRepository.GetJobsByTitleAsync(jobParams, title);

            Response.AddPaginationHeader(jobs.CurrentPage, jobs.PageSize,
                jobs.TotalCount, jobs.TotalPages);

            return Ok(jobs);
        }

        [HttpGet("poster/{id}")]
        public async Task<ActionResult<IEnumerable<JobDto>>> GetJobsByPosterId([FromQuery] JobParams jobParams, int id)
        {
            var jobs = await _jobRepository.GetJobsByPosterIdAsync(jobParams, id);

            Response.AddPaginationHeader(jobs.CurrentPage, jobs.PageSize,
                jobs.TotalCount, jobs.TotalPages);

            return Ok(jobs);
        }

        //Update existing Job
        [Authorize(Policy = "RequireForteMembershipRole")]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateJob(JobUpdateDto jobUpdateDto, int id)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            var job = await _jobRepository.GetJobByIdAsync(id);

            if (job.JobPoster!.Id != user.Id) return BadRequest("You are not permitted to perform this action. Nice try ;)");

            if (job.JobPoster != user) return BadRequest("Job Not Found");

            _mapper.Map(jobUpdateDto, job);

            _jobRepository.Update(job);
            if (await _jobRepository.SaveAllAsync())
                return NoContent();

            return BadRequest("Failed to update user");
        }

        // Add a new Job
        [Authorize(Policy = "RequireForteMembershipRole")]
        [HttpPost("add")]
        public async Task<ActionResult<JobRegisterDto>> AddNewJobByPosterId(JobRegisterDto jobRegisterDto)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user == null) return BadRequest("User not found");

            var id = jobRegisterDto.ConfirmedOrgId;

            var org = await _organizationRepository.GetOrganizationByIdAsync(id);
            if (org == null) return NotFound("Organization not found");

            var job = _mapper.Map<Job>(jobRegisterDto);

            var userRoles = await _userManager.GetRolesAsync(user);
            var IsOwner = org.OwnerId == user.Id;
            var IsAdmin = userRoles.Contains("Admin");
            var IsModerator = userRoles.Contains("Moderator");
            var IsOrgMember = user.Affiliation != null && user.Affiliation.Contains(org);

            if (IsOwner || IsAdmin || IsModerator || IsOrgMember)
            {
                job.JobPoster = user;
                job.Organization = org;

                _jobRepository.Add(job);
                if (await _jobRepository.SaveAllAsync())
                    return NoContent();
            }
            return BadRequest("You are not permitted to post a job.");
        }

        //Delete a Job
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteJob(int id)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            if (user == null) return BadRequest("User not found");
            _mapper.Map<AppUser>(user);

            var job = await _jobRepository.GetJobByIdAsync(id);
            //var job = user.CreatedJobs.FirstOrDefault(x => x.Id == id);
            if (job == null) return NotFound();

            var orgs = await _organizationRepository.GetOrganizationsAsync();
            var affiliatedOrgs = orgs.Where(o => o.Members!.Contains(user));
            var thisOrg = await _organizationRepository.GetOrganizationByIdAsync(id);

            var userRoles = await _userManager.GetRolesAsync(user);
            var IsAdmin = userRoles.Contains("Admin");
            var IsModerator = userRoles.Contains("Moderator");
            var IsOrgMember = affiliatedOrgs.Contains(thisOrg);
            var IsOrgAdmin = userRoles.Contains("OrgAdmin") && IsOrgMember;
            var IsOrgModerator = userRoles.Contains("OrgModerator") && IsOrgMember;
            var IsJobPoster = job.JobPoster!.Id == user.Id;

            if (IsAdmin | IsModerator | IsOrgAdmin | IsOrgModerator | IsJobPoster)
            {
                var isDeleted = _jobRepository.DeleteJobById(id);

                if (isDeleted)
                {
                    if (await _jobRepository.SaveAllAsync())
                    {
                        return Ok("Job has successfully been deleted!");
                    }
                    return BadRequest("Failed to save changes to the database");
                }

                return BadRequest("Failed to delete the job.");
            }
            return Unauthorized("You are not permitted to perform this action. Nice try ;)");
        }

    }
}