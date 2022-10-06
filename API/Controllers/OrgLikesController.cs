using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers

{
    public class OrgLikesController : BaseApiController
    {
        private readonly IOrgLikesRepository _orgLikesRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUserRepository _userRepository;

        public OrgLikesController(IOrganizationRepository organizationRepository, IOrgLikesRepository orgLikesRepository,
                                    IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _organizationRepository = organizationRepository;
            _orgLikesRepository = orgLikesRepository;
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> AddLike( int id )
        {
            var userId = User.GetUserId();
            var user = await _userRepository.GetUserByIdAsync(userId);
            var organization= await _orgLikesRepository.GetOrganizationWithLikes(id);

            if (user == null) return NotFound();
            var orgLike = await _orgLikesRepository.GetOrganizationLike(id, user.Id);

            if (orgLike != null) return BadRequest("You already like this organization");

            orgLike = new OrgLike
            {
                OrgId = id,
                LikedUserId = user.Id
            };

            organization.LikedByUser!.Add(orgLike);

            if (await _organizationRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to like this organization");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrgLikeDto>>> GetOrganizationLikesByOrgId(int id)
            {
                var org = await _organizationRepository.GetOrganizationByIdAsync(id);

                if (org == null) return NotFound("Organization not found");

                return Ok(await _orgLikesRepository.GetOrganizationLikesByOrgId(id));     
        }

        [HttpGet("liked")]
        public async Task<ActionResult<IEnumerable<OrgLikeDto>>> GetLikedOrganizations([FromQuery] OrgLikeParams orgLikeParams)
        {
            orgLikeParams.UserId = User.GetUserId();
            var likedOrgs = await _orgLikesRepository.GetLikedOrganizations(orgLikeParams);

            Response.AddPaginationHeader(likedOrgs.CurrentPage, likedOrgs.PageSize,
        likedOrgs.TotalCount, likedOrgs.TotalPages);

            return Ok(likedOrgs);
        }

        [HttpGet("likedby")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetLikedByUsers([FromQuery] OrgLikeParams orgLikeParams, int id)
        {
            var org = await _organizationRepository.GetOrganizationByIdAsync(id);
            if (org == null) return NotFound("Organization not found");

            var likedByUsers = await _orgLikesRepository.GetLikedByUsers(orgLikeParams, id);

            Response.AddPaginationHeader(likedByUsers.CurrentPage, likedByUsers.PageSize,
        likedByUsers.TotalCount, likedByUsers.TotalPages);

            return Ok(likedByUsers);
        }

        [HttpDelete("unlike/{orgId}")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> UnlikeOrganization(int orgId)
        {
            var userId = User.GetUserId();
            if (User.GetUsername() == null) return BadRequest("You must login");
            
            var org = await _organizationRepository.GetOrganizationByIdAsync(orgId);
            if (org == null) return NotFound("Organization not found");

            var isDeleted = _orgLikesRepository.UnlikeOrganization(orgId, userId);

            if (isDeleted)
            {
                if (await _orgLikesRepository.SaveAllAsync())
                {
                    return Ok("You have unliked this organization.");
                }
                return BadRequest("Failed to save changes.");
            }
            return BadRequest("Failed to unlike this organization.");
        }
    }
}