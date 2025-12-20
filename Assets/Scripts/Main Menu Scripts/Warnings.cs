using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Warnings : MonoBehaviour
{
    public Transform warningBlack;
    private Transform lastCurrentScreen;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseWarning();
        }
    }
    public void CreateWarning(string msg, Transform currentScreen)
    {
        lastCurrentScreen = currentScreen;
        
        foreach(Transform obj in currentScreen)
        {
            if(obj.GetComponent<BoxCollider2D>() != null)
            {
                obj.GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        warningBlack.gameObject.SetActive(true);

        transform.Find("MessageText").GetComponent<TextMeshProUGUI>().text = msg;
        transform.Find("MessageText").GetComponent<TextMeshProUGUI>().enabled = true;
        transform.Find("WarningText").GetComponent<TextMeshProUGUI>().enabled = true;
        transform.Find("EscapeText").GetComponent<TextMeshProUGUI>().enabled = true;
        GetComponent<Image>().enabled = true;
    }
    public void CloseWarning()
    {
        if(lastCurrentScreen != null)
        {
            foreach (Transform obj in lastCurrentScreen)
            {
                if (obj.GetComponent<BoxCollider2D>() != null)
                {
                    obj.GetComponent<BoxCollider2D>().enabled = true;
                }
            }
        }
        warningBlack.gameObject.SetActive(false);

        transform.Find("MessageText").GetComponent<TextMeshProUGUI>().enabled = false;
        transform.Find("WarningText").GetComponent<TextMeshProUGUI>().enabled = false;
        transform.Find("EscapeText").GetComponent<TextMeshProUGUI>().enabled = false;
        GetComponent<Image>().enabled = false;

        GetComponent<CheckForDependencies>().hasChecked = true;
    }
}
