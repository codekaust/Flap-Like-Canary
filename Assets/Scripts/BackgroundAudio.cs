using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundAudio : MonoBehaviour {

	public AudioSource backSource;
	public AudioClip backClip;

	private bool started = false;

	// Use this for initialization
	void Start () {
		backSource.clip = backClip;
	}
	
	// Update is called once per frame
	void Update () {
		if (!GameControl.instance.gameOver && !started) {
			backSource.Play ();
			started = true;
		} else if(GameControl.instance.gameOver && started){
			backSource.Stop();
			started = false;
		}
	}
}
