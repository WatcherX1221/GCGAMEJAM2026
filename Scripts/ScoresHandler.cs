using NUnit.Framework;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class ScoresHandler : MonoBehaviour
{
    public SavedScores[] LoadedScores = new SavedScores[0];
    public SavedScores levelScore = new SavedScores();
    public int ScoreCount = 0;
    public bool hasLoadedScoresThisSession;

    void Start()
    {
        
        hasLoadedScoresThisSession = false;

        LoadScores();
    }
    public void LoadScores()
    {
        string dataPath = "W:/TemporarySaveLocation";

        if(!hasLoadedScoresThisSession)
        {
            ReadFiles(dataPath);
        }
    }

    public string[] ReadFiles(string path)
    {
        int fileCount = System.IO.Directory.GetFiles(path).Length;
        Debug.Log("FileCount: " + fileCount);
        string[] files = System.IO.Directory.GetFiles(path);
        foreach (string file in files)
        {
            string fileData = System.IO.File.ReadAllText(file);
            Debug.Log(fileData);
        }

        return new string[0];
    }
}
