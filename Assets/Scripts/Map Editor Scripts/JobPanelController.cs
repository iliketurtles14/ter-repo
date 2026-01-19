using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class JobPanelController : MonoBehaviour
{
    private SubMenuController smc;
    public Transform uic;
    private bool canStart = false;
    public Sprite checkedSprite;
    public Sprite uncheckedSprite;
    private void Start()
    {
        smc = GetComponent<SubMenuController>();
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        canStart = true;
    }
    private void Update()
    {
        if (!canStart)
        {
            return;
        }

        if (smc.janitor)
        {
            uic.Find("JobPanel").Find("CheckBoxGrid1").Find("JanitorCheckbox").GetComponent<Image>().sprite = checkedSprite;
        }
        else
        {
            uic.Find("JobPanel").Find("CheckBoxGrid1").Find("JanitorCheckbox").GetComponent<Image>().sprite = uncheckedSprite;
        }
        if (smc.gardening)
        {
            uic.Find("JobPanel").Find("CheckBoxGrid1").Find("GardeningCheckbox").GetComponent<Image>().sprite = checkedSprite;
        }
        else
        {
            uic.Find("JobPanel").Find("CheckBoxGrid1").Find("GardeningCheckbox").GetComponent<Image>().sprite = uncheckedSprite;
        }
        if (smc.laundry)
        {
            uic.Find("JobPanel").Find("CheckBoxGrid1").Find("LaundryCheckbox").GetComponent<Image>().sprite = checkedSprite;
        }
        else
        {
            uic.Find("JobPanel").Find("CheckBoxGrid1").Find("LaundryCheckbox").GetComponent<Image>().sprite = uncheckedSprite;
        }
        if (smc.kitchen)
        {
            uic.Find("JobPanel").Find("CheckBoxGrid1").Find("KitchenCheckbox").GetComponent<Image>().sprite = checkedSprite;
        }
        else
        {
            uic.Find("JobPanel").Find("CheckBoxGrid1").Find("KitchenCheckbox").GetComponent<Image>().sprite = uncheckedSprite;
        }
        if (smc.tailor)
        {
            uic.Find("JobPanel").Find("CheckBoxGrid1").Find("TailorCheckbox").GetComponent<Image>().sprite = checkedSprite;
        }
        else
        {
            uic.Find("JobPanel").Find("CheckBoxGrid1").Find("TailorCheckbox").GetComponent<Image>().sprite = uncheckedSprite;
        }
        if (smc.woodshop)
        {
            uic.Find("JobPanel").Find("CheckBoxGrid2").Find("WoodshopCheckbox").GetComponent<Image>().sprite = checkedSprite;
        }
        else
        {
            uic.Find("JobPanel").Find("CheckBoxGrid2").Find("WoodshopCheckbox").GetComponent<Image>().sprite = uncheckedSprite;
        }
        if (smc.metalshop)
        {
            uic.Find("JobPanel").Find("CheckBoxGrid2").Find("MetalshopCheckbox").GetComponent<Image>().sprite = checkedSprite;
        }
        else
        {
            uic.Find("JobPanel").Find("CheckBoxGrid2").Find("MetalshopCheckbox").GetComponent<Image>().sprite = uncheckedSprite;
        }
        if (smc.deliveries)
        {
            uic.Find("JobPanel").Find("CheckBoxGrid2").Find("DeliveriesCheckbox").GetComponent<Image>().sprite = checkedSprite;
        }
        else
        {
            uic.Find("JobPanel").Find("CheckBoxGrid2").Find("DeliveriesCheckbox").GetComponent<Image>().sprite = uncheckedSprite;
        }
        if (smc.mailman)
        {
            uic.Find("JobPanel").Find("CheckBoxGrid2").Find("MailmanCheckbox").GetComponent<Image>().sprite = checkedSprite;
        }
        else
        {
            uic.Find("JobPanel").Find("CheckBoxGrid2").Find("MailmanCheckbox").GetComponent<Image>().sprite = uncheckedSprite;
        }
        if (smc.library)
        {
            uic.Find("JobPanel").Find("CheckBoxGrid2").Find("LibraryCheckbox").GetComponent<Image>().sprite = checkedSprite;
        }
        else
        {
            uic.Find("JobPanel").Find("CheckBoxGrid2").Find("LibraryCheckbox").GetComponent<Image>().sprite = uncheckedSprite;
        }
        if (smc.snowing)
        {
            uic.Find("ExtrasPanel").Find("SnowingCheckbox").GetComponent<Image>().sprite = checkedSprite;
        }
        else
        {
            uic.Find("ExtrasPanel").Find("SnowingCheckbox").GetComponent<Image>().sprite = uncheckedSprite;
        }
        if (smc.stunRods)
        {
            uic.Find("ExtrasPanel").Find("StunRodCheckbox").GetComponent<Image>().sprite = checkedSprite;
        }
        else
        {
            uic.Find("ExtrasPanel").Find("StunRodCheckbox").GetComponent<Image>().sprite = uncheckedSprite;
        }
        if (smc.powOutfits)
        {
            uic.Find("ExtrasPanel").Find("POWCheckbox").GetComponent<Image>().sprite = checkedSprite;
        }
        else
        {
            uic.Find("ExtrasPanel").Find("POWCheckbox").GetComponent<Image>().sprite = uncheckedSprite;
        }
    }
}
