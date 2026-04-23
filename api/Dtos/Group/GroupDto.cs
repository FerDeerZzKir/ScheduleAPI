using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using api.Models;

namespace api.Dtos.Group
{
    public class GroupDto
    {       
            [JsonPropertyName("id")]
            public int Id { get; set; }

            [JsonPropertyName("name")]
            public string GroupName { get; set; } = null!;
        
            [JsonPropertyName("faculty")]
            public string Faculty { get; set; } = null!;

    }
}