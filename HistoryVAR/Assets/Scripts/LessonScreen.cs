using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
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
	public GameObject ListImages;
	public Image LessonImg;

	private GameObject ImageGO;

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



				//Add image to list
				foreach (var SingleImage in Images) 
				{
					AddButton (SingleImage);
				}
			}
		}

	}

	private void AddButton(ArtImage ImgName)
	{
		//Add GameObject to listbox
		GameObject ButtonGO = new GameObject(ImgName.ReturnFileName());

		//Add RectTransform component to Gameobject and set parent and scale
		RectTransform ButtonRT = ButtonGO.AddComponent<RectTransform> ();
		ButtonRT.SetParent (ListImages.transform);
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
			ShowImg(ImgName);
		});

		//Add Image component to GameObject
		Image ButtonIM = ButtonGO.AddComponent<Image> ();


		//Add GameObject to Button component
		GameObject ButtonTXT = new GameObject("Text");

		//Add RectTransform component to GameObject and set Parent and scale
		RectTransform ButtonTXTRT = ButtonTXT.AddComponent<RectTransform> ();
		ButtonTXTRT.SetParent (ButtonGO.transform);
		ButtonTXTRT.localScale = new Vector3 (1.0f, 1.0f);

		//Add Text component to GameObject and add text,color,font,fontsize and set alignment and vertical overflow 
		Text ButtonTitle = ButtonTXT.AddComponent<Text> ();
		ButtonTitle.text = ImgName.ReturnFileName();
		ButtonTitle.color = Color.black;
		ButtonTitle.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
		ButtonTitle.fontSize = 23;
		ButtonTitle.alignment = TextAnchor.MiddleCenter;
		ButtonTitle.verticalOverflow = VerticalWrapMode.Overflow;
	}

	private void ShowImg(ArtImage Img)
	{
		Texture2D tex = new Texture2D (100, 100);
		tex.LoadImage (Img.ReturnImageData());

		Sprite sprite = Sprite.Create (tex, new Rect(0,0,tex.width,tex.height), new Vector2(0.5f, 0.5f));
		LessonImg.sprite = sprite;
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
