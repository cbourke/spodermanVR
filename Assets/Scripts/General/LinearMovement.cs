/*
 * This whole script was just a small experiment. Unused in final project.
 * 
 * 
 * 
 * */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMovement : MonoBehaviour {
	public float distance;
	public float speed;
	public enum moveAxis {
		x,
		y,
		z,
		negx,
		negy,
		negz
	}
	public moveAxis axis;
	private bool forward;
	private float startCoord;

	// Use this for initialization
	void Start () {
		forward = true;
		switch (axis) {
		case moveAxis.x:
			startCoord = transform.position.x;
			break;
		case moveAxis.y:
			startCoord = transform.position.y;
			break;
		case moveAxis.z: 
			startCoord = transform.position.z;
			break;
//		case moveAxis.negx:
//			startCoord = transform.position.x;
//			forward = false;
//			distance = -distance;
//			break;
//		case moveAxis.negy:
//			startCoord = transform.position.y;
//			forward = false;
//			distance = -distance;
//			break;
//		case moveAxis.negz:
//			startCoord = transform.position.z;
//			forward = false;
//			distance = -distance;
//			break;
		default:
			startCoord = 0f;
			break;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (axis == moveAxis.x || axis == moveAxis.negx) {
			if (forward) {
				transform.Translate (Vector3.right * speed * Time.deltaTime);
			} else {
				transform.Translate (Vector3.left * speed * Time.deltaTime);
			}

			if (transform.position.x >= startCoord + distance) {
				forward = false;
			}
			if (transform.position.x <= startCoord) {
				forward = true;
			}
		}
		if (axis == moveAxis.y) {
			if (forward) {
				transform.Translate (Vector3.up * speed * Time.deltaTime);
			} else {
				transform.Translate (Vector3.down * speed * Time.deltaTime);
			}

			if (transform.position.y >= startCoord + distance) {
				forward = false;
			}
			if (transform.position.y <= startCoord) {
				forward = true;
			}
		}
		if (axis == moveAxis.z) {
			if (forward) {
				transform.Translate (Vector3.forward * speed * Time.deltaTime);
			} else {
				transform.Translate (Vector3.back * speed * Time.deltaTime);
			}

			if (transform.position.z >= startCoord + distance) {
				forward = false;
			}
			if (transform.position.z <= startCoord) {
				forward = true;
			}
		}
	}
}
