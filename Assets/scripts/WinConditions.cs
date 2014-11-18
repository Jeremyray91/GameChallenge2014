using UnityEngine;
using System.Collections;

public class WinConditions : MonoBehaviour
{
    #region Members

                int m_NbPlayer;
                int m_NbRadioUsed;
        public bool m_GameOver;

    #endregion

    #region MonoBehaviour

        void Start () 
        {
            m_GameOver = false;
	    }
	
	    void Update () 
        {
	        m_NbPlayer = GameObject.FindGameObjectsWithTag("Player").Length;
            Debug.Log(m_NbPlayer);
            if (m_NbPlayer == 0)
            {
                m_GameOver = true;
                renderer.material.color = Color.green;
                renderer.enabled = true;
                Time.timeScale = 0;
            }
            m_NbRadioUsed = GetComponent<Radio_manager>().radio_declencher;
            if (m_NbRadioUsed == 3)
            {
                m_GameOver = true;
                renderer.material.color = Color.red;
                renderer.enabled = true;
                Time.timeScale = 0;
            }
	    }

    #endregion
}
