using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour
{
    private int loadCount = 0;
    private int currentPercentage = 0;

    public GameObject barLine;
    public Sprite clearSprite;
    public Sprite barSprite;
    public GameObject titlePanel;
    public void Start()
    {
        titlePanel.transform.Find("PlayButton").GetComponent<Button>().enabled = false;
        titlePanel.transform.Find("OptionsButton").GetComponent<Button>().enabled = false;
        titlePanel.transform.Find("MapEditorButton").GetComponent<Button>().enabled = false;
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            GetGivenData.instance.doneWithGivenLoad = true;
        }

        if (GetGivenData.instance.doneWithGivenLoad)
        {
            titlePanel.transform.Find("PlayButton").GetComponent<Button>().enabled = true;
            titlePanel.transform.Find("OptionsButton").GetComponent<Button>().enabled = true;
            titlePanel.transform.Find("MapEditorButton").GetComponent<Button>().enabled = true;
            GetComponent<Animator>().Play("LoadingScreenAnim");
            StartCoroutine(WaitLoop());
        }
    }
    public void LogLoad(string log)
    {
        transform.Find("LoadText").GetComponent<TextMeshProUGUI>().text = log;
        Debug.Log(log);
        IncrementLoadCount();
    }
    public void IncrementLoadCount()
    {
        loadCount++;
    }
    public IEnumerator WaitLoop()
    {
        while(GetComponent<Image>().color.a > 0)
        {
            yield return null;
        }
        Debug.Log("amount of loads: " + loadCount);
        gameObject.SetActive(false);
    }
}
