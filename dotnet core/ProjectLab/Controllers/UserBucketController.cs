using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectLab.Models;

namespace ProjectLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBucketController : ControllerBase
    {
        private readonly LabContext _context;
        public UserBucketController(LabContext context)
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult addUserBucket(UsersBucket ub)
        {
            if(ub == null)
            {
                return BadRequest();
            }
            _context.UsersBuckets.Add(ub);
            _context.SaveChanges();
            return Ok(ub);
        }

    }
}
