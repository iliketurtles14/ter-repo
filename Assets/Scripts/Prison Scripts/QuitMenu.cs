using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class QuitMenu : MonoBehaviour
{
    private Pause pauseScript;
    private Transform mc;
    public bool isQuitting;
    private int groundLayer;
    private int undergroundLayer;
    private int ventLayer;
    private int roofLayer;
    private int playerLayer;
    private int uiLayer;
    private int ventCoverLayer;
    private Transform blocker;
    private void Start()
    {
        mc = RootObjectCache.GetRoot("MenuCanvas").transform;
        pauseScript = mc.Find("PauseMenuPanel").GetComponent<Pause>();
        groundLayer = LayerMask.NameToLayer("Ground");
        undergroundLayer = LayerMask.NameToLayer("Underground");
        ventLayer = LayerMask.NameToLayer("Vents");
        roofLayer = LayerMask.NameToLayer("Roof");
        playerLayer = LayerMask.NameToLayer("Player");
        uiLayer = LayerMask.NameToLayer("UI");
        ventCoverLayer = LayerMask.NameToLayer("VentCovers");
        blocker = RootObjectCache.GetRoot("BlockerCanvas").transform;
        Cancel(true);
    }
    public IEnumerator Quit()
    {
        if (isQuitting)
        {
            yield break;
        }
        isQuitting = true;
        blocker.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(.6f);
        blocker.GetComponent<Animator>().enabled = false;
        Addressables.LoadSceneAsync("Main Menu");
        GetGivenData.instance.GetComponent<DumperStartStop>().isGoingToMainMenu = true;
        Physics2D.IgnoreLayerCollision(uiLayer, groundLayer, false);
        Physics2D.IgnoreLayerCollision(uiLayer, undergroundLayer, true);
        Physics2D.IgnoreLayerCollision(uiLayer, ventLayer, true);
        Physics2D.IgnoreLayerCollision(uiLayer, roofLayer, true);
        Physics2D.IgnoreLayerCollision(uiLayer, ventCoverLayer, true);
        Physics2D.IgnoreLayerCollision(playerLayer, groundLayer, false);
        Physics2D.IgnoreLayerCollision(playerLayer, undergroundLayer, true);
        Physics2D.IgnoreLayerCollision(playerLayer, ventLayer, true);
        Physics2D.IgnoreLayerCollision(playerLayer, roofLayer, true);
    }
    public void Cancel(bool atStart)
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        GetComponent<Image>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        if (!atStart)
        {
            pauseScript.OpenPauseMenu();
        }
    }
    public void Open()
    {
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
        GetComponent<Image>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = false;
    }
}
