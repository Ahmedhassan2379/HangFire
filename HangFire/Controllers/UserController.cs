using HangFire.Dtos.Auth;
using HangFire.Helpers;
using HangFire.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.RegularExpressions;

namespace HangFire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] LogedUser userobj)
        {
            if(userobj == null)
            {
                return BadRequest();
            }
            var logeduser = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userobj.UserName);
             if(logeduser == null)
             {
                return NotFound(new { Message = "User Not Found!" });
             }
             if(!(HasingPasword.VerifyPassword(userobj.Password,logeduser.Password)))
             {
                return NotFound(new { Message = "Password is Wrong!" });
             }
            return Ok(new {Message ="Login Success"});
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto userobj)
        {
            if (userobj == null)
            {
                return BadRequest();
            }
            //check UserName
            if(await(checkUserNameExist(userobj.UserName)))
            {
                return BadRequest(new { message = "UserName Already Exist!" });
            }
            // check Email
            if(await(checkUserEmailExist(userobj.Email)))
            {
                return BadRequest(new { message = "Email Already Exist!" });

            }

            var pass = checkpasswordExist(userobj.Password);
            if(!string.IsNullOrEmpty(pass))
            {
                return BadRequest(new { message = pass.ToString() });

            }
            userobj.Password= HasingPasword.HashPassword(userobj.Password);
            User user = new User()
            {
                UserName = userobj.UserName,
                Password = userobj.Password,
                Email = userobj.Email,
                LastName = userobj.LastName,
                FirstName = userobj.FirstName,
            };
            await _context.Users.AddAsync(user);
             _context.SaveChanges();
            return Ok(new { Message = "User Registered" });
        }

        private async Task<bool> checkUserNameExist(string userName)
        {
            return await _context.Users.AnyAsync(x=>x.UserName==userName);
        }

        private async Task<bool> checkUserEmailExist(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email);
        }

        private string checkpasswordExist(string password)
        {
            StringBuilder sb = new StringBuilder();
            if(password.Length < 8)
            {
                sb.Append("minimum length must be 8 " + Environment.NewLine);
            };
            if(Regex.IsMatch(password, " ^(?=.*?[a - z])"))
            {
                 sb.Append("password must be contained at least one small character " + Environment.NewLine);

            }
            if (Regex.IsMatch(password, " ^(?=.*?[A - Z])"))
            {
                sb.Append("password must be contained at least one capital character " + Environment.NewLine);

            }
            if (Regex.IsMatch(password, " ^(?=.*?[0 - 9])"))
            {
                sb.Append("password must be contained at least one number " + Environment.NewLine);

            }
            if (Regex.IsMatch(password, " ^(?=.*?[#?!@$%^&*-])"))
            {
                sb.Append("password must be contained at least one special character " + Environment.NewLine);

            }
            return sb.ToString();
        }
    }
}
