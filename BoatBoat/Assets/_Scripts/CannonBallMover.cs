using UnityEngine;
using System.Collections;

public class CannonBallMover : MonoBehaviour {

	public float speed;

	// Use this for initialization
	void Start () 
	{
		//on left side of boat
		if (this.transform.position.x < 0.0f) {
			rigidbody.velocity = -transform.right * speed;
		}
		else{  //on right side of boat
			rigidbody.velocity = transform.right * speed;
		}

	}

}
