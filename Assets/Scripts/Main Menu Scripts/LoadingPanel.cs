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
        foreach (Transform child in transform.Find("LoadBar"))
        {
            child.GetComponent<Image>().sprite = clearSprite;
        }
        titlePanel.transform.Find("PlayButton").GetComponent<BoxCollider2D>().enabled = false;
        titlePanel.transform.Find("OptionsButton").GetComponent<BoxCollider2D>().enabled = false;
        titlePanel.transform.Find("MapEditorButton").GetComponent<BoxCollider2D>().enabled = false;
    }
    public void LogLoad(string log)
    {
        transform.Find("LoadText").GetComponent<TextMeshProUGUI>().text = log;
        IncrementLoadCount();
    }
    public void IncrementLoadCount()
    {
        loadCount++;
        currentPercentage = Mathf.RoundToInt((loadCount / 105f) * 100);
        Transform loadBarTransform = transform.Find("LoadBar");
        int childCount = loadBarTransform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            Transform child = loadBarTransform.GetChild(i);
            Image childImage = child.GetComponent<Image>();

            if (i < currentPercentage)
            {
                childImage.sprite = barSprite;
            }
            else
            {
                childImage.sprite = clearSprite;
            }
        }
        if (loadCount == 97)
        {
            titlePanel.transform.Find("PlayButton").GetComponent<BoxCollider2D>().enabled = true;
            titlePanel.transform.Find("OptionsButton").GetComponent<BoxCollider2D>().enabled = true;
            titlePanel.transform.Find("MapEditorButton").GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<Animator>().Play("LoadingScreenAnim");
            StartCoroutine(WaitLoop());
        }
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
