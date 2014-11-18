using UnityEngine;
using System.Collections;

public class BombardierTwo : Bombardier
{
    #region Members

        RaycastHit m_Hit;
              bool m_TargetingRay;
              bool m_TargetingHit;
        GameObject m_TouchedBuilding;

    #endregion

    #region MonoBehaviour

        void Start()
        {
            m_Buildings = GameObject.FindGameObjectsWithTag("Building");
            m_BuildingColor = GameObject.FindGameObjectWithTag("Building").GetComponent<Light>().color;
            m_TargetingRay = false;
            m_TouchedBuilding = null;
            m_Ready = false;
            m_Stop = false;
        }

        void Update()
        {
            Debug.DrawRay(Camera.main.transform.position, transform.position - Camera.main.transform.position, Color.white);
            RaycastHit hitInfo;
            if (Physics.Linecast(Camera.main.transform.position, transform.position, out hitInfo, Physics.DefaultRaycastLayers) == true) {
                Building building = hitInfo.collider.gameObject.GetComponent<Building>();
                m_TouchedBuilding = building.gameObject;
                m_TargetingRay = true;
            } else {
                if (Physics.Raycast(transform.position + new Vector3(0.0f, 1000.0f, 0.0f), new Vector3(0.0f, -1.0f, 0.0f), out hitInfo, 10000.0f, Physics.DefaultRaycastLayers) == true) {
                    Building building = hitInfo.collider.gameObject.GetComponent<Building>();
                    if (building == null) {
                        m_TargetingRay = false;
                    } else {
                        m_TouchedBuilding = building.gameObject;
                        m_TargetingRay = true;
                    }
                } else {
                    m_TargetingRay = true;
                }
            }

            if (m_Stop == false)
            {
                if (m_Ready == false)
                {
                    if (Input.GetKey("up"))
                    {
                        transform.position += new Vector3(-25, 0, 25) * Time.deltaTime;
                    }
                    if (Input.GetKey("down"))
                    {
                        transform.position += new Vector3(25, 0, -25) * Time.deltaTime;
                    }
                    if (Input.GetKey("right"))
                    {
                        transform.position += new Vector3(25, 0, 25) * Time.deltaTime;
                    }
                    if (Input.GetKey("left"))
                    {
                        transform.position += new Vector3(-25, 0, -25) * Time.deltaTime;
                    }
                    if (Input.GetKeyDown(KeyCode.KeypadEnter) && (m_TargetingRay == true || m_TargetingHit == true))
                    {
                        m_Ready = true;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    m_Ready = false;
                }
            }

            if (m_TouchedBuilding == null) {
                m_TargetingRay = false;
            }

            if (m_Ready == false) {
                if (m_TargetingRay == true) {
                    m_BuildingColor = GameObject.FindGameObjectWithTag("Building").GetComponent<Light>().color;
                    m_TouchedBuilding.GetComponent<Light>().color = transform.Find("Point light").GetComponent<Light>().color;
                    //m_TouchedBuilding.GetComponent<Light>().intensity = 1;
                    m_TouchedBuilding.GetComponent<Building>().ForceLightOn(true);
                } else if (m_TouchedBuilding != null) {
                    m_TouchedBuilding.GetComponent<Light>().color = m_BuildingColor;
                    //m_TouchedBuilding.GetComponent<Light>().intensity = 2;
                    //m_TouchedBuilding.GetComponent<Light>().intensity = 2;
                    m_TouchedBuilding.GetComponent<Building>().ForceLightOn(false);
                    m_TouchedBuilding = null;
                }
            } else {
                //m_TouchedBuilding.GetComponent<Light>().intensity = 3;
                //m_TouchedBuilding.GetComponent<Light>().intensity = 0;
            }
        }

    #endregion
}
