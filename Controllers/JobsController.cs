using AutoMapper;
using JobsAPI.Models;
using JobsAPI.WebModels.DepartmentWebModel;
using JobsAPI.WebModels.JobWebModel;
using JobsAPI.WebModels.LocationWebModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Reflection;

namespace JobsAPI.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<JobsController> _logger;

        public JobsController(DatabaseContext context, IMapper mapper, ILogger<JobsController> logger)
        {
            this._context = context;
            this._mapper = mapper;
            this._logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Job>>> GetJob()
        {
            try
            {
                var result = await _context.Jobs.OrderByDescending(i=>i.PostedDate).ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetJobResponseWebModel>> GetJob(int id)
        {
            try
            {
                var res = await _context.Jobs.Include("Department").Include("Location").FirstOrDefaultAsync(x => x.JobId == id);
                if (res == null)
                {
                    return NotFound($"Job with ID = {id} not found");
                }
                return new GetJobResponseWebModel
                {
                    Id = res.JobId,
                    Code = res.Code,
                    Title = res.Title,
                    Description = res.Description,
                    PostedDate = res.PostedDate,
                    ClosingDate = res.ClosingDate,
                    Department = res.Department == null ? null : new GetDepartmentResponseWebModel
                    {
                        Id = res.Department.DepartmentId,
                        Title = res.Department?.Title,
                    },
                    Location = res.Location == null ? null : new GetLocationResponseWebModel
                    {
                        Id = res.Location.LocationId,
                        Title = res.Location?.Title,
                        City = res.Location?.City,
                        State = res.Location?.State,
                        Country = res.Location?.Country,
                        Zip = res.Location?.Zip,
                    },


                };
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error retrieving job");
            }
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutJob(int id, [FromBody] UpdateJobRequestWebModel updatemodel)
        {
            try
            {
                var jobup = await _context.Jobs.FindAsync(id);
                if (jobup == null)
                    return NotFound($"Job with ID = {id} not found");
                jobup.Title = updatemodel.Title;
                jobup.Description = updatemodel.Description;
                if (updatemodel.LocationId != 0)
                {
                    jobup.LocationId = updatemodel.LocationId;
                }
                if (updatemodel.DepartmentId !=0)
                {
                    jobup.DepartmentId = updatemodel.DepartmentId;                    
                }
                jobup.ClosingDate = updatemodel.ClosingDate;
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error updating job");
            }

        }
        [HttpPost]
        public async Task<ActionResult<GetJobResponseWebModel>> PostJob(CreateJobRequestWebModel createmodel)
        {
            try
            {
                var result = new Job
                {
                    Title = createmodel.Title,
                    Description = createmodel.Description,
                    LocationId = createmodel.LocationId,
                    DepartmentId = createmodel.DepartmentId,
                    ClosingDate = Convert.ToDateTime(createmodel.ClosingDate).ToUniversalTime(),
                    PostedDate = DateTime.UtcNow,
                };
                _context.Jobs.Add(result);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetJob), new {id = result.JobId}, result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error creating job");
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteJob(int id)
        {
            try
            {
                var jobto = await _context.Jobs.FindAsync(id);
                if (jobto == null)
                {
                    return NotFound($"Job with ID = {id} not found");
                }

                _context.Jobs.Remove(jobto);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error deleting job");
            }
        
        }
    }
}
