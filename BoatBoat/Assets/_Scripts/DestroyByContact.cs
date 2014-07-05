using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour {

	public GameObject explosion;

	void OnCollisionEnter(Collision other)
	{
		if (other.collider.tag == "Spout Sector")
		{
			return;
		}
		Instantiate(explosion, transform.position, transform.rotation);
		Destroy(gameObject);
		//Add when enemy boat needs to die, maybe put on ship.cs
//		if ((other.collider.tag == "Player") || (other.collider.tag == "Enemy"))
//		{
//			call sinking animation
//		}
//		Destroy(other.gameObject, lifeTime);
	}
}
