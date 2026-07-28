using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public float movSpeed;
    private float speedX, speedY;
    private Rigidbody2D rb;
    public bool canMove;
    private PauseController pc;
    private PlayerCollectionData playerColData;
    private Escaping escapingScript;

    private IniFile iniFile;

    void Start()
    {
        canMove = true;

        pc = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();
        rb = GetComponent<Rigidbody2D>();
        playerColData = GetComponent<PlayerCollectionData>();
        escapingScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Escaping>();
        iniFile = new IniFile(System.IO.Path.Combine(Application.streamingAssetsPath, "UserData.ini"));
        StartCoroutine(EfficiencyLoop());
    }
    void Update()
    {
        if (pc.isPaused)
        {
            return;
        }

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
    private IEnumerator EfficiencyLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f / 45f);
            if (canMove && rb.linearVelocity.x != 0 && rb.linearVelocity.y != 0 && !escapingScript.hasEscaped)
            {
                escapingScript.effNum++;
            }
        }
    }
}