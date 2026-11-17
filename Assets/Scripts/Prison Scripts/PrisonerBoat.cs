using System.Collections;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class PrisonerBoat : MonoBehaviour
{
    private PlayerCollectionData playerColData;
    private Transform player;
    private PauseController pc;
    private Escaping escapeScript;
    private bool didMessage = false;
    private void Start()
    {
        playerColData = RootObjectCache.GetRoot("Player").GetComponent<PlayerCollectionData>();
        player = RootObjectCache.GetRoot("Player").transform;
        pc = RootObjectCache.GetRoot("ScriptObject").GetComponent<PauseController>();
        escapeScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Escaping>();
        Transform canvas = transform.Find("MessageCanvas");
        canvas.Find("Border").GetComponent<Image>().enabled = false;
        canvas.Find("Backdrop").GetComponent<Image>().enabled = false;
        canvas.Find("Text").GetComponent<TextMeshProUGUI>().color = new Color(0, 0, 0, 0);
        StartCoroutine(Anim());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void OnEnable()
    {
        StartCoroutine(Anim());
    }
    private void Update()
    {
        if(Vector2.Distance(player.position, transform.position) <= 5.6f)
        {
            if(playerColData.playerData.money < 250)
            {
                if (!didMessage)
                {
                    StartCoroutine(Um250());
                    didMessage = true;
                }
            }
            else
            {
                Escape();
            }
        }
        else
        {
            didMessage = false;
        }
    }
    private IEnumerator Um250()
    {
        Transform canvas = transform.Find("MessageCanvas");

        if (canvas.Find("Backdrop").GetComponent<Image>().enabled)
        {
            yield break;
        }

        canvas.Find("Text").GetComponent<TextMeshProUGUI>().text = "Um... $250?";
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(canvas.Find("Text").GetComponent<RectTransform>());

        canvas.Find("Backdrop").GetComponent<RectTransform>().sizeDelta = canvas.Find("Text").GetComponent<RectTransform>().sizeDelta;
        canvas.Find("Border").GetComponent<RectTransform>().sizeDelta = canvas.Find("Text").GetComponent<RectTransform>().sizeDelta + new Vector2(10, 10);

        canvas.Find("Border").GetComponent<Image>().enabled = true;
        canvas.Find("Backdrop").GetComponent<Image>().enabled = true;
        canvas.Find("Text").GetComponent<TextMeshProUGUI>().color = new Color(0, 0, 0, 1);

        yield return new WaitForSeconds(2);

        canvas.Find("Border").GetComponent<Image>().enabled = false;
        canvas.Find("Backdrop").GetComponent<Image>().enabled = false;
        canvas.Find("Text").GetComponent<TextMeshProUGUI>().color = new Color(0, 0, 0, 0);
    }
    private void Escape()
    {
        StartCoroutine(escapeScript.Escape());
    }
    private IEnumerator Anim()
    {
        DataSender ds = DataSender.instance;
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        //251, 252, .5 secodns
        while (true)
        {
            sr.sprite = ds.PrisonObjectImages[251];
            float time = 0f;
            while(time < .5f)
            {
                if (pc != null && pc.isPaused)
                {
                    yield return null;
                    continue;
                }
                time += Time.deltaTime;
                yield return null;
            }
            sr.sprite = ds.PrisonObjectImages[252];
            time = 0f;
            while(time < .5f)
            {
                if (pc != null && pc.isPaused)
                {
                    yield return null;
                    continue;
                }
                time += Time.deltaTime;
                yield return null;
            }
            yield return null;
        }
    }
}
