using UnityEngine;
using System.Collections;

public class Survivor : MonoBehaviour {

    public int m_ID;
    public float m_Speed = 18.0f;


    private float m_Sensi = 0.5f;
    private bool m_IsDead = false;
    private GameObject m_MainCamera;
    private Light m_PointLight;

    private Color[] m_Colors;
    // Le buiding dans lequel on se trouve
    private GameObject m_Building = null;

    private bool m_GoToNearestBuilding = false;
    private GameObject m_NearestBuilding = null;

            bool m_GameOver;

	// Use this for initialization
	void Start () {
        m_MainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        m_PointLight = GetComponent<Light>();

        m_Colors = new Color[4] {
            new Color(1.0f, 0.0f, 0.0f), // RED
            new Color(0.0f, 1.0f, 0.0f), // GREEN
            new Color(0.0f, 0.0f, 1.0f), // BLUE
            new Color(0.96f, 0.81f, 0.08f) // YELOW
        };
	}
	
	// Update is called once per frame
	void Update () {
        m_GameOver = GameObject.Find("GameOver").GetComponent<WinConditions>().m_GameOver;
        if (!IsDead() || m_GameOver == false) {
            if (!m_GoToNearestBuilding) {
                Vector2 direction2D = new Vector2(Input.GetAxis("Horizontal" + m_ID), Input.GetAxis("Vertical" + m_ID));
                if (direction2D.magnitude >= m_Sensi) {
                    float angle = m_MainCamera.transform.rotation.eulerAngles.y;

                    direction2D = Rotate(direction2D, -angle);
                    //direction2D.Normalize();
                    Vector3 direction3D = new Vector3(direction2D.x, 0.0f, direction2D.y);

                    transform.position = transform.position + direction3D * m_Speed * Time.deltaTime;
                }

                if (Input.GetAxis("Red" + m_ID) == 1.0f) {
                    m_PointLight.color = m_Colors[0];
                } else if (Input.GetAxis("Green" + m_ID) == 1.0f) {
                    m_PointLight.color = m_Colors[1];
                } else if (Input.GetAxis("Blue" + m_ID) == 1.0f) {
                    m_PointLight.color = m_Colors[2];
                } else if (Input.GetAxis("Yellow" + m_ID) == 1.0f) {
                    m_PointLight.color = m_Colors[3];
                }
            } else {
                if (m_Building == null) {
                    Vector2 direction2D = m_NearestBuilding.transform.position - transform.position;
                    direction2D.Normalize();
                    Vector3 direction3D = new Vector3(direction2D.x, 0.0f, direction2D.y);

                    transform.position = transform.position + direction3D * m_Speed * Time.deltaTime;
                }
            }

            RaycastHit hitInfo;
            if (Physics.Linecast(transform.position, m_MainCamera.transform.position, out hitInfo, Physics.DefaultRaycastLayers) == true) {
                Building building = hitInfo.collider.gameObject.GetComponent<Building>();
                if ((m_Building == null) && (building != null)) {
                    m_Building = building.gameObject;
                    building.Enter(gameObject);
                }
            } else {
                if (Physics.Raycast(transform.position + new Vector3(0.0f, 1000.0f, 0.0f), new Vector3(0.0f, -1.0f, 0.0f), out hitInfo, 10000.0f, Physics.DefaultRaycastLayers) == true) {
                    Building building = hitInfo.collider.gameObject.GetComponent<Building>();
                    if ((building == null) && (m_Building != null)) {
                        m_Building.GetComponent<Building>().Exit(gameObject);
                        m_Building = null;
                    } else if ((m_Building == null) && (building != null)) {
                        m_Building = building.gameObject;
                        building.Enter(gameObject);
                    }
                } else {
                    if (m_Building != null) {
                        m_Building.GetComponent<Building>().Exit(gameObject);
                        m_Building = null;
                    }
                }
            }
        }
	}

    public bool IsDead() {
        return m_IsDead;
    }

    public void Kill()
    {
        m_IsDead = true;
        gameObject.SetActive(false);
    }

    public void GoToNearestBuilding() {
        if (m_Building == null) {
            GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
            float nearestDistance = float.MaxValue;
            GameObject nearest = null;
            foreach (GameObject building in buildings) {
                if (nearest == null) {
                    nearest = building;
                } else {
                    float dist = (gameObject.transform.position - building.transform.position).magnitude;
                    if (dist < nearestDistance) {
                        nearestDistance = dist;
                        nearest = building;
                    }
                }
            }
            m_NearestBuilding = nearest;
            m_GoToNearestBuilding = true;
        }
    }

    public void StartNewTurn() {
        m_GoToNearestBuilding = false;
        m_NearestBuilding = null;
    }

    public bool IsInBuilding() {
        return m_Building != null;
    }

    /**
     * Fonction utilitaire pour férer l'angle de déplacement des persos.
     **/
    public static Vector2 Rotate(Vector2 v, float degrees) {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);
        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
}
