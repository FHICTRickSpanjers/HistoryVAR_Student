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
		var StudentGroup = DB.FindGroupIDBasedOnStudent(StudentID);
		var GroupLessons = DB.GetLessonsBasedOnGroupID (StudentGroup.Get_Group_ID());
		var GroupLessonsForAll = DB.GetLessonsBasedOnGroupID (0);

		int i = 0;
		Debug.Log ("Aantal lessen: "+GroupLessonsForAll.Count);

		//Make a TextBox and button for every lesson that is available
		foreach (var SingleLesson in GroupLessons) 
		{
			if (SingleLesson.LessonStatus == "Published") 
			{
				AddButton (SingleLesson.GetLessonName().ToString());
				Debug.Log (SingleLesson.GetLessonName().ToString() + " toegevoegd");
				i++;
			}	
		}

		//Do the same as before just for lessons available for all groups
		foreach(var SingleLesson in GroupLessonsForAll)
		{
			if (SingleLesson.LessonStatus == "Published") 
			{
				AddButton (SingleLesson.GetLessonName().ToString());
				Debug.Log (SingleLesson.GetLessonName().ToString() + " toegevoegd");
				i++;
			}	
		}

	}

	private void AddButton(string LessonName)
	{
		//Add GameObject to listbox
		GameObject ButtonGO = new GameObject(LessonName);

		//Add RectTransform component to Gameobject and set parent and scale
		RectTransform ButtonRT = ButtonGO.AddComponent<RectTransform> ();
		ButtonRT.SetParent (LstLessons.transform);
		ButtonRT.localScale = new Vector3 (1.0f, 1.0f);

		//Add Vertical Layout Group component to GameObject and add padding
		VerticalLayoutGroup ButtonVLG = ButtonGO.AddComponent<VerticalLayoutGroup> ();
		ButtonVLG.padding.left = 20;
		ButtonVLG.padding.right = 20;
		ButtonVLG.padding.bottom = 20;
		ButtonVLG.padding.top = 20;

		//Add Button component to GameObject and set click eventhandler
		Button ButtonBTN = ButtonGO.AddComponent<Button> ();
		ButtonBTN.onClick.AddListener (() => {
			OpenLesson(LessonName);
		});

		//Add Image component to GameObject
		Image ButtonIM = ButtonGO.AddComponent<Image> ();


		//Add GameObject to Button component
		GameObject ButtonTXT = new GameObject("Text");

		//Add RectTransform component to GameObject and set Parent and scale
		RectTransform ButtonTXTRT = ButtonTXT.AddComponent<RectTransform> ();
		ButtonTXTRT.SetParent (ButtonGO.transform);
		ButtonTXTRT.localScale = new Vector3 (1.0f, 1.0f);

		//Add Text component to GameObject and add text,color,font,fontsize and set alignment and resize text for best fit
		Text ButtonTitle = ButtonTXT.AddComponent<Text> ();
		ButtonTitle.text = LessonName;
		ButtonTitle.color = Color.black;
		ButtonTitle.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
		ButtonTitle.fontSize = 28;
		ButtonTitle.alignment = TextAnchor.MiddleCenter;
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
