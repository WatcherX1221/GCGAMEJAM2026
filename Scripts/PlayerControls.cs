using NUnit.Framework;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.Rendering;

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

    // Power
    public float PowerLimit = 5f;
    public Vector2 ShotPower;

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
    }

    void Update()
    {
        // Get the mouse's world position
        MousePositionVector = GameCamera.ScreenToWorldPoint(MousePositionInput.ReadValue<Vector2>());

        // Get distance from the mouse to the player
        MouseDistPlayer = new Vector2(MousePositionVector.x - transform.position.x, MousePositionVector.y - transform.position.y);

        ShotPower = new Vector2(Mathf.Clamp(MouseDistPlayer.x, -PowerLimit, PowerLimit), Mathf.Clamp(MouseDistPlayer.y, -PowerLimit, PowerLimit));

        for (int i = 0; i < AimObjects.Count; i++)
        {
            AimObjects[i].transform.localPosition = Vector2.Lerp(Vector2.zero, -ShotPower, ((float)i + 1) / AimObjects.Count);

        }

        if (MouseLeftClick.WasPressedThisFrame())
        {
            LaunchAtom(ShotPower);
        }
    }

    public void LaunchAtom(Vector2 power)
    {
        rb.linearVelocity = -power * 3;
    }
}
