using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.User
{
    public class PatchUserRequestDto
    {
        public string? Username { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? SurName { get; set; }

        public int? GroupId { get; set; }
        public string? LecturerId { get; set; }

        public string? Role { get; set; } = null!;

        public long? LastMessageId { get; set; }
    }
}