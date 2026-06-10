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

    public List<GameObject> allSubatoms = new List<GameObject>();

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
                int.TryParse(seedInput.GetComponent<TMP_InputField>().text, out seed);
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
            GameObject newSubatom = Instantiate(subatomPrefabs[subatomType], new Vector2(UnityEngine.Random.Range(-spawnBounds.x, spawnBounds.x), UnityEngine.Random.Range(-spawnBounds.y, spawnBounds.y)), transform.rotation);
            Debug.Log("Loop count: " + i + "Subatom Type: " + subatomType);
            allSubatoms.Add(newSubatom);
        }

        GameManager gm = GetComponent<GameManager>();
        ScoresHandler sh = GetComponent<ScoresHandler>();
        gm.IsPlaying = true;
        gm.scoreManager.MovesLeft = UnityEngine.Random.Range(3, 16);
        gm.UpdateUI(2);
        gm.ScoreUIUpdate(gm.scoreManager);
        sh.levelScore = new SavedScores();
        sh.levelScore.Seed = seed;
        gm.scoreManager.SubatomsLeft = GameObject.FindGameObjectsWithTag("Proton").Length + GameObject.FindGameObjectsWithTag("Neutron").Length + GameObject.FindGameObjectsWithTag("Electron").Length;
        CalculateBestScore(gm.scoreManager, sh.levelScore);
        Debug.Log("SubatomCount: " + subAtomCount);
        Debug.Log(bestPossibleScore);
    }

    public void CalculateBestScore(ScoreManager scoreManager, SavedScores savedScores)
    {
        scoreManager.MostNeutrons = GameObject.FindGameObjectsWithTag("Neutron").Length;
        savedScores.MostNeutrons = scoreManager.MostNeutrons;
        Debug.Log("Most Neutrons" + scoreManager.MostNeutrons);
        scoreManager.MaxMoves = scoreManager.MovesLeft;
        savedScores.ParMoves = scoreManager.MaxMoves;
        Debug.Log("Par: " + scoreManager.MaxMoves);
        scoreManager.BestSubatomCount = Mathf.Abs(GameObject.FindGameObjectsWithTag("Proton").Length - GameObject.FindGameObjectsWithTag("Electron").Length);
        savedScores.BestSubatomCount = scoreManager.BestSubatomCount;
        Debug.Log("Best SubatomCount" + scoreManager.BestSubatomCount);
        
    }
}
