using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class AffiliationDto
    {
            public int Id { get; set; }
            public string? Name { get; set; }
            public string? Introduction { get; set; }
            public string? PhotoUrl { get; set; }
            public int MembersCount { get; set; }
            public int Likes { get; set; }
            public int Established { get; set; }
        }
}