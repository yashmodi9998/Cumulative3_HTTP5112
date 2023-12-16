using Cumulative_HTTP5112.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cumulative_HTTP5112.Controllers
{
    public class TeacherDataController : ApiController
    {
        private DBContext DbCon = new DBContext();

        //This Controller Will access the teacher table of our school database.
        /// <summary>
        /// Returns a list of Teacher in the system
        /// </summary>
        /// <example>GET api/TeacherData/ListTeachers</example>
        /// <returns>
        /// A list of Teachers (first names and last names)
        /// </returns>
        [HttpGet]
        public IEnumerable<Teacher> ListTeacher(string SearchKey = null)
        {
            MySqlConnection Conn = DbCon.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();
            /// This query will find for searching data based on searchKey; 
            cmd.CommandText = "Select * from Teachers where lower(teacherfname) like lower(@key) or lower(teacherlname) like lower(@key) or lower(concat(teacherfname, ' ', teacherlname)) like lower(@key)";

            cmd.Parameters.AddWithValue("@key", "%" + SearchKey + "%");
            cmd.Prepare();

            MySqlDataReader ResultSet = cmd.ExecuteReader();
            List<Teacher> Teachers = new List<Teacher> { };

            while (ResultSet.Read())
            {
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string EmployeeNumber = ResultSet["employeenumber"].ToString();
                DateTime HireDate = (DateTime)ResultSet["hiredate"];
                string Salary = ResultSet["salary"].ToString();


                Teacher NewTeacher = new Teacher();
                NewTeacher.TeacherId = TeacherId;

                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;

                Teachers.Add(NewTeacher);
            }

            Conn.Close();

            return Teachers;
        }


        /// <summary>
        /// Finds Teacher in the system given an ID
        /// </summary>
        /// <param name="id">The teacher primary key</param>
        /// <returns>A Student object</returns>
        [HttpGet]
        public Teacher FindTeacher(int id)
        {
            Teacher NewTeacher = new Teacher();

            MySqlConnection Conn = DbCon.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "Select * from Teachers where teacherid = " + id;

            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                int TeacherId = (int)ResultSet["teacherid"];
                string TeacherFname = ResultSet["teacherfname"].ToString();
                string TeacherLname = ResultSet["teacherlname"].ToString();
                string EmployeeNumber = ResultSet["employeenumber"].ToString();
                DateTime HireDate = (DateTime)ResultSet["hiredate"];
                string Salary = ResultSet["salary"].ToString();

                NewTeacher.TeacherId = TeacherId;

                NewTeacher.TeacherFname = TeacherFname;
                NewTeacher.TeacherLname = TeacherLname;
                NewTeacher.EmployeeNumber = EmployeeNumber;
                NewTeacher.HireDate = HireDate;
                NewTeacher.Salary = Salary;


            }
            return NewTeacher;
        }
        [HttpPost]
        public void DeleteTeacher(int id)
        {
            MySqlConnection Conn = DbCon.AccessDatabase();

            // open connection
            Conn.Open();

            // creating command for query
            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "DELETE FROM classes WHERE classes.teacherid = @teacherID";

            // sanitizing data
            cmd.Parameters.AddWithValue("@teacherID", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            // close connection
            Conn.Close();

            Conn.Open();

            cmd.CommandText = "Delete from teachers where teacherid=@id";
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();


        }

        /// <summary>
        /// Adds an Teacher to the MySQL Database.
        /// </summary>
        /// <param name="NewTeacher">An object with fields that map to the columns of the teacher's table. Non-Deterministic.</param>
        /// <example>
        /// POST api/TeacherData/AddTeacher
        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"TeacherFname":"Christine",
        ///	"TeacherLname":"Bittle",
        ///	"EmployeeNumber":"T45",
        ///	"HireDate":"2014-06-17 00:00:00"
        ///	"Salary":"89.80"
        /// }
        /// </example>
        [HttpPost]
        ///    [EnableCors(origins: "*", methods: "*", headers: "*")]
        public void AddTeacher([FromBody] Teacher NewTeacher)
        {

            MySqlConnection Conn = DbCon.AccessDatabase();

            Conn.Open();

            MySqlCommand cmd = Conn.CreateCommand();

            cmd.CommandText = "insert into teachers (teacherfname, teacherlname, employeenumber, hiredate, salary) values (@TeacherFname,@TeacherLname,@EmployeeNumber,@HireDate, @Salary)";
            cmd.Parameters.AddWithValue("@TeacherFname", NewTeacher.TeacherFname);
            cmd.Parameters.AddWithValue("@TeacherLname", NewTeacher.TeacherLname);
            cmd.Parameters.AddWithValue("@EmployeeNumber", NewTeacher.EmployeeNumber);
            cmd.Parameters.AddWithValue("@HireDate", NewTeacher.HireDate);
            cmd.Parameters.AddWithValue("@Salary", NewTeacher.Salary);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            Conn.Close();
        }


        /// <summary>
        /// Updates a School DB.
        /// </summary>
        /// <param name="teacherUpdate">An object with fields that map to the columns of the teacher's table.</param>
        /// <example>
        /// POST api/Teacher/UpdateTeacher/208 

        /// FORM DATA / POST DATA / REQUEST BODY 
        /// {
        ///	"TeacherFname":"Christine",
        ///	"TeacherLname":"Bittle",
        ///	"EmployeeNumber":"T45",
        ///	"HireDate":"2014-06-17 00:00:00"
        ///	"Salary":"89.80"
        /// }
        /// </example>
        [HttpPost]
        public int UpdateTeacher(int id, [FromBody] Teacher teacherUpdate)
        {
            // Server Side Validation
            try
            {
                // Validate input parameters
                if (string.IsNullOrWhiteSpace(teacherUpdate.TeacherFname) ||
                    string.IsNullOrWhiteSpace(teacherUpdate.TeacherLname) ||
                    string.IsNullOrWhiteSpace(teacherUpdate.EmployeeNumber) ||
                    string.IsNullOrWhiteSpace(teacherUpdate.Salary))
                {
                    // Return 0 if any required field is missing
                    return 0;
                }
                MySqlConnection Conn = DbCon.AccessDatabase();

                Conn.Open();

                MySqlCommand cmd = Conn.CreateCommand();

                cmd.CommandText = "update teachers set teacherfname=@TeacherFname, teacherlname=@TeacherLname, employeenumber=@EmployeeNumber, salary=@Salary  where teacherid=@TeacherId";

                cmd.Parameters.AddWithValue("@TeacherFname", teacherUpdate.TeacherFname);
                cmd.Parameters.AddWithValue("@TeacherLname", teacherUpdate.TeacherLname);
                cmd.Parameters.AddWithValue("@EmployeeNumber", teacherUpdate.EmployeeNumber);
                cmd.Parameters.AddWithValue("@HireDate", teacherUpdate.HireDate);
                cmd.Parameters.AddWithValue("@Salary", teacherUpdate.Salary);

                cmd.Parameters.AddWithValue("@TeacherId", id);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                Conn.Close();

                // Return 1 when the operation was successful
                return 1;
            }
            catch (Exception)
            {
                // Return -1 when an error occurs
                return -1;
            }
        }

    }
}
