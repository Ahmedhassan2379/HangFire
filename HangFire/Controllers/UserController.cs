using HangFire.Dtos.Auth;
using HangFire.Helpers;
using HangFire.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
            logeduser.Token = CreateJWT(logeduser);
            return Ok(new 
            {
                Token = logeduser.Token,
                Message ="Login Success"
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User userobj)
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
          
            await _context.Users.AddAsync(userobj);
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
        
        private string CreateJWT(User user)
        {
            var jwt = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("very-very-very-secret.....");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role,user.Role),
                new Claim(ClaimTypes.Name,user.UserName)
            });
            var credntial = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject=identity,
                Expires= DateTime.Now.AddDays(1),
                SigningCredentials=credntial,
            };
            var tokenHandler = jwt.CreateToken(tokenDescriptor);
             var r = jwt.WriteToken(tokenHandler);
            return r;
        }

        [Authorize]
        [HttpGet]
        public List<User> GetAllUsers()
        {
            var users = _context.Users.ToList();
            return users;
        }
    }
}
