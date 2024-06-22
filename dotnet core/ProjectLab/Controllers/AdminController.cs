using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectLab.Models;

namespace ProjectLab.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly LabContext _context;
        public AdminController(LabContext lc) 
        { 
            _context = lc;
        }

        [HttpGet("{email}/{pass}")]
        public IActionResult Get(string email, string pass)
        {
            if(email == null || pass == null)
            {
                return Ok();
            }
            var admins = _context.Admins
                .Where(a => a.Email.Equals(email) & a.Password.Equals(pass))
                .ToList();
            if(admins.Count == 0)
            {
                return Ok();
            }
            return Ok(admins);
        }
    }
}
