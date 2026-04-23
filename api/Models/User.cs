using System;
using System.Collections.Generic;

namespace api.Models;

public partial class User
{
    public long UserId { get; set; }

    public string? Username { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? SurName { get; set; }

    public int? GroupId { get; set; }

    public string Role { get; set; } = null!;

    public long? LastMessageId { get; set; }

    public DateOnly CreatedAt { get; set; }

    public string? LecturerId { get; set; }

    public virtual Group? Group { get; set; }
}
