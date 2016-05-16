using UnityEngine.Assertions;
using System.Linq;
using System;

class WeightedRandomizer
{
    static Random rng = new Random();

    public static int GetRandomIndex(double[] weights)
    {
        double rnd = rng.NextDouble() * weights.Sum();

        for (int i = 0; i < weights.Length; i++)
        {
            if (rnd < weights[i])
                return i;

            rnd -= weights[i];
        }

        Assert.IsTrue(true, "Something went wrong in the WeightedRandomizer");
        return -1;
    }
}

