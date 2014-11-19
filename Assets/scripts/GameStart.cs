using UnityEngine;
using System.Collections;

public class GameStart : MonoBehaviour
{
    #region Members

               GameObject[] m_Player;
        public   GameObject m_Bot;
                        int m_CheckHide;
        public         bool m_HasStarted;
        public float m_TimeBeforeBotSpawn;
        public int m_BotPerPlayer;

    #endregion

    #region MonoBehaviour

        void Start()
        {
            m_Player = GameObject.FindGameObjectsWithTag("Player");
            m_CheckHide = 0;
            m_HasStarted = true;
            GetComponent<Turn>().enabled = true;

            StartCoroutine(SpawnBots());
        }

        void Update()
        {
            /*if (m_HasStarted == false)
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
                            Instantiate(m_Bot, m_Player[i].transform.position, Quaternion.identity);
                        }
                    }
                    m_HasStarted = true;
                    GetComponent<Turn>().enabled = true;
                }
            }*/

        }

        IEnumerator SpawnBots() {
            yield return new WaitForSeconds(m_TimeBeforeBotSpawn);

            for (int i = 0; i < m_Player.Length; i++) {
                for (int j = 0; j < m_BotPerPlayer; j++) {
                    Instantiate(m_Bot, m_Player[i].transform.position, Quaternion.identity);
                }
            }
        }

    #endregion
}
