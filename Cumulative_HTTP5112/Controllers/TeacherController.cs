using Cumulative_HTTP5112.Models;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cumulative_HTTP5112.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }
        //GET : /Teacher/List
        public ActionResult List(string SearchKey = null)
        {
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> Teachers = controller.ListTeacher(SearchKey);
           
            return View(Teachers);
        }

        //GET : /Teacher/Show/{id}
        public ActionResult Show(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.FindTeacher(id);


            return View(NewTeacher);
        }
        //GET : /Teacher/DeleteData/{id}
        public ActionResult DeleteData(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher NewTeacher = controller.FindTeacher(id);

            return View(NewTeacher);
        }
        public ActionResult Delete(int id) {
            TeacherDataController controller = new TeacherDataController();
             controller.DeleteTeacher(id);

            return RedirectToAction("List");
        }

        //GET : /Teacher/New
        public ActionResult New()
        {
            return View();
        }

  

        //POST : /Teacher/Create    
        [HttpPost]
        public ActionResult Create(string TeacherFname, string TeacherLname, string EmployeeNumber, DateTime HireDate, string Salary)
        {


            Debug.WriteLine("I have accessed the Create Method!");
            Debug.WriteLine(TeacherFname);
            Debug.WriteLine(TeacherLname);
            Debug.WriteLine(EmployeeNumber);

            Teacher NewTeacher = new Teacher();
            NewTeacher.TeacherFname = TeacherFname;
            NewTeacher.TeacherLname = TeacherLname;
            NewTeacher.EmployeeNumber = EmployeeNumber;
            NewTeacher.HireDate = HireDate;
            NewTeacher.Salary = Salary;

            TeacherDataController controller = new TeacherDataController();
            controller.AddTeacher(NewTeacher);

            return RedirectToAction("List");
        }

    

    /// <summary>
    ///  "Teacher Update" Page..
    /// </summary>
    /// <param name="id">Id of the Teacher</param>
    /// <returns>A dynamic "Update Teacher" webpage which provides the current information of the teacher and asks the user for new information as part of a form.</returns>
    /// <example>GET : /Teacher/Update/5</example>
    public ActionResult Update(int id)
    {
        TeacherDataController controller = new TeacherDataController();
        Teacher SelectedTeacher = controller.FindTeacher(id);

        return View(SelectedTeacher);
    }

        /// <summary>
        /// Receives a POST request containing information about an existing teacher in the system, with new values. Conveys this information to the API, and redirects to the "Teachers Show" .
        /// </summary>
        /// <param name="id">Id of the ID to update</param>
        /// <param name=" TeacherFname">The updated first name of the teacher</param>
        /// <param name="TeacherLname">The updated last name of the teacher</param>
        /// <param name="EmployeeNumber">The updated EmployeeID of teacher.</param>
        /// <param name="HireDate">The updated hiring date of teacher.</param>
        /// <param name="Salary">The updated salaryof teacher.</param>
        /// <returns>A dynamic webpage which provides the current information of the Teacher.</returns>
        /// <example>
        /// POST : /Teacher/Update/10
        /// FORM DATA / POST DATA / REQUEST BODY 
        ///	"TeacherFname":"Christine",
        ///	"TeacherLname":"Bittle",
        ///	"EmployeeNumber":"T45",
        ///	"HireDate":"2014-06-17 00:00:00"
        ///	"Salary":"89.80"
        /// }
        /// </example>
        //  POST : /Teacher/Update/{id}
        [HttpPost]
        public ActionResult Update(int id,string TeacherFname, string TeacherLname, string EmployeeNumber, DateTime HireDate, string Salary)
        {
            Teacher techerUpdate = new Teacher();
            techerUpdate.TeacherFname = TeacherFname;
            techerUpdate.TeacherLname = TeacherLname;
            techerUpdate.EmployeeNumber = EmployeeNumber;
            techerUpdate.HireDate = HireDate;
            techerUpdate.Salary = Salary;

            TeacherDataController controller = new TeacherDataController();
      
            int result = controller.UpdateTeacher(id, techerUpdate);
            if (result == 0)
            {
                // Validation failed: Please check the input data.
                ViewBag.Result = 0;
            }
            else if (result == -1)
            {
                // An error occurred during validation.
                ViewBag.Result = -1;
            }
            return RedirectToAction("Show/" + id);
        }

    }
}