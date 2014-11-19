using UnityEngine;
using System.Collections;

public class Bot_tuto : MonoBehaviour {

	public float m_Speed = 18.0f;
	
	private bool m_IsDead = false;
	private GameObject m_MainCamera;
	private Light m_PointLight;
	private NavMeshAgent m_NavMeshComponent;
	
	private Color[] m_Colors;
	private int nextColor = 0;

	private int etape = 0;
	private int secconds = 2;
	
	private Vector3 m_TargetPosition;
	
	private GameObject m_Building = null;
	private Vector3 posInit;
	
	private bool m_GoToNearestBuilding = false;
	private GameObject m_NearestBuilding = null;

	private bool bouge = false;
	
	// Use this for initialization
	void Start() {
		m_MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
		m_PointLight = GetComponent<Light>();
		m_NavMeshComponent = GetComponent<NavMeshAgent>();
		m_NavMeshComponent.speed = m_Speed;
		
		m_Colors = new Color[4] {
			new Color(0.0f, 1.0f, 0.0f), // GREEN
			new Color(1.0f, 0.0f, 0.0f), // RED
			new Color(0.96f, 0.81f, 0.08f), // YELLOW
			new Color(0.0f, 0.0f, 1.0f) // BLUE
		};
		transform.Find ("bouton_A").renderer.material.SetFloat ("_A", 0);
		transform.Find ("bouton_B").renderer.material.SetFloat ("_B", 0);
		transform.Find ("bouton_Y").renderer.material.SetFloat ("_Y", 0);
		transform.Find ("bouton_X").renderer.material.SetFloat ("_X", 0);

		posInit = gameObject.transform.position;

		StartCoroutine(switchStates());
	}
	
	// Update is called once per frame
	void Update() {

		Vector3 direction3D;
		float magn;
		
		switch (etape) {
			case 0: 
				parle();
				break;
			case 1: 
				print ("case 1");
				afficheBouton(nextColor);
				ChangeColor(nextColor);
				break;
			case 2: 
				print ("case 2");
				bouge = true;
				if (m_NearestBuilding == null) GoToNearestBuilding();
				direction3D = m_NearestBuilding.transform.position - transform.position;
				direction3D.y = 0.0f;
				direction3D.Normalize ();
				
					transform.position = transform.position + direction3D * m_Speed * Time.deltaTime;
				if ((m_NearestBuilding.transform.position - transform.position).magnitude < 0.5){
					m_NearestBuilding = null;
					etape++;
				}
				break;
			case 3: 
				print ("case 3");
				direction3D = posInit - transform.position;
				direction3D.y = 0.0f;
				direction3D.Normalize ();
				magn = (posInit - transform.position).magnitude;
				transform.position = transform.position + direction3D * m_Speed * Time.deltaTime;
				if ((magn < 3) || (magn < m_Speed * Time.deltaTime))
				{
					etape = 0;
					nextColor = 0;	
					StartCoroutine(switchStates());
					bouge = false;
				}
				break;
		default:
			break;
		}
		if (bouge) {
			RaycastHit hitInfo;
			if (Physics.Linecast (transform.position, m_MainCamera.transform.position, out hitInfo, Physics.DefaultRaycastLayers) == true) {
				Building building = hitInfo.collider.gameObject.GetComponent<Building> ();
				if ((m_Building == null) && (building != null)) {
					m_Building = building.gameObject;
					building.Enter (gameObject);
					m_PointLight.enabled = false;
				}
			} else {
				if (Physics.Raycast (transform.position + new Vector3 (0.0f, 1000.0f, 0.0f), new Vector3 (0.0f, -1.0f, 0.0f), out hitInfo, 10000.0f, Physics.DefaultRaycastLayers) == true) {
					Building building = hitInfo.collider.gameObject.GetComponent<Building> ();
					if ((building == null) && (m_Building != null)) {
						m_Building.GetComponent<Building> ().Exit (gameObject);
						m_Building = null;
						m_PointLight.enabled = true;
					} else if ((m_Building == null) && (building != null)) {
						m_Building = building.gameObject;
						building.Enter (gameObject);
						m_PointLight.enabled = false;
					}
				} else {
					if (m_Building != null) {
						m_Building.GetComponent<Building> ().Exit (gameObject);
						m_Building = null;
						m_PointLight.enabled = true;
					}
				}
			}	
		}
	}	
	
	private void ChangeColor(int color) {
		if (color < m_Colors.Length)
			m_PointLight.color = m_Colors[color];
	}

	private void afficheBouton(int color) {
		switch (color) {
		case 0: 
			transform.Find ("bouton_A").renderer.material.SetFloat ("_A", 9);
			break;
		case 1: 
			transform.Find ("bouton_A").renderer.material.SetFloat ("_A", 0);
			transform.Find ("bouton_B").renderer.material.SetFloat ("_B", 9);
			break;
		case 2: 
			transform.Find ("bouton_B").renderer.material.SetFloat ("_B", 0);
			transform.Find ("bouton_Y").renderer.material.SetFloat ("_Y", 9);
			break;
		case 3: 
			transform.Find ("bouton_Y").renderer.material.SetFloat ("_Y", 0);
			transform.Find ("bouton_X").renderer.material.SetFloat ("_X", 9);
			break;
		case 4:
			transform.Find ("bouton_X").renderer.material.SetFloat ("_X", 0);
			break;
		default:
			break;
		}
	}
	
	public void GoToNearestBuilding() {
		if (m_Building == null) {
			GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
			float nearestDistance = float.MaxValue;
			GameObject nearest = null;
			foreach (GameObject building in buildings) {
                if (!building.GetComponent<Building>().get_IsDestroyed()) {
					if (nearest == null) {
						nearest = building;
						nearestDistance = (gameObject.transform.position - building.transform.position).magnitude;
					} else {
						float dist = (gameObject.transform.position - building.transform.position).magnitude;
						if (dist < nearestDistance) {
							nearestDistance = dist;
							nearest = building;
						}
					}
				}
			}
			m_NearestBuilding = nearest;
			m_GoToNearestBuilding = true;
			if (m_NavMeshComponent != null) {
				//print("Stop ?");
				m_NavMeshComponent.enabled = false;
			}
		}
	}

	public void parle(){
		//audio.clip
	}


	IEnumerator switchStates() {
			yield return new WaitForSeconds(secconds);
			secconds = 1;

			if (etape < 2) {
				if (etape > 0 && nextColor < 4) {
					nextColor++;
				} else {
					etape++;
				}
				StartCoroutine(switchStates());
			};
	}

	
	public bool IsInBuilding() {
		return m_Building != null;
	}
}
