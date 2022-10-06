using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class JobSaveDto
    {
        public int Id{get;set;}
        public string? Title{get;set;}
        public int OrgId{get;set;}

        public int JobPosterId{get;set;}
        public string? JobPosterName { get; set; }

        public string? LogoUrl{get;set;}
        public string? Description{get;set;}
        public int Salary{get;set;}
        public string? City{set;get;}
        public string? ProvinceOrState{get;set;}
        public string? Country{get;set;}
        public string? Genres{get; set;}
        public string? JobType{get;set;}
        public string? SkillsRequired{get; set;}
        public string? ApplicationUrl{get;set;}
        public DateTime DateCreated{get;set;}
        public DateTime Deadline{get;set;}
        public DateTime LastUpdated{get;set;}


    }
}