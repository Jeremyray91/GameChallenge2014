using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
    #region MonoBehaviour

        void OnTriggerEnter(Collider collider)
        {
            print("BOMB TRIGGER");
            if (collider.gameObject.tag == "Building")
            {
                collider.gameObject.GetComponent<Building>().Destroyed();
                Destroy(gameObject);
            }
        }

    #endregion
}
