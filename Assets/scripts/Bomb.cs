using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
    #region MonoBehaviour

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Building")
            {
                collision.gameObject.GetComponent<Building>().Destroyed();
                Destroy(gameObject);
            }
        }

    #endregion
}
