using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using History_VAR.Classes;

public class frmLesson : MonoBehaviour {

	//Private variables
	private string LessonTitle;
	private string ActiveUser;

	//Public variables
	public static GameObject SelectedArtObject;

	//GameObjects
	public Text Title;
	public Text Description;
	public Text Subject;
	public Button ShowAR;
	public Image LessonImg;
	public GameObject Back;
	public GameObject ListImages;
	public GameObject ListLessonObjects;

	// Use this for initialization
	void Start () 
	{
		LessonTitle = frmStudentScreen.SelectedLesson.name;
		ActiveUser = frmLogin.SelectedUser.name;

		Title.text = LessonTitle;

		LessonData ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void LessonData()
	{
		DBRepository DB = DBRepository.GetInstance ();

		var Lessons = DB.FindLessonsData ();

		foreach (var SingleLesson in Lessons) 
		{
			if (SingleLesson.GetLessonName() == LessonTitle) 
			{
				Description.text = SingleLesson.GetLessonDesc ();
				Subject.text = SingleLesson.GetLessonSubject ();

				var Images = DB.ReceiveImagesInLesson (SingleLesson);

				foreach (var SingleImage in Images) 
				{
					AddButton (SingleImage);
				}

				//Find all artobject of this lesson
				var ArtObjectsLesson = DB.ReceiveArtObjInLesson (SingleLesson);
				foreach (var ArtObjectLesson in ArtObjectsLesson) 
				{
					AddButton (ArtObjectLesson);
				}
			}	
		}
	}

	private void AddButton(ArtImage artimage)
	{
		//Add GameObject to listbox
		GameObject btnGO = new GameObject(artimage.ReturnFileName());

		//Add RectTransform component to Gameobject and set parent and scale
		RectTransform btnRT = btnGO.AddComponent<RectTransform> ();
		btnRT.SetParent (ListImages.transform);
		btnRT.localScale = new Vector3 (1.0f, 1.0f);

		//Add Vertical Layout Group component to GameObject and add padding
		VerticalLayoutGroup btnVLG = btnGO.AddComponent<VerticalLayoutGroup> ();
		btnVLG.padding.left = 20;
		btnVLG.padding.right = 20;
		btnVLG.padding.bottom = 20;
		btnVLG.padding.top = 20;

		//Add Image component to GameObject
		Image btnIM = btnGO.AddComponent<Image> ();
		btnIM.color = new Color(150f/255.0f, 32f/255.0f, 37f/255.0f);

		//Add Button component to GameObject and set click eventhandler
		Button btnBTN = btnGO.AddComponent<Button> ();
		btnBTN.onClick.AddListener (() => {
			ShowImg(artimage, btnIM);
		});

		//Add GameObject to Button component
		GameObject btnTXT = new GameObject("Text");

		//Add RectTransform component to GameObject and set Parent and scale
		RectTransform btnTXTRT = btnTXT.AddComponent<RectTransform> ();
		btnTXTRT.SetParent (btnGO.transform);
		btnTXTRT.localScale = new Vector3 (1.0f, 1.0f);

		//Add Text component to GameObject and add text,color,font,fontsize and set alignment and vertical overflow 
		Text btnTitle = btnTXT.AddComponent<Text> ();
		btnTitle.text = artimage.ReturnFileName();
		btnTitle.color = Color.white;
		btnTitle.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
		btnTitle.fontSize = 23;
		btnTitle.alignment = TextAnchor.MiddleCenter;
		btnTitle.verticalOverflow = VerticalWrapMode.Overflow;
	}

	private void ShowImg(ArtImage artimage, Image background)
	{
		Texture2D tex = new Texture2D (100, 100);
		tex.LoadImage (artimage.ReturnImageData());

		Sprite sprite = Sprite.Create (tex, new Rect(0,0,tex.width,tex.height), new Vector2(0.5f, 0.5f));
		LessonImg.sprite = sprite;

		background.color = new Color(220f/255.0f, 51f/255.0f, 74f/255.0f);
	}

	private void AddButton(ArtObject Object)
	{
		//Add GameObject to listbox
		GameObject btnGO = new GameObject(Object.ReturnArtTitle());

		//Add RectTransform component to Gameobject and set parent and scale
		RectTransform btnRT = btnGO.AddComponent<RectTransform> ();
		btnRT.SetParent (ListLessonObjects.transform);
		btnRT.localScale = new Vector3 (1.0f, 1.0f);

		//Add Vertical Layout Group component to GameObject and add padding
		VerticalLayoutGroup btnVLG = btnGO.AddComponent<VerticalLayoutGroup> ();
		btnVLG.padding.left = 20;
		btnVLG.padding.right = 20;
		btnVLG.padding.bottom = 20;
		btnVLG.padding.top = 20;

		//Add Image component to GameObject
		Image btnIM = btnGO.AddComponent<Image> ();
		btnIM.color = new Color(150f/255.0f, 32f/255.0f, 37f/255.0f);

		//Add Button component to GameObject and set click eventhandler
		Button btnBTN = btnGO.AddComponent<Button> ();
		btnBTN.onClick.AddListener (() => {
			ShowArtObject(Object);
		});

		//Add GameObject to Button component
		GameObject btnTXT = new GameObject("Text");

		//Add RectTransform component to GameObject and set Parent and scale
		RectTransform btnTXTRT = btnTXT.AddComponent<RectTransform> ();
		btnTXTRT.SetParent (btnGO.transform);
		btnTXTRT.localScale = new Vector3 (1.0f, 1.0f);

		//Add Text component to GameObject and add text,color,font,fontsize and set alignment and vertical overflow 
		Text btnTitle = btnTXT.AddComponent<Text> ();
		btnTitle.text = Object.ReturnArtTitle();
		btnTitle.color = Color.white;
		btnTitle.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
		btnTitle.fontSize = 23;
		btnTitle.alignment = TextAnchor.MiddleCenter;
		btnTitle.verticalOverflow = VerticalWrapMode.Overflow;
	}

	private void ShowArtObject(ArtObject Object)
	{
		SelectedArtObject = new GameObject (Object.ReturnQRCode());
		DontDestroyOnLoad (SelectedArtObject);
		SceneManager.LoadScene ("AR_Screen");
	}

	public void GoToStudentScreen()
	{
		DestroyObject (frmStudentScreen.SelectedLesson);
		SceneManager.LoadScene ("Student_Screen");
	}
}
