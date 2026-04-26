using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WarningMessage : MonoBehaviour
{
    private Transform player;
    private int amountOfMessages;
    private void Start()
    {
        player = RootObjectCache.GetRoot("Player").transform;
        player.Find("WarningCanvas").Find("WarningSides").GetComponent<Image>().enabled = false;
        player.Find("WarningCanvas").Find("WarningBackdrop").GetComponent<Image>().enabled = false;
        player.Find("WarningCanvas").Find("WarningText").GetComponent<TextMeshProUGUI>().color = Color.clear;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            StartCoroutine(CreateWarningMessage("This is a test warning!"));
        }
    }
    public IEnumerator CreateWarningMessage(string msg)
    {
        amountOfMessages++;
        
        player.Find("WarningCanvas").Find("WarningText").GetComponent<TextMeshProUGUI>().text = msg;
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(player.Find("WarningCanvas").Find("WarningText").GetComponent<RectTransform>());

        player.Find("WarningCanvas").Find("WarningBackdrop").GetComponent<RectTransform>().sizeDelta = player.Find("WarningCanvas").Find("WarningText").GetComponent<RectTransform>().sizeDelta;
        player.Find("WarningCanvas").Find("WarningSides").GetComponent<RectTransform>().sizeDelta = player.Find("WarningCanvas").Find("WarningText").GetComponent<RectTransform>().sizeDelta + new Vector2(10, 10);

        player.Find("WarningCanvas").Find("WarningSides").GetComponent<Image>().enabled = true;
        player.Find("WarningCanvas").Find("WarningBackdrop").GetComponent<Image>().enabled = true;
        player.Find("WarningCanvas").Find("WarningText").GetComponent<TextMeshProUGUI>().color = new Color(0, 0, 0, 255);

        yield return new WaitForSeconds(1);
        if(amountOfMessages <= 1)
        {
            player.Find("WarningCanvas").Find("WarningSides").GetComponent<Image>().enabled = false;
            player.Find("WarningCanvas").Find("WarningBackdrop").GetComponent<Image>().enabled = false;
            player.Find("WarningCanvas").Find("WarningText").GetComponent<TextMeshProUGUI>().color = Color.clear;

        }
        amountOfMessages--;
    }
}
