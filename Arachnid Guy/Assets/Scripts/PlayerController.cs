using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	private Rigidbody rb;
	public float speed;
	private int count;
	public Text countText;
	public Text winText;


	// Use this for initialization
	void Start () {

		count = 0;
		SetCountText ();
		rb = GetComponent<Rigidbody> ();
		winText.text = "";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate () {

		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		movement = movement * speed;

		rb.AddForce (movement);

	}

	void OnTriggerEnter(Collider other) {

		if (other.gameObject.CompareTag("Pickup")) {

			other.gameObject.SetActive (false);
			count++;
			SetCountText ();

		}



	}

	void SetCountText()
	{
		countText.text = "Count: " + count.ToString();
		if (count >= 12) {
			winText.text = "YOU WIN";
			Vector3 winMove = new Vector3 (0, 10000, 0);
			rb.AddForce (winMove);
		}
	}
}
