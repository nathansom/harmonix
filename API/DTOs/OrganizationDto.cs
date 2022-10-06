namespace API.DTOs
{
    public class OrganizationDto
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public string? Name { get; set; }
        public string? Introduction { get; set; }
        public string? PhotoUrl { get; set; }
        public int Established { get; set; }
        public int Likes { get; set; }
        public string? City { get; set; }
        public string? ProvinceOrState { get; set; }
        public string? Country { get; set; }
        public DateTime Created { get; set; } 
        public DateTime LastUpdated { get; set; }
        public ICollection<OrgPhotoDto>? Photos { get; set; }
        public ICollection<OrgMemberDto>? Members { get; set; }
        public ICollection<JobDto>? Jobs { get; set; }
    }
}