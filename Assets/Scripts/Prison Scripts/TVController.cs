using System.Collections;
using UnityEngine;

public class TVController : MonoBehaviour
{
    //full - 6.655 frame - .05

    private Sprite tv1Sprite;
    private Sprite tv2Sprite;
    private Sprite tv3Sprite;
    private Sprite tv4Sprite;
    private Sprite tvOffSprite;

    private bool tvIsOn;
    private bool ready;

    private Coroutine tvStartCoroutine;
    private SpriteRenderer sr;

    private MouseCollisionOnItems mcs;
    private Transform player;

    private void Start()
    {
        mcs = RootObjectCache.GetRoot("InventoryCanvas").transform.Find("MouseOverlay").GetComponent<MouseCollisionOnItems>();
        sr = GetComponent<SpriteRenderer>();
        player = RootObjectCache.GetRoot("Player").transform;
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
        tv1Sprite = ds.PrisonObjectImages[207];
        tv2Sprite = ds.PrisonObjectImages[208];
        tv3Sprite = ds.PrisonObjectImages[209];
        tv4Sprite = ds.PrisonObjectImages[210];
        tvOffSprite = ds.PrisonObjectImages[205];
        ready = true;
    }
    private void Update()
    {
        if (!ready)
        {
            return;
        }

        if (!tvIsOn)
        {
            sr.sprite = tvOffSprite;
        }

        if(mcs.isTouchingTV && Input.GetMouseButtonDown(0) && mcs.touchedTV == gameObject)
        {
            float distance = Vector2.Distance(player.position, mcs.touchedTV.transform.position);
            if(distance <= 2.4f)
            {
                tvIsOn = true;
                if (tvStartCoroutine != null)
                {
                    StopCoroutine(tvStartCoroutine);
                }
                tvStartCoroutine = StartCoroutine(StartTV());
            }
        }
    }
    private IEnumerator StartTV() //its tv tiiiiiimmmmmeeeeeee!!!!!!
    {
        float end = Time.time + 6.655f;
        while(Time.time < end)
        {
            sr.sprite = tv1Sprite;
            yield return new WaitForSeconds(.05f);
            sr.sprite = tv2Sprite;
            yield return new WaitForSeconds(.05f);
            sr.sprite = tv3Sprite;
            yield return new WaitForSeconds(.05f);
            sr.sprite = tv4Sprite;
            yield return new WaitForSeconds(.05f);
        }
        tvIsOn = false;
    }
}
