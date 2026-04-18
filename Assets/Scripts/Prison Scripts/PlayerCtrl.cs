using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public float movSpeed;
    private float speedX, speedY;
    private Rigidbody2D rb;
    public bool canMove;

    private IniFile iniFile;

    void Start()
    {
        canMove = true;

        rb = GetComponent<Rigidbody2D>();

        iniFile = new IniFile(System.IO.Path.Combine(Application.streamingAssetsPath, "UserData.ini"));
    }
    void Update()
    {
        if (canMove)
        {
            speedX = Input.GetAxisRaw("Horizontal");
            speedY = Input.GetAxisRaw("Vertical");

            if (iniFile.Read("NormalizePlayerMovement", "Settings") == "True")
            {
                Vector2 movement = new Vector2(speedX, speedY).normalized * movSpeed;
                rb.linearVelocity = movement;
            }
            else if (iniFile.Read("NormalizePlayerMovement", "Settings") == "False")
            {
                Vector2 movement = new Vector2(speedX, speedY) * movSpeed;
                rb.linearVelocity = movement;
            }
        }
    }
}