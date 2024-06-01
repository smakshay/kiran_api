using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CG_AirLineApi.Models;
using Microsoft.Extensions.Configuration;
using CG_AirLineApi.Dtos;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace CG_AirLineApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly CG_AirlinesContext _context;
        private readonly IConfiguration config;

        public UsersController(CG_AirlinesContext context, IConfiguration config)
        {

            _context = context;

            this.config = config;

        }
        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.Users.ToListAsync();
        }
        /*  // GET: api/Users/5
          [HttpGet("{id}")]
          public async Task<ActionResult<User>> GetUserTable(int id)
          {
              var userTable = await _context.Users.FindAsync(id); if (userTable == null)
              {
                  return NotFound();
              }
              return userTable;
          }     */


        // GET: api/Users/5
        [HttpGet("ReservationHistory/{id}")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetReservation(int id)
        {
            var reservation = await _context.Reservations.Where(r => r.Id == id).ToListAsync();
            if (reservation == null)
            {
                return NotFound();
            }
            return reservation;
        }
        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
       /* [HttpPut("{id}")]
        public async Task<IActionResult> PutUserTable(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }
            _context.Entry(user).State = EntityState.Modified; try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }  */    
       // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> Register(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            user.RoleId = 2;
            _context.Users.Add(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (UserExists(user.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }
        [HttpPost("login")]
        public async Task<ActionResult<LoginDto>> Login(User User)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //credintial check
            var existingUser = await _context.Users.Include(r=>r.Role).FirstOrDefaultAsync
                (u => u.Username == User.Username && u.Password == User.Password);
            if (existingUser == null)
            {
                return NotFound();
            }
            //login dto object send
            var loginDto = new LoginDto
            {
                username = User.Username,
                id=existingUser.Id,
                RoleName=existingUser.Role.RoleName,
                Token = GenerateJwtToken(existingUser)
            };
            return Ok(loginDto);
        }
        private string GenerateJwtToken(User existingUser)
        {
            var secret = config["Secret"];
            var expiryDays = config.GetValue<int>("ExpiryDays");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = config["Issuer"],
                Audience = config["Audience"],
                Subject = new ClaimsIdentity(new[]
            {
 new Claim(JwtRegisteredClaimNames.Jti, existingUser.Id.ToString()),
 }),
                Expires = DateTime.UtcNow.AddDays(expiryDays),
                SigningCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)),
            SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }         // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync(); return NoContent();
        }
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}