using System;
using System.Text;
using UnityEngine;

public class RandomGenerator : MonoBehaviour
{
    public string Seed;
    private static int currentSeed;
    protected static System.Random random;

    void Awake()
    {
        SetSeed(Seed);
    }

    public static void SetSeed(string newSeed)
    {
        currentSeed = GetSeed(newSeed);
        random = new System.Random();
        UnityEngine.Random.InitState(currentSeed);
    }

    public static int SeededRange(int Min, int Max)
    {
        return UnityEngine.Random.Range(Min, Max);
    }

    public static float SeededRange(float Min, float Max)
    {
        return UnityEngine.Random.Range(Min, Max);
    }

    public static int UnseededRange(int Min, int Max)
    {
        return random.Next(Min, Max);
    }

    public static float UnseededRange(float Min, float Max)
    {
        return ((float)(random.Next((int)(Min * 1000), (int)(Max * 1000)))) / 1000;
    }

    public static bool SeededRandomBool()
    {
        return SeededRange(0, 100) >= 50;
    }

    public static bool UnseededRandomBool()
    {
        return UnseededRange(0, 100) >= 50;
    }

    #region GenerationSeed
    protected static int GetSeed(string seedValue)
    {
        byte[] bytes = Encoding.ASCII.GetBytes(seedValue);
        int retval = 0;
        foreach (var b in bytes)
        {
            retval += Convert.ToInt32(b);
        }
        return retval;
    }
    #endregion

}