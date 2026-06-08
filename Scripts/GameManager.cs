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
    public GameObject subatomUI;

    public bool IsPlaying;

    List<GameObject> uiPanels = new List<GameObject>();

    private void Start()
    {
        uiPanels.AddRange(GameObject.FindGameObjectsWithTag("UI"));

        IsPlaying = false;
        scoreManager = new ScoreManager();
        UpdateScores(0, 0, 0, true);
        UpdateUI(0);
    }
    public void UpdateScores(int chargeScore, int neutronScore, int subatomsScore, bool initialUpdate)
    {
        scoreManager.ChargeScore += chargeScore;
        scoreManager.NeutronScore += neutronScore;
        scoreManager.SubatomsLeft += subatomsScore;
        if (!initialUpdate)
        {
            GetComponent<ScoresHandler>().levelScore.Charge = scoreManager.ChargeScore;
            GetComponent<ScoresHandler>().levelScore.Neutrons = scoreManager.NeutronScore;
            GetComponent<ScoresHandler>().levelScore.Subatoms = scoreManager.SubatomsLeft;
        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<AtomVisualController>().UpdateSprite(scoreManager.ChargeScore);
        ScoreUIUpdate(scoreManager);

        if (scoreManager.SubatomsLeft <= 0 && !initialUpdate)
        {
            EndGame();
        }
    }

    public void ScoreUIUpdate(ScoreManager score)
    {
        chargeUI.GetComponent<TMP_Text>().text = "Charge: " + score.ChargeScore;
        neutronUI.GetComponent<TMP_Text>().text = "Neutrons: " + score.NeutronScore;
        moveUI.GetComponent<TMP_Text>().text = "Moves: " + score.MovesLeft;
        subatomUI.GetComponent<TMP_Text>().text = "Subatoms: " + score.SubatomsLeft;
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
            case 3:
                foreach (GameObject go in uiPanels)
                {
                    if (go.name == "GameEndPanel")
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
    public void EndGame()
    {
        // Set is playing off so that player cannot input when they shouldn't
        IsPlaying = false;
        // Save the scores for the seed
        GetComponent<ScoresHandler>().SaveSeed();
        // Delete all remaining subatoms
        for(int i = 0; i < GetComponent<LevelGenerator>().allSubatoms.Count; i++)
        {
            Destroy(GetComponent<LevelGenerator>().allSubatoms[i]);
        }
        GetComponent<LevelGenerator>().allSubatoms.Clear();
        // Reset player position & velocity
        GameObject.FindGameObjectWithTag("Player").transform.position = Vector2.zero;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        // Recreate score manager instance to reset scores
        scoreManager = new ScoreManager();
        // Update the UI to fit
        UpdateScores(0, 0, 0, true);
        // Show game end UI
        UpdateUI(3);
    }
}
