namespace API.Entities
{
    public class Organization
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? OrgType { get; set; }
        public string? Introduction { get; set; }
        public int Established { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime LastUpdated { get; set; } = DateTime.Now;
        public string? City { get; set; }
        public string? ProvinceOrState { get; set; }
        public string? Country { get; set; }
        public int OwnerId { get; set; }
        public ICollection<AppUser>? Members { get; set; }
        public ICollection<Job>? Jobs { get; set; }
        public ICollection<OrgLike>? LikedByUser { get; set; }
        public ICollection<OrgPhoto>? Photos { get; set; }
    }
}