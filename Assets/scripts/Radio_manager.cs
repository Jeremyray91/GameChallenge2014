using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Radio_manager : MonoBehaviour {

	public int tour =0;
	public int radio_declencher=0;
    public GameObject[] m_batiments;

    private List<GameObject> m_AvailableBuildings;
	private GameObject radio_object=null;

	// Use this for initialization
	void Start () {

        m_batiments = GameObject.FindGameObjectsWithTag("Building");
        m_AvailableBuildings = new List<GameObject>(m_batiments);

        m_AvailableBuildings.Sort((a, b)=> 1 - 2* Random.Range(0, 2));
        ChangeRadio();
	}

	// Update is called once per frame
	void Update () {
		
		if (Input.GetKey(KeyCode.F)){
			endtour();
		}
		
	}
    /**
     * ATTENTION : selectionne une nouvelle radio.
     */
	void ChangeRadio() {
		
		//------------traitement de radio-------------------//
		if (radio_object != null){
			radio_object.GetComponent<Radio>().enabled = false; //radio supprimer 
		}
		radio_object = m_AvailableBuildings[0];
        m_AvailableBuildings.RemoveAt(0);
        radio_object.AddComponent<Radio>();
		//------------traitement de radio-------------------//

	}

	void endtour() {
        if (radio_object != null) {
		    //verfier si la tour à ete detruite ou pas 
		    if (radio_object.GetComponent<Radio>().detruit == true) {
				    //le batiment qui contient la radio est détruit
				    //retirer le batiment de la liste 
				    //et affecter la radio à un nouveau batiment

			    ChangeRadio();

            } else {
                Building building = radio_object.GetComponent<Building>();
                if ((building != null) && (!building.HasPlayer())) { //connaitre si il ya joueur dedans
                    radio_declencher++;
                    ChangeRadio();
                }
		    }
        }
	}
	

}
