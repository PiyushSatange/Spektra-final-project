using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProjectLab.Models;
using Microsoft.Data.SqlClient;

namespace ProjectLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BucketController : ControllerBase
    {
        private readonly LabContext _context;
        public BucketController(LabContext DbContext)
        {
            _context = DbContext;
        }

        [HttpGet("getAll")]
        public IActionResult Get()
        {
            var Buckets = _context.Buckets.ToList();
            if(Buckets == null)
            {
                return NotFound();
            }
            return Ok(Buckets);
        }

        [HttpGet("GetAllActiveBuckets")]
        public IActionResult GetActiveBuckets()
        {
            var buckets = _context.Buckets.ToList();
            if (buckets == null || !buckets.Any())
            {
                return NotFound();
            }

            var activeBuckets = buckets
                .Where(b => b.RecordStatus == "Active")
                .OrderBy(b => b.CreatedDate)
                .ToList();

            return Ok(activeBuckets);
        }

        [HttpGet("GetAllDestroyedBuckets")]
        public IActionResult GetDestroyedBuckets()
        {
            var buckets = _context.Buckets.ToList();
            if (buckets == null || !buckets.Any())
            {
                return NotFound();
            }

            var destroyedBuckets = buckets
                .Where(b => b.RecordStatus == "Destroyed")
                .OrderBy(b => b.CreatedDate)
                .ToList();

            return Ok(destroyedBuckets);
        }


        [HttpGet("getById/{id}")]
        public IActionResult Get(int id)
        {
            Bucket bucket = _context.Buckets.Find(id);
            if(bucket == null)
            {
                return NotFound();
            }
            return Ok(bucket);
        }

        [HttpGet("getByEmail")]
        public IActionResult GetByEmail(string email)
        {
            var emailParam = new SqlParameter("@mail", email);
            var buckets = _context.Pr_BucketsByEmailClasses
                .FromSqlRaw("EXECUTE dbo.pr_getBucketByEmail @mail", emailParam)
                .ToList();

            return Ok(buckets);
        }


    }
}
