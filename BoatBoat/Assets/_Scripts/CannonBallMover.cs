using UnityEngine;
using System.Collections;

public class CannonBallMover : MonoBehaviour {

	public float speed;
	
	void ShotDirection(int direction)
	{
		//on left side of boat
		if (direction == -1) {
			rigidbody.velocity = -transform.right * speed;
		}
		else{  //on right side of boat
			rigidbody.velocity = transform.right * speed;
		}
	}

}
