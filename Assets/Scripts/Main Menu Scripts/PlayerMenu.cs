using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenu : MonoBehaviour
{
    public MouseCollisionOnButtons mcs;
    public NPCSave saveScript;
    public string playerName;
    public string playerCharacter;
    public PrisonSelect prisonSelectScript;
    private List<int> availablePlayerChars = new List<int>();
    public string playerOutfit;
    public int characterNum;
    public Canvas MainMenuCanvas;
    public Sprite normalSprite;
    public Sprite pressedSprite;
    public string setName;
    public int setCharacter;

    public void OnEnable()
    {
        SetPlayerThings();
        characterNum = Random.Range(0, availablePlayerChars.Count);
        playerCharacter = CharacterEnumClass.GetCharacterString(characterNum);
        transform.Find("NameText").GetComponent<TMP_InputField>().text = playerCharacter;
        transform.Find("NameText").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text = playerCharacter;
        transform.Find("Player").gameObject.SetActive(false);
        StartCoroutine(Wait());
    }
    private void SetPlayerThings()
    {
        playerOutfit = prisonSelectScript.currentPrisonPlayerOutfit;
        switch (prisonSelectScript.currentPrisonPlayerBody)
        {
            case "Normal":
                for(int i = 0; i < 9; i++)
                {
                    availablePlayerChars.Add(i);
                }
                break;
            case "SS":
                availablePlayerChars.Add(10); //buddy
                break;
            case "ET":
                availablePlayerChars.Add(30); //clint
                break;
            case "DTAF":
                availablePlayerChars.Add(19); //connelly
                break;
        }
    }
    private IEnumerator Wait()
    {
        yield return new WaitForEndOfFrame();
        transform.Find("Player").gameObject.SetActive(true);
    }
    public void Update()
    {
        if(saveScript == null)
        {
            saveScript = NPCSave.instance;
        }
        
        playerCharacter = CharacterEnumClass.GetCharacterString(characterNum);
        characterNum = CharacterEnumClass.GetCharacterInt(playerCharacter);
        if (characterNum == 0)
        {
            transform.Find("LeftArrow").GetComponent<Image>().enabled = false;
            transform.Find("LeftArrow").GetComponent<Button>().enabled = false;
        }
        else
        {
            transform.Find("LeftArrow").GetComponent<Image>().enabled = true;
            transform.Find("LeftArrow").GetComponent<Button>().enabled = true;
        }

        if (characterNum == availablePlayerChars.Count - 1)
        {
            transform.Find("RightArrow").GetComponent<Image>().enabled = false;
            transform.Find("RightArrow").GetComponent<Button>().enabled = false;
        }
        else
        {
            transform.Find("RightArrow").GetComponent<Image>().enabled = true;
            transform.Find("RightArrow").GetComponent<Button>().enabled = true;
        }
    }
}
