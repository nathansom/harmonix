using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.DTOs
{
    public class JobUpdateDto
    {
        public string? Description{get;set;}
        public int Salary{get;set;}
        public string? City{set;get;}
        public string? ProvinceOrState{get;set;}
        public string? Country{get;set;}
        public string? Genres{get; set;}
        public string? JobType{get;set;}
        public string? SkillsRequired{get; set;}
         public DateTime Deadline{get;set;}
        public DateTime LastUpdated{get;set;}
        
    }
}