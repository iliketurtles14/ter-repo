using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatEffects : MonoBehaviour
{
    private Sprite heat;
    private Sprite good;
    private Sprite bad;
    private Sprite speed;
    private Sprite strength;
    private Sprite intellect;
    public Sprite energy;
    public Sprite health;
    private Dictionary<string, Sprite> spriteDict;
    private Dictionary<string, Vector2> sizeDict = new Dictionary<string, Vector2>
    {
        { "good", new Vector2(.7f, .7f) }, { "bad", new Vector2(.7f, .7f) },
        { "heat", new Vector2(.7f, .8f) }, { "speed", new Vector2(.9f, .7f) },
        { "strength", new Vector2(.9f, .7f) }, { "intellect", new Vector2(.7f, .7f) },
        { "energy", new Vector2(.5f, .7f) }, { "health", new Vector2(.6f, .6f) }
    };
    private void Start()
    {
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        DataSender ds = DataSender.instance;
        good = ds.UIImages[345];
        bad = ds.UIImages[348];
        heat = ds.UIImages[354];
        speed = ds.UIImages[449];
        strength = ds.UIImages[450];
        intellect = ds.UIImages[451];
        spriteDict = new Dictionary<string, Sprite>
        {
            { "good", good }, { "bad", bad }, { "heat", heat }, { "speed", speed }, { "strength", strength },
            { "intellect", intellect }, { "energy", energy }, { "health", health }
        };
    }
    public IEnumerator MakeEffect(Transform obj, string type, string layer)//heat, good, bad, speed, strength, intellect
    {
        Sprite sprite = spriteDict[type];
        GameObject effectObj = new GameObject("EffectObject");
        effectObj.AddComponent<SpriteRenderer>().sortingOrder = 100;
        effectObj.GetComponent<SpriteRenderer>().sortingLayerName = layer;
        effectObj.GetComponent<SpriteRenderer>().sprite = sprite;
        effectObj.GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
        effectObj.GetComponent<SpriteRenderer>().size = sizeDict[type];

        //2
        float time = 0f;
        while (time <= 1.75f)
        {
            float percentage = time / 1.75f;
            effectObj.transform.position = obj.position + new Vector3(0, .8f + (percentage * 2f), 0);
            effectObj.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f - percentage);
            time += Time.deltaTime;
            yield return null;
        }
        Destroy(effectObj);
    }
}
