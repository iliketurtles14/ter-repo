using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public float movSpeed;
    private float speedX, speedY;
    private Rigidbody2D rb;

    private IniFile iniFile;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        iniFile = new IniFile(System.IO.Path.Combine(Application.streamingAssetsPath, "CTFAK", "config.ini"));
    }
    void Update()
    {
        speedX = Input.GetAxisRaw("Horizontal");
        speedY = Input.GetAxisRaw("Vertical");

        if (iniFile.Read("NormalizePlayerMovement", "Settings") == "true")
        {
            Vector2 movement = new Vector2(speedX, speedY).normalized * movSpeed;
            rb.linearVelocity = movement;
        }
        else if (iniFile.Read("NormalizePlayerMovement", "Settings") == "false")
        {
            Vector2 movement = new Vector2(speedX, speedY) * movSpeed;
            rb.linearVelocity = movement;
        }

    }
}