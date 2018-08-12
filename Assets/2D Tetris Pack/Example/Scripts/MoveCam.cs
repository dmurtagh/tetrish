using UnityEngine;
using System.Collections;

public class MoveCam : MonoBehaviour {

	public GameObject cam;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float xAxisValue = Input.GetAxis("Horizontal");
		float yAxisValue = 0 ; //Input.GetAxis("Vertical");
		
		if(cam != null){
			cam.transform.Translate(new Vector3(xAxisValue/3, yAxisValue/3, 0.0f));
		}	

	}
}
