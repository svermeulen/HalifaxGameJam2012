using UnityEngine;
using System.Collections;

public class Stalact : MonoBehaviour {
	
	public GameObject pieces;
	
	public float acceleration;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += Vector3.down * acceleration * Time.deltaTime;
	}
	
	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "level")
		{
			GameObject.Instantiate(pieces, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
}
