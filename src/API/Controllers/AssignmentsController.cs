using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDo.Data;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssignmentsController : ControllerBase
    {
        private readonly ToDoContext _context;
        public AssignmentsController(ToDoContext context) => _context = context;

        [HttpGet]
        public async Task<ActionResult<List<Assignment>>> Get()
        {
            var assigments = await _context.Assignments.ToListAsync();

            return assigments.Count==0 ? NotFound("There are no assignments") : (ActionResult<List<Assignment>>)Ok(assigments);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Assignment>> Get(int id)
        {
            var assignment = await _context.Assignments.FirstOrDefaultAsync(a => a.Id == id);

            if (assignment == null)
            {
                return NotFound($"Id {id} assignment does not exist");
            }
            return Ok(assignment);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Assignment assignment)
        {
            await _context.AddAsync(assignment);
            await _context.SaveChangesAsync();
            return Ok(assignment);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Assignment assignment)
        {
            Assignment dbAssignment = await _context.Assignments.FindAsync(assignment.Id);
            if (dbAssignment == default(Assignment)) return NotFound();


            _context.Entry(dbAssignment).CurrentValues.SetValues(assignment);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
