using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using History_VAR.Classes;

public class StudentScreen : MonoBehaviour {

	//Private Variables
	private string ActiveUser;

	//Gameobjects
	public Text UserLabel;
	public GameObject LstLessons;
	private GameObject LessonGO;
	private GameObject LessonTXT;
	public static GameObject SelectedLesson;

	// Use this for initialization
	void Start () 
	{
		ActiveUser = LoginScreen.SelectedUser.name;

		UserLabel.text = "Welcome " + ActiveUser;
		GetAllAvailableLessons ();	

	}

	private void GetAllAvailableLessons()
	{
		//Get DBRepos instance
		DBRepository DB = DBRepository.GetInstance ();

		//Get list of Students
		var Students = DB.FindStudentDetails();
		int StudentID = 0;

		//Foreach student in the list of student
		foreach (var SingleStudent in Students) 
		{
			//If the user equals the logged in user 
			if (ActiveUser == SingleStudent.GetStudentName()) 
			{
				//Get student ID
				StudentID = SingleStudent.GetStudentID();
			}
		}

		//Get StudentGroup, Lessons for the group & lessons for all groups
		var StudentGroup = DB.FindGroupIDBasedOnLessonID(StudentID);
		var GroupLessons = DB.GetLessonsBasedOnGroupID (StudentGroup.Get_Group_ID());
		var GroupLessonsForAll = DB.GetLessonsBasedOnGroupID (0);

		int i = 0;
		//Make a TextBox and button for every lesson that is available
		foreach (var SingleLesson in GroupLessons) 
		{
			if (SingleLesson.LessonStatus == "Published") 
			{
				AddButton (SingleLesson.GetLessonName().ToString());
				i++;
			}	
		}

	}

	private void AddButton(string LessonName)
	{
		//Add button to listbox
		LessonGO = new GameObject(LessonName);
		RectTransform LessonRT = LessonGO.AddComponent<RectTransform> ();
		LessonRT.SetParent (LstLessons.transform);
		LessonRT.sizeDelta = new Vector2 (420.0f, 80.0f);
		Button LessonBTN = LessonGO.AddComponent<Button> ();
		Image LessonIM = LessonGO.AddComponent<Image> ();
		LessonBTN.onClick.AddListener (() => {
			OpenLesson(LessonName);
		});

		//Add textbox to button
		LessonTXT = new GameObject("Text");
		RectTransform LessonTXTRT = LessonTXT.AddComponent<RectTransform> ();
		LessonTXTRT.SetParent (LessonGO.transform);
		LessonTXTRT.sizeDelta = new Vector2 (420.0f, 80.0f);
		Text LessonTitle = LessonTXT.AddComponent<Text> ();
		LessonTitle.text = LessonName;
		LessonTitle.color = Color.black;
		LessonTitle.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
		LessonTitle.fontSize = 28;
		LessonTitle.alignment = TextAnchor.MiddleCenter;
	}

	private void OpenLesson(string LessonName)
	{
		Debug.Log ("Open: " + LessonName);
		SelectedLesson = new GameObject (LessonName);
		DontDestroyOnLoad (SelectedLesson);
		SceneManager.LoadScene ("Lesson_Screen");
	}

	public void Logout()
	{
		bool result = EditorUtility.DisplayDialog ("Logout", "Are you sure to logout?", "Yes", "Cancel");
		if (result) 
		{
			Destroy (LoginScreen.SelectedUser);
			SceneManager.LoadScene ("Login_Screen");
		}
	}
}
