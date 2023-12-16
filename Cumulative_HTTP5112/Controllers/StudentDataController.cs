using Cumulative_HTTP5112.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Cumulative_HTTP5112.Controllers
{
    public class StudentDataController : ApiController

    {
        private DBContext DbCon = new DBContext();

        //This Controller Will access the Student table of our school database.
        /// <summary>
        /// Returns a list of Student in the system
        /// </summary>
        /// <example>GET api/StudentData/ListStudents</example>
        /// <returns>
        /// A list of Students (first names and last names)
        /// </returns>
        [HttpGet]
        public IEnumerable<Students> ListStudent()
        {
            //Create an instance of a connection
            MySqlConnection Conn = DbCon.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from students";

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            //Create an empty list of Students
            List<Students> StudentList = new List<Students> { };

            //Loop Through Each Row the Result Set
            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                string StudentId = ResultSet["studentid"].ToString();
                string StudentFname = ResultSet["studentfname"].ToString();
                string StudentLname = ResultSet["studentlname"].ToString();
                string StudentNumber = ResultSet["studentnumber"].ToString();
                string EnrolDate = ResultSet["enroldate"].ToString();
               

                Students NewStudent = new Students();
                NewStudent.StudentId = StudentId;

                NewStudent.StudentFname = StudentFname;
                NewStudent.StudentLname = StudentLname;
                NewStudent.StudentNumber = StudentNumber;
                NewStudent.EnrolDate = EnrolDate;

                StudentList.Add(NewStudent);
            }

            //Close the connection between the MySQL Database and the WebServer
            Conn.Close();

            //Return the final list of Student names
            return StudentList;
        }


        /// <summary>
        /// Finds a Student in the system given an ID
        /// </summary>
        /// <param name="id">The Student primary key</param>
        /// <returns>Student object</returns>
        [HttpGet]
        public Students FindStudent(int id)
        {
            Students NewStudent = new Students();

            //Create an instance of a connection
            MySqlConnection Conn = DbCon.AccessDatabase();

            //Open the connection between the web server and database
            Conn.Open();

            //Establish a new command (query) for our database
            MySqlCommand cmd = Conn.CreateCommand();

            //SQL QUERY
            cmd.CommandText = "Select * from students where studentid = " + id;

            //Gather Result Set of Query into a variable
            MySqlDataReader ResultSet = cmd.ExecuteReader();

            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index

                string StudentId = ResultSet["studentid"].ToString();
                string StudentFname = ResultSet["studentfname"].ToString();
                string StudentLname = ResultSet["studentlname"].ToString();
                string StudentNumber = ResultSet["studentnumber"].ToString();
                string EnrolDate = ResultSet["enroldate"].ToString();
               
                NewStudent.StudentId = StudentId;

                NewStudent.StudentFname = StudentFname;
                NewStudent.StudentLname = StudentLname;
                NewStudent.StudentNumber = StudentNumber;
                NewStudent.EnrolDate = EnrolDate;


            }


            return NewStudent;
        }

    }
}
