using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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

        public async Task<IEnumerable<OrganizationDto>> GetOrganizationsAsync()
        {
            return await  _context.Organizations
                .ProjectTo<OrganizationDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<Organization> GetOrganizationByIdAsync(int id)
        {
            return await _context.Organizations.FindAsync(id);
           // throw new NotImplementedException();
        }

        public Task<Organization> GetOrganizationByOrganizationameAsync(string organizationname)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveAllAsync()
        {
            throw new NotImplementedException();
        }

        // public void Update(AppUser user)
        // {
        //     throw new NotImplementedException();
        // }

    }
}