using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class disableVuforia : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<VuforiaBehaviour> ().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
