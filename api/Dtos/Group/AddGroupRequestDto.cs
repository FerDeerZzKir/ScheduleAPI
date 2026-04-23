using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace api.Dtos.Group
{
    public class AddGroupRequestDto
    {
            public int Id { get; set; }
            public string GroupName { get; set; } = null!;
            public string Faculty { get; set; } = null!;
    }
}