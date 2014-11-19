using UnityEngine;
using System.Collections;

public class Bomb : MonoBehaviour
{
    #region MonoBehaviour

        void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.tag == "Building")
            {
                Destroy(collider.gameObject);
                Destroy(gameObject);
            }
        }

    #endregion
}
