using UnityEngine;
using System.Collections;

public class BombardierOne : Bombardier
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
            m_BuildingColor = GameObject.FindGameObjectWithTag("Building").transform.Find("Point light one").GetComponent<Light>().color;
            m_TargetingRay = false;
            m_TargetingHit = false;
            m_TouchedBuilding = null;
            m_Ready = false;
            m_Stop = false;
        }

        void Update()
        {
            Debug.DrawRay(Camera.main.transform.position, transform.position - Camera.main.transform.position, Color.white);
            if (Physics.Raycast(transform.position, transform.position - Camera.main.transform.position, out m_Hit))
            {
                if (m_Hit.transform.gameObject.tag == "Building")
                {
                    m_TargetingRay = true;
                    m_TouchedBuilding = m_Hit.collider.gameObject;
                }
                else
                {
                    m_TargetingRay = false;
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
                m_TargetingHit = false;
            }

            if (m_Ready == false)
            {
                if (m_TargetingRay == true || m_TargetingHit == true)
                {
                    m_TouchedBuilding.transform.Find("Point light one").GetComponent<Light>().color = transform.Find("Point light").GetComponent<Light>().color;
                    m_TouchedBuilding.transform.Find("Point light two").GetComponent<Light>().intensity = 1;
                }
                else if (m_TouchedBuilding != null)
                {
                    m_TouchedBuilding.transform.Find("Point light one").GetComponent<Light>().color = m_BuildingColor;
                    m_TouchedBuilding.transform.Find("Point light one").GetComponent<Light>().intensity = 2;
                    m_TouchedBuilding.transform.Find("Point light two").GetComponent<Light>().intensity = 2;
                }
            }
            else
            {
                m_TouchedBuilding.transform.Find("Point light one").GetComponent<Light>().intensity = 3;
                m_TouchedBuilding.transform.Find("Point light two").GetComponent<Light>().intensity = 0;
            }
        }

        void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag == "Building")
            {
                m_TargetingHit = true;
                m_TouchedBuilding = collider.gameObject;
            }
        }

        void OnTriggerExit(Collider collider)
        {
            if (collider.gameObject.tag == "Building")
            {
                m_TargetingHit = false;
            }
        }

    #endregion
}
