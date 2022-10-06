using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Data
{
    public class OrgLikesRepository : IOrgLikesRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public OrgLikesRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        
        public async Task<OrgLike> GetOrganizationLike(int OrgId, int likedUserId)
        {
            var result = await _context.OrgLikes!.FindAsync(OrgId, likedUserId);
            return result!;
        }

        public async Task<IEnumerable<OrgLikeDto>> GetOrganizationLikesByOrgId(int orgId)
        {
            return await _context.OrgLikes!
                            .Where(o => o.OrgId == orgId)
                            .Include(o => o.Org)
                            .Include(o => o.LikedUser)
                            .ProjectTo<OrgLikeDto>(_mapper.ConfigurationProvider)
                            .ToListAsync();
        }

        public async Task<Organization> GetOrganizationWithLikes(int orgId)
        {
            var result = await _context.Organizations!
                .Include(x => x.LikedByUser!)
                .FirstOrDefaultAsync(x => x.Id == orgId);

            return result!;
        }

        public async Task<PagedList<OrganizationDto>> GetLikedOrganizations(OrgLikeParams orgLikeParams)
        {
            var orgs = _context.Organizations!.OrderBy(o => o.Name).AsQueryable();
            var orgLikes = _context.OrgLikes!.OrderBy(o => o.Org!.Name).AsQueryable();

            
            orgLikes = orgLikes.Where(like => like.LikedUserId == orgLikeParams.UserId);
            orgs = orgLikes.Select(like => like.Org)!;

            var result = await GetPaginatedResult<OrganizationDto>(orgs.ProjectTo<OrganizationDto>(_mapper.ConfigurationProvider), orgLikeParams);

            // var likedUsers = users.Select(user => new LikeDto
            // {
            //     Username = user.UserName,
            //     KnownAs = user.KnownAs,
            //     PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
            //     City = user.City,
            //     Id = user.Id
            // });

            // return await _context.OrgLikes
            //                 .Include(o => o.Org)
            //                 .Include(o => o.LikedUser)
            //                 .ProjectTo<OrgLikeDto>(_mapper.ConfigurationProvider)
            //                 .ToListAsync();
            return result;
        }

        public async Task<PagedList<MemberDto>> GetLikedByUsers(OrgLikeParams orgLikeParams, int orgId)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var orgLikes = _context.OrgLikes!.OrderBy(o => o.Org!.Name).AsQueryable();

            orgLikes = orgLikes.Where(like => like.OrgId == orgId);
            users = orgLikes.Select(like => like.LikedUser)!;

            var result = await GetPaginatedResult<MemberDto>(users.ProjectTo<MemberDto>(_mapper.ConfigurationProvider), orgLikeParams);

            return result;
        }

        private async Task<PagedList<T>> GetPaginatedResult<T>(IQueryable<T> query, OrgLikeParams orgLikeParams)
        {
            return await PagedList<T>.CreateAsync(
                    query,
                    orgLikeParams.PageNumber,
                    orgLikeParams.PageSize);
        }

        public bool UnlikeOrganization(int orgId, int userId)
        {
            var orgLikes = _context.OrgLikes!.Where(o => o.OrgId == orgId).AsQueryable();
            var orgLike = orgLikes.Where(o => o.LikedUserId == userId).SingleOrDefault();

            var likeExisted = orgLike != null;

            if (likeExisted)
            {
                _context.OrgLikes!.Remove(orgLike!);
            }

            return likeExisted;
        }
    }
}