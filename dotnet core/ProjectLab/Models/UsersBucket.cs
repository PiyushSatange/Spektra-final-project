using System;
using System.Collections.Generic;

namespace ProjectLab.Models;

public partial class UsersBucket
{
    public int Id { get; set; }

    public string? UserEmail { get; set; }

    public int? BucketId { get; set; }

    public virtual Bucket? Bucket { get; set; }

    public virtual User? UserEmailNavigation { get; set; }
}
