using AutoMapper;
using likeshoesapi.DTOs;
using likeshoesapi.DTOs.Shoe;
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
        public async Task<ActionResult<List<ShoeSectionDTO>>> GetSections()
        {
            try
            {
                var sections = await _context
                    .ShoeSections.Include(x => x.ShoeSectionShoeType)
                    .ThenInclude(x => x.ShoeType)
                    .ToListAsync();

                return _mapper.Map<List<ShoeSectionDTO>>(sections);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    "Ocurrió un error interno en el servidor al procesar la solicitud."
                );
            }
        }

        // Debo agregarle el filtro
        [HttpGet("shoe-catalog")]
        public async Task<ActionResult<List<ShoeDTO>>> GetShoes(int shoeSectionId, int shoeTypeId)
        {
            var shoes = new List<Shoe>();
            try
            {
                if (shoeTypeId == 0)
                {
                    shoes = await _context
                        .Shoe.Where(x => x.ShoeSectionId.Equals(shoeSectionId))
                        .ToListAsync();
                }
                else
                {
                    shoes = await _context
                        .Shoe.Where(x =>
                            x.ShoeSectionId.Equals(shoeSectionId) && x.ShoeTypeId.Equals(shoeTypeId)
                        )
                        .ToListAsync();
                }

                return _mapper.Map<List<ShoeDTO>>(shoes);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    "Ocurrio un error interno en el servidor al procesar la solicitud"
                );
            }
        }

        [HttpGet("section")]
        public async Task<ActionResult<ShoeSectionDTO>> GetSection(int id)
        {
            var shoeSection = await _context
                .ShoeSections.Include(x => x.ShoeSectionShoeType)
                .ThenInclude(x => x.ShoeType)
                .FirstOrDefaultAsync(x => x.Id == id);

            return _mapper.Map<ShoeSectionDTO>(shoeSection);
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

        [HttpPost("shoe-type")]
        public async Task<ActionResult> PostShoeType(ShoeTypeDTO shoeTypeDTO)
        {
            var shoeType = await _context
                .ShoeTypes.Where(shoeType => shoeType.Id.Equals(shoeTypeDTO.Id))
                .SingleOrDefaultAsync();

            if (shoeType != null)
            {
                return BadRequest($"El zapato con id {shoeTypeDTO.Id} ya existe");
            }

            var shoeTypeMap = _mapper.Map<ShoeType>(shoeTypeDTO);
            _context.Add(shoeTypeMap);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("shoe")]
        public async Task<ActionResult> PostShoe(ShoeDTO shoeDTO)
        {
            try
            {
                var shoeSectionId = await _context
                    .ShoeSections.Where(shoeSection => shoeDTO.ShoeSectionId.Equals(shoeSection.Id))
                    .Select(x => x.Id)
                    .FirstAsync();

                if (shoeDTO.ShoeSectionId != shoeSectionId)
                {
                    return BadRequest("No existe la seccion enviada");
                }

                var shoe = _mapper.Map<Shoe>(shoeDTO);

                _context.Add(shoe);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Error en DB: " + ex);
            }
        }
    }
}
