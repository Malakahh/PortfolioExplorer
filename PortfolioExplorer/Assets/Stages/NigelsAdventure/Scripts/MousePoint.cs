using UnityEngine;
using System.Collections;

namespace NigelsAdventure
{
    public class MousePoint : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Debug.Log("Hit!");
                this.transform.position = hit.point;
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Whooop");
        }
    }
}