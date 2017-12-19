using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.SceneManagement;
using History_VAR.Classes;

public class LoginScreen : MonoBehaviour 
{
	//Username of the logged in user
	public static GameObject SelectedUser;

	//GameObjects
	public InputField InputUsername;
	public InputField InputPassword;
		
	public void Login_Click()
	{
		LogInStudent ();
	}


	private void LogInStudent()
	{
		//Check if textboxes are empty
		if (string.IsNullOrEmpty (InputUsername.text) || string.IsNullOrEmpty (InputPassword.text)) 
		{
			EditorUtility.DisplayDialog ("Incorrect username or password", "Incorrect username or password", "OK");
		} 
		else 
		{
			//New instance of DB Class
			DBRepository DB = DBRepository.GetInstance ();
			var LoggedIn = DB.FindLoginData (InputUsername.text, InputPassword.text, "Student");

			//Valid login
			if (LoggedIn == true) 
			{
				Debug.Log ("Login was succesful");	
				SceneManager.LoadScene ("Student_Screen");
				SelectedUser = new GameObject (InputUsername.text);
				DontDestroyOnLoad (SelectedUser);

			} else 
			{
				EditorUtility.DisplayDialog ("Incorrect username or password", "Incorrect username or password", "OK");
			}
		}
	}
}
