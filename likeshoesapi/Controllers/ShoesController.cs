using AutoMapper;
using likeshoesapi.DTOs;
using likeshoesapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace likeshoesapi.Controllers
{
    [ApiController]
    [Route("api/shoes")]
    public class ShoesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ShoesController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("sections")]
        public async Task<ActionResult<List<ShoeSection>>> GetSections()
        {
            var sections = new List<ShoeSection>();
            try
            {
                sections = await _context.ShoeSections.ToListAsync();

                return Ok(sections);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno en el servidor al procesar la solicitud."
                );
            }
        }

        [HttpPost("section")]
        public async Task<ActionResult> PostSection(ShoeSectionPostDTO shoeSectionPostDTO)
        {
            var shoeTypeIds = await _context
                .ShoeTypes.Where(shoeType => shoeSectionPostDTO.ShoeTypeIds.Contains(shoeType.Id))
                .Select(x => x.Id)
                .ToListAsync();

            if (shoeSectionPostDTO.ShoeTypeIds?.Count != shoeTypeIds.Count)
            {
                return BadRequest("No existe unos de los tipos de zapato enviado");
            }

            var shoeSection = _mapper.Map<ShoeSection>(shoeSectionPostDTO);
            _context.Add(shoeSection);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
