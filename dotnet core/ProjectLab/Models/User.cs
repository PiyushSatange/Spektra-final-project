using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectLab.Models;

public partial class User
{
    public string Email { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Uid { get; set; } = null!;

    public virtual ICollection<UsersBucket> UsersBuckets { get; set; } = new List<UsersBucket>();
}
