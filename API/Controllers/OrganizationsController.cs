using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace API.Controllers
{

    [Authorize]
    public class OrganizationsController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IMapper _mapper;
        public readonly IPhotoService _photoService;
        private readonly UserManager<AppUser> _userManager;

        public OrganizationsController(UserManager<AppUser> userManager, IUserRepository userRepository, IOrganizationRepository organizationRepository,
                                        IMapper mapper, IPhotoService photoService)

        {
            _userManager = userManager;
            _photoService = photoService;
            _userRepository = userRepository;
            _organizationRepository = organizationRepository;
            _mapper = mapper;

        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrganizationDto>>> GetOrganizations([FromQuery] OrganizationParams organizationParams)
        {
            var organizations = await _organizationRepository.GetCompactOrganizationsAsync(organizationParams);

            Response.AddPaginationHeader(organizations.CurrentPage, organizations.PageSize,
        organizations.TotalCount, organizations.TotalPages);

            return Ok(organizations);

        }

        [AllowAnonymous]
        [HttpGet("names")]
        public async Task<ActionResult<IEnumerable<string>>> GetOrganizationNames()
        {
            var orgNames = await _organizationRepository.GetAllOrganizationNames();
            return Ok(orgNames);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrganizationDto>> GetOrganizationById(int id)
        {
            return await _organizationRepository.GetCompactOrganizationByIdAsync(id);
        }

        [HttpGet("{id}/users")]
        public async Task<ActionResult<IEnumerable<OrgMemberDto>>> GetMembersByOrganizationId([FromQuery] UserParams userParams, int id)
        {
            var members = await _organizationRepository.GetMembersByOrganizationIdAsync(userParams, id);

            Response.AddPaginationHeader(members.CurrentPage, members.PageSize,
        members.TotalCount, members.TotalPages);

            return Ok(members);
        }

        [HttpGet("{id}/jobs")]
        public async Task<ActionResult<IEnumerable<JobDto>>> GetJobsByOrganizationId([FromQuery] JobParams jobParams, int id)
        {
            var jobs = await _organizationRepository.GetJobsByOrganizationIdAsync(jobParams, id);

            Response.AddPaginationHeader(jobs.CurrentPage, jobs.PageSize,
        jobs.TotalCount, jobs.TotalPages);

            return Ok(jobs);
        }

        [HttpGet("owned")]
        public async Task<ActionResult<IEnumerable<OrganizationDto>>> GetOwnedOrganizations([FromQuery] OrganizationParams organizationParams)
        {
            var id = User.GetUserId();
            var organizations = await _organizationRepository.GetOwnedOrganizationsAsync(organizationParams, id);

            Response.AddPaginationHeader(organizations.CurrentPage, organizations.PageSize,
        organizations.TotalCount, organizations.TotalPages);

            return Ok(organizations);

        }

        [HttpGet("affiliated")]
        public async Task<ActionResult<IEnumerable<OrganizationDto>>> GetAffiliatedOrganizations([FromQuery] OrganizationParams organizationParams)
        {
            var id = User.GetUserId();
            var organizations = await _organizationRepository.GetAffiliatedOrganizationsAsync(organizationParams, id);

            Response.AddPaginationHeader(organizations.CurrentPage, organizations.PageSize,
        organizations.TotalCount, organizations.TotalPages);

            return Ok(organizations);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateOrganization(OrganizationUpdateDto organizationUpdateDto, int id)
        {
            var user = _userRepository.GetUserByUsernameAsync(User.GetUsername());
            var organization = await _organizationRepository.GetOrganizationByIdAsync(id);
            if (organization.OwnerId != user.Id) return BadRequest("You cannot update this organization!");
            _mapper.Map<Organization>(organization);
            _organizationRepository.Update(organization);

            if (await _organizationRepository.SaveAllAsync())
                return NoContent();
            return BadRequest("Failed to update user");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<OrgPhotoDto>> AddPhoto(IFormFile file, int id)
        {
            var user = _userRepository.GetUserByUsernameAsync(User.GetUsername());
            var organization = await _organizationRepository.GetOrganizationByIdAsync(id);
            if (organization.OwnerId != user.Id) return BadRequest("You cannot add photo to this organization!");
            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);
            var photo = new OrgPhoto
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (organization.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            organization.Photos.Add(photo);
            if (await _organizationRepository.SaveAllAsync())
            {
                return _mapper.Map<OrgPhotoDto>(photo);
            }
            return BadRequest("Problem addding photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId, int id)
        {
            var user = _userRepository.GetUserByUsernameAsync(User.GetUsername());
            var organization = await _organizationRepository.GetOrganizationByIdAsync(id);
            if (organization.OwnerId != user.Id) return BadRequest("You are not permitted to set main photo.");

            var photo = organization.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain = organization.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _organizationRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to set main photo");
        }

        [Authorize(Policy = "RequireForteMembershipRole")]
        [HttpPost("add")]
        public async Task<ActionResult<OrganizationRegisterDto>> AddNewOrganization(OrganizationRegisterDto organizationRegisterDto)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            user = _mapper.Map<AppUser>(user);
            var organization = _mapper.Map<Organization>(organizationRegisterDto);
            var ownedOrgs = await _organizationRepository.GetOwnedOrganizationsRawAsync(user.Id);

            if (ownedOrgs.Select(o => o.Name).ToList().Contains(organizationRegisterDto.Name))
                return BadRequest("You already own an organization with this name");

            var userRoles = await _userManager.GetRolesAsync(user);

            organization.OwnerId = user.Id;
            _organizationRepository.Add(organization);

            if (await _organizationRepository.SaveAllAsync())
            {
                var updatedOwnedOrgs = await _organizationRepository.GetOwnedOrganizationsRawAsync(user.Id);
                var thisOrg = updatedOwnedOrgs.LastOrDefault(x => x.Name == organizationRegisterDto.Name);
                thisOrg.Members.Add(user);

                if (!userRoles.Contains("OrgAdmin"))
                    await _userManager.AddToRoleAsync(user, "OrgAdmin");

                if (await _organizationRepository.SaveAllAsync())
                {
                    return NoContent();
                } else
                {
                    return BadRequest("Organization is created but failed to associate it with your account.");
                }
            }

            return BadRequest("Failed to add organization");

        }

        [Authorize(Policy = "RequireOrgAdminRole")]
        [HttpPost("add-member/{id}")]
        public async Task<ActionResult<Organization>> AddMember(string username, int id)
        {
            var user = await _userRepository.GetUserByUsernameAsync(username);
            user = _mapper.Map<AppUser>(user);
            var owner = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            var org = await _organizationRepository.GetOrganizationByIdAsync(id);

            if (org.OwnerId != owner.Id)
                return BadRequest("Failed to add member. You are not the owner. Nice try ;)");

            if (org.Members.Contains(user)) return BadRequest("This user is already a member of this organization.");

            org.Members.Add(user);
            if (await _organizationRepository.SaveAllAsync())
                return NoContent();

            return BadRequest("Failed to add member.");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId, int id)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());
            var organization = await _organizationRepository.GetOrganizationByIdAsync(id);
            var photo = organization.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);

                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            if (organization.OwnerId != user.Id)
            {
                return BadRequest("Failed delete photo. You are not the owner. Nice try ;)");
            }

            organization.Photos.Remove(photo);

            if (await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to delete the photo");
        }
    }
}