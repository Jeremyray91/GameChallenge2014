using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Radio_manager : MonoBehaviour {

	public int tour =0;
	public static int radio_declencher=0;
    private GameObject[] m_batiments;

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
	}
    /**
     * ATTENTION : selectionne une nouvelle radio.
     */
	void ChangeRadio() {
		
		//------------traitement de radio-------------------//
		if (radio_object != null){
			radio_object.GetComponent<Radio>().enabled = false; //radio supprimer
			Destroy(radio_object.GetComponent<Radio>());
			radio_object.GetComponent<Light>().enabled = false;
        }
        bool foundBuilding = false;
        while ((!foundBuilding) && (m_AvailableBuildings.Count > 0)) {
            radio_object = m_AvailableBuildings[0];
            m_AvailableBuildings.RemoveAt(0);
            if (!radio_object.GetComponent<Building>().get_IsDestroyed()) {
                radio_object.AddComponent<Radio>();
                radio_object.GetComponent<Building>().contient_radio = true;
                foundBuilding = true;
            }
        }
		//------------traitement de radio-------------------//

	}

	public void endtour() {
        if (radio_object != null) {
		    //verfier si la tour à ete detruite ou pas 
		    if (radio_object.GetComponent<Building>().get_IsDestroyed() == true) {
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
