using AutoMapper;
using likeshoesapi.DTOs;
using likeshoesapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace likeshoesapi.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UsersController(ApplicationDbContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        [HttpGet("login")]
        public async Task<ActionResult<UserDTO>> UserLogin([FromBody] UserLoginDTO userLoginDTO)
        {
            if (userLoginDTO == null || userLoginDTO.Email == null || userLoginDTO.Password == null)
            {
                return BadRequest("Invalid user data");
            }

            var user = await _context
                .Users
                .Where(
                    user =>
                        userLoginDTO.Email.Equals(user.Email)
                        && userLoginDTO.Password.Equals(user.Password)
                )
                .SingleOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<UserDTO>(user);

            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<UserDTO>> Post([FromBody] UserPostDTO userPostDTO)
        {
            try
            {
                if (
                    userPostDTO == null
                    || userPostDTO.Email == null
                    || userPostDTO.Password == null
                )
                {
                    return BadRequest("Invalid user data");
                }

                var userExistence = await _context
                    .Users
                    .AnyAsync(u => userPostDTO.Email.Equals(u.Email));

                if (userExistence)
                {
                    return Conflict("El correo electronico ya esta en uso");
                }

                var userPost = _mapper.Map<User>(userPostDTO);
                _context.Add(userPost);
                await _context.SaveChangesAsync();
                var userDTO = _mapper.Map<UserDTO>(userPost);

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
    }
}
