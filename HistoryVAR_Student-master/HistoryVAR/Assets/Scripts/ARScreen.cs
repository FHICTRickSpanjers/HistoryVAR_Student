using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ARScreen : MonoBehaviour {

	//GameObjects
	public Button Back;

	public void GoToLessonScreen()
	{
		SceneManager.LoadScene ("Lesson_Screen");
	}
}
