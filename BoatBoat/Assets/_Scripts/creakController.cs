﻿using UnityEngine;
using System.Collections;

public class creakController : MonoBehaviour {

	public AudioClip creak;

	// Use this for initialization
	void Start () {
		audio.Stop ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter(Collision collision){
		if(collision.gameObject.name == "Ship"){
			audio.Play ();
		}
	}
}
