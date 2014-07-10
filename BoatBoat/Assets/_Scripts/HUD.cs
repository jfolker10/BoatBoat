using UnityEngine;
using System.Collections;

public class HUD : MonoBehaviour {
	public string personName;
	public string levelTheme;
	public GameObject hudGUI;
	public float health;
	public Ship ship;
	public 

	// Use this for initialization
	void Start () {
		hudGUI.guiText.text = personName + "\n" + levelTheme + "\nShip Health: " + health;
	}
	
	// Update is called once per frame
	void Update () {
		health = ship.health;
		if (Input.GetKeyUp(KeyCode.Alpha1)) {
			Application.LoadLevel("playtestScene");
		} else if (Input.GetKeyUp(KeyCode.Alpha2)) {
			Application.LoadLevel("playtestScene");
		} else if (Input.GetKeyUp(KeyCode.Alpha3)) {
			Application.LoadLevel("playtestScene");
		} else if (Input.GetKeyUp(KeyCode.Alpha4)) {
			Application.LoadLevel("playtestScene");
		} 
		hudGUI.guiText.text = personName + "\n" + levelTheme + "\nShip Health: " + health;
	}
}
