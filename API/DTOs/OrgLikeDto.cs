namespace API.DTOs
{
    public class OrgLikeDto
    {
        public int OrgId { get; set; }
        public string? Organization { get; set; }
        public string? Introduction { get; set; }
        public string? OrganizationCity { get; set; }
        public string? OrganizationState { get; set; }
        public string? OrganizationCountry { get; set; }
        public string? OrganizationPhotoUrl { get; set; }
        public int LikedUserId { get; set; }
        public string? LikedUserFirstname { get; set; }
        public string? LikedUserLastname { get; set; }
        public string? LikedUserKnownAs { get; set; }
        public string? LikedUserCity { get; set; }
        public string? LikedUserState { get; set; }
        public string? LikedUserCountry { get; set; }
        public string? LikedUserPhotoUrl { get; set; }
    }
}
