using UnityEngine;
using System.Collections;

public class zoom : MonoBehaviour {

	private bool zoomOut = false;
	public float zoomEnd;
	public float zoomSpeed;
	public bool autoStart = false;


	// Use this for initialization
	void Start () {
		if (autoStart) 
		{
			doTheZoom();
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (zoomOut) {
			float zoom = gameObject.GetComponent<Camera>().orthographicSize + zoomSpeed * Time.deltaTime;
			print(zoom);
			gameObject.GetComponent<Camera>().orthographicSize = zoom;
			print(gameObject.GetComponent<Camera>().orthographicSize);
			if (Mathf.Abs(zoom - zoomEnd) < 1 )
			{
				zoomOut = false;
			}
		}
	}

	public void doTheZoom(){
		zoomOut = true;
	}

}
