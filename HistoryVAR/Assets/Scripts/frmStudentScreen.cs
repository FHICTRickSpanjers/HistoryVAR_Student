using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using History_VAR.Classes;

public class frmStudentScreen : MonoBehaviour {

	private string ActiveUser;

	//Gameobjects
	public Text lblUser;
	public GameObject lstLessons;
	public static GameObject SelectedLesson;

	// Use this for initialization
	void Start () 
	{
		ActiveUser = frmLogin.SelectedUser.name;
		lblUser.text = "Welkom " + ActiveUser;
		AllAvailableLessons ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void AllAvailableLessons()
	{
		DBRepository DB = DBRepository.GetInstance ();

		//Get list of students
		List<Student>  Students = DB.FindStudentDetails();
		int StudentID = 0;

		foreach (Student SingleStudent in Students) 
		{
			if (ActiveUser == SingleStudent.GetStudentName()) 
			{
				StudentID = SingleStudent.GetStudentID ();	
			}	
		}

		//Get StudentGroup, Lessons for the group & lessons for all groups
		var StudentGroup = DB.FindGroupIDBasedOnStudent(StudentID);
		var GroupLessons = DB.GetLessonsBasedOnGroupID (StudentGroup.Get_Group_ID());
		var GroupLessonsForAll = DB.GetLessonsBasedOnGroupID (0);

		//Make a TextBox and button for every lesson that is available
		foreach (var SingleLesson in GroupLessons) 
		{
			if (SingleLesson.LessonStatus == "Published") 
			{
				AddButton (SingleLesson.GetLessonName());
			}	
		}

		//Do the same as before just for lessons available for all groups
		foreach(var SingleLesson in GroupLessonsForAll)
		{
			if (SingleLesson.LessonStatus == "Published") 
			{
				AddButton (SingleLesson.GetLessonName());
			}	
		}
	}

	private void AddButton(string LessonName)
	{
		//Add GameObject to listbox
		GameObject btnGO = new GameObject(LessonName);

		//Add Components to GameObject
		//RectTransform
		RectTransform btnRT = btnGO.AddComponent<RectTransform>();
		btnRT.SetParent (lstLessons.transform);
		btnRT.localScale = new Vector3 (1.0f, 1.0f, 1.0f);

		//Verical Layout Group
		VerticalLayoutGroup btnVLG = btnGO.AddComponent<VerticalLayoutGroup>();
		btnVLG.padding.left = 20;
		btnVLG.padding.right = 20;
		btnVLG.padding.bottom = 20;
		btnVLG.padding.top = 20;

		//Add Image
		Image btnIMG = btnGO.AddComponent<Image> ();
		btnIMG.color = new Color(150f/255.0f, 32f/255.0f, 37f/255.0f);

		//Button
		Button btnBTN = btnGO.AddComponent<Button> ();
		btnBTN.onClick.AddListener (() => {
			OpenLesson(LessonName, btnIMG);
		});

		//Add GameObject to Button component
		GameObject btnTXT = new GameObject("Text");

		//Add RectTransform component to GameObject and set Parent and scale
		RectTransform btnTXTRT = btnTXT.AddComponent<RectTransform> ();
		btnTXTRT.SetParent (btnGO.transform);
		btnTXTRT.localScale = new Vector3 (1.0f, 1.0f);

		//Add Text component to GameObject and add text,color,font,fontsize and set alignment and resize text for best fit
		Text ButtonTitle = btnTXT.AddComponent<Text> ();
		ButtonTitle.text = LessonName;
		ButtonTitle.color = Color.white;
		ButtonTitle.font = Resources.Load("Roboto-Regular") as Font;
		ButtonTitle.fontSize = 28;
		ButtonTitle.alignment = TextAnchor.MiddleCenter;
	}

	private void OpenLesson(string LessonName, Image background)
	{
		background.color = new Color(220f/255.0f, 51f/255.0f, 74f/255.0f);

		SelectedLesson = new GameObject (LessonName);
		DontDestroyOnLoad (SelectedLesson);
		SceneManager.LoadScene ("Lesson_Screen");
	}

	public void Logout()
	{
		bool result = EditorUtility.DisplayDialog ("Logout", "Are you sure to logout?", "Yes", "Cancel");
		if (result) 
		{
			Destroy (frmLogin.SelectedUser);
			SceneManager.LoadScene ("Login_Screen");
		}
	}
}
