using AutoMapper;
using JobsAPI.Models;
using JobsAPI.WebModels.LocationWebModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace JobsAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        
        public LocationController(DatabaseContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetLocationResponseWebModel>>> GetLocation()
        {
            try
            {
                /*var result = await _context.Locations.Select(x => new GetLocationResponseWebModel
                {
                Id = x.LocationId,
                City = x.City,
                State = x.State,
                Country = x.Country,
                Zip = x.Zip,
                Title = x.Title
                }).ToListAsync();
                return Ok(result);*/
                var result = await _context.Locations.Select(x => _mapper.Map<GetLocationResponseWebModel>(x)).ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Could not retrieve data from the Location List");
            }
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetLocationResponseWebModel>> GetLocation(int id)
        {
            try
            {
                var result = await _context.Locations.FindAsync(id);
                if (result==null)
                {
                    return NotFound($"The Location with id:{id} does not exist");
                }
                return new GetLocationResponseWebModel
                {
                    Id = result.LocationId,
                    City = result.City,
                    State = result.State,
                    Zip = result.Zip,
                    Country = result.Country,
                    Title = result.Title,
                };
            }
            catch(Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutLocation(int id, [FromBody] UpdateLocationRequestWebModel model)
        {
            try
            {
                var result = await _context.Locations.FindAsync(id);
                if (result==null) { return NotFound($"THe Location with id:{id} does not exist"); }
                result.City = model.City;
                result.State = model.State;
                result.Zip = model.Zip;
                result.Country = model.Country;
                result.Title = model.Title;
                await _context.SaveChangesAsync();
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<Location>> PostLocation([FromBody]CreateLocationRequestWebModel model)
        {
            try
            {
                var result = _mapper.Map<Location>(model);
                if (result==null) { throw new ArgumentNullException(nameof(model)); }
                await _context.Locations.AddAsync(result);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetLocation), new { id = result.LocationId }, null);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            try
            {
                var result = await _context.Locations.FindAsync(id);
                if (result == null)
                {
                    return NotFound($"the locations with id:{id} does not exist");
                }
                _context.Locations.Remove(result);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Could not retrieve data from the Location");
            }
        }
    }
}
