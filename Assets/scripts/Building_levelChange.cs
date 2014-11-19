using UnityEngine;
using System.Collections;

public class Building_levelChange : MonoBehaviour {

	private GameObject[] players;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		bool allIn = true;
		players = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject player in players) {
			allIn = allIn && player.GetComponent<Survivor>().IsInBuilding();
		}
		if (allIn)
			loadLevel ();
	}

	void loadLevel() {
		Application.LoadLevel ("level-tuto02");
	}
}
