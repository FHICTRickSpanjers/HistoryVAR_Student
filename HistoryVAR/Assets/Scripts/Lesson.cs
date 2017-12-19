using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace History_VAR.Classes
{
    class Lesson
    {
        //Lesson subjects, ID, Name, Status, Description, TeacherID, List of Artobjects
        private string Lesson_subject;
        private int LessonID;
        private string Lesson_name;
        private string Lesson_status;
        private string Lesson_desc;
        private int Teacher_ID;
        private List<ArtObject> ListofArtObjects = new List<ArtObject>();
        

        /// <summary>
        /// Constructor that creates a new lesson
        /// </summary>
        /// <param name="LName">Name of the lesson</param>
        /// <param name="LessonID">ID of the lesson</param>
        /// <param name="lessonstatus">Status of the lesson</param>
        /// <param name="lessondesc">Descripton of the lesson</param>
        /// <param name="lessonsubject">Subject of the lesson</param>
        /// <param name="TID">Teacher ID of the lesson</param>
        public Lesson(string LName, int LessonID, string lessonstatus, string lessondesc, string lessonsubject, int TID)
        {
            //Setting class data
            this.Lesson_name = LName;
            this.LessonID = LessonID;
            this.Lesson_status = lessonstatus;
            this.Lesson_desc = lessondesc;
            this.Lesson_subject = lessonsubject;
            this.Teacher_ID = TID;
        }

        /// <summary>
        /// Constructor that just needs the ID
        /// </summary>
        /// <param name="ID">ID of the lesson</param>
        public Lesson(int ID)
        {
            //Set int LessonID
            LessonID = ID;
        }

        /// <summary>
        /// Empty constructor for Lesson
        /// </summary>
        public Lesson()
        {
            //Does nothing
        }


        /// <summary>
        /// Gets and sets the lesson status
        /// </summary>
        public string LessonStatus
        {
            get
            {  
                //Return Lesson status
                return this.Lesson_status;
            }
            set
            {
                //Set lesson status
                this.Lesson_status = value;
            }
        }

        /// <summary>
        /// Get the name of the lesson
        /// </summary>
        /// <returns>Return a string of the lesson name</returns>
        public string GetLessonName()
        {
            //Return string lesson name
            return Lesson_name;
        }

        /// <summary>
        /// Get the ID of the lesson
        /// </summary>
        /// <returns>Int Lesson ID</returns>
        public int GetLessonID()
        {
            //Return ID of lesson
            return LessonID;
        }

        /// <summary>
        /// Get description of the lesson
        /// </summary>
        /// <returns>Return the lesson description as string</returns>
        public string GetLessonDesc()
        {
            ///Return the lesson description
            return Lesson_desc;
        }

        /// <summary>
        /// Get the lesson subject
        /// </summary>
        /// <returns>Return lesson subject (string)</returns>
        public string GetLessonSubject()
        {
            //Return lesson subject
            return Lesson_subject;
        }

        /// <summary>
        /// Get the ID of the teacher
        /// </summary>
        /// <returns>Teacher ID as INT</returns>
        public int GetTeacherID()
        {
            //Return Teacher ID (int)
            return this.Teacher_ID;
        }

    }
}
