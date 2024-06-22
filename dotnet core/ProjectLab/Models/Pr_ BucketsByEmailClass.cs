using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProjectLab.Models
{
    public class Pr__BucketsByEmailClass
    {
        public string? Email { get; set; }
        public int Bucket_Id { get; set; }
        public string? Bucket_Name { get; set; }
        public string? Region { get; set; }
        public string? Platform { get; set; }
        public string? Record_Status { get; set; }
        public DateTime Created_Date { get; set; }
        public DateTime? Destroyed_Date { get; set; }
        public int? Running_Hours { get; set; }
        public string? Resource_Type { get; set; }
    }
}
