using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.Rendering;
using Unity.VisualScripting;

public class PlayerControls : MonoBehaviour
{
    // Mouse Position (Duh)
    public Vector2 MousePositionVector;

    // Input Action refs
    InputAction MousePositionInput;
    InputAction MouseLeftClick;

    // Camera refs for world position
    public Camera GameCamera;

    // Processed values
    public Vector2 MouseDistPlayer;

    // Aim object objects & positions
    public List<GameObject> AimObjects = new List<GameObject>();

    // Gameplay
    public float PowerLimit = 5f;
    public float PowerMult = 1f;
    public Vector2 ShotPower;
    public int playerMoveCooldown;

    public GameManager gameManager;

    public Vector2 bounds = new Vector2(10f, 6f);

    Rigidbody2D rb;
    void Start()
    {
        // Define as 0, 0 for now
        MouseDistPlayer = new Vector2(0f, 0f);

        // Reference input actions
        MousePositionInput = InputSystem.actions.FindAction("MousePosition");
        MouseLeftClick = InputSystem.actions.FindAction("MouseLeftClick");

        // Component Refs
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        // Only run any of this if the game is in the gameplay state. Otherwise, ignore everything in Update
        if (gameManager.IsPlaying)
        {
            // Get the mouse's world position
            MousePositionVector = GameCamera.ScreenToWorldPoint(MousePositionInput.ReadValue<Vector2>());

            // Get distance from the mouse to the player
            MouseDistPlayer = new Vector2(MousePositionVector.x - transform.position.x, MousePositionVector.y - transform.position.y);

            // Clamp the values to create a maximum power.
            ShotPower = new Vector2(Mathf.Clamp(MouseDistPlayer.x, -PowerLimit, PowerLimit), Mathf.Clamp(MouseDistPlayer.y, -PowerLimit, PowerLimit));

            // Interpolate along the inverse direction of the cursor in relation to the atom
            for (int i = 0; i < AimObjects.Count; i++)
            {
                AimObjects[i].transform.localPosition = Vector2.Lerp(Vector2.zero, -ShotPower, ((float)i + 1) / AimObjects.Count);
            }

            // Launch the atom if the cooldown is available, then reset cooldown.
            if (MouseLeftClick.WasPressedThisFrame() && playerMoveCooldown <= 0 && gameManager.scoreManager.MovesLeft > 0)
            {
                LaunchAtom(ShotPower);
                playerMoveCooldown = 100;
                gameManager.scoreManager.MovesLeft -= 1;
                gameManager.ScoreUIUpdate(gameManager.scoreManager);
            }
        }
    }

    public void LaunchAtom(Vector2 power)
    {
        // Launch the atom inverse to the power value
        rb.linearVelocity = -power * PowerMult;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If a trigger is collided with, do the corresponding action
        switch (collision.tag)
        {
            case "Proton":
                // Double the player's current velocity
                rb.linearVelocity = rb.linearVelocity * 2;
                gameManager.UpdateScores(1, 0);
                collision.gameObject.SetActive(false);
                break;
            case "Neutron":
                // Add to the neutron score
                gameManager.UpdateScores(0, 1);
                collision.gameObject.SetActive(false);
                break;
            case "Electron":
                // Reverse the player's current velocity and double it
                rb.linearVelocity = -rb.linearVelocity * 2;
                gameManager.UpdateScores(-1, 0);
                collision.gameObject.SetActive(false);
                break;
        }
    }

    private void FixedUpdate()
    {
        if(playerMoveCooldown >= 0)
        {
            playerMoveCooldown -= 1;
        }

        if(Mathf.Abs(transform.position.x) > bounds.x || Mathf.Abs(transform.position.y) > bounds.y)
        {
            transform.position = new Vector2(0, 0);
        }
    }
}
