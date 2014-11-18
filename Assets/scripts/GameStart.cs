using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour
{
    #region Members

               GameObject[] m_Player;
        public   GameObject m_Bot;
                        int m_CheckHide;
        public         bool m_HasStarted;

    #endregion

    #region MonoBehaviour

        void Start()
        {
            m_Player = GameObject.FindGameObjectsWithTag("Player");
            m_CheckHide = 0;
            m_HasStarted = false;
        }

        void Update()
        {
            if (m_HasStarted == false)
            {
                m_CheckHide = 0;
                for (int i = 0; i < m_Player.Length; i++)
                {
                    if (m_Player[i].GetComponent<Survivor>().IsInBuilding() == false)
                    {
                        m_CheckHide -= 1;
                    }
                    else
                    {
                        m_CheckHide += 1;
                    }
                }

                if (m_CheckHide >= m_Player.Length)
                {
                    for (int i = 0; i < m_Player.Length; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            GameObject clone = Instantiate(m_Bot, m_Player[i].transform.position, Quaternion.identity) as GameObject;
                        }
                    }
                    m_HasStarted = true;
                    GetComponent<Turn>().enabled = true;
                }
            }
        }

    #endregion
}
