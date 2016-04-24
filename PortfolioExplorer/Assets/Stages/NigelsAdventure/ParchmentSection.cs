using UnityEngine;
using System;
using System.Collections.Generic;

public class ParchmentSection
{
    #region Shapes
    #region Basic
    public static ParchmentSection fullWater = new ParchmentSection(new short[] { 0, 0, 0, 0, 0 });
    public static ParchmentSection basicIsland_0 = new ParchmentSection(new short[] { 0, 1, 1, 1, 1 });
    public static ParchmentSection basicIsland_1 = new ParchmentSection(new short[] { 1, 0, 1, 1, 1 });
    public static ParchmentSection basicIsland_2 = new ParchmentSection(new short[] { 1, 1, 0, 1, 1 });
    public static ParchmentSection basicIsland_3 = new ParchmentSection(new short[] { 1, 1, 1, 0, 1 });
    public static ParchmentSection basicIsland_4 = new ParchmentSection(new short[] { 1, 1, 1, 1, 0 });
    public static ParchmentSection basicIsland_5 = new ParchmentSection(new short[] { 0, 1, 1, 1, 0 });
    public static ParchmentSection basicIsland_6 = new ParchmentSection(new short[] { 1, 0, 1, 0, 1 });
    public static ParchmentSection basicIsland_7 = new ParchmentSection(new short[] { 1, 1, 0, 1, 1 });
    public static ParchmentSection basicIsland_8 = new ParchmentSection(new short[] { 0, 0, 0, 1, 1 });
    public static ParchmentSection basicIsland_9 = new ParchmentSection(new short[] { 1, 1, 0, 0, 0 });
    #endregion

    #region Complex
    public static ParchmentSection easySlalom0 = new ParchmentSection(fullWater, fullWater, basicIsland_9, fullWater, fullWater, basicIsland_8, fullWater, fullWater);
    public static ParchmentSection hardSlalom0 = new ParchmentSection(fullWater, fullWater, basicIsland_0, fullWater, fullWater, basicIsland_4, fullWater, fullWater);
    public static ParchmentSection easySlalom1 = new ParchmentSection(fullWater, fullWater, basicIsland_8, fullWater, fullWater, basicIsland_9, fullWater, fullWater);
    public static ParchmentSection hardSlalom1 = new ParchmentSection(fullWater, fullWater, basicIsland_4, fullWater, fullWater, basicIsland_0, fullWater, fullWater);
    #endregion
    #endregion

    public List<short[]> Definition;
    public Texture2D Texture;

    public ParchmentSection(params short[][] args)
    {
        Definition = new List<short[]>(args);
    }

    public ParchmentSection(params ParchmentSection[] args)
    {
        Definition = new List<short[]>();

        for (int i = 0; i < args.Length; i++)
        {
            Definition.AddRange(args[i].Definition);
        }
    }

    void GenerateTexture()
    {
        //TODO;
    }
}
