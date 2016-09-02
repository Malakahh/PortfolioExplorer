using UnityEngine;
using System.Collections.Generic;

namespace NigelsAdventure
{
    [System.Serializable]
    public class ParchmentSection
    {
        #region Tiles
        public static readonly short water = 0;
        public static readonly short island = 1;
        #endregion

        #region Shapes
        #region Basic
        public static readonly short[] fullWater = new short[] { water, water, water, water, water };
        public static readonly short[] basicIsland_0 = new short[] { water, island, island, island, island };
        public static readonly short[] basicIsland_1 = new short[] { island, water, island, island, island };
        public static readonly short[] basicIsland_2 = new short[] { island, island, water, island, island };
        public static readonly short[] basicIsland_3 = new short[] { island, island, island, water, island };
        public static readonly short[] basicIsland_4 = new short[] { island, island, island, island, water };
        public static readonly short[] basicIsland_5 = new short[] { water, island, island, island, water };
        public static readonly short[] basicIsland_6 = new short[] { island, water, island, water, island };
        public static readonly short[] basicIsland_7 = new short[] { island, island, water, island, island };
        public static readonly short[] basicIsland_8 = new short[] { water, water, water, island, island };
        public static readonly short[] basicIsland_9 = new short[] { island, island, water, water, water };
        #endregion

        #region Complex
        public static readonly short[][] easySlalom_0 = new short[][] { fullWater, fullWater, basicIsland_9, fullWater, fullWater, basicIsland_8, fullWater, fullWater};
        public static readonly short[][] hardSlalom_0 = new short[][] { fullWater, fullWater, basicIsland_0, fullWater, fullWater, basicIsland_4, fullWater, fullWater};
        public static readonly short[][] easySlalom_1 = new short[][] { fullWater, fullWater, basicIsland_8, fullWater, fullWater, basicIsland_9, fullWater, fullWater};
        public static readonly short[][] hardSlalom_1 = new short[][] { fullWater, fullWater, basicIsland_4, fullWater, fullWater, basicIsland_0, fullWater, fullWater};
        public static readonly short[][] largeWater = new short[][] { fullWater, fullWater, fullWater, fullWater, fullWater, fullWater, fullWater, fullWater, fullWater, fullWater, fullWater, fullWater, fullWater, fullWater};
        #endregion
        #endregion

        public static Texture2D GenerateTexture(params short[][] args)
        {
            int width = TextureManager.Instance.NigelsAdventure.Tiles[0].width;
            int height = TextureManager.Instance.NigelsAdventure.Tiles[0].height;

            Texture2D texture = new Texture2D(
                args.Length * width,
                args[0].Length * height
            );

            for (int x = 0; x < args.Length; x++)
            {
                for (int y = 0; y < args[x].Length; y++)
                {
                    texture.SetPixels(
                        x * width,
                        y * height,
                        width,
                        height,
                        TextureManager.Instance.NigelsAdventure.Tiles[args[x][y]].GetPixels()
                    );
                }
            }

            texture.Apply();
            return texture;
        }
    }
}
