using NUnit.Framework;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{
    public int seed;

    public (int, int) bestPossibleScore;
    [SerializeField]
    GameObject seedInput;
    [SerializeField]
    GameObject useSeedToggle;
    [SerializeField]
    bool usesSeed;

    public List<GameObject> subatomPrefabs = new List<GameObject>();

    public (int, int) minMaxObjects = (5, 50);

    public Vector2 spawnBounds;
    public void UpdateSeed()
    {
        int.TryParse(seedInput.GetComponent<TMP_InputField>().text, out seed);
    }
    public void ToggleSeedUsage()
    {
        usesSeed = useSeedToggle.GetComponent<Toggle>().isOn;
    }
    public void GenerateLevel()
    {
        int levelSeed;

        switch(usesSeed)
        {
            case true:
                UnityEngine.Random.InitState(seed);
                levelSeed = seed;
                break;
            case false:
                UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
                levelSeed = (int)DateTime.Now.Ticks;
                seed = levelSeed;
                break;
        }
        Debug.Log("Level Seed: " + levelSeed);

        int subAtomCount = UnityEngine.Random.Range(minMaxObjects.Item1, minMaxObjects.Item2);

        for (int i = 0; i < subAtomCount; i++)
        {
            // Pick subatom type
            int subatomType = UnityEngine.Random.Range(0, 3);

            // Create new subatom
            GameObject newProton = Instantiate(subatomPrefabs[subatomType], new Vector2(UnityEngine.Random.Range(-spawnBounds.x, spawnBounds.x), UnityEngine.Random.Range(-spawnBounds.y, spawnBounds.y)), transform.rotation);
            Debug.Log("Loop count: " + i + "Subatom Type: " + subatomType);
        }

        GetComponent<GameManager>().IsPlaying = true;
        GetComponent<GameManager>().scoreManager.MovesLeft = UnityEngine.Random.Range(3, 16);
        GetComponent<GameManager>().UpdateUI(2);
        GetComponent<GameManager>().ScoreUIUpdate(GetComponent<GameManager>().scoreManager);
        Debug.Log("SubatomCount: " + subAtomCount);
        Debug.Log(bestPossibleScore);
    }
}
