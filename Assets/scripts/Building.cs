using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Building : MonoBehaviour {


    // La liste des joueurs/Bot dans ce building
    private Dictionary<GameObject, int> m_Survivors;

    private Light m_LightComponent;
    private Material m_Material;

    private bool m_ForceLightOn = false;
    private bool m_IsDestroyed = false;

	// Use this for initialization
	void Start () {
        m_Survivors = new Dictionary<GameObject, int>();
        m_LightComponent = GetComponent<Light>();
        if (m_LightComponent != null) {
            m_LightComponent.color = new Color(1.0f, 1.0f, 1.0f);
            //m_LightComponent.enabled = false;
        }
        m_Material = renderer.material;
        if (m_Material != null) {
            m_Material.SetFloat("_ParamFloat1", 0.0f);
        
        }
	}
	
	// Update is called once per frame
	void Update () {
    	bool on = false;
	    foreach (KeyValuePair<GameObject, int> kvp in m_Survivors) {
	        on = on || (kvp.Value > 0) ;
	        if (on) {
	            break;
	        }
	    }
	    if (on) {
	        if (m_Material != null) {
	            m_Material.SetFloat("_ParamFloat1", 9.0f);
	        }
	    } else {
	        if (m_Material != null) {
	            m_Material.SetFloat("_ParamFloat1", 0.0f);
	        }
		}
		if (m_LightComponent != null) {

			if (on) print (on);
			//m_LightComponent.enabled = on || m_ForceLightOn;
        }
	}

    public void Enter(GameObject survivor) {
		print ("Enter");
        if (m_Survivors.ContainsKey(survivor)) {
            m_Survivors[survivor] += 1;
        } else {
            m_Survivors.Add(key: survivor, value: 1);
        }
    }


    public void Exit(GameObject survivor) {
        if (m_Survivors.ContainsKey(survivor)) {
            m_Survivors[survivor] -= 1;
            /*if (m_Survivors[survivor] < -1) {
                m_Survivors.Remove(survivor);
            }*/
        }
    }

    public bool HasPlayer() {
        bool hasPlayer = false;
        foreach (KeyValuePair<GameObject, int> kvp in m_Survivors) {
            if (kvp.Key.GetComponent<Survivor>() != null) {
                hasPlayer = hasPlayer || (kvp.Value > 0);
            }
        }

        return hasPlayer;
    }

    public void ForceLightOn(bool force) {
        m_ForceLightOn = force;
    }

    public void Destroy() {
        m_IsDestroyed = true;
    }

    public bool IsDestroyed() {
        return m_IsDestroyed;
    }
}
