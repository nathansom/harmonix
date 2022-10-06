using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public OrganizationRepository(DataContext context , IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<Organization>> GetOrganizationsAsync()
        {
            return await  _context.Organizations!
                .Include(p => p.Photos)
                .Include(m => m.Members)
                .Include(j => j.Jobs)
                .ToListAsync();
        }

        public async Task<PagedList<OrganizationDto>> GetCompactOrganizationsAsync(OrganizationParams organizationParams)
        {
            var query = _context.Organizations!.AsQueryable();

            if (organizationParams.Name != null)
                query = query.Where(o => o.Name!.ToLower().Trim().Contains(organizationParams.Name.ToLower().Trim())).AsQueryable();
            if (organizationParams.OrgType != null)
                query = query.Where(o => o.OrgType!.ToLower().Trim().Contains(organizationParams.OrgType.ToLower().Trim())).AsQueryable();
            if (organizationParams.Established != null && organizationParams.Established > 0)
                query = query.Where(o => o.Established == organizationParams.Established).AsQueryable();
            if (organizationParams.City != null)
                query = query.Where(o => o.City!.ToLower().Trim().Contains(organizationParams.City.ToLower().Trim())).AsQueryable();
            if (organizationParams.ProvinceOrState != null)
                query = query.Where(o => o.ProvinceOrState!.ToLower().Trim().Contains(organizationParams.ProvinceOrState.ToLower().Trim())).AsQueryable();
            if (organizationParams.Country != null)
                query = query.Where(o => o.Country!.ToLower().Trim().Contains(organizationParams.Country.ToLower().Trim())).AsQueryable();

            query = organizationParams.OrderBy switch
            {
                "alphabetical" => query.OrderBy(o => o.Name),
                "established" => query.OrderBy(o => o.Established),
                _ => query.OrderByDescending(o => o.LikedByUser!.Count)
            };

            return await PagedList<OrganizationDto>.CreateAsync(
                query.ProjectTo<OrganizationDto>(_mapper.ConfigurationProvider).AsNoTracking(),
                organizationParams.PageNumber,
                organizationParams.PageSize
            );
        }

        public async Task<Organization> GetOrganizationByIdAsync(int id)
        {
            var result = await _context.Organizations!
                    .Where(o => o.Id == id)
                    .Include(p => p.Photos!)
                    .Include(m => m.Members!)
                    .Include(j => j.Jobs!)
                    .SingleOrDefaultAsync();

            return result!;
        }

        public async Task<OrganizationDto> GetCompactOrganizationByIdAsync(int id)
        {
            var organization = await _context.Organizations!
                    .Where(o => o.Id == id)
                    .Include(p => p.Photos)
                    .Include(m => m.Members)
                    .Include(j => j.Jobs)
                    .ProjectTo<OrganizationDto>(_mapper.ConfigurationProvider)
                    .SingleOrDefaultAsync();

            return organization!;
        }

        public async Task<Organization> GetOrganizationByOrgnameAsync(string orgname)
        {
            var result = await _context.Organizations!
                    .Where(x => x.Name == orgname)
                    .Include(p => p.Photos!)
                    .Include(m => m.Members!)
                    .Include(j => j.Jobs!)
                    .SingleOrDefaultAsync();

            return result!;  
                
        // ! Do not delete - we can use this for a search function
        // var organization = await _context.Organizations.Where(o=>o.Name.ToLower().Contains(orgname.ToLower())).ToListAsync();
        // return  organization;
        }        

        public async Task<bool> SaveAllAsync()
        {
            var updates = await _context.SaveChangesAsync();
            var isUpdated = updates > 0;
            
            return isUpdated;
        }


        public void Add(Organization organization)
        {
            _context.Organizations!.Add(organization);
        }

        public void Update(Organization organization)
        {
            _context.Entry(organization).State = EntityState.Modified;
        }

        public async Task<PagedList<OrgMemberDto>> GetMembersByOrganizationIdAsync(UserParams userParams, int id)
        {
            var org = _context.Organizations!.SingleOrDefault(o => o.Id == id);
            var query = _context.Users.Where(u => u.Affiliation!.Contains(org!));
            query = query.Where(u => u.UserName != userParams.CurrentUsername);

            if (userParams.Occupation != null)
                query = query.Where(u => u.Occupation!.ToLower().Trim().Contains(userParams.Occupation.ToLower().Trim()));
            if (userParams.Skill != null)
                query = query.Where(u => u.Skills!.ToLower().Trim().Contains(userParams.Skill.ToLower().Trim()));
            if (userParams.Genre != null)
                query = query.Where(u => u.Genres.ToLower().Trim().Contains(userParams.Genre.ToLower().Trim()));
            if (userParams.City != null)
                query = query.Where(u => u.City!.ToLower().Trim().Contains(userParams.City.ToLower().Trim()));
            if (userParams.ProvinceOrState != null)
                query = query.Where(u => u.ProvinceOrState!.ToLower().Trim().Contains(userParams.ProvinceOrState.ToLower().Trim()));
            if (userParams.Country != null)
                query = query.Where(u => u.Country!.ToLower().Trim().Contains(userParams.Country.ToLower().Trim()));

            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(u => u.Created),
                _ => query.OrderByDescending(u => u.LastActive)
            };

            return await PagedList<OrgMemberDto>.CreateAsync(
                    query.ProjectTo<OrgMemberDto>(_mapper.ConfigurationProvider).AsNoTracking(),
                    userParams.PageNumber,
                    userParams.PageSize
                );
        }

        public async Task<PagedList<JobDto>> GetJobsByOrganizationIdAsync(JobParams jobParams, int id)
        {
            var org = _context.Organizations!.SingleOrDefault(o => o.Id == id);
            var query = _context.Jobs!.Where(j => j.Organization!.Equals(org));

            if (jobParams.Title != null)
                query = query.Where(j => j.Title!.ToLower().Trim().Contains(jobParams.Title.ToLower().Trim())).AsQueryable();
            if (jobParams.JobType != null)
                query = query.Where(j => j.JobType!.ToLower().Trim().Contains(jobParams.JobType.ToLower().Trim())).AsQueryable();
            if (jobParams.PosterID != null && jobParams.PosterID > 0)
                query = query.Where(j => j.JobPoster!.Id == jobParams.PosterID).AsQueryable();
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

            query = query
                        .Include(j => j.Organization)
                        .Include(j => j.Organization!.Photos)
                        .Include(j => j.JobPoster)
                        .Include(j => j.JobPoster!.Photos).AsQueryable();

            return await PagedList<JobDto>.CreateAsync(
                    query.ProjectTo<JobDto>(_mapper.ConfigurationProvider).AsNoTracking(),
                    jobParams.PageNumber,
                    jobParams.PageSize
                );
        }
        public async Task<PagedList<OrganizationDto>> GetOwnedOrganizationsAsync(OrganizationParams organizationParams, int id)
        {
            var organizations = _context.Organizations!.Where(o => o.OwnerId == id);
            var query = organizations;

            if (organizationParams.Name != null)
                query = query.Where(o => o.Name!.ToLower().Trim().Contains(organizationParams.Name.ToLower().Trim())).AsQueryable();
            if (organizationParams.OrgType != null)
                query = query.Where(o => o.OrgType!.ToLower().Trim().Contains(organizationParams.OrgType.ToLower().Trim())).AsQueryable();
            if (organizationParams.Established != null && organizationParams.Established > 0)
                query = query.Where(o => o.Established == organizationParams.Established).AsQueryable();
            if (organizationParams.City != null)
                query = query.Where(o => o.City!.ToLower().Trim().Contains(organizationParams.City.ToLower().Trim())).AsQueryable();
            if (organizationParams.ProvinceOrState != null)
                query = query.Where(o => o.ProvinceOrState!.ToLower().Trim().Contains(organizationParams.ProvinceOrState.ToLower().Trim())).AsQueryable();
            if (organizationParams.Country != null)
                query = query.Where(o => o.Country!.ToLower().Trim().Contains(organizationParams.Country.ToLower().Trim())).AsQueryable();

            query = organizationParams.OrderBy switch
            {
                "alphabetical" => query.OrderBy(o => o.Name),
                "established" => query.OrderBy(o => o.Established),
                _ => query.OrderByDescending(o => o.LikedByUser!.Count)
            };

            return await PagedList<OrganizationDto>.CreateAsync(
                query.ProjectTo<OrganizationDto>(_mapper.ConfigurationProvider).AsNoTracking(),
                organizationParams.PageNumber,
                organizationParams.PageSize
            );
        }

        public async Task<PagedList<OrganizationDto>> GetAffiliatedOrganizationsAsync(OrganizationParams organizationParams, int id)
        {
            var query = _context.Users.Where(u => u.Id == id).SelectMany(u => u.Affiliation!).AsQueryable();

            if (organizationParams.Name != null)
                query = query.Where(o => o.Name!.ToLower().Trim().Contains(organizationParams.Name.ToLower().Trim())).AsQueryable();
            if (organizationParams.OrgType != null)
                query = query.Where(o => o.OrgType!.ToLower().Trim().Contains(organizationParams.OrgType.ToLower().Trim())).AsQueryable();
            if (organizationParams.Established != null && organizationParams.Established > 0)
                query = query.Where(o => o.Established == organizationParams.Established).AsQueryable();
            if (organizationParams.City != null)
                query = query.Where(o => o.City!.ToLower().Trim().Contains(organizationParams.City.ToLower().Trim())).AsQueryable();
            if (organizationParams.ProvinceOrState != null)
                query = query.Where(o => o.ProvinceOrState!.ToLower().Trim().Contains(organizationParams.ProvinceOrState.ToLower().Trim())).AsQueryable();
            if (organizationParams.Country != null)
                query = query.Where(o => o.Country!.ToLower().Trim().Contains(organizationParams.Country.ToLower().Trim())).AsQueryable();

            query = organizationParams.OrderBy switch
            {
                "alphabetical" => query.OrderBy(o => o.Name),
                "established" => query.OrderBy(o => o.Established),
                _ => query.OrderByDescending(o => o.LikedByUser!.Count)
            };

            return await PagedList<OrganizationDto>.CreateAsync(
                query.ProjectTo<OrganizationDto>(_mapper.ConfigurationProvider).AsNoTracking(),
                organizationParams.PageNumber,
                organizationParams.PageSize
            );
        }

        public async Task<IEnumerable<Organization>> GetOwnedOrganizationsRawAsync(int id)
        {
            return await _context.Organizations!
                .Where(o => o.OwnerId == id)
                .Include(p => p.Photos)
                .Include(m => m.Members)
                .Include(j => j.Jobs)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetAllOrganizationNames()
        {
            return await _context.Organizations!
                .Select(o => o.Name!)
                .ToListAsync();
        }

        public bool DeleteOrganizationById(int id)
        {
            var organization = _context.Organizations!.Where(o => o.Id == id).SingleOrDefault();

            var IsExisted = organization != null;

            if (IsExisted)
            {
                _context.Organizations!.Remove(organization!);
            }

            return IsExisted;
        }
    }
}