using System.IO;
using UnityEngine;

public class PhysicsModeSet : MonoBehaviour
{
    private PhysicsMaterial2D bouncyMat;
    private Transform player;
    private PlayerCtrl ctrl;
    private Rigidbody2D rb;
    private IniFile data;
    private bool setToNormal = true;
    private MouseGrabPlayer mgp;
    private void Start()
    {
        bouncyMat = Resources.Load<PhysicsMaterial2D>("PrisonResources/Bouncy");
        player = RootObjectCache.GetRoot("Player").transform;
        rb = player.GetComponent<Rigidbody2D>();
        ctrl = player.GetComponent<PlayerCtrl>();
        data = new IniFile(Path.Combine(Application.streamingAssetsPath, "UserData.ini"));
        mgp = GetComponent<MouseGrabPlayer>();
    }
    private void Update()
    {
        if(data.Read("PhysicsMode", "Settings") == "True")
        {
            ctrl.enabled = false;
            rb.gravityScale = 1;
            rb.sharedMaterial = bouncyMat;
            rb.constraints = RigidbodyConstraints2D.None;
            mgp.enabled = true;
            setToNormal = false;
        }
        else if (!setToNormal)
        {
            ctrl.enabled = true;
            rb.gravityScale = 0;
            rb.sharedMaterial = null;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            mgp.enabled = false;
            setToNormal = true;
        }
    }
}
