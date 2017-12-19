using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using History_VAR.Classes;

public class LessonScreen : MonoBehaviour {

	//Private variables
	private string LessonTitle;
	private string ActiveUser;

	//GameObjects
	public Text Title;
	public GameObject Back;
	public Text Description;
	public Text Subject;
	public Button ShowAR;
	public Image SelectedArtImage;

	// Use this for initialization
	void Start () {

		//Set the lesson name and username
		LessonTitle = StudentScreen.SelectedLesson.name;
		ActiveUser = LoginScreen.SelectedUser.name;

		//Set the data from the lesson
		SetLessonData();
	}

	public void SetLessonData()
	{
		//Set lesson title
		Title.text = LessonTitle;

		//Make a new instance
		DBRepository DB = DBRepository.GetInstance();

		//Get lesson objects
		var Lessons = DB.FindLessonsData();

		//For every lesson in the lessons do the following
		foreach (var SingleLesson in Lessons) 
		{
			if (SingleLesson.GetLessonName() == LessonTitle) 
			{
				//Set the lesson description and subject
				Description.text = SingleLesson.GetLessonDesc();
				Subject.text = SingleLesson.GetLessonSubject ();

				//Get all images that are in the lesson
				var Images = DB.ReceiveImagesInLesson (SingleLesson);
				var tex = new Texture2D(64,64);


				//Add image to panel
				foreach (var SingleImage in Images) 
				{
					tex.LoadRawTextureData (SingleImage.ReturnImageData());
					tex.Apply ();
					SelectedArtImage.sprite = Sprite.Create (tex, new Rect(0, 0, 266, 199) , new Vector2 (0.5f, 0.5f));
				}
			}
		}

	}


	public void GoToARScreen()
	{
		SceneManager.LoadScene ("AR_Screen");
	}

	public void GoToStudentScreen()
	{
		DestroyObject (StudentScreen.SelectedLesson);
		SceneManager.LoadScene ("Student_Screen");
	}
}
