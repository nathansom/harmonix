using API.Entities;
using API.DTOs;
using API.Helpers;


namespace API.Interfaces
{
    public interface IOrgLikesRepository
    {
        Task<OrgLike> GetOrganizationLike(int OrgId, int likedUserId);
        Task<Organization> GetOrganizationWithLikes(int orgId);
        Task<IEnumerable<OrgLikeDto>> GetOrganizationLikesByOrgId(int orgId);
        Task<PagedList<OrganizationDto>> GetLikedOrganizations(OrgLikeParams orgLikeParams);
        Task<PagedList<MemberDto>> GetLikedByUsers(OrgLikeParams orgLikeParams, int orgId);
        Task<bool> SaveAllAsync();
        bool UnlikeOrganization(int orgId, int userId);
    }
}