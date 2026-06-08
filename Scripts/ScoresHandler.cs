using NUnit.Framework;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class ScoresHandler : MonoBehaviour
{
    public SavedScores[] LoadedScores = new SavedScores[0];
    public SavedScores levelScore;
    public int ScoreCount = 0;
    public bool hasLoadedScoresThisSession;
    public bool needsLoadScore;

    public GameObject ScrollObject;
    public GameObject SeedScoreInstance;

    void Start()
    {
        hasLoadedScoresThisSession = false;
        needsLoadScore = true;
    }
    public void LoadScores()
    {

        string dataPath = System.IO.Path.Combine(Application.dataPath, "saves");

        try
        {
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
                Debug.Log("Created Directory at:" + dataPath);
            }

        }
        catch (System.Exception ex)
        {
            string ErrorMessages = "Failed to Create folder" + ex.Message;
            Debug.LogError(ErrorMessages);
        }

        if (needsLoadScore)
        {
            GameObject[] oldObjects = GameObject.FindGameObjectsWithTag("SeedInstance");
            if (oldObjects.Length > 0)
            {
                for (int i = 0; i < oldObjects.Length; i++)
                {
                    Destroy(oldObjects[i]);
                }
            }    
            LoadedScores = new SavedScores[0];
            ReadFiles(dataPath);
            hasLoadedScoresThisSession = true;
            if (LoadedScores.Length > 0)
            {
                for (int i = 0; i < LoadedScores.Length; i++)
                {
                    GameObject newSeedInstance = Instantiate(SeedScoreInstance, ScrollObject.transform);
                    newSeedInstance.GetComponent<SeedInstanceManager>().GetSeedScores(LoadedScores[i]);
                }
                Debug.Log("Rect Size: " + ScrollObject.GetComponent<RectTransform>().rect);
            }
            needsLoadScore = false;
        }
    }
    public void SaveSeed()
    {

        string dataPath = System.IO.Path.Combine(Application.dataPath, "saves");
        int instanceOfSeedInSave = 0;
        try
        {
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
                Debug.Log("Created Directory at:" + dataPath);
            }
            string[] allFiles = Directory.GetFiles(dataPath);
            for (int i = 0; i < allFiles.Length; i++)
            {
                Debug.Log("Should end with: " + levelScore.Seed.ToString() + "_" + (instanceOfSeedInSave + 1) + ".json");
                if (allFiles[i].EndsWith(levelScore.Seed.ToString() + "_" + (instanceOfSeedInSave + 1) + ".json"))
                {
                    instanceOfSeedInSave++;
                }
            }
            Debug.Log("Instances of seed " + levelScore.Seed + " : " + instanceOfSeedInSave);
            Debug.Log("Seed: " + levelScore.Seed + "Charge: " + levelScore.Charge + "Subatoms: " + levelScore.Subatoms + "Neutrons: " + levelScore.Neutrons + "Moves: " + levelScore.MovesLeft);
            dataPath += "/" + levelScore.Seed + "_" + (instanceOfSeedInSave + 1) + ".json";

            try
            {
                string saveData = JsonUtility.ToJson(levelScore);
                System.IO.File.WriteAllText(dataPath, saveData);
                Debug.Log("Saved file:" + dataPath);
            }
            catch (System.Exception ex)
            {
                string ErrorMessages = "Failed to Write" + ex.Message;
                Debug.LogError(ErrorMessages);
            }
        }
        finally
        {

        }
        needsLoadScore = true;
    }

    public void ReadFiles(string path)
    {
        string[] files = System.IO.Directory.GetFiles(path);
        LoadedScores = new SavedScores[files.Length];
        Debug.Log("File Count: " + files.Length);
        if(files.Length > 0)
        {
            for (int i = 0; i < files.Length; i++)
            {
                if (!files[i].Contains(".meta"))
                {
                    Debug.Log("Loop count: " + i);
                    string fileData = System.IO.File.ReadAllText(files[i]);
                    LoadedScores[i] = JsonUtility.FromJson<SavedScores>(fileData);
                }
            }
        }
    }

    public void DeleteSaveData()
    {
        Debug.Log("Attempting Delete");
        string dataPath = System.IO.Path.Combine(Application.dataPath, "saves");
        Debug.Log("Path to delete: " + dataPath);
        if (Directory.Exists(dataPath))
        {
            string[] saveFiles = System.IO.Directory.GetFiles(dataPath);
            for (int i = 0;i < saveFiles.Length; i++)
            {
                System.IO.File.Delete(saveFiles[i]);
                Debug.Log("Deleted: " +  saveFiles[i]);
            }
        }
    }
}
