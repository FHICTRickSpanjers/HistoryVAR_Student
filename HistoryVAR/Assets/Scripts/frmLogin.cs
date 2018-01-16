using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using History_VAR.Classes;

public class frmLogin : MonoBehaviour {

	//Username logged in user
	public static GameObject SelectedUser;

	//Gameobjects
	public InputField InputUsername;
	public InputField InputPassword;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		//Nothing to do here
	}

	public void Login_Click()
	{
		Login ();
	}

	private void Login()
	{
		//Check if textboxes are empty
		if (string.IsNullOrEmpty (InputUsername.text) || string.IsNullOrEmpty (InputPassword.text)) {
			EditorUtility.DisplayDialog ("Gebruikersnaam en/of wachtwoorden zijn niet ingevuld.", "Gebruikersnaam en/of wachtwoord velden niet ingevuld", "OK");
		} 
		else 
		{
			DBRepository DB = DBRepository.GetInstance ();
			var LoginCheckStudent = DB.FindLoginData (InputUsername.text, InputPassword.text, "Student");

			if (LoginCheckStudent == true) {
				SceneManager.LoadScene ("Student_Screen");
				SelectedUser = new GameObject (InputUsername.text);
				DontDestroyOnLoad (SelectedUser);
			} 
			else 
			{
				EditorUtility.DisplayDialog ("Gebruikersnaam en/of wachtwoorden zijn verkeerd.", "Gebruikersnaam en/of wachtwoord verkeerd", "OK");
			}
		}

	}

}
