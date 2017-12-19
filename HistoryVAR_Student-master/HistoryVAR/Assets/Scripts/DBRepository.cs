using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace History_VAR.Classes
{
    class DBRepository
    {
        enum Users
        {
            Teacher,
            Student
        };

        /// <summary>
        /// //Instance will start at null (This makes sure that nothing is running etc)
        /// </summary>
        private static DBRepository instance = null;

        /// <summary>
        /// Simple empty constructor
        /// </summary>
        private DBRepository()
        {

        }

        /// <summary>
        /// This creates an instance of the DBRepository class if none exists already
        /// </summary>
        /// <returns></returns>
        public static DBRepository GetInstance()
        {
            if (instance == null)
            {
                instance = new DBRepository();
            }
            return instance;
        }



        /// <summary>
        /// Get login data for Teacher User
        /// </summary>
        /// <param name="username">Username as string</param>
        /// <returns>Password</returns>
        public bool FindLoginData(string username, string password, string user)
        {
            bool Result = false;

            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    string query = "";

                    if(user == "Teacher")
                    {
                         query = "SELECT * FROM Teacher WHERE Username = @username AND Password = @password";
                    }
                    else if(user == "Student"){

                        query = "SELECT * FROM Student WHERE Username = @username AND Password = @password";
                    }
                    
                    SqlCommand cmd = new SqlCommand(query, cnn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);
                    cmd.Parameters.AddWithValue("@user", user);

                    cmd.CommandType = CommandType.Text;
                    cnn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Result = true;
                        }
                    }

                    cnn.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return Result;
        }


      
        /// <summary>
        /// Create new lessons (teachers)
        /// </summary>
        /// <param name="lesson_subject">string lesson subject</param>
        /// <param name="teacherid">Id of the teacher</param>
        /// <param name="lesson_name">Name of the lesson</param>
        /// <param name="lesson_status">Status of the lesson</param>
        /// <param name="lesson_desc">Description of the lesson</param>
        public void CreateNewLesson(Lesson L)
        {
            try
            {

                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    if (cnn.State == ConnectionState.Closed)
                    {
                        cnn.Open();
                    }

                    SqlCommand NewCmd = cnn.CreateCommand();
                    NewCmd.Connection = cnn;
                    NewCmd.CommandType = CommandType.Text;
                    NewCmd.CommandText = "INSERT INTO Lesson (Lesson_subject, Teacher_ID, Lesson_Name, Lesson_Status, Lesson_Description) VALUES (@Lesson_Subject, @Teacher_ID, @Lesson_Name, @Lesson_Status, @Lesson_Description)";

                    NewCmd.Parameters.AddWithValue("@Lesson_Subject", L.GetLessonSubject());
                    NewCmd.Parameters.AddWithValue("@Teacher_ID", L.GetTeacherID());
                    NewCmd.Parameters.AddWithValue("@Lesson_Name", L.GetLessonName());
                    NewCmd.Parameters.AddWithValue("@Lesson_Status", L.LessonStatus);
                    NewCmd.Parameters.AddWithValue("@Lesson_Description", L.GetLessonDesc());

                    NewCmd.ExecuteNonQuery();

                    cnn.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        //Insert Artobjects per lesson
        /// <summary>
        /// Insert Artobjects into lesson
        /// </summary>
        /// <param name="lessonid">ID of the lesson</param>
        /// <param name="artobjid">ID of the art object</param>
        public void InsertArtobjectsInLessons(int lessonid, int artobjid)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    if (cnn.State == ConnectionState.Closed)
                    {
                        cnn.Open();
                    }
                    SqlCommand NewCmd = cnn.CreateCommand();
                    NewCmd.Connection = cnn;
                    NewCmd.CommandType = CommandType.Text;
                    NewCmd.CommandText = "INSERT INTO LessonObj (Lesson_ID, Art_ID) VALUES (@Lesson_ID, @Art_ID)";

                    NewCmd.Parameters.AddWithValue("@Lesson_ID", lessonid);
                    NewCmd.Parameters.AddWithValue("@Art_ID", artobjid);
                    NewCmd.ExecuteNonQuery();

                    cnn.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        /// <summary>
        /// Select all the lessons
        /// </summary>
        /// <returns>return a string of lessons</returns>
        public List<Lesson> FindLessonsData()
        {
            List<Lesson> ListOfLessons = new List<Lesson>();

            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    string query = "SELECT * FROM Lesson";
                    SqlCommand cmd = new SqlCommand(query, cnn);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            int LessonID = Convert.ToInt32(dr["Lesson_ID"]);
                            string LessonName = dr["Lesson_name"].ToString();
                            int TeacherID = Convert.ToInt32(dr["Teacher_ID"]);
                            string LessonStatus = dr["Lesson_status"].ToString();
                            string LessonDesc = dr["Lesson_description"].ToString();
                            string LessonSubject = dr["Lesson_subject"].ToString();

                            Lesson L = new Lesson(LessonName, LessonID, LessonStatus, LessonDesc, LessonSubject, TeacherID);
                            ListOfLessons.Add(L);
                        }
                    }

                    cnn.Close();
                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }

            return ListOfLessons;
        }


        //Get All Artobjects
        public List<ArtObject> FindArtData()
        {
            List<ArtObject> ListOfArt = new List<ArtObject>();

            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    string query = "SELECT * FROM ArtObj";
                    SqlCommand cmd = new SqlCommand(query, cnn);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            int ArtID = Convert.ToInt32(dr["Art_ID"]);
                            string Art_title = dr["Art_title"].ToString();
                            string Art_type = dr["Art_type"].ToString();
                            string Art_genre = dr["Art_genre"].ToString();
                            int Art_year = Convert.ToInt32(dr["Art_year"]);
                            string Art_period = dr["Art_period"].ToString();
                            string Art_city = dr["Art_city"].ToString();
                            string Art_country = dr["Art_country"].ToString();
                            decimal Art_width = Convert.ToDecimal(dr["Art_width"]);
                            decimal Art_height = Convert.ToDecimal(dr["Art_height"]);
                            decimal Art_length = Convert.ToDecimal(dr["Art_length"]);
                            string Art_material = dr["Art_material"].ToString();
                            string Art_where_is_original_located = dr["Art_where_is_original_located"].ToString();
                            string Art_description = dr["Art_description"].ToString();

                            ArtObject A = new ArtObject(ArtID, Art_title, Art_type, Art_genre, Art_year,
                                Art_period, Art_city, Art_country, Art_width, Art_height, Art_length, Art_material, 
                                Art_where_is_original_located, Art_description);

                            ListOfArt.Add(A);
                        }
                    }

                    cnn.Close();
                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }

            return ListOfArt;
        }


        /// <summary>
        /// Get Lesson ID
        /// </summary>
        /// <param name="name">Name of the lesson</param>
        /// <returns>Integer with Lesson ID</returns>
        public int FindLessonIDByName(string name)
        {

            int LessonID = 0;

            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    string query = "SELECT Lesson_ID FROM Lesson WHERE Lesson_name = @Lesson_Name";
                    SqlCommand cmd = new SqlCommand(query, cnn);
                    cmd.Parameters.AddWithValue("@Lesson_Name", name);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Lesson L = new Lesson(Convert.ToInt32(dr["Lesson_ID"]));
                            LessonID = L.GetLessonID();
                        }
                    }

                    cnn.Close();
                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }

            return LessonID;
        }


        /// <summary>
        /// Delete lesson by ID
        /// </summary>
        /// <param name="Lesson_ID"></param>
        public void DeleteLessonByID(int Lesson_ID)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    string query = "DELETE FROM Lesson WHERE Lesson_ID = @Lesson_ID";
                    SqlCommand cmd = new SqlCommand(query, cnn);

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Lesson_ID", Lesson_ID);
                    cnn.Open();

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        //Delete the lesson for the groups that have it
        public void DeleteLessonAtGroupsByID(int Lesson_ID)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    string query = "DELETE FROM LessonGroups WHERE Lesson_ID = @Lesson_ID";
                    SqlCommand cmd = new SqlCommand(query, cnn);

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Lesson_ID", Lesson_ID);
                    cnn.Open();

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        /// <summary>
        /// Get current lesson status
        /// </summary>
        /// <param name="lessonid">ID of the lesson</param>
        /// <returns>Lesson object</returns>
        public Lesson FindLessonByID(int lessonid)
        {
            Lesson Les = new Lesson();
            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    string query = "SELECT * FROM Lesson";
                    SqlCommand cmd = new SqlCommand(query, cnn);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Les.LessonStatus =  (dr["Lesson_status"].ToString());
                        }
                    }

                    cnn.Close();
                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }

            return Les;
        }


        /// <summary>
        /// Update a lesson
        /// </summary>
        /// <param name="Lesson_ID">ID of the lesson</param>
        /// <param name="status">status of the lesson</param>
        public void UpdateLessonStatusByID(int Lesson_ID, string status)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    string query = "UPDATE Lesson SET Lesson_status=@Lesson_status WHERE Lesson_ID = @Lesson_ID";

                    SqlCommand cmd = new SqlCommand(query, cnn);

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Lesson_ID", Lesson_ID);
                    cmd.Parameters.AddWithValue("@Lesson_status", status);

                    cnn.Open();

                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


        }


        
        /// <summary>
        /// Select necessary data to update
        /// </summary>
        /// <param name="Lesson_ID">ID of the lesson</param>
        /// <returns></returns>
        public Lesson GetLessonDataByID(int Lesson_ID)
        {
            Lesson L = null;

            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    string query = "SELECT * FROM Lesson WHERE Lesson_ID = @Lesson_ID";
                    SqlCommand cmd = new SqlCommand(query, cnn);

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Lesson_ID", Lesson_ID);
                    cnn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            int LessonID = Convert.ToInt32(dr["Lesson_ID"]);
                            string LessonName = dr["Lesson_name"].ToString();
                            int TeacherID = Convert.ToInt32(dr["Teacher_ID"]);
                            string LessonStatus = dr["Lesson_status"].ToString();
                            string LessonDesc = dr["Lesson_description"].ToString();
                            string LessonSubject = dr["Lesson_subject"].ToString();

                            L = new Lesson(LessonName, LessonID, LessonStatus, LessonDesc, LessonSubject, TeacherID);
                        }
                    }


                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }

            return L;
        }


     /// <summary>
     /// Update a lesson
     /// </summary>
     /// <param name="L">Lesson obj</param>
        public void UpdateLesson(Lesson L)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    if (cnn.State == ConnectionState.Closed)
                    {
                        cnn.Open();
                    }

                    SqlCommand NewCmd = cnn.CreateCommand();
                    NewCmd.Connection = cnn;
                    NewCmd.CommandType = CommandType.Text;
                    NewCmd.CommandText = "UPDATE Lesson SET Lesson_subject=@Lesson_Subject, Lesson_name=@name, Lesson_status=@Lesson_Status, Lesson_description=@Lesson_Description WHERE Lesson_ID = @Lesson_ID";

                    NewCmd.Parameters.AddWithValue("@Lesson_Subject", L.GetLessonSubject());
                    NewCmd.Parameters.AddWithValue("@name", L.GetLessonName());
                    NewCmd.Parameters.AddWithValue("@Lesson_Status", L.LessonStatus);
                    NewCmd.Parameters.AddWithValue("@Lesson_Description", L.GetLessonDesc());
                    NewCmd.Parameters.AddWithValue("@Lesson_ID", L.GetLessonID());
                    

                    NewCmd.ExecuteNonQuery();

                    cnn.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }




        //Select groups from database
        /// <summary>
        /// Select Group Data
        /// </summary>
        /// <returns>Object of class Group</returns>
        public List<Group> FindGroupData()
        {
            List<Group> ListOfGroups = new List<Group>();

            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    string query = "SELECT * FROM \"Group\"";
                    SqlCommand cmd = new SqlCommand(query, cnn);

                    cmd.CommandType = CommandType.Text;
                    cnn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Group G = new Group(Convert.ToInt32(dr["Group_ID"]), dr["Group_name"].ToString());
                            ListOfGroups.Add(G);
                        }
                    }

                    cnn.Close();
                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }

            return ListOfGroups;
        }



        //Get Teacher ID by name
        /// <summary>
        /// Find ID of Teacher
        /// </summary>
        /// <param name="name">Teacher name</param>
        /// <returns></returns>
        public Teacher FindTeacherID(string name)
        {
            Teacher T = new Teacher();

            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    string query = "SELECT Teacher_ID FROM Teacher WHERE Username = @username";
                    SqlCommand cmd = new SqlCommand(query, cnn);
                    cmd.Parameters.AddWithValue("@username", name);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {                     
                            T.TeacherIdentification = Convert.ToInt32(dr["Teacher_ID"]);
                        }
                    }

                    cnn.Close();
                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }

            return T;
        }


        /// <summary>
        /// Get Teacher Name
        /// </summary>
        /// <param name="ID">ID of teacher</param>
        /// <returns></returns>
        public Teacher FindTeacherName(int ID)
        {
            Teacher T = null;

            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    string query = "SELECT Username FROM Teacher WHERE Teacher_ID = @ID";
                    SqlCommand cmd = new SqlCommand(query, cnn);
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            T = new Teacher(dr["Username"].ToString());
                        }
                    }

                    cnn.Close();
                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }

            return T;
        }

        /// <summary>
        /// Get Group ID
        /// </summary>
        /// <param name="name">Name OF GROUP</param>
        /// <returns></returns>
        public Group FindGroupID(string name)
        {
            Group G = null;

            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    string query = "SELECT Group_ID FROM \"Group\" WHERE Group_name = @name";
                    SqlCommand cmd = new SqlCommand(query, cnn);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            int groupid = Convert.ToInt32(dr["Group_ID"]);
                            G = new Group(groupid);
                        }
                    }

                    cnn.Close();
                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }

            return G;
        }


        /// <summary>
        /// Insert lesson for groups
        /// </summary>
        /// <param name="Lesson_ID">Lesson ID</param>
        /// <param name="Group_ID">Group ID</param>
        public void InsertLessonForGroups(int Lesson_ID, int Group_ID)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    if (cnn.State == ConnectionState.Closed)
                    {
                        cnn.Open();
                    }

                    SqlCommand NewCmd = cnn.CreateCommand();
                    NewCmd.Connection = cnn;
                    NewCmd.CommandType = CommandType.Text;
                    NewCmd.CommandText = "INSERT INTO LessonGroups(Lesson_ID, Group_ID) VALUES (@Lesson_ID, @Group_ID)";

                    NewCmd.Parameters.AddWithValue("@Lesson_ID", Lesson_ID);
                    NewCmd.Parameters.AddWithValue("@Group_ID", Group_ID);
                    NewCmd.ExecuteNonQuery();

                    cnn.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }        
        }


        /// <summary>
        /// Get Group ID based on lesson
        /// </summary>
        /// <param name="ID">Lesson ID</param>
        /// <returns>Group OBJ</returns>
        public Group FindGroupIDBasedOnLessonID(int ID)
        {
            Group G = null;

            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    string query = "SELECT Group_ID FROM LessonGroups WHERE Lesson_ID = @ID";
                    SqlCommand cmd = new SqlCommand(query, cnn);
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            int groupid = Convert.ToInt32(dr["Group_ID"]);
                            G = new Group(groupid);
                        }
                    }

                    cnn.Close();
                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }

            return G;
        }

        /// <summary>
        /// Get Lesson based on Group ID
        /// </summary>
        /// <param name="ID">Group ID</param>
        /// <returns>List of lessons</returns>
        public List<Lesson> GetLessonsBasedOnGroupID(int ID)
        {
            List<Lesson> ListofLessons = new List<Lesson>();

            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    string query = "SELECT * FROM Lesson WHERE Lesson_ID IN(SELECT Lesson_ID FROM LessonGroups WHERE Group_ID = @ID)";
                    SqlCommand cmd = new SqlCommand(query, cnn);
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            int LessonID = Convert.ToInt32(dr["Lesson_ID"]);
                            string LessonName = dr["Lesson_name"].ToString();
                            int TeacherID = Convert.ToInt32(dr["Teacher_ID"]);
                            string LessonStatus = dr["Lesson_status"].ToString();
                            string LessonDesc = dr["Lesson_description"].ToString();
                            string LessonSubject = dr["Lesson_subject"].ToString();

                             Lesson L = new Lesson(LessonName, LessonID, LessonStatus, LessonDesc, LessonSubject, TeacherID);
                            ListofLessons.Add(L);
 
                        }
                    }

                    cnn.Close();
                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }

            return ListofLessons;
        }

        /// <summary>
        /// Get Group Data based on Group ID
        /// </summary>
        /// <param name="ID">Group ID</param>
        /// <returns>Group OBJ</returns>
        public Group FindGroupDataBasedOnGroupID(int ID)
        {
            Group G = null;

            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    string query = "SELECT * FROM \"Group\" WHERE Group_ID = @ID";
                    SqlCommand cmd = new SqlCommand(query, cnn);
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            int groupid = Convert.ToInt32(dr["Group_ID"]);
                            string groupname = dr["Group_name"].ToString();
                            G = new Group(groupid, groupname);
                        }
                    }

                    cnn.Close();
                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }

            return G;
        }


        /// <summary>
        /// Get info on students
        /// </summary>
        /// <returns>List of students</returns>
        public List<Student> FindStudentDetails()
        {

            List<Student> ListofStudents = new List<Student>();

            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    string query = "SELECT * FROM Student";
                    SqlCommand cmd = new SqlCommand(query, cnn);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            int StudentID = Convert.ToInt32(dr["Student_ID"]);
                            string StudentName = dr["Username"].ToString();
                            int SchoolID = Convert.ToInt32(dr["School_ID"]);

                            Student S = new Student(StudentID, StudentName, SchoolID);

                            ListofStudents.Add(S);
                        }
                    }

                    cnn.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return ListofStudents;
        }

        /// <summary>
        /// Get Group ID based on student
        /// </summary>
        /// <param name="ID">Student ID</param>
        /// <returns>Group OBJ</returns>
        public Group FindGroupIDBasedOnStudent(int ID)
        {
            Group G = null;

            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    string query = "SELECT Group_ID FROM StudentGroup WHERE Student_ID = @ID";
                    SqlCommand cmd = new SqlCommand(query, cnn);
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            int groupid = Convert.ToInt32(dr["Group_ID"]);
                            G = new Group(groupid);

                        }
                    }

                    cnn.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return G;
        }

        /// <summary>
        /// Insert image into Database
        /// </summary>
        /// <param name="Filename">Name of file as string</param>
        /// <param name="Image">Byte array image data</param>
        public void InsertImageInDB(string Filename, Byte[] Image)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    if (cnn.State == ConnectionState.Closed)
                    {
                        cnn.Open();
                    }

                    SqlCommand NewCmd = cnn.CreateCommand();
                    NewCmd.Connection = cnn;
                    NewCmd.CommandType = CommandType.Text;
                    NewCmd.CommandText = "INSERT INTO Image (Filename, Data) VALUES (@Filename, @Data)";

                    NewCmd.Parameters.AddWithValue("@Filename", Filename);
                    NewCmd.Parameters.AddWithValue("@Data", Image);
                    NewCmd.ExecuteNonQuery();

                    cnn.Close();
                }
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }

        /// <summary>
        /// Get images from DB
        /// </summary>
        /// <returns>List of image objects</returns>
        public List<ArtImage> ReceiveImagesFromDB()
        {
            List<ArtImage> ListofImages = new List<ArtImage>();

            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    string query = "SELECT * FROM Image";
                    SqlCommand cmd = new SqlCommand(query, cnn);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            int Img_ID = Convert.ToInt32(dr["IMG_ID"]);
                            byte[] DataArr = (byte[])dr["Data"];
                            string FileName = dr["FileName"].ToString();
                            ArtImage I = new ArtImage(Img_ID, FileName, DataArr);
                            ListofImages.Add(I);
                        }
                    }

                    cnn.Close();
                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }

            return ListofImages;
        }

        /// <summary>
        /// Insert image into lesson
        /// </summary>
        /// <param name="LessonID">Lesson ID</param>
        /// <param name="Img_ID">Image ID</param>
        public void InsertImageInLesson(int LessonID, int Img_ID)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    if (cnn.State == ConnectionState.Closed)
                    {
                        cnn.Open();
                    }

                    SqlCommand NewCmd = cnn.CreateCommand();
                    NewCmd.Connection = cnn;
                    NewCmd.CommandType = CommandType.Text;
                    NewCmd.CommandText = "INSERT INTO LessonImage (Lesson_ID, Image_ID) VALUES (@Lesson_ID, @Image_ID)";

                    NewCmd.Parameters.AddWithValue("@Lesson_ID", LessonID);
                    NewCmd.Parameters.AddWithValue("@Image_ID", Img_ID);
                    NewCmd.ExecuteNonQuery();

                    cnn.Close();
                }
            }
            catch (NullReferenceException e)
            {
                Console.Write(e.Message);
            }
        }


        //Delete the images that were set for the lesson
        /// <summary>
        /// Delete images from lesson
        /// </summary>
        /// <param name="Lesson_ID">Lesson ID</param>
        public void DeleteImageFromLessonByID(int Lesson_ID)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    string query = "DELETE FROM LessonImage WHERE Lesson_ID = @Lesson_ID";
                    SqlCommand cmd = new SqlCommand(query, cnn);

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Lesson_ID", Lesson_ID);
                    cnn.Open();

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Get images in lesson
        /// </summary>
        /// <param name="LessonID">Lesson ID</param>
        /// <returns>List of Image objects</returns>
        public List<ArtImage> ReceiveImagesInLesson(Lesson l)
        {
            List<ArtImage> ListofImages = new List<ArtImage>();
            int LessonID = l.GetLessonID();

            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    string query = "SELECT * FROM Image WHERE IMG_ID IN(SELECT Image_ID FROM LessonImage WHERE Lesson_ID = @Lesson_ID)";
                    SqlCommand cmd = new SqlCommand(query, cnn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Lesson_ID", LessonID);
                    cnn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            int Img_ID = Convert.ToInt32(dr["IMG_ID"]);
                            byte[] DataArr = (byte[])dr["Data"];
                            string FileName = dr["FileName"].ToString();
                            ArtImage I = new ArtImage(Img_ID, FileName, DataArr);
                            ListofImages.Add(I);
                        }
                    }

                    cnn.Close();
                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }

            return ListofImages;
        }


        /// <summary>
        /// Insert Artobject into lesson
        /// </summary>
        /// <param name="LessonID">Lesson ID</param>
        /// <param name="Art_ID">ART_ID</param>
        public void InsertArtObjInLesson(int LessonID, int Art_ID)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    if (cnn.State == ConnectionState.Closed)
                    {
                        cnn.Open();
                    }

                    SqlCommand NewCmd = cnn.CreateCommand();
                    NewCmd.Connection = cnn;
                    NewCmd.CommandType = CommandType.Text;
                    NewCmd.CommandText = "INSERT INTO LessonObj (Lesson_ID, Art_ID) VALUES (@Lesson_ID, @Art_ID)";

                    NewCmd.Parameters.AddWithValue("@Lesson_ID", LessonID);
                    NewCmd.Parameters.AddWithValue("@Art_ID", Art_ID);
                    NewCmd.ExecuteNonQuery();

                    cnn.Close();
                }
            }
            catch (NullReferenceException e)
            {
                Console.Write(e.Message);
            }
        }


        //Delete the art objects that were set for the lesson
        /// <summary>
        /// Delete art from lesson
        /// </summary>
        /// <param name="Lesson_ID">Lesson ID</param>
        public void DeleteArtObjFromLessonByID(int Lesson_ID)
        {
            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    string query = "DELETE FROM LessonObj WHERE Lesson_ID = @Lesson_ID";
                    SqlCommand cmd = new SqlCommand(query, cnn);

                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Lesson_ID", Lesson_ID);
                    cnn.Open();

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        /// <summary>
        /// Get images in lesson
        /// </summary>
        /// <param name="LessonID">Lesson ID</param>
        /// <returns>List of Image objects</returns>
        public List<ArtObject> ReceiveArtObjInLesson(Lesson L)
        {
            List<ArtObject> ListofArt = new List<ArtObject>();
            int LessonID = L.GetLessonID();

            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    string query = "SELECT * FROM ArtObj WHERE Art_ID IN(SELECT Art_ID FROM LessonObj WHERE Lesson_ID = @Lesson_ID)";
                    SqlCommand cmd = new SqlCommand(query, cnn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Lesson_ID", LessonID);
                    cnn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            int ArtID = Convert.ToInt32(dr["Art_ID"]);
                            string Art_title = dr["Art_title"].ToString();
                            string Art_type = dr["Art_type"].ToString();
                            string Art_genre = dr["Art_genre"].ToString();
                            int Art_year = Convert.ToInt32(dr["Art_year"]);
                            string Art_period = dr["Art_period"].ToString();
                            string Art_city = dr["Art_city"].ToString();
                            string Art_country = dr["Art_country"].ToString();
                            decimal Art_width = Convert.ToDecimal(dr["Art_width"]);
                            decimal Art_height = Convert.ToDecimal(dr["Art_height"]);
                            decimal Art_length = Convert.ToDecimal(dr["Art_length"]);
                            string Art_material = dr["Art_material"].ToString();
                            string Art_where_is_original_located = dr["Art_where_is_original_located"].ToString();
                            string Art_description = dr["Art_description"].ToString();

                            ArtObject A = new ArtObject(ArtID, Art_title, Art_type, Art_genre, Art_year,
                                Art_period, Art_city, Art_country, Art_width, Art_height, Art_length, Art_material,
                                Art_where_is_original_located, Art_description);

                            ListofArt.Add(A);
                        }
                    }

                    cnn.Close();
                }
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine(e.Message);
            }

            return ListofArt;
        }
    }
}
