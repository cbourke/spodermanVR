﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour {


	// Update is called once per frame
	void Update () {

		transform.Rotate (Vector3.up , 40 * Time.deltaTime);

	}
}
