using Assignment.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Services.ViewModels;
using System.Diagnostics;

namespace Assignment.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IStudentServices studentServices;

        public HomeController(ILogger<HomeController> logger , IStudentServices studentServices)
        {
            _logger = logger;
            this.studentServices = studentServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> StudentData(DataViewModel obj)
        {
            DataViewModel dataObj = await studentServices.GetStudentData(obj);
            return PartialView("_StudentTable", dataObj);
        }

        public async Task DeleteStudent(int studentId)
        {
            await studentServices.DeleteStudent(studentId);
        }

        public IActionResult StudentForm(StudentForm obj)
        {
            obj.flag = 1;
            return PartialView("_StudentForm" , obj);
        }

        public async Task<IActionResult> EditStudentForm(StudentForm obj)
        {
            StudentForm dataObj = await studentServices.EditStudentFormData(obj);
            return PartialView("_StudentForm", dataObj);
        }

        public async Task AddNewStudent(StudentForm obj)
        {
            await studentServices.AddNewStudent(obj);
        }

        public async Task EditExistingStudent(StudentForm obj)
        {
            await studentServices.EditExistingStudent(obj);
        }

        public async Task<bool> ValidateCourse(string course)
        {
            return await studentServices.ValidateCourse(course);
        }
    }
}