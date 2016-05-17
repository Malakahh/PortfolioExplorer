using UnityEngine;
using System.Collections;

namespace NigelsAdventure
{
    public class Tile : MonoBehaviour
    {
        public ParchmentSection ParchmentSection;
        public MeshRenderer MeshRenderer;


	    // Use this for initialization
	    void Start () {
	
	    }
	
	    // Update is called once per frame
	    void Update () {
	
	    }

        void OnTriggerEnter(Collider col)
        {
            Debug.Log("OnTriggerEnter");
        }
    }
}
