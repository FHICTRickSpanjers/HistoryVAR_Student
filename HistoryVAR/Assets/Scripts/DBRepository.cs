using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;

namespace History_VAR.Classes
{
    class DBRepository
    {

        //Instance always null
        private static DBRepository instance = null;

        //Empty constructor
        private DBRepository()
        {

        }

        //Create Instance
        public static DBRepository GetInstance()
        {
            if (instance == null)
            {
                instance = new DBRepository();
            }
            return instance;
        }


		/*
        /// <summary>
        /// Get login data for Teacher User
        /// </summary>
        /// <param name="username">Username as string</param>
        /// <returns>Password</returns>
        public string Get_Login_Data_Teachers(string username)
        {
            string ResultQuery = "";

            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    string query = "SELECT Teacher_password FROM Teacher WHERE Teacher_username = @username";
                    SqlCommand cmd = new SqlCommand(query, cnn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ResultQuery = dr["Teacher_password"].ToString();
                        }
                    }

                    cnn.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return ResultQuery;
        }

		*/
        /// <summary>
        /// Get login data for the student users
        /// </summary>
        /// <param name="username">string username</param>
        /// <returns>password as string</returns>
        public string Get_Login_Data_Student(string username)
        {
            string ResultQuery = "";

            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    string query = "SELECT Student_password FROM Student WHERE Student_username = @username";
                    SqlCommand cmd = new SqlCommand(query, cnn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            ResultQuery = dr["Student_password"].ToString();
                        }
                    }

                    cnn.Close();
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
            }

            return ResultQuery;
        }


		/*
        /// <summary>
        /// Create new lessons (teachers)
        /// </summary>
        /// <param name="lesson_subject">string lesson subject</param>
        /// <param name="teacherid">Id of the teacher</param>
        /// <param name="lesson_name">Name of the lesson</param>
        /// <param name="lesson_status">Status of the lesson</param>
        /// <param name="lesson_desc">Description of the lesson</param>
        public void CreateNewLesson(string lesson_subject, int teacherid, string lesson_name, string lesson_status, string lesson_desc)
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

                    NewCmd.Parameters.AddWithValue("@Lesson_Subject", lesson_subject);
                    NewCmd.Parameters.AddWithValue("@Teacher_ID", teacherid);
                    NewCmd.Parameters.AddWithValue("@Lesson_Name", lesson_name);
                    NewCmd.Parameters.AddWithValue("@Lesson_Status", lesson_status);
                    NewCmd.Parameters.AddWithValue("@Lesson_Description", lesson_desc);

                    NewCmd.ExecuteNonQuery();

                    cnn.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        //Insert Artobjects per lesson
        public void Insert_Artobjects_In_lessons(int lessonid, int artobjid)
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
                MessageBox.Show(e.Message);
            }
        }


        //Insert ArtObjects into Database
        public void Insert_Artobjects(string title, string type, string creator, string movement, int YearMade, string periode, string city, 
            string country,  decimal width, decimal height, decimal length,  string material, string original_located, string description)
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
                    NewCmd.CommandText = "INSERT INTO ArtObj (Art_title, Art_type, Art_movement, Art_year, Art_periode, Art_city, " +
                        "Art_country, Art_width, Art_height, Art_length, Art_material, Art_where_is_original_located, Art_description) " +
                        "VALUES " +
                        "(@Art_title, @Art_type, @Art_movement, @Art_year, @Art_periode, @Art_city, " +
                        "@Art_country, @Art_width, @Art_height, @Art_length, @Art_material, @Art_where_is_original_located, @Art_description)";

                    NewCmd.Parameters.AddWithValue("@Art_title", title);
                    NewCmd.Parameters.AddWithValue("@Art_type", type);
                    NewCmd.Parameters.AddWithValue("@Art_movement", movement);
                    NewCmd.Parameters.AddWithValue("@Art_year", YearMade);
                    NewCmd.Parameters.AddWithValue("@Art_periode", periode);
                    NewCmd.Parameters.AddWithValue("@Art_city", city);
                    NewCmd.Parameters.AddWithValue("@Art_country", country);
                    NewCmd.Parameters.AddWithValue("@Art_width", width);
                    NewCmd.Parameters.AddWithValue("@Art_height", height);
                    NewCmd.Parameters.AddWithValue("@Art_length", length);
                    NewCmd.Parameters.AddWithValue("@Art_material", material);
                    NewCmd.Parameters.AddWithValue("@Art_where_is_original_located", original_located);
                    NewCmd.Parameters.AddWithValue("@Art_description", description);
                    NewCmd.ExecuteNonQuery();

                    cnn.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }



        /// <summary>
        /// Select all the lessons
        /// </summary>
        /// <returns>return a string of lessons</returns>
        public List<Lesson> Get_Lessons_Data()
        {
            List<Lesson> listOfLessons = new List<Lesson>();

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
                            string Lname = dr["Lesson_name"].ToString();
                            int LID = Convert.ToInt32(dr["Lesson_ID"]);
                            string Lstatus = dr["Lesson_status"].ToString();
                            string Ldesc = dr["Lesson_description"].ToString();
                            string Lsubject = dr["Lesson_subject"].ToString();

                            Lesson L = new Lesson(Lname, LID, Lstatus, Ldesc, Lsubject);
                            listOfLessons.Add(L);
                        }
                    }

                    cnn.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return listOfLessons;
        }


        public int GetLessonID(string name)
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
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return LessonID;
        }


        /// <summary>
        /// Delete lesson by ID
        /// </summary>
        /// <param name="Lesson_ID"></param>
        public void Delete_Lesson_By_ID(int Lesson_ID)
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
                MessageBox.Show(e.Message);
            }
        }


        //Delete the lesson for the groups that have it
        public void Delete_Lesson_At_Groups_By_ID(int Lesson_ID)
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
                MessageBox.Show(e.Message);
            }
        }


        /// <summary>
        /// Get current lesson status
        /// </summary>
        /// <param name="lessonid">ID of the lesson</param>
        /// <returns>Lesson object</returns>
        public Lesson Get_Lesson_Status(int lessonid)
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
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return Les;
        }


        /// <summary>
        /// Update a lesson
        /// </summary>
        /// <param name="Lesson_ID">ID of the lesson</param>
        /// <param name="status">status of the lesson</param>
        public void Update_Lesson_Status_By_ID(int Lesson_ID, string status)
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
                MessageBox.Show(e.Message);
            }


        }


        
        /// <summary>
        /// Select necessary data to update
        /// </summary>
        /// <param name="Lesson_ID">ID of the lesson</param>
        /// <param name="searchinfo">Anything really</param>
        /// <returns></returns>
        public string Get_Lesson_Data_By_ID(int Lesson_ID, string searchinfo)
        {
            string required_data = "";

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
                            required_data = dr[searchinfo].ToString();
                        }
                    }


                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return required_data;
        }


        //Make new Lesson (set)
        public void UpdateLesson(int Lesson_ID, string lesson_subject, string lesson_name, string lesson_status, string lesson_desc)
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
                    NewCmd.CommandText = "UPDATE Lesson SET Lesson_subject=@Lesson_Subject, Lesson_name=@Lesson_Name, Lesson_status=@Lesson_Status, Lesson_description=@Lesson_Description WHERE Lesson_ID = @Lesson_ID";

                    NewCmd.Parameters.AddWithValue("@Lesson_Subject", lesson_subject);
                    NewCmd.Parameters.AddWithValue("@Lesson_Name", lesson_name);
                    NewCmd.Parameters.AddWithValue("@Lesson_Status", lesson_status);
                    NewCmd.Parameters.AddWithValue("@Lesson_Description", lesson_desc);
                    NewCmd.Parameters.AddWithValue("@Lesson_ID", Lesson_ID);

                    NewCmd.ExecuteNonQuery();

                    cnn.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }




        //Select groups from database

        public List<Group> Get_Group_Data()
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
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return ListOfGroups;
        }



        //Get Teacher ID by name
        public Teacher Get_Teacher_ID(string name)
        {
            Teacher T = new Teacher();

            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    string query = "SELECT Teacher_ID FROM Teacher WHERE Teacher_username = @username";
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
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return T;
        }


        public Teacher Get_Teacher_Name(int ID)
        {
            Teacher T = null;

            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    string query = "SELECT Teacher_username FROM Teacher WHERE Teacher_ID = @ID";
                    SqlCommand cmd = new SqlCommand(query, cnn);
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            T = new Teacher(dr["Teacher_username"].ToString());
                        }
                    }

                    cnn.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return T;
        }

        public Group Get_Group_ID(string name)
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
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return G;
        }


        public void Insert_Lesson_For_Groups(int Lesson_ID, int Group_ID)
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
                MessageBox.Show(e.Message);
            }        
        }



        public Group Get_Group_ID_Based_On_Lesson_ID(int ID)
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
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return G;
        }

        public List<Lesson> Get_Lessons_Based_On_GroupID(int ID)
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
                            string LessonStatus = dr["Lesson_status"].ToString();
                            string LessonDesc = dr["Lesson_description"].ToString();
                            string LessonSubject = dr["Lesson_subject"].ToString();

                            Lesson L = new Lesson(LessonName, LessonID, LessonStatus, LessonDesc, LessonSubject);
                            ListofLessons.Add(L);
 
                        }
                    }

                    cnn.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return ListofLessons;
        }


        public Group Get_Group_Data_Based_On_GroupID(int ID)
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
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return G;
        }


        public List<Student> GetStudentDetails()
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
                            string StudentName = dr["Student_username"].ToString() ;
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
                MessageBox.Show(e.Message);
            }

            return ListofStudents;
        }


        public Group Get_Group_ID_Based_On_Student(int ID)
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
                MessageBox.Show(e.Message);
            }

            return G;
        }

        public void Insert_Image_In_DB(string Filename, Byte[] Image)
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
                    NewCmd.CommandText = "INSERT INTO Images (Filename, Data) VALUES (@Filename, @Data)";

                    NewCmd.Parameters.AddWithValue("@Filename", Filename);
                    NewCmd.Parameters.AddWithValue("@Data", Image);
                    NewCmd.ExecuteNonQuery();

                    cnn.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        public Image Reveive_Images_From_DB()
        {
            Image I = null;

            try
            {
                using (SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);"))
                {
                    if (cnn.State == ConnectionState.Closed)
                    {
                        cnn.Open();
                    }

                    string query = "SELECT * FROM Images";
                    SqlCommand cmd = new SqlCommand(query, cnn);
                    cmd.CommandType = CommandType.Text;
                    cnn.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            byte[] DataArr = (byte[])dr["Data"];
                            string FileName = dr["FileName"].ToString();
                            I = new Image(FileName, DataArr);
                        }
                    }

                    cnn.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return I;
        }  
		*/
    }
}
