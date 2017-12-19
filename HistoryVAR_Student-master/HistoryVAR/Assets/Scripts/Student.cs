using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace History_VAR.Classes
{
    class Student
    {
        //Name, StudentID, SchoolID
        private string StudentName;
        private int StudentID;
        private int SchoolID;

        /// <summary>
        /// Constructor for student that gets the ID, Name and School ID
        /// </summary>
        /// <param name="stuID">Student ID (int)</param>
        /// <param name="StuName">Student name (string)</param>
        /// <param name="SchoID">School ID (int)</param>
        public Student(int stuID, string StuName, int SchoID)
        {
            //Setting the data
            this.StudentID = stuID;
            this.StudentName = StuName;
            this.SchoolID = SchoID;
        }

        /// <summary>
        /// Get the name of the student
        /// </summary>
        /// <returns>Return string student name</returns>
        public string GetStudentName()
        {
            //return string studentname
            return StudentName;
        }

        /// <summary>
        /// Get the student ID
        /// </summary>
        /// <returns>int student ID</returns>
        public int GetStudentID()
        {
            //return int studentid
            return StudentID;
        }
     
    }
}
