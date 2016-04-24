using UnityEngine;
using System.Collections.Generic;

public class TextureManager : MonoBehaviour {
    public static TextureManager Instance;

    public NigelsAdventureContainer NigelsAdventure = new TextureManager.NigelsAdventureContainer();

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(Instance);
    }
    
    [System.Serializable]
    public class NigelsAdventureContainer
    {
        public List<Texture2D> Tiles;
    }
}
