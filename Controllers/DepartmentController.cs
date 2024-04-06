using JobsAPI.Models;
using JobsAPI.WebModels.DepartmentWebModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace JobsAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    
    public class DepartmentController : ControllerBase
    {
        private readonly DatabaseContext _context;
        public DepartmentController(DatabaseContext context)
        {
            this._context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetDepartmentResponseWebModel>>> DetDepartment()
        {
            try
            {
                var result = await _context.Departments.Select(x => new GetDepartmentResponseWebModel
                {
                    Id = x.DepartmentId,
                    Title = x.Title,
                }).ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error Retrieving Department List");
            }
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetDepartmentResponseWebModel>> GetDepartment(int id)
        {
            try
            {
                var result = await _context.Departments.FindAsync(id);
                if (result==null)
                {
                    return NotFound($"Department with id:{id} does not exist");
                }
                return new GetDepartmentResponseWebModel
                {
                    Id = result.DepartmentId,
                    Title = result.Title,
                };
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error Retrieving Department data");
            }
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutDepartment(int id, UpdateDepartmentRequestWebModel model)
        {
            try
            {
                var result = await _context.Departments.FindAsync(id);
                if (result==null) { return NotFound($"Department with id:{id} does not exist"); }
                result.Title = model.Title;
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error Retrieving Department data");
            }
        }
        [HttpPost]
        public async Task<ActionResult<Department>> PostDepartment(CreateDepartmentRequestWebModel model)
        {
            try
            {
                var result = new Department
                {
                    Title = model.Title,
                };
                _context.Departments.Add(result);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetDepartment), new { id = result.DepartmentId }, null);
            }
            catch(Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Unable to post the department data");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                var dataToDelete = await _context.Departments.FindAsync(id);
                if (dataToDelete == null) { return NotFound($"The Department id:{id} does not exist"); }
                _context.Departments.Remove(dataToDelete);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Error deleting department");
            }
        }
    }
}
