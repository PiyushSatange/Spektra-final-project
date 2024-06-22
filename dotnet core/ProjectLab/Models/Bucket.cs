using System;
using System.Collections.Generic;

namespace ProjectLab.Models;

public partial class Bucket
{
    public int BucketId { get; set; }

    public string BucketName { get; set; } = null!;

    public string Region { get; set; } = null!;

    public string Platform { get; set; } = null!;

    public string RecordStatus { get; set; } = null!;

    public DateTime CreatedDate { get; set; }

    public DateTime? DestroyedDate { get; set; }

    public int? RunningHours { get; set; }

    public string? ResourceType { get; set; }

    public virtual ICollection<UsersBucket> UsersBuckets { get; set; } = new List<UsersBucket>();
}
