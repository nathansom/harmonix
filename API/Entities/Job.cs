using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    [Table("Jobs")]
    public class Job
    {
         public int Id{get;set;}
         public string? Title{get;set;}
        public int ConfirmedOrgId {get;set;}
        public Organization? Organization{get;set;}
        public AppUser? JobPoster { get; set; }
        public string? Description{get;set;}
        public int Salary{get;set;}
        public string? City{set;get;}
        public string? ProvinceOrState{get;set;}
        public string? Country{get;set;}
        public string? Genres{get; set;}
        public string? JobType {get;set;}
        public string? SkillsRequired {get; set;}
        public string? ApplicationUrl {get;set;}
        public DateTime DateCreated{get;set;} = DateTime.Now;
        public DateTime Deadline{get;set;} = DateTime.Now;
        public DateTime LastUpdated{get;set;} = DateTime.Now;
        public ICollection<JobSave>? SavedByUsers{get;set;}  
    }
}
