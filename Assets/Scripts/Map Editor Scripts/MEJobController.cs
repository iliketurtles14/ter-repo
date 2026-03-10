using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MEJobController : MonoBehaviour
{
    public Transform currentDoor;
    public Transform panel;
    private List<string> validJobs = new List<string>()
    {
        "Janitor", "Gardening", "Kitchen", "Woodshop", "Metalshop", "Mailman", "Deliveries",
        "Tailor", "Laundry", "Library"
    };
    private void Update()
    {
        if (validJobs.Contains(panel.Find("JobInputField").GetComponent<TMP_InputField>().text))
        {
            panel.Find("InvalidText").GetComponent<TextMeshProUGUI>().text = "Valid job.";
            panel.Find("InvalidText").GetComponent<TextMeshProUGUI>().color = new Color(0, 1, 0);
        }
        else
        {
            panel.Find("InvalidText").GetComponent<TextMeshProUGUI>().text = "Invalid job.";
            panel.Find("InvalidText").GetComponent<TextMeshProUGUI>().color = new Color(1, 0, 0);
        }
    }
}
