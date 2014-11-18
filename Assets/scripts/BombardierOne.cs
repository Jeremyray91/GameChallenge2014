using UnityEngine;
using System.Collections;

public class BombardierOne : Bombardier
{
    #region Members

        RaycastHit m_Hit;
              bool m_TargetingRay;
              bool m_TargetingHit;
        GameObject m_TouchedBuilding;
              bool m_GameOver;

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
            m_GameOver = GameObject.Find("GameOver").GetComponent<WinConditions>().m_GameOver;
            if (m_GameOver == false)
            {
                Debug.DrawRay(Camera.main.transform.position, transform.position - Camera.main.transform.position, Color.white);
                RaycastHit hitInfo;
                if (Physics.Linecast(Camera.main.transform.position, transform.position, out hitInfo, Physics.DefaultRaycastLayers) == true)
                {
                    Building building = hitInfo.collider.gameObject.GetComponent<Building>();
                    if (building == null)
                    {
                        m_TargetingRay = false;
                    }
                    else
                    {
                        m_TouchedBuilding = building.gameObject;
                        m_TargetingRay = true;
                    }
                }
                else
                {
                    if (Physics.Raycast(transform.position + new Vector3(0.0f, 1000.0f, 0.0f), new Vector3(0.0f, -1.0f, 0.0f), out hitInfo, 10000.0f, Physics.DefaultRaycastLayers) == true)
                    {
                        Building building = hitInfo.collider.gameObject.GetComponent<Building>();
                        if (building == null)
                        {
                            m_TargetingRay = false;
                        }
                        else
                        {
                            m_TouchedBuilding = building.gameObject;
                            m_TargetingRay = true;
                        }
                    }
                    else
                    {
                        m_TargetingRay = true;
                    }
                }

                if (m_Stop == false)
                {
                    if (m_Ready == false)
                    {
                        if (Input.GetKey("z"))
                        {
                            transform.position += new Vector3(-25, 0, 25) * Time.deltaTime;
                        }
                        if (Input.GetKey("s"))
                        {
                            transform.position += new Vector3(25, 0, -25) * Time.deltaTime;
                        }
                        if (Input.GetKey("d"))
                        {
                            transform.position += new Vector3(25, 0, 25) * Time.deltaTime;
                        }
                        if (Input.GetKey("q"))
                        {
                            transform.position += new Vector3(-25, 0, -25) * Time.deltaTime;
                        }
                        if (Input.GetKeyDown(KeyCode.Space) && (m_TargetingRay == true || m_TargetingHit == true))
                        {
                            m_Ready = true;
                        }
                    }
                    else if (Input.GetKeyDown(KeyCode.Space))
                    {
                        m_Ready = false;
                    }
                }

                if (m_TouchedBuilding == null)
                {
                    m_TargetingRay = false;
                }

                if (m_Ready == false)
                {
                    if (m_TargetingRay == true)
                    {
                        transform.Find("Cursor").renderer.material.SetFloat("_select", 1);
                    }
                    else if (m_TouchedBuilding != null)
                    {
                        transform.Find("Cursor").renderer.material.SetFloat("_select", 0);
                    }
                }
            }
        }

    #endregion

    #region Core

        public GameObject GetTargetedBuilding()
        {
            return m_TouchedBuilding;
        }

    #endregion
}
