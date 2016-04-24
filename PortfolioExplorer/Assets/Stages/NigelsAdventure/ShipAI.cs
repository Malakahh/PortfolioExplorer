using UnityEngine;
using System.Collections;

public class ShipAI : MonoBehaviour {



	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        Vector3 movement = new Vector3(1, 0);
        float speed = 15;

	    if (Input.GetKey(KeyCode.UpArrow))
        {
            movement.y = 1;
            Debug.Log("Up");
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            movement.y = -1;
            Debug.Log("Down");
        }

        this.transform.position += movement * Time.deltaTime * speed;
        Camera.main.transform.position.Set(
            this.transform.position.x,
            this.transform.position.y,
            Camera.main.transform.position.z);
	}
}
