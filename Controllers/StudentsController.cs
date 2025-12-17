using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Data;
using StudentPortal.Models;
using StudentPortal.Models.Entities;

namespace StudentPortal.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        public StudentsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddStudentViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid data submitted" });
            }
            
            var student = new Models.Entities.Student
            {
                Name = viewModel.Name,
                Email = viewModel.Email,
                Phone = viewModel.Phone
            };

            await dbContext.Students.AddAsync(student);
            await dbContext.SaveChangesAsync();

            return Json(new { success = true, message = "Student added successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
           var students = await dbContext.Students.ToListAsync();

            return View(students);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var student = await dbContext.Students.FindAsync(id);

            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Student viewModel)
        {
            var student = await dbContext.Students.FindAsync(viewModel.Id);
            
            if(student is not null)
            {
                student.Name = viewModel.Name;
                student.Email = viewModel.Email;
                student.Phone = viewModel.Phone;

                await dbContext.SaveChangesAsync();
            }
            // return Json(new { success = true, message = "Student updated successfully" });

            return RedirectToAction("List" , "Students");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var student = await dbContext.Students.FindAsync(Id);

            if (student is  null)
            {
                return Json(new { success = false, message = "Student not found" });
            }

            dbContext.Students.Remove(student);
            await dbContext.SaveChangesAsync();

            // return RedirectToAction("List", "Students");
            return Json(new { success = true, message = "Student deleted successfully" });
        }


    }
}
