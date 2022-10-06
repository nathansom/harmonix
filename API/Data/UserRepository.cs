using System.ComponentModel;
using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            var result = await _context.Users
              .Where(x => x.UserName == username)
              .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
              .SingleOrDefaultAsync();

            return result!;
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            var query = _context.Users.AsQueryable();
            
            if (userParams.CurrentUsername != null)
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

            return await PagedList<MemberDto>.CreateAsync(
              query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider).AsNoTracking(),
              userParams.PageNumber,
              userParams.PageSize
            );
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            var result = await _context.Users.FindAsync(id);

            return result!;
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            var result = await _context.Users
              .Include(p => p.Photos!)
              .Include(j => j.CreatedJobs!)
              .Include(j => j.SavedJobs!)
              .SingleOrDefaultAsync(x => x.UserName == username);

            return result!;
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
              .Include(p => p.Photos)
              .Include(j => j.CreatedJobs)
              .Include(j => j.SavedJobs)
              .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}