using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace History_VAR.Classes
{
    class Teacher
    {
        //Teacher ID, Name and Schoolname
        private int TeacherID;
        private string TeacherName;
        private string SchoolName;

        /// <summary>
        /// Empty constructor
        /// </summary>
        public Teacher()
        {
            //Does nothing
        }

        /// <summary>
        /// Constructor that receives a name
        /// </summary>
        /// <param name="name"></param>
        public Teacher(string name)
        {
            //Set teacher name (string)
            this.TeacherName = name;
        }

        /// <summary>
        /// Get name of the teacher
        /// </summary>
        /// <returns>string Teacher Name</returns>
        public string GetTeacherName()
        {
            //Returns name of the teacher
            return this.TeacherName;
        }

        /// <summary>
        /// Get and set Teacher ID
        /// </summary>
        public int TeacherIdentification
        {
            get
            {
                //Get Teacher ID
                return this.TeacherID;
            }
            set
            {
                //Set teacher ID
                this.TeacherID = value;
            }
        }
    }
}
