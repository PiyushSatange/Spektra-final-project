using Microsoft.AspNetCore.Mvc;
using ProjectLab.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly LabContext _context;
        public UserController(LabContext dbContext) 
        { 
            _context = dbContext;
        }

        [HttpGet("{email}")]
        public IActionResult Get(string email)
        {
            User user = _context.Users.Find(email);
            if(user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }


        // POST api/<UserController>
        [HttpPost("adduser")]
        public IActionResult Post(User user)
        {
            if(user == null)
            {
                return NotFound(user);
            }
            User user1 = _context.Users.Find(user.Email);   
            if(user1 != null)
            {
                return BadRequest("User already exist");
            }
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok(user);
        }

        [HttpGet("getAll")]
        public IActionResult getAll()
        {
            var users = _context.Users.ToList();
            if(users == null)
            {
                return NotFound();
            }
            return Ok(users);
        }

        [HttpGet("getByUid")]
        public IActionResult GetUser(string uid)
        {
            if (uid == null)
            {
                return NotFound();
            }
            var users = _context.Users.ToList();
            var user = users
                .Where(u => u.Uid == uid)
                .ToList();
            
            return Ok(user);
        }

        [HttpDelete("BlockUser/{email}")]
        public IActionResult BlockUser(string email)
        {
            if(email == null)
            {
                return BadRequest("user is null");
            }
            var user = _context.Users.Find(email);
            if(user == null)
            {
                return NotFound();
            }
            var userbuckets = _context.UsersBuckets.Where(b => b.UserEmail.Equals(email))
                .Select(b => b.Id);
            foreach(var u in userbuckets)
            {
                //var b = _context.UsersBuckets.Find(u);
                Console.WriteLine(u);
                var ub = _context.UsersBuckets.Find(u);
                _context.UsersBuckets.Remove(ub);
                _context.SaveChanges();
                _context.Dispose();
            }
            _context.Users.Remove(user);
            _context.SaveChanges();
            return Ok();
        }
    }
}
