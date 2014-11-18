using UnityEngine;
using System.Collections;

public class Turn : MonoBehaviour
{
    #region Members

        public          int m_Turn;
               GameObject[] m_Bombardier;
               GameObject[] m_Player;
               GameObject[] m_Bot;
        public   GameObject m_Bomb;
        public          int m_TimerPlayerTurn;
        public          int m_TimerBomberTurn;
                      float m_Timer;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        m_Bombardier = new GameObject[2];
        m_Bombardier[0] = GameObject.Find("BombardierOne");
        m_Bombardier[1] = GameObject.Find("BombardierTwo");
        m_Player = GameObject.FindGameObjectsWithTag("Player");
        m_Bot = GameObject.FindGameObjectsWithTag("Bot");

        m_Timer = 0;

        if (m_Turn == 0)
        {
            m_Bombardier[0].GetComponent<Bombardier>().m_Stop = true;
            m_Bombardier[1].GetComponent<Bombardier>().m_Stop = true;
        }
        else if (m_Turn == 1)
        {
            for (int i = 0; i < m_Player.Length; i++)
            {
                m_Player[i].GetComponent<Survivor>().GoToNearestBuilding();
            }
            for (int i = 0; i < m_Bot.Length; i++)
            {
                m_Bot[i].GetComponent<Bot>().GoToNearestBuilding();
            }
        }
    }

    void Update()
    {
        if (m_Turn == 0)
        {
            if (m_Timer >= m_TimerPlayerTurn)
            {
                for (int i = 0; i < m_Player.Length; i++)
                {
                    m_Player[i].GetComponent<Survivor>().GoToNearestBuilding();
                }
                for (int i = 0; i < m_Bot.Length; i++)
                {
                    m_Bot[i].GetComponent<Bot>().GoToNearestBuilding();
                }

                for (int i = 0; i < m_Player.Length; i++)
                {
                    if (!m_Player[i].GetComponent<Survivor>().IsInBuilding())
                    {
                        break;
                    }
                    else
                    {

                        for (int j = 0; j < m_Bot.Length; j++)
                        {
                            if (!m_Bot[j].GetComponent<Bot>().IsInBuilding())
                            {
                                break;
                            }

                            if (m_Bot[m_Bot.Length - 1].GetComponent<Bot>().IsInBuilding())
                            {
                                m_Bombardier[0].GetComponent<Bombardier>().m_Stop = false;
                                m_Bombardier[1].GetComponent<Bombardier>().m_Stop = false;
                                m_Bombardier[0].GetComponent<Bombardier>().m_Ready = false;
                                m_Bombardier[1].GetComponent<Bombardier>().m_Ready = false;
                                m_Turn = 1;
                                m_Timer = 0;
                            }
                        }
                    }
                }
            }
            else
            {
                m_Timer += Time.deltaTime;
            }
        }
        else if (m_Turn == 1)
        {

            if (m_Timer >= m_TimerBomberTurn || (m_Bombardier[0].GetComponent<Bombardier>().m_Ready == true && m_Bombardier[1].GetComponent<Bombardier>().m_Ready == true))
            {
                m_Bombardier[0].GetComponent<Bombardier>().m_Stop = true;
                m_Bombardier[1].GetComponent<Bombardier>().m_Stop = true;
                if (m_Bombardier[0].GetComponent<Bombardier>().m_Ready == true && m_Bombardier[1].GetComponent<Bombardier>().m_Ready == true)
                {
                    GameObject cloneOne = Instantiate(m_Bomb, m_Bombardier[0].transform.position + new Vector3(0, 30, 0), Quaternion.identity) as GameObject;
                    GameObject cloneTwo = Instantiate(m_Bomb, m_Bombardier[1].transform.position + new Vector3(0, 30, 0), Quaternion.identity) as GameObject;
                    m_Turn = 0;
                    m_Timer = 0;
                        for (int i = 0; i < m_Player.Length; i++)
                        {
                            m_Player[i].GetComponent<Survivor>().StartNewTurn();
                        }
                        for (int i = 0; i < m_Bot.Length; i++)
                        {
                            m_Bot[i].GetComponent<Bot>().StartNewTurn();
                        }
                    
                }
                else
                {
                    for (int i = 0; i < m_Player.Length; i++)
                    {
                        m_Player[i].GetComponent<Survivor>().StartNewTurn();
                    }
                    for (int i = 0; i < m_Bot.Length; i++)
                    {
                        m_Bot[i].GetComponent<Bot>().StartNewTurn();
                    }
                }
            }
            else
            {
                m_Timer += Time.deltaTime;
            }
        }
        //Debug.Log(m_Timer);
    }

    #endregion
}
