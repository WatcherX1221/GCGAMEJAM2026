using UnityEngine;
using System.IO;

public class SavedScores
{
    public int Seed;
    public int Charge;
    public int Subatoms;
    public int Neutrons;
    public int MovesLeft;

    public void SaveSeed()
    {
        string dataPath = "W:/TemporarySaveLocation";

        try
        {
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
                Debug.Log("Created Directory at:" + dataPath);
            }
            dataPath += "/" + Seed + ".json";

            try
            {
                string saveData = JsonUtility.ToJson(Seed + Charge + Subatoms + Neutrons + MovesLeft);
                System.IO.File.WriteAllText(dataPath, saveData);
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
    }
}
