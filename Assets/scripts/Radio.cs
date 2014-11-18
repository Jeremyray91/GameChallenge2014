using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Radio : MonoBehaviour {

	public bool detruit =false;
	public bool appel_radio = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        GetComponent<Light>().color = new Color(1.0f, 0.0f, 0.0f);
        GetComponent<Light>().enabled = true;
	
		if (this.detruit.Equals(true) && this.appel_radio.Equals(false)){
			//callback ici si le batiment est détruit sans que la radio soit declencher

		}

		else if(this.detruit.Equals(true) && this.appel_radio.Equals(true)){
			//callback ici si le batiment est détruit et qui contient une radio qui est delenché 


		}
		else {
			//callback ici si le batiment est detruit et qui ne contient pas la radio
		}

	}
}
