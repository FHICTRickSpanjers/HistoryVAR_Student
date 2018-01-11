using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class frmAR : MonoBehaviour {

	//GameObjects
	public Button Back;

	// Use this for initialization
	void Start () 
	{
		GameObject[] ArtObjects = GameObject.FindGameObjectsWithTag ("ArtObject");
		foreach (var ArtObject in ArtObjects) 
		{
			if (ArtObject.name != frmLesson.SelectedArtObject.name) 
			{
				ArtObject.SetActive (false);	
			}	
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Nothing to do here	
	}

	public void GoToLessonScreen()
	{
		DestroyObject (frmLesson.SelectedArtObject);
		SceneManager.LoadScene ("Lesson_Screen");
	}
}
