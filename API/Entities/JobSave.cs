using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class JobSave
    {
        public Job SavedJob{get;set;}
        public int JobId{get;set;}

        public AppUser SavedUser{get;set;}

        public int SavedUserId{get;set;}
    } 

}


