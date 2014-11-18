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
                 GameObject m_BombOne;
                 GameObject m_BombTwo;
        public          int m_TimerPlayerTurn;
        public          int m_TimerBomberTurn;
                      float m_Timer;
                       bool m_Firing;
                       bool m_Mooving;

    #endregion

    #region MonoBehaviour

    void Start()
    {
        m_Bombardier = new GameObject[2];
        m_Bombardier[0] = GameObject.Find("BombardierOne");
        m_Bombardier[1] = GameObject.Find("BombardierTwo");
        m_Player = GameObject.FindGameObjectsWithTag("Player");
        m_Bot = GameObject.FindGameObjectsWithTag("Bot");
        m_BombOne = null;
        m_BombTwo = null;
        m_Firing = false;
        m_Mooving = false;

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
                Debug.Log(0);
                if (m_Timer >= m_TimerPlayerTurn)
                {
                    if (m_Mooving == false)
                    {
                        ChangeToBombardier();
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
                                    m_Mooving = false;
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
                Debug.Log(1);
                if (m_Timer >= m_TimerBomberTurn || (m_Bombardier[0].GetComponent<Bombardier>().m_Ready == true && m_Bombardier[1].GetComponent<Bombardier>().m_Ready == true))
                {
                    m_Bombardier[0].GetComponent<Bombardier>().m_Stop = true;
                    m_Bombardier[1].GetComponent<Bombardier>().m_Stop = true;

                    if (m_Firing == false)
                    {
                        ChangeToPlayer();
                    }

                    if (m_BombOne == null && m_BombTwo == null)
                    {
                        for (int i = 0; i < m_Player.Length; i++)
                        {
                            m_Player[i].GetComponent<Survivor>().StartNewTurn();
                        }
                        for (int i = 0; i < m_Bot.Length; i++)
                        {
                            m_Bot[i].GetComponent<Bot>().StartNewTurn();
                        }
                        m_Turn = 0;
                        m_Timer = 0;
                        m_Firing = false;
                    }
                }
                else
                {
                    m_Timer += Time.deltaTime;
                }
            }
            Debug.Log(m_Timer);
    }

    #endregion

    #region Core

    void ChangeToBombardier()
    {
        for (int i = 0; i < m_Player.Length; i++)
        {
            m_Player[i].GetComponent<Survivor>().GoToNearestBuilding();
        }
        for (int i = 0; i < m_Bot.Length; i++)
        {
            m_Bot[i].GetComponent<Bot>().GoToNearestBuilding();
        }
        m_Mooving = true;
    }

    void ChangeToPlayer()
    {
        if (m_Bombardier[0].GetComponent<Bombardier>().m_Ready == true && m_Bombardier[1].GetComponent<Bombardier>().m_Ready == true)
        {
            GameObject cloneOne = Instantiate(m_Bomb, m_Bombardier[0].GetComponent<BombardierOne>().GetTargetedBuilding().transform.position + new Vector3(0, 30, 0), Quaternion.identity) as GameObject;
            GameObject cloneTwo = Instantiate(m_Bomb, m_Bombardier[1].GetComponent<BombardierTwo>().GetTargetedBuilding().transform.position + new Vector3(0, 30, 0), Quaternion.identity) as GameObject;
            m_BombOne = cloneOne.gameObject;
            m_BombTwo = cloneTwo.gameObject;
            m_Firing = true;
        }
    }

    #endregion
}
