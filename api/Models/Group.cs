using System;
using System.Collections.Generic;

namespace api.Models;

public partial class Group
{
    public int Id { get; set; }

    public string GroupName { get; set; } = null!;

    public string Faculty { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
