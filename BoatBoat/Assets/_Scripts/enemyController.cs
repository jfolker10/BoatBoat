using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class enemyController : MonoBehaviour {
	private Vector3 curPos;
	
	public float forceFactor;
	public float rotFactor, curRotFactor;
	public float maxSpeed, curSpeed;
	public float maxTurn, curMaxTurn;
	public float idleDrag;
	private bool movingForward, movingBackward;

	public float fakeVerticalAxis, fakeHorizontalAxis;
	
	public Vector3 target;
	public NavMeshAgent navagent;
	public List<Vector3> waypoints;
	public List<GameObject> wayspheres;

	public enum State {
		PATROL,
		CHASE,
		ATTACK,
		STUCK
	}

	public State state;

	// Use this for initialization
	void Start () {
		state = State.CHASE;
	}

	void Update() {
		GetWaypoints();
		DrawWaylines();
		DrawWayspheres();

		switch(state) {
			case State.CHASE:
				if (waypoints.Count > 1) {
					Vector3 toWaypoint = waypoints[1] - navagent.transform.position;
					float angle = AngleSigned(navagent.transform.forward, toWaypoint, Vector3.up);
					fakeHorizontalAxis = angle/180f;

					float temp = Mathf.Abs(angle/180f);
					float targetSpeed;

					if (temp <= 2/5) {
						targetSpeed = ((2/5 - temp) + 3/5) * maxSpeed;
					} else {
						targetSpeed = (13 - 10*temp)/15 * maxSpeed;//((1f - (temp - 2/5)/(3/5))*(2/5) + 1/5) * maxSpeed;
					}

					fakeVerticalAxis = (targetSpeed - curSpeed)/maxSpeed;
				}
				break;
			default:
				break;
		}
		
	}

	public Vector3 GetTarget() {
		return Vector3.zero;
	}

	public void GetWaypoints() {
		navagent.transform.position = new Vector3(this.transform.position.x, -4, this.transform.position.z);
		navagent.SetDestination(GetTarget());
		waypoints.Clear();
		
		foreach (Vector3 wp in navagent.path.corners) {
			waypoints.Add(wp);
		}
	}

	public void DrawWaylines() {
		for (int i = 0; i < waypoints.Count-1; i++) {
			Vector3 wp1 = new Vector3(waypoints[i].x, 4f, waypoints[i].z);
			Vector3 wp2 = new Vector3(waypoints[i+1].x, 4f, waypoints[i+1].z);
			Debug.DrawLine(wp1, wp2, Color.green);
		}
	}

	public void DrawWayspheres() {
		foreach (GameObject ws in wayspheres) {
			Destroy(ws.gameObject);
		}
		wayspheres.Clear();

		foreach (Vector3 wp in waypoints) {
			GameObject ws = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			ws.transform.position = new Vector3(wp.x, 4f, wp.z);
			ws.transform.localScale = Vector3.one * 1f;
			ws.name = "Waysphere";
			wayspheres.Add(ws);
		}
	}
	
	void FixedUpdate () {
		fakeVerticalAxis = Mathf.Clamp(fakeVerticalAxis, -1f, 1f);
		fakeHorizontalAxis = Mathf.Clamp(fakeHorizontalAxis, -1f, 1f);
		
		Vector3 localVelocity = this.transform.InverseTransformDirection(this.rigidbody.velocity);
		curSpeed = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z).magnitude;

		// turning speed limited when going to slow or going too fast (max is at 3/5 of top speed)
		if (curSpeed > maxSpeed) {
			curMaxTurn = maxTurn * Mathf.Sin(maxSpeed/maxSpeed * (Mathf.PI/2) * 5/3);
		} else {
			curMaxTurn = maxTurn * Mathf.Sin(curSpeed/maxSpeed * (Mathf.PI/2) * 5/3);
		}
		curRotFactor = curMaxTurn/maxTurn * rotFactor;
		
		movingForward = (localVelocity.z >= 0);
		movingBackward = !movingForward;

		if (fakeVerticalAxis > 0) {
			// ACCELERATE
			this.rigidbody.drag = 0;
			if (localVelocity.z < maxSpeed) {
				this.rigidbody.AddForce(this.transform.forward * forceFactor);
			}
		} else if (fakeVerticalAxis < 0) {
			// DECELERATE
			this.rigidbody.drag = 0;
			if (localVelocity.z > -maxSpeed/2) {
				this.rigidbody.AddForce(this.transform.forward * -forceFactor/2);
			}
		} else {
			this.rigidbody.drag = idleDrag;
		}

		if (fakeHorizontalAxis > 0) {
			// RIGHT TURN
			if (movingForward && rigidbody.angularVelocity.y < curMaxTurn) {
				this.rigidbody.AddTorque(this.transform.up * curRotFactor);
			} else if (movingBackward && rigidbody.angularVelocity.y > -curMaxTurn) {
				this.rigidbody.AddTorque(this.transform.up * -curRotFactor);
			}
		} else if (fakeHorizontalAxis < 0) {
			// LEFT TURN
			if (movingForward && rigidbody.angularVelocity.y > -curMaxTurn) {
				this.rigidbody.AddTorque(this.transform.up * -curRotFactor);
			} else if (movingBackward && rigidbody.angularVelocity.y < curMaxTurn) {
				this.rigidbody.AddTorque(this.transform.up * curRotFactor);
			}
		}

		Vector2 planeVelocity = new Vector2(this.rigidbody.velocity.x, this.rigidbody.velocity.z);
		if (movingForward) {
			// if moving forward, redirect all velocity forward after turning
			this.rigidbody.velocity = this.transform.forward * planeVelocity.magnitude;
		} else if (movingBackward) {
			// if moving backward, redirect all velocity backward after turning
			this.rigidbody.velocity = this.transform.forward * -planeVelocity.magnitude;
		}
	}

	public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n) {
		return Mathf.Atan2(
			Vector3.Dot(n, Vector3.Cross(v1, v2)),
			Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
	}

	void OnCollisionEnter(Collision collision) {
		Debug.Log("Ship Collision: " + collision.gameObject.name);
	}
}