namespace API.Helpers
{
    public class JobParams : PaginationParams
    {
        public string? Title { get; set; }
        public string? JobType { get; set; }
        public int? PosterID { get; set; }
        public bool SelfPost { get; set; }
        public string OrderBy { get; set; } = "mostRecent";
        public string? Genres { get; set; }
        public string? SkillsRequired { get; set; }
        public string? City { get; set; }
        public string? ProvinceOrState { get; set; }
        public string? Country { get; set; }
    }
}