using UnityEngine;
using System;
using System.Collections.Generic;

public class ParchmentSection
{
    #region Shapes
    #region Basic
    public static ParchmentSection fullWater;
    public static ParchmentSection basicIsland_0;
    public static ParchmentSection basicIsland_1;
    public static ParchmentSection basicIsland_2;
    public static ParchmentSection basicIsland_3;
    public static ParchmentSection basicIsland_4;
    public static ParchmentSection basicIsland_5;
    public static ParchmentSection basicIsland_6;
    public static ParchmentSection basicIsland_7;
    public static ParchmentSection basicIsland_8;
    public static ParchmentSection basicIsland_9;
    #endregion

    #region Complex
    public static ParchmentSection easySlalom0;
    public static ParchmentSection hardSlalom0;
    public static ParchmentSection easySlalom1;
    public static ParchmentSection hardSlalom1;
    #endregion
    #endregion

    public List<short[]> Definition;
    public Texture2D Texture;

    public ParchmentSection(params short[][] args)
    {
        Definition = new List<short[]>(args);

        GenerateTexture();
    }

    public ParchmentSection(params ParchmentSection[] args)
    {
        Definition = new List<short[]>();

        for (int i = 0; i < args.Length; i++)
        {
            Definition.AddRange(args[i].Definition);
        }

        GenerateTexture();
    }

    void GenerateTexture()
    {
        int width = TextureManager.Instance.NigelsAdventure.Tiles[0].width;
        int height = TextureManager.Instance.NigelsAdventure.Tiles[0].height;

        Texture = new Texture2D(
            Definition.Count * width,
            Definition[0].Length * height
        );

        for (int x = 0; x < Definition.Count; x++)
        {
            for (int y = 0; y < Definition[x].Length; y++)
            {
                int texId = Definition[x][y];

                Texture.SetPixels(
                    x * width,
                    y * height,
                    width,
                    height,
                    TextureManager.Instance.NigelsAdventure.Tiles[texId].GetPixels()
                );
            }
        }
    }

    public static void InitializeStatics()
    {
        fullWater = new ParchmentSection(new short[] { 0, 0, 0, 0, 0 });
        basicIsland_0 = new ParchmentSection(new short[] { 0, 1, 1, 1, 1 });
        basicIsland_1 = new ParchmentSection(new short[] { 1, 0, 1, 1, 1 });
        basicIsland_2 = new ParchmentSection(new short[] { 1, 1, 0, 1, 1 });
        basicIsland_3 = new ParchmentSection(new short[] { 1, 1, 1, 0, 1 });
        basicIsland_4 = new ParchmentSection(new short[] { 1, 1, 1, 1, 0 });
        basicIsland_5 = new ParchmentSection(new short[] { 0, 1, 1, 1, 0 });
        basicIsland_6 = new ParchmentSection(new short[] { 1, 0, 1, 0, 1 });
        basicIsland_7 = new ParchmentSection(new short[] { 1, 1, 0, 1, 1 });
        basicIsland_8 = new ParchmentSection(new short[] { 0, 0, 0, 1, 1 });
        basicIsland_9 = new ParchmentSection(new short[] { 1, 1, 0, 0, 0 });
        easySlalom0 = new ParchmentSection(fullWater, fullWater, basicIsland_9, fullWater, fullWater, basicIsland_8, fullWater, fullWater);
        hardSlalom0 = new ParchmentSection(fullWater, fullWater, basicIsland_0, fullWater, fullWater, basicIsland_4, fullWater, fullWater);
        easySlalom1 = new ParchmentSection(fullWater, fullWater, basicIsland_8, fullWater, fullWater, basicIsland_9, fullWater, fullWater);
        hardSlalom1 = new ParchmentSection(fullWater, fullWater, basicIsland_4, fullWater, fullWater, basicIsland_0, fullWater, fullWater);
    }
}
