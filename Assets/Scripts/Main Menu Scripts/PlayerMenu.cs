using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenu : MonoBehaviour
{
    public MouseCollisionOnButtons mcs;
    public NPCSave saveScript;
    public string playerName;
    public string playerCharacter;
    public int characterNum;
    public Canvas MainMenuCanvas;
    public Sprite normalSprite;
    public Sprite pressedSprite;
    public string setName;
    public int setCharacter;

    public void OnEnable()
    {
        characterNum = Random.Range(0, 9);
        playerCharacter = CharacterEnumClass.GetCharacterString(characterNum);
        transform.Find("NameText").GetComponent<TMP_InputField>().text = playerCharacter;
        transform.Find("NameText").Find("Text Area").Find("Text").GetComponent<TextMeshProUGUI>().text = playerCharacter;
    }
    public void Update()
    {
        playerCharacter = CharacterEnumClass.GetCharacterString(characterNum);
        characterNum = CharacterEnumClass.GetCharacterInt(playerCharacter);
        if (characterNum == 0)
        {
            transform.Find("LeftArrow").GetComponent<Image>().enabled = false;
            transform.Find("LeftArrow").GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            transform.Find("LeftArrow").GetComponent<Image>().enabled = true;
            transform.Find("LeftArrow").GetComponent<BoxCollider2D>().enabled = true;
        }

        if (characterNum == 8)
        {
            transform.Find("RightArrow").GetComponent<Image>().enabled = false;
            transform.Find("RightArrow").GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {
            transform.Find("RightArrow").GetComponent<Image>().enabled = true;
            transform.Find("RightArrow").GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
