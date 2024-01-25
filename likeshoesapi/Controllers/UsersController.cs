using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using likeshoesapi.DTOs;
using likeshoesapi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace likeshoesapi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<IdentityUser> _signInManager;

        public UsersController(
            ApplicationDbContext context,
            IMapper mapper,
            UserManager<IdentityUser> userManager,
            IConfiguration configuration,
            SignInManager<IdentityUser> signInManager
        )
        {
            this._context = context;
            this._mapper = mapper;
            this._userManager = userManager;
            this._configuration = configuration;
            this._signInManager = signInManager;
        }

        [HttpGet("login")]
        public async Task<ActionResult<AutenticationResponse>> Login(
            UserCredentials userCredentials
        )
        {
            var result = await _signInManager.PasswordSignInAsync(
                userCredentials.Email,
                userCredentials.Password,
                isPersistent: false,
                lockoutOnFailure: false
            );

            if (result.Succeeded)
            {
                return BuildToken(userCredentials);
            }
            else
            {
                return BadRequest("Login incorrecto");
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<AutenticationResponse>> Register(
            UserCredentials userCredentials
        )
        {
            var user = new IdentityUser
            {
                UserName = userCredentials.Email,
                Email = userCredentials.Email
            };
            var result = await _userManager.CreateAsync(user, userCredentials.Password);

            if (result.Succeeded)
            {
                return BuildToken(userCredentials);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpGet("getUsers")]
        public async Task<ActionResult<List<UserDTO>>> GetUsers()
        {
            try
            {
                var users = await _context.Users.ToListAsync();

                var usersDTO = _mapper.Map<List<UserDTO>>(users);

                return Ok(usersDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno en el servidor al procesar la solicitud."
                );
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> Get(int id)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(user => user.Id.Equals(id));

                if (user == null)
                {
                    return NotFound();
                }

                var userDTO = _mapper.Map<UserDTO>(user);

                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno en el servidor al procesar la solicitud."
                );
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);

                if (user == null)
                {
                    return NotFound();
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno en el servidor al procesar la solicitud."
                );
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDTO>> Put(int id, [FromBody] UserPostDTO userPostDTO)
        {
            try
            {
                var user = _mapper.Map<User>(userPostDTO);
                user.Id = id;
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                var userDTO = _mapper.Map<UserDTO>(user);

                return Ok(userDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno en el servidor al procesar la solicitud."
                );
            }
        }

        private AutenticationResponse BuildToken(UserCredentials userCredentials)
        {
            var claims = new List<Claim>() { new Claim("email", userCredentials.Email) };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["keyjwt"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddDays(1);

            var securityToken = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            return new AutenticationResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiration = expiration
            };
        }
    }
}
