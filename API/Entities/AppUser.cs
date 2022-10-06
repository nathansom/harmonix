using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser<int>
  {
    public DateTime DateOfBirth { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? KnownAs { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;
    public DateTime LastActive { get; set; } = DateTime.Now;
    public string? Gender { get; set; }
    public string? Introduction { get; set; }
    public string? LookingFor { get; set; }
    public string? Interests { get; set; }
    public string? Occupation { get; set; }
    public string? Skills { get; set; }
    public string Genres { get; set; } = "";
    public string? City { get; set; }
    public string? ProvinceOrState { get; set; }
    public string? Country { get; set; }
    public ICollection<Photo>? Photos { get; set; }
    public ICollection<UserLike>? LikedByUsers { get; set; }
    public ICollection<UserLike>? LikedUsers { get; set; }
    public ICollection<Message>? MessagesSent { get; set; }
    public ICollection<Message>? MessagesReceived { get; set; }
    public ICollection<AppUserRole>? UserRoles { get; set; }

    public ICollection<JobSave>? SavedJobs{get; set;}
    public ICollection<Job>? CreatedJobs{get; set;}
    public ICollection<Organization>? Affiliation { get; set; }
    public ICollection<OrgLike>? LikedByOrganizations { get; set; }
    // ! should have LikedOrganizations instead
  }
}