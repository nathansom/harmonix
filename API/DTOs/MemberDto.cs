namespace API.DTOs
{
    public class MemberDto
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhotoUrl { get; set; }
        public int Age { get; set; }
        public string? KnownAs { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }
        public string? Gender{ get; set; }
        public string? Introduction { get; set; }
        public string? LookingFor { get; set; }
        public string? Interests { get; set; }
        public string? Occupation { get; set; }
        public string? Skills { get; set; }
        public string? Genres { get; set; }
        public string? City { get; set; }
        public string? ProvinceOrState { get; set; }
        public string? Country { get; set; }
        public ICollection<PhotoDto>? Photos { get; set; }
        public ICollection<JobDto>? CreatedJobs { get; set; }
        public ICollection<JobSaveDto>? SavedJobs { get; set; }
        public ICollection<AffiliationDto>? Affiliation {get; set;}
    }
}