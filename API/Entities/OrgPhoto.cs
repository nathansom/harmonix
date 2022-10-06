using System.ComponentModel.DataAnnotations.Schema; 

namespace API.Entities
{
  [Table("OrgPhotos")]

    public class OrgPhoto
    {

    public int Id { get; set; }
    public string? Url { get; set; }
    public bool IsMain { get; set; }
    public string? PublicId { get; set; }
    public Organization? Organization { get; set; }
    public int OrganizationId { get; set; }
    
  }
}
