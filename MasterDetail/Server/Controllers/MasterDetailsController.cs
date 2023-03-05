using MasterDetail.Server.Models;
using MasterDetail.Server.Pages;
using MasterDetail.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;

namespace MasterDetail.Server.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
  
    public class MasterDetailsController : ControllerBase
    {
        private readonly EmployeeDbContext _context;
        private readonly IWebHostEnvironment _env;

        public MasterDetailsController(EmployeeDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            this._env = env;
        }

        [HttpGet]
        [Route("GetDepartments")]
        public async Task<ActionResult<IEnumerable<Department>>> GetDepartments()
        {
            return await _context.Departments.ToListAsync();
        }

        [HttpGet]
        [Route("GetEmployees")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.Include(c => c.BookingEntries).ThenInclude(b => b.Department).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<Employee> GetEmployeeById(int id)
        {
            return await _context.Employees.Where(x => x.EmployeeId == id).Include(c => c.BookingEntries).ThenInclude(b => b.Department).FirstOrDefaultAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm]EmployeeVM employeeVM)
        {

            if (ModelState.IsValid)
            {
                Employee employee = new Employee()
                {
                    EmployeeName = employeeVM.EmployeeName,
                    BirthDate = employeeVM.BirthDate,
                    Phone = employeeVM.Phone,
                    MaritialStatus = employeeVM.MaritialStatus
                };

                //Image
                if (employeeVM.PictureFile is not null)
                {
                    string webroot = _env.WebRootPath;
                    string folder = "Images";
                    string imgFileName = Guid.NewGuid().ToString() + Path.GetExtension(employeeVM.PictureFile.FileName);
                    string fileToWrite = Path.Combine(webroot, folder, imgFileName);

                    using (var stream = new FileStream(fileToWrite, FileMode.Create))
                    {
                        await employeeVM.PictureFile.CopyToAsync(stream);
                        employee.Picture = imgFileName;
                    }
                }
                else
                {
                    employee.Picture = "default.jpg";
                }
                _context.Employees.Add(employee);

                if (employeeVM.DepartmentList.Count() > 0)
                {
                    foreach (Department department in employeeVM.DepartmentList)
                    {
                        _context.BookingEntries.Add(new BookingEntry
                        {
                            Employee = employee,
                            EmployeeId = employee.EmployeeId,
                            DepartmentId = department.DepartmentId
                        });
                    }
                }


                await _context.SaveChangesAsync();
                return Ok(employee);
            }
            return BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] EmployeeVM employeeVM)
        {

            if (ModelState.IsValid)
            {
                Employee employee = _context.Employees.Find(employeeVM.EmployeeId);
                employee.EmployeeName = employeeVM.EmployeeName;
                employee.BirthDate = employeeVM.BirthDate;
                employee.Phone = employeeVM.Phone;
                employee.MaritialStatus = employeeVM.MaritialStatus;

                //Image
                if (employeeVM.PictureFile is not null)
                {
                    string webroot = _env.WebRootPath;
                    string folder = "Images";
                    string imgFileName = Guid.NewGuid().ToString() + Path.GetExtension(employeeVM.PictureFile.FileName);
                    string fileToWrite = Path.Combine(webroot, folder, imgFileName);

                    using (var stream = new FileStream(fileToWrite, FileMode.Create))
                    {
                        await employeeVM.PictureFile.CopyToAsync(stream);
                        employee.Picture = imgFileName;
                    }
                }

                var existsBookings = _context.BookingEntries.Where(x => x.EmployeeId == employeeVM.EmployeeId).ToList();
                if (existsBookings is not null)
                {
                    foreach (var entry in existsBookings)
                    {
                        _context.Remove(entry);
                    }
                }
                

                if (employeeVM.DepartmentList.Count() > 0)
                {
                    foreach (Department department in employeeVM.DepartmentList)
                    {
                        _context.BookingEntries.Add(new BookingEntry
                        {
                            EmployeeId = employee.EmployeeId,
                            DepartmentId = department.DepartmentId
                        });
                    }
                }

                _context.Entry(employee).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(employee);
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            Employee employee = _context.Employees.Find(id);

            var existsBookings = _context.BookingEntries.Where(x => x.EmployeeId == employee.EmployeeId).ToList();
            if (existsBookings is not null)
            {
                foreach (var entry in existsBookings)
                {
                    _context.Remove(entry);
                }
            }
            _context.Remove(employee);
            try
            {
                await _context.SaveChangesAsync();

                return new OkObjectResult(employee);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }



    }
}
