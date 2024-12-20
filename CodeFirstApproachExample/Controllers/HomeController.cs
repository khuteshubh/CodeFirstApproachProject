using CodeFirstApproachExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace CodeFirstApproachExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly EmployeeDBContext employeeDB;

        //private readonly ILogger<HomeController> _logger;

        // public HomeController(ILogger<HomeController> logger)
        // {
        //logger = logger;
        //}

           


        public HomeController(EmployeeDBContext employeeDB)
        {
            this.employeeDB = employeeDB;
        }

        public async Task<IActionResult> Index()
        {
            var empData = await employeeDB.Employees.ToListAsync();
            return View(empData);
        }

        //create method for creating data



        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Employee emp)
        {
            if(ModelState.IsValid)
            {
                await employeeDB.Employees.AddRangeAsync(emp);
                await employeeDB.SaveChangesAsync();
                return RedirectToAction("Index","Home");
            }
            return View(emp);

        }

        //Edit method
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var empData = await employeeDB.Employees.FindAsync(id);
            if(empData == null)
            {
                return NotFound();
            }

            return View(empData);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int? id,Employee editEnployee)
        {
            if(id != editEnployee.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                //If update the employee data in the database
                employeeDB.Entry(editEnployee).State = EntityState.Modified;
                await employeeDB.SaveChangesAsync();

                //redirect to the details action 
                return RedirectToAction("Index", "Home");

            }
            return View(editEnployee);
        }

        //Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || employeeDB.Employees == null)
            {
                return NotFound();  


            }

           var empData =  await employeeDB.Employees.FirstOrDefaultAsync(a => a.Id == id);
            if(empData == null)
            {
                return NotFound();
            }
            return View(empData);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || employeeDB.Employees == null)
            {
                return NotFound();


            }

            var empData = await employeeDB.Employees.FindAsync(id);
            if (empData == null)
            {
                return NotFound();
            }
            return View(empData);
        }

        
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, Employee editEnployee)
        {
            if (id == null)
            {
                return NotFound(); 
            }

           
            var empData = await employeeDB.Employees.FindAsync(id);
            if (empData == null)
            {
                return NotFound();  
            }

            
            employeeDB.Employees.Remove(empData);
            await employeeDB.SaveChangesAsync();

            // Redirect to the Index action after successful deletion
            return RedirectToAction("Index", "Home");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}