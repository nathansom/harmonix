namespace API.DTOs
{
    public class OrganizationRegisterDto
    {
        public string? Name { get; set; }
        public string? OrgType { get; set; }
        public string? Introduction { get; set; }
        public int Established { get; set; }
        public string? City { get; set; }
        public string? ProvinceOrState { get; set; }
        public string? Country { get; set; }
    }
}