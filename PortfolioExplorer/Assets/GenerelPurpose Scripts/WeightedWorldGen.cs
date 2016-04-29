using UnityEngine;
using System.Collections.Generic;

public class WeightedWorldGen : MonoBehaviour
{
    //public List<WeightedTile> hi = new List<WeightedTile>();
    public System.Type hi;






    void Start()
    {
        
    }

    [System.Serializable]
    public class WeightedTile
    {
        public int intile;
        public List<int> hi = new List<int>();

        public WeightedTile(int tile)
        {

        }
    }
}
