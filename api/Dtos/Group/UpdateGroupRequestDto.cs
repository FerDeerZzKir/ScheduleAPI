using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Group
{
    public class UpdateGroupRequestDto
    {

            public string GroupName { get; set; } = null!;

            public string Faculty { get; set; } = null!;
    }
}