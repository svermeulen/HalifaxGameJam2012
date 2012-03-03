using UnityEngine;
using System.Collections;

public class DamagePlayer : MonoBehaviour
{
    public float playerDamage = 50;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.SendMessage("TakeDamage", playerDamage);
        }
        else if (other.gameObject.tag == "diver")
        {
            other.gameObject.SendMessage("OnDie", SendMessageOptions.DontRequireReceiver);
        }
    }
}
