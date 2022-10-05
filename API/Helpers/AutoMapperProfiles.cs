using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AppUser, MemberDto>()
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src =>
                src.Photos.FirstOrDefault(x => x.IsMain).Url))
            .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDto>();
            CreateMap<MemberDto, AppUser>();
            CreateMap<MemberUpdateDto, AppUser>();
            CreateMap<RegisterDto, AppUser>();
            CreateMap<Message, MessageDto>()
                .ForMember(dest => dest.SenderPhotoUrl, opt => opt.MapFrom(src =>
                    src.Sender.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.RecipientPhotoUrl, opt => opt.MapFrom(src =>
                    src.Recipient.Photos.FirstOrDefault(x => x.IsMain).Url));
            CreateMap<DateTime, DateTime>().ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));

            CreateMap<Job, JobDto>()
            .ForMember(dest => dest.JobPosterId, opt => opt.MapFrom(src =>
            src.JobPoster.Id))
            .ForMember(dest => dest.JobPosterName, opt => opt.MapFrom( src =>
            src.JobPoster.KnownAs))
            .ForMember(dest => dest.OrgId, opt => opt.MapFrom(src =>
             src.Organization.Id))
             .ForMember(dest => dest.Organization, opt => opt.MapFrom(src =>
             src.Organization == null ? "Private Event" : src.Organization.Name))
             .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src =>
             src.Organization.Photos == null || src.Organization == null 
             ? src.JobPoster.Photos.FirstOrDefault(p => p.IsMain).Url
             : src.Organization.Photos.FirstOrDefault(x => x.IsMain).Url));

            CreateMap<JobUpdateDto, Job>();
            CreateMap<JobRegisterDto, Job>();

            CreateMap<JobSave, JobSaveDto>()
             .ForMember(dest => dest.Title, opt => opt.MapFrom(src =>
                src.SavedJob.Title))
             .ForMember(dest => dest.OrgId, opt => opt.MapFrom(src =>
                src.SavedJob.Organization.Id))
             .ForMember(dest => dest.JobPosterId, opt => opt.MapFrom(src =>
                src.SavedJob.JobPoster.Id))
             .ForMember(dest => dest.JobPosterName, opt => opt.MapFrom(src =>
                src.SavedJob.JobPoster.KnownAs))
             .ForMember(dest => dest.LogoUrl, opt => opt.MapFrom(src =>
                src.SavedJob.JobPoster.Photos.FirstOrDefault(x => x.IsMain).Url))
             .ForMember(dest => dest.Description, opt => opt.MapFrom(src =>
                src.SavedJob.Description))
             .ForMember(dest => dest.Salary, opt => opt.MapFrom(src =>
                src.SavedJob.Salary))
             .ForMember(dest => dest.City, opt => opt.MapFrom(src =>
                src.SavedJob.City))
             .ForMember(dest => dest.ProvinceOrState, opt => opt.MapFrom(src =>
                src.SavedJob.ProvinceOrState))
             .ForMember(dest => dest.Country, opt => opt.MapFrom(src =>
                src.SavedJob.Country))
             .ForMember(dest => dest.Genres, opt => opt.MapFrom(src =>
                src.SavedJob.Genres))
             .ForMember(dest => dest.JobType, opt => opt.MapFrom(src =>
                src.SavedJob.JobType))
             .ForMember(dest => dest.SkillsRequired, opt => opt.MapFrom(src =>
                src.SavedJob.SkillsRequired))
             .ForMember(dest => dest.ApplicationUrl, opt => opt.MapFrom(src =>
                src.SavedJob.ApplicationUrl))
             .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src =>
                src.SavedJob.DateCreated))
             .ForMember(dest => dest.Deadline, opt => opt.MapFrom(src =>
                src.SavedJob.Deadline))
             .ForMember(dest => dest.LastUpdated, opt => opt.MapFrom(src =>
                src.SavedJob.LastUpdated));

            CreateMap<Organization, OrganizationDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src =>
                    src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.Likes, opt => opt.MapFrom(src =>
                    src.LikedByUser.Count()))
                .ForMember(dest => dest.Jobs, opt => opt.MapFrom(src => 
                     src.Jobs.ToList()));

            CreateMap<OrganizationDto, Organization>();

            CreateMap<Organization, AffiliationDto>()
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src =>
                    src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.MembersCount, opt => opt.MapFrom(src =>
                    src.Members.Count()))
                .ForMember(dest => dest.Likes, opt => opt.MapFrom(src =>
                    src.LikedByUser.Count()));

            //CreateMap<AffiliationDto, Organization>();

            CreateMap<AppUser, OrgMemberDto>()
            .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom(src =>
                    src.Photos.FirstOrDefault(x => x.IsMain).Url));


            CreateMap<OrgPhoto, OrgPhotoDto>();
            CreateMap<OrganizationUpdateDto, Organization>();
            CreateMap<OrganizationRegisterDto, Organization>();
            CreateMap<OrgLike, OrgLikeDto>()
                .ForMember(dest => dest.Organization, opt => opt.MapFrom(src =>
                    src.Org.Name))
                .ForMember(dest => dest.Introduction, opt => opt.MapFrom(src =>
                    src.Org.Introduction))
                .ForMember(dest => dest.OrganizationPhotoUrl, opt => opt.MapFrom(src =>
                    src.Org.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.OrganizationCity, opt => opt.MapFrom(src =>
                    src.Org.City))
                .ForMember(dest => dest.OrganizationState, opt => opt.MapFrom(src =>
                    src.Org.ProvinceOrState))
                .ForMember(dest => dest.OrganizationCountry, opt => opt.MapFrom(src =>
                    src.Org.Country))
                .ForMember(dest => dest.LikedUserFirstname, opt => opt.MapFrom(src =>
                    src.LikedUser.FirstName))
                .ForMember(dest => dest.LikedUserLastname, opt => opt.MapFrom(src =>
                    src.LikedUser.LastName))
                .ForMember(dest => dest.LikedUserKnownAs, opt => opt.MapFrom(src =>
                    src.LikedUser.KnownAs))
                .ForMember(dest => dest.LikedUserCity, opt => opt.MapFrom(src =>
                    src.LikedUser.City))
                .ForMember(dest => dest.LikedUserState, opt => opt.MapFrom(src =>
                    src.LikedUser.ProvinceOrState))
                .ForMember(dest => dest.LikedUserCountry, opt => opt.MapFrom(src =>
                    src.LikedUser.Country))
                .ForMember(dest => dest.LikedUserPhotoUrl, opt => opt.MapFrom(src =>
                    src.LikedUser.Photos.FirstOrDefault(x => x.IsMain).Url));

            CreateMap<Organization, OrgLikeDto>()
            .ForMember(dest => dest.Organization, opt => opt.MapFrom(src =>
                    src.Name))
                .ForMember(dest => dest.Introduction, opt => opt.MapFrom(src =>
                    src.Introduction))
                .ForMember(dest => dest.OrganizationPhotoUrl, opt => opt.MapFrom(src =>
                    src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.OrganizationCity, opt => opt.MapFrom(src =>
                    src.City))
                .ForMember(dest => dest.OrganizationState, opt => opt.MapFrom(src =>
                    src.ProvinceOrState))
                .ForMember(dest => dest.OrganizationCountry, opt => opt.MapFrom(src =>
                    src.Country));
            ;
        }
    }
}