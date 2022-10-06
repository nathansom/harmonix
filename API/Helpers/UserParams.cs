namespace API.Helpers
{
  public class UserParams : PaginationParams
    {
        public string? CurrentUsername { get; set; }
        public string? Gender { get; set; }
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 150;
        public string? Occupation { get; set; }
        public string? Skill { get; set; }
        public string? Genre { get; set; }
        public string? City { get; set; }
        public string? ProvinceOrState { get; set; }
        public string? Country { get; set; }
        public string OrderBy { get; set; } = "lastActive";
    }
}