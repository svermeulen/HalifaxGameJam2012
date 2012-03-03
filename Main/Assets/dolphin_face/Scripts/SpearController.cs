using UnityEngine;
using System.Collections;

public class SpearController : MonoBehaviour
{
    public LinkedSpriteManager spriteManager;
    public float width = 1.0f;
    public float height = 1.0f;

    public float spearDamage = 5;

    // Use this for initialization
    void Start()
    {
        spriteManager.AddSprite(gameObject, width, height, 0, 127, 128, 128, false);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            var controller = other.gameObject.GetComponent<DolphinController>();
            controller.TakeDamage(spearDamage);
            Destroy(transform.parent.gameObject);
        }
    }
}
