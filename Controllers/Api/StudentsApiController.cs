using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models.Entities;

namespace StudentPortal.Controllers.Api
{
    [ApiController]
    [Route("api/students")]
    public class StudentsApiController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        public StudentsApiController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var students = await dbContext.Students.ToListAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(Guid id)
        {
            var student = await dbContext.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound(new { message = "Student not found" });
            }
            return Ok(student);
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(Student student)
        {
            await dbContext.Students.AddAsync(student);
            await dbContext.SaveChangesAsync();
            return Ok(new { message = "Student created successfully", student });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(Guid id, Student updatedStudent)
        {
            var student = await dbContext.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound(new { message = "Student not found" });
            }

            student.Name = updatedStudent.Name;
            student.Email = updatedStudent.Email;
            student.Phone = updatedStudent.Phone;

            dbContext.Students.Update(student);
            await dbContext.SaveChangesAsync();

            return Ok(new { message = "Student updated successfully", student });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(Guid id)
        {
            var student = await dbContext.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound(new { message = "Student not found" });
            }

            dbContext.Students.Remove(student);
            await dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}