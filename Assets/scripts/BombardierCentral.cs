using UnityEngine;
using System.Collections;

public class BombardierCentral : MonoBehaviour
{
    #region Members

        public          int m_NbMunitions;
        public GameObject[] m_Bombardiers = new GameObject[2];
        public   GameObject m_Bomb;

    #endregion

    #region MonoBehaviour

        void Start()
        {

        }

        void Update()
        {
            if (m_Bombardiers[0].GetComponent<Bombardier>().m_Ready == true && m_Bombardiers[1].GetComponent<Bombardier>().m_Ready == true)
            {
                m_Bombardiers[0].GetComponent<Bombardier>().m_Stop = true;
                m_Bombardiers[1].GetComponent<Bombardier>().m_Stop = true;
                GameObject cloneOne = Instantiate(m_Bomb, m_Bombardiers[0].transform.position + new Vector3(0, 30, 0), Quaternion.identity) as GameObject;
                GameObject cloneTwo = Instantiate(m_Bomb, m_Bombardiers[1].transform.position + new Vector3(0, 30, 0), Quaternion.identity) as GameObject;
                m_NbMunitions -= 2;

                m_Bombardiers[0].GetComponent<Bombardier>().m_Stop = false;
                m_Bombardiers[1].GetComponent<Bombardier>().m_Stop = false;
                m_Bombardiers[0].GetComponent<Bombardier>().m_Ready = false;
                m_Bombardiers[1].GetComponent<Bombardier>().m_Ready = false;
            }
        }

    #endregion
}
