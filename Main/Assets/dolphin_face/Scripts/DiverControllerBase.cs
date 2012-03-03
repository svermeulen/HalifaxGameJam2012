using UnityEngine;
using System.Collections;

public abstract class DiverControllerBase : MonoBehaviour
{
    public float activationDistance = 5;

    protected bool isActive = false;
    protected GameObject player;
    protected bool isDying = false;

    public virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected abstract void OnActivate();

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isDying)
        {
            other.gameObject.SendMessage("TakeDamage", 1);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !isDying)
        {
            Messenger<string>.Invoke("OnPlaySFX", "Dolphin_Damage");
        }
    }

    public virtual void Update()
    {
        if (GameState.CurrentGameState != GameState.EGameState.GameState_Active) 
        { 
            return; 
        }

        if (!isActive)
        {
            float dist = Mathf.Abs(player.transform.position.y - transform.position.y);

            //Debugging.Instance.ShowText("dist: " + dist);
            if (dist < activationDistance)
            {
                OnActivate();
                isActive = true;
            }
        }
    }
}
