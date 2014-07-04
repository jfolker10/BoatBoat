using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour {

	public GameObject explosion;
	
	//void OnTriggerEnter(Collider other) 
	void OnCollisionEnter(Collision other)
	{
		if (other.collider.tag == "Spout Sector")
		{
			return;
		}
		Instantiate(explosion, transform.position, transform.rotation);
		Destroy(gameObject);
		//Add when enemy boat needs to die
//		if (other.tag == "Player")
//		{
//			Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
//			gameController.GameOver ();
//		}
//		Destroy(other.gameObject);
	}
}
