using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class ScoreManager
{
    public int ChargeScore;
    public int NeutronScore;
    public int MovesLeft;
    public int SubatomsLeft;
}

public class GameManager : MonoBehaviour
{
    public ScoreManager scoreManager;

    public GameObject chargeUI;
    public GameObject neutronUI;
    public GameObject moveUI;

    public bool IsPlaying;

    List<GameObject> uiPanels = new List<GameObject>();

    private void Start()
    {
        uiPanels.AddRange(GameObject.FindGameObjectsWithTag("UI"));

        IsPlaying = false;
        scoreManager = new ScoreManager();
        UpdateScores(0, 0);
        UpdateUI(0);
    }
    public void UpdateScores(int chargeScore, int neutronScore)
    {
        scoreManager.ChargeScore += chargeScore;
        scoreManager.NeutronScore += neutronScore;
        ScoreUIUpdate(scoreManager);
    }

    public void ScoreUIUpdate(ScoreManager score)
    {
        chargeUI.GetComponent<TMP_Text>().text = "Charge: " + score.ChargeScore;
        neutronUI.GetComponent<TMP_Text>().text = "Neutrons: " + score.NeutronScore;
        moveUI.GetComponent<TMP_Text>().text = "Moves: " + score.MovesLeft; 
    }

    public void UpdateUI(int currentPage)
    {       
        switch(currentPage)
        {
            // Generate
            case 0:
                foreach (GameObject go in uiPanels)
                {
                    if (go.name == "GeneratePanel")
                    {
                        go.SetActive(true);
                    }
                    else
                    {
                        go.SetActive(false);
                    }
                }
                break;
            // Level Select
            case 1:
                foreach (GameObject go in uiPanels)
                {
                    if (go.name == "ScoresPanel")
                    {
                        go.SetActive(true);
                    }
                    else
                    {
                        go.SetActive(false);
                    }
                }
                break;
            // Gameplay
            case 2:
                foreach (GameObject go in uiPanels)
                {
                    if (go.name == "GameplayPanel")
                    {
                        go.SetActive(true);
                    }
                    else
                    {
                        go.SetActive(false);
                    }
                }
                break;
        }
        
    }
}
