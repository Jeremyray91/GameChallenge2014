using UnityEngine;
using System.Collections;

public class Bot : MonoBehaviour {
    public float m_Speed = 10.0f;

    private bool m_IsDead = false;
    private GameObject m_MainCamera;
    private Light m_PointLight;
    private NavMeshAgent m_NavMeshComponent;

    private Color[] m_Colors;

    public float m_MinRandomColorChange;
    public float m_MaxRandomColorChange;
    private float m_NewRandomColorChange;

    private Vector3 m_TargetPosition;

    // Use this for initialization
    void Start() {
        m_MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        m_PointLight = GetComponent<Light>();
        m_NavMeshComponent = GetComponent<NavMeshAgent>();

        m_Colors = new Color[4] {
            new Color(1.0f, 0.0f, 0.0f), // RED
            new Color(0.0f, 1.0f, 0.0f), // GREEN
            new Color(0.0f, 0.0f, 1.0f), // BLUE
            new Color(0.96f, 0.81f, 0.08f) // YELOW
        };

        m_NewRandomColorChange = Random.value * (m_MaxRandomColorChange - m_MinRandomColorChange) + m_MinRandomColorChange;
        print(m_NewRandomColorChange);
        StartCoroutine(ChangeColor());

        GenerateNewTarget();
    }

    // Update is called once per frame
    void Update() {
        if (!IsDead()) {
            float dist = (transform.position - m_TargetPosition).magnitude;
            if (dist < 0.5) {
                GenerateNewTarget();
            }

            RaycastHit hitInfo;
            if (Physics.Linecast(transform.position, m_MainCamera.transform.position, out hitInfo, Physics.DefaultRaycastLayers) == true) {
                /*Building building = hitInfo.collider.gameObject.GetComponent<Building>();
                if (building != null) {
                    // TODO Ajouter la gestion du building
                }*/
            }
        }
    }

    private void GenerateNewTarget() {
        float z = Random.value * (50.0f + 30.0f) - 30.0f;
        float x = Random.value * (50.0f + 30.0f) - 30.0f;
        m_TargetPosition = new Vector3(x, 0.0f, z);
        m_NavMeshComponent.SetDestination(m_TargetPosition);
    }

    public bool IsDead() {
        return m_IsDead;
    }

    IEnumerator ChangeColor() {
        yield return new WaitForSeconds(m_NewRandomColorChange);

        int index = Mathf.RoundToInt(Random.value * 2.0f);
        m_PointLight.color = m_Colors[index];

        m_NewRandomColorChange = Random.value * (m_MaxRandomColorChange - m_MinRandomColorChange) + m_MinRandomColorChange;
        StartCoroutine(ChangeColor());
    }

    void OnTriggerEnter2D(Collider2D hit) {
        /*Building building = hitInfo.collider.gameObject.GetComponent<Building>();
        if (building != null) {
            // TODO Ajouter la gestion du building
        }*/
    }
    void OnCollisionEnter(Collision collision) {
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
