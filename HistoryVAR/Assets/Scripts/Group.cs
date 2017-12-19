using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace History_VAR.Classes
{
    class Group
    {
        //Group ID, Name, List of teachers and List of students
        private int GroupID;
        private string GroupName;
        private List<Teacher> ListofTeachers = new List<Teacher>();
        private List<Student> ListofStudents = new List<Student>();
        private string schoolName;

        //Empty constructor for Group
        public Group()
        {
            //Does nothing
        }

        /// <summary>
        /// Constructor that receives the group name
        /// </summary>
        /// <param name="name">string groupname</param>
        public Group(string name)
        {
            this.GroupName = name;
        }

        /// <summary>
        /// Constructor that receives the GroupID
        /// </summary>
        /// <param name="GroupID">ID of the group</param>
        public Group(int GroupID)
        {
            this.GroupID = GroupID;
        }

        /// <summary>
        /// Constructor that receives name and ID
        /// </summary>
        /// <param name="GroupID">ID of Group</param>
        /// <param name="GroupName">Name of Group</param>
        public Group(int GroupID, string GroupName)
        {
            this.GroupID = GroupID;
            this.GroupName = GroupName;
        }

        /// <summary>
        /// Get the name of the group
        /// </summary>
        /// <returns>string groupname</returns>
        public string GetGroupName()
        {
            return this.GroupName;
        }

        /// <summary>
        /// Get students from group
        /// </summary>
        /// <returns>List of students</returns>
        public List<Student> getStudents()
        {
            return ListofStudents;
        }

        /// <summary>
        /// Get teachers from group
        /// </summary>
        /// <returns>List of teachers</returns>
        public List<Teacher> getTeachers()
        {
            return ListofTeachers;
        }
        
        /// <summary>
        /// Get ID from Group
        /// </summary>
        /// <returns>Returns a group ID (int)</returns>
        public int Get_Group_ID()
        {
            return GroupID;
        }


        /// <summary>
        /// Set students into a group
        /// </summary>
        public List<Student> StudentsInGroup
        {
            get{
               //Get the list of students
               return this.ListofStudents;
            }
            set{
                //Set the list of students (In the group)
                this.ListofStudents = value;
            }
        }


    }
}
