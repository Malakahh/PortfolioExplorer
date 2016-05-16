using UnityEngine;
using System.Linq;

namespace NigelsAdventure
{
    class WorldGen : MonoBehaviour
    {
        WeightedTreeNode<ParchmentSection> rootNode;
        Tile tileToPlace;

        void Start()
        {
            ParchmentSection.InitializeStatics();
            BuildWeightedTree();

            for (int i = 0; i < 10; i++)
            {
                PlaceTile();
            }
        }

        void BuildWeightedTree()
        {
            rootNode = new WeightedTreeNode<ParchmentSection>(null);

            ParchmentSection[] all = new ParchmentSection[]
            {
                ParchmentSection.fullWater,
                ParchmentSection.basicIsland_0,
                ParchmentSection.basicIsland_1,
                ParchmentSection.basicIsland_2,
                ParchmentSection.basicIsland_3,
                ParchmentSection.basicIsland_4,
                ParchmentSection.basicIsland_5,
                ParchmentSection.basicIsland_6,
                ParchmentSection.basicIsland_7,
                ParchmentSection.basicIsland_8,
                ParchmentSection.basicIsland_9,
                ParchmentSection.easySlalom_0,
                ParchmentSection.hardSlalom_0,
                ParchmentSection.easySlalom_1,
                ParchmentSection.hardSlalom_1
            };

            //TODO: Improve selection here.. maybe some AI?
            for (int i = 0; i < all.Length; i++)
            {
                rootNode.AddChild(all[i], 1);
                for (int j = 0; j < all.Length; j++)
                {
                    if (j > 0 && j < 11 && i > 0 && i < 11)
                    {
                        continue;
                    }

                    rootNode.Children[i].AddChild(all[j], 10);
                }
            }
        }

        void PlaceTile()
        {
            if (tileToPlace == null)
            {
                tileToPlace = ObjectPool.Instance.Acquire<NigelsAdventure.Tile>();
                //tileToPlace.ParchmentSection = rootNode.GetRandomChild().Value;
                //tileToPlace.ParchmentSection = ParchmentSection.start;
                tileToPlace.ParchmentSection = ParchmentSection.fullWater;

                PlaceTile();
            }

            tileToPlace.transform.localScale = new Vector3(tileToPlace.ParchmentSection.Definition.Count, tileToPlace.ParchmentSection.Definition[0].Length, 1);
            tileToPlace.transform.position = this.transform.position;
            tileToPlace.transform.position += new Vector3(tileToPlace.transform.localScale.x / 2, 0);
            this.transform.position += new Vector3(tileToPlace.ParchmentSection.Definition.Count, 0);

            GenerateColliders(tileToPlace);
            tileToPlace.MeshRenderer.material.mainTexture = tileToPlace.ParchmentSection.Texture;
            tileToPlace.gameObject.SetActive(true);

            Tile t = ObjectPool.Instance.Acquire<NigelsAdventure.Tile>();
            t.ParchmentSection = rootNode.Children.First(x => x.Value == tileToPlace.ParchmentSection).GetRandomChild().Value;
            tileToPlace = t;
        }

        void GenerateColliders(Tile t)
        {
            float colWidth = 1f / t.ParchmentSection.Definition.Count;
            
            for (int x = 0; x < t.ParchmentSection.Definition.Count; x++)
            {
                float colHeight = 1f / t.ParchmentSection.Definition[x].Length;

                for (int y = 0; y < t.ParchmentSection.Definition[x].Length; y++)
                {
                    if (t.ParchmentSection.Definition[x][y] != 0)
                    {
                        BoxCollider2D col = t.gameObject.AddComponent<BoxCollider2D>();

                        col.size = new Vector2(colWidth, colHeight);
                        col.offset = new Vector2(colWidth * x - ((x == 0) ? 0 : 0.5f - 0.5f * colWidth), y * colHeight - 0.4f);
                    }
                }
            }
        }
    }
}
