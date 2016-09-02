using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using AStar = Pathfinding.AStar<UnityEngine.Vector3, string>;

namespace NigelsAdventure
{
    class WorldGen : MonoBehaviour
    {
        WeightedTreeNode<short[][]> rootNode;
        Tile tileToPlace;
        List<Tile> tilesPlaced = new List<Tile>();
        Graphs.DirectedGraph<Vector3, string> graph = new Graphs.DirectedGraph<Vector3, string>();

        void Start()
        {
            BuildWeightedTree();

            for (int i = 0; i < 10; i++)
            {
                PlaceTile();
            }

            Debug.Log(graph.ToString());
            //AStar.FindPath()
        }

        void BuildWeightedTree()
        {
            rootNode = new WeightedTreeNode<short[][]>(null);

            short[][][] all = new short[][][]
            {
                ParchmentSection.easySlalom_0,
                ParchmentSection.hardSlalom_0,
                ParchmentSection.easySlalom_1,
                ParchmentSection.hardSlalom_1,
                ParchmentSection.largeWater
            };

            //TODO: Improve selection here.. maybe some AI?
            for (int i = 0; i < all.Length; i++)
            {
                rootNode.AddChild(all[i], 1);
                for (int j = 0; j < all.Length; j++)
                {
                    rootNode.Children[i].AddChild(all[j], 10);
                }
            }

        }

        void PlaceTile()
        {
            if (tileToPlace == null)
            {
                tileToPlace = ObjectPool.Instance.Acquire<NigelsAdventure.Tile>();
                tileToPlace.ParchmentSection = ParchmentSection.largeWater;

                PlaceTile();
                return;
            }

            tileToPlace.transform.localScale = new Vector3(tileToPlace.ParchmentSection.Length, tileToPlace.ParchmentSection[0].Length, 1);
            tileToPlace.transform.position = this.transform.position;
            tileToPlace.transform.position += new Vector3(tileToPlace.transform.localScale.x / 2, 0);
            this.transform.position += new Vector3(tileToPlace.ParchmentSection.Length, 0);

            GeneratePathfindingGraph(tileToPlace);
            tileToPlace.MeshRenderer.material.mainTexture = ParchmentSection.GenerateTexture(tileToPlace.ParchmentSection);
            tileToPlace.gameObject.SetActive(true);

            Tile t = ObjectPool.Instance.Acquire<NigelsAdventure.Tile>();
            t.ParchmentSection = rootNode.Children.First(x => x.Value == tileToPlace.ParchmentSection).GetRandomChild().Value;
            tileToPlace = t;
        }

        Vector3 CalcSquarePos(int x, int y, float width, float height)
        {
            //Some bugs here. It appears to generate to same result for center tile column in two cases. Its actual case, and x == 0
            return new Vector3(width * x - ((x == 0) ? 0 : 0.5f - 0.5f * width), y * height - 0.4f);
        }

        void GeneratePathfindingGraph(Tile t)
        {
            float sqWidth = 1f / t.ParchmentSection.Length;

            for (int x = 0; x < t.ParchmentSection.Length; x++)
            {
                float sqHeight = 1f / t.ParchmentSection[x].Length;

                for (int y = 0; y < t.ParchmentSection[x].Length; y++)
                {
                    if (t.ParchmentSection[x][y] == ParchmentSection.water) //Generate pathfinding node and edges
                    {
                        AStar.Node current = new AStar.Node(t.transform.position + CalcSquarePos(x ,y, sqWidth, sqHeight));
                        graph.AddNode(current);

                        ///
                        // Add edges to previously created nodes
                        ///
                        
                        if (y > 0 && t.ParchmentSection[x][y - 1] == ParchmentSection.water) //Top
                        {
                            Graphs.DirectedGraphNode<Vector3> from = graph.GetNode(t.transform.position + CalcSquarePos(x, y - 1, sqWidth, sqHeight));
                            graph.AddEdge(from, current, new AStar.EdgeData(from.data.ToString() + " -> " + current.data.ToString(), 1));
                        }

                        if (y > 0 && x > 0 && t.ParchmentSection[x - 1][y - 1] == ParchmentSection.water) //Top Left
                        {
                            Graphs.DirectedGraphNode<Vector3> from = graph.GetNode(t.transform.position + CalcSquarePos(x - 1, y - 1, sqWidth, sqHeight));
                            graph.AddEdge(from, current, new AStar.EdgeData(from.data.ToString() + " -> " + current.data.ToString(), 1.4));
                        }

                        if (x > 0 && t.ParchmentSection[x - 1][y] == ParchmentSection.water) //Left
                        {
                            Graphs.DirectedGraphNode<Vector3> from = graph.GetNode(t.transform.position + CalcSquarePos(x - 1, y, sqWidth, sqHeight));
                            graph.AddEdge(from, current, new AStar.EdgeData(from.data.ToString() + " -> " + current.data.ToString(), 1));
                        }

                        if (y + 1 < t.ParchmentSection[x].Length && x > 0 && t.ParchmentSection[x - 1][y + 1] == ParchmentSection.water) //Bottom Left
                        {
                            Graphs.DirectedGraphNode<Vector3> from = graph.GetNode(t.transform.position + CalcSquarePos(x - 1, y + 1, sqWidth, sqHeight));
                            graph.AddEdge(from, current, new AStar.EdgeData(from.data.ToString() + " -> " + current.data.ToString(), 1.4));
                        }

                        //Case of previous tile...
                        if (tilesPlaced.Count > 0)
                        {
                            Tile previousTile = tilesPlaced.Last();
                            Debug.Log(graph.ToString());
                            if (y > 0 && x == 0 && previousTile.ParchmentSection[previousTile.ParchmentSection.Length - 1][y - 1] == ParchmentSection.water) //Top Left
                            {
                                Graphs.DirectedGraphNode<Vector3> from = graph.GetNode(previousTile.transform.position + CalcSquarePos(previousTile.ParchmentSection.Length - 1, y - 1, sqWidth, sqHeight));
                                graph.AddEdge(from, current, new AStar.EdgeData(from.data.ToString() + " -> " + current.data.ToString(), 1.4));
                            }

                            if (x == 0 && previousTile.ParchmentSection[previousTile.ParchmentSection.Length - 1][y] == ParchmentSection.water) //Left
                            {
                                Graphs.DirectedGraphNode<Vector3> from = graph.GetNode(previousTile.transform.position + CalcSquarePos(previousTile.ParchmentSection.Length - 1, y, sqWidth, sqHeight));
                                graph.AddEdge(from, current, new AStar.EdgeData(from.data.ToString() + " -> " + current.data.ToString(), 1));
                            }

                            if (y + 1 < previousTile.ParchmentSection[x].Length && x == 0 && previousTile.ParchmentSection[previousTile.ParchmentSection.Length - 1][y + 1] == ParchmentSection.water) //Bottom Left
                            {
                                Graphs.DirectedGraphNode<Vector3> from = graph.GetNode(previousTile.transform.position + CalcSquarePos(previousTile.ParchmentSection.Length - 1, y + 1, sqWidth, sqHeight));
                                graph.AddEdge(from, current, new AStar.EdgeData(from.data.ToString() + " -> " + current.data.ToString(), 1.4));
                            }
                        }
                    }
                    else if (t.ParchmentSection[x][y] == ParchmentSection.island) //place collider
                    {
                        BoxCollider2D col = t.gameObject.AddComponent<BoxCollider2D>();
                        //col.isTrigger = true;
                        col.size = new Vector2(sqWidth, sqHeight);
                        col.offset = CalcSquarePos(x, y, sqWidth, sqHeight);
                    }
                    
                }
            }

            tilesPlaced.Add(t);
        }

        void GenerateColliders(Tile t)
        {
            float colWidth = 1f / t.ParchmentSection.Length;
            
            for (int x = 0; x < t.ParchmentSection.Length; x++)
            {
                float colHeight = 1f / t.ParchmentSection[x].Length;

                for (int y = 0; y < t.ParchmentSection[x].Length; y++)
                {
                    if (t.ParchmentSection[x][y] != 0)
                    {
                        BoxCollider2D col = t.gameObject.AddComponent<BoxCollider2D>();
                        //col.isTrigger = true;
                        col.size = new Vector2(colWidth, colHeight);
                        col.offset = new Vector2(colWidth * x - ((x == 0) ? 0 : 0.5f - 0.5f * colWidth), y * colHeight - 0.4f);
                    }
                }
            }
        }
    }
}
