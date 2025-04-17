using UnityEngine;

public class RandomSeedInitializer : MonoBehaviour
{
    [Header("Random Seed Settings")]
    public int seed = 0;
    public bool resetOnStart = false;

    void Start()
    {
        if (resetOnStart)
            ResetSeed();
    }

    public void ResetSeed()
    {
        Random.InitState(seed);
        Debug.Log($"Random seed reset to: {seed}");
    }
}