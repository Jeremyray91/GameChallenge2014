using UnityEngine;
using System.Collections;

public class Survivor : MonoBehaviour {

    public int m_ID;
    public float m_Speed = 10.0f;


    private float m_Sensi = 0.5f;
    private bool m_IsDead = false;
    private GameObject m_MainCamera;
    private Light m_PointLight;

    private Color[] m_Colors;

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
        if (!IsDead()) {
            Vector2 direction2D = new Vector2(Input.GetAxis("Horizontal" + m_ID), Input.GetAxis("Vertical" + m_ID));
            if (direction2D.magnitude >= m_Sensi) {
                float angle = m_MainCamera.transform.rotation.eulerAngles.y;
                
                direction2D = Rotate(direction2D, -angle);
                direction2D.Normalize();
                Vector3 direction3D = new Vector3(direction2D.x, 0.0f, direction2D.y);

                transform.position = transform.position + direction3D * m_Speed * Time.deltaTime;

                RaycastHit hitInfo;
                if (Physics.Linecast(transform.position, m_MainCamera.transform.position, out hitInfo, Physics.DefaultRaycastLayers) == true) {
                    hitInfo.collider.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // A Effacer
                    /*Building building = hitInfo.collider.gameObject.GetComponent<Building>();
                    if (building != null) {
                        // TODO Ajouter la gestion du building
                    }*/
                }
            }
            
            if (Input.GetAxis("Red" + m_ID) == 1.0f) {
                print("Red" + m_ID);
                m_PointLight.color = m_Colors[0];
            } else if (Input.GetAxis("Green" + m_ID) == 1.0f) {
                m_PointLight.color = m_Colors[1];
            } else if (Input.GetAxis("Blue" + m_ID) == 1.0f) {
                m_PointLight.color = m_Colors[2];
            } else if (Input.GetAxis("Yellow" + m_ID) == 1.0f) {
                m_PointLight.color = m_Colors[3];
            }
        }
	}

    public bool IsDead() {
        return m_IsDead;
    }

    void OnTriggerEnter2D(Collider2D hit) {
        hit.collider.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // A Effacer
        /*Building building = hitInfo.collider.gameObject.GetComponent<Building>();
        if (building != null) {
            // TODO Ajouter la gestion du building
        }*/
    }
    void OnCollisionEnter(Collision collision) {
        collision.collider.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // A Effacer
        /*Building building = hitInfo.collider.gameObject.GetComponent<Building>();
        if (building != null) {
            // TODO Ajouter la gestion du building
        }*/
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
