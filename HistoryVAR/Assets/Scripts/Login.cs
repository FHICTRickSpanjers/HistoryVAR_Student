using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using History_VAR.Classes;
using System.Data;
using System.Data.SqlClient;

public class Login : MonoBehaviour {

	public InputField username;
	public InputField password;
	public Text status;

	public void Log_In_Student()
	{
		if (string.IsNullOrEmpty(username.text) || string.IsNullOrEmpty(password.text))
		{
			print("The username or password was not filled");
		}
		else
		{
			//DBRepository DB = DBRepository.GetInstance();
			//var DBPassword = DB.Get_Login_Data_Student(username.text);
			SqlConnection cnn = new SqlConnection("Server=mssql.fhict.local;Database=dbi367493;User Id=dbi367493;Password=$5esa8);");
				cnn.Open();

				if (cnn.State == ConnectionState.Open) {
					status.text = "Connection is open";
				}
				else
				{
					status.text = "Connection is closed";
				}
			
				
			if (password.text == DBPassword)
			{
				print ("Login succesful");
				SceneManager.LoadScene ("Overview");
			}

		}
	}
}
