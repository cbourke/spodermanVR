using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class debugEvents : MonoBehaviour {
	public Texture[] tex;
//	public Texture[] tex2;
	private GameObject windowText;
//	private GameObject windowText2;
	// Use this for initialization
	void Awake () {
		windowText = GameObject.Find ("Window").transform.Find ("WindowAnim").transform.Find ("windowText").gameObject;

//		windowText2 = GameObject.Find ("Window (1)").transform.Find ("WindowAnim").transform.Find ("windowText").gameObject;
	}
	void Start () {
		windowText.GetComponent<WindowTextController> ().updateArray (tex);
//		windowText2.GetComponent<WindowTextController> ().updateArray (tex2);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
