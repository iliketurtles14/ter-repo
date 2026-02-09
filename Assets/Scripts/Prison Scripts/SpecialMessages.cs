using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpecialMessages : MonoBehaviour
{
    private bool isDisplaying;
    private float speed = 1000;
    private int here;

    private int which = 1; //this is just for testing
    public IEnumerator MakeMessage(string message, string type) //types: favor, guard, achievement (?)
    {
        here++;
        int waitTime = here;
        while (isDisplaying)
        {
            for (int i = 0; i < waitTime; i++)
            {
                yield return new WaitForEndOfFrame(); //this should make it so that if multiple
                                                      //things are trying to display a message,
                                                      //it waits. prolly doesnt work but idk
            }
        }

        isDisplaying = true;

        transform.Find("Message").GetComponent<TextMeshProUGUI>().text = message;
        //make different types do stuff and whatever

        while (true)
        {
            //1190 -> 730

            transform.localPosition = new Vector2(transform.localPosition.x - (Time.deltaTime * speed), transform.localPosition.y);

            if(transform.localPosition.x <= 730)
            {
                transform.localPosition = new Vector2(730, transform.localPosition.y);
                break;
            }

            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(2.5f);

        while (true)
        {
            //730 -> 1190

            transform.localPosition = new Vector2(transform.localPosition.x + (Time.deltaTime * speed), transform.localPosition.y);

            if(transform.localPosition.x >= 1190)
            {
                transform.localPosition = new Vector2(1190, transform.localPosition.y);
                break;
            }

            yield return new WaitForEndOfFrame();
        }
        isDisplaying = false;
        here--;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            StartCoroutine(MakeMessage("This is the " + which + " test!!!", "favor"));
            StartCoroutine(MakeMessage("This is the " + which + " (2) test!!!", "favor"));
            which++;
        }
    }
}
