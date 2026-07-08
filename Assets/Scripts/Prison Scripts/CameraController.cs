using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    private int followDir = 0; //0-4 looking left to looking right
    private int scanDir = 0;
    public int camMode = 0; //0 is normal, 1 is chasing, 2 is off
    public float angleTest;
    private bool ready = false;
    private List<Vector2> vectors0 = new List<Vector2>();
    private List<Vector2> vectors1 = new List<Vector2>();
    private List<Vector2> vectors2 = new List<Vector2>();
    private List<Vector2> vectors3 = new List<Vector2>();
    private List<Vector2> vectors4 = new List<Vector2>();
    private List<Sprite> normalSprites = new List<Sprite>();
    private List<Sprite> seenSprites = new List<Sprite>();
    private List<Sprite> offSprites = new List<Sprite>();
    private List<Vector2> currentVectors = new List<Vector2>();
    private List<string> availableBadObjects = new List<string>
    {
        "onDesk", "pickedUp", "inWrongCell", "playerPunching", "wrongRoutine", "noOutfit", "guardBreakingTile"
    };
    private float rangeOfSight = 8f;
    private PauseController pc;
    private Map currentMap;
    private Transform player;
    private StatEffects statEffectsScript;
    private GeneratorController genScript;
    public int camTime;
    private void Update()
    {
        if(camMode != 2)
        {
            camTime = -1;
        }
    }
    private void Start()
    {
        Transform so = RootObjectCache.GetRoot("ScriptObject").transform;
        statEffectsScript = so.GetComponent<StatEffects>();
        player = RootObjectCache.GetRoot("Player").transform;
        pc = so.GetComponent<PauseController>();
        genScript = so.GetComponent<GeneratorController>();
        MakeVectorLists();
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        currentMap = RootObjectCache.GetRoot("ScriptObject").GetComponent<LoadPrison>().currentMap;
        MakeSpriteLists();
        StartCoroutine(LookWait());       //all these loops are made to run in parallel to eachother
        StartCoroutine(Following());
        StartCoroutine(ScanAnim());
        StartCoroutine(FollowAnim());
        StartCoroutine(CooldownWait());
        ready = true;
    }
    private void MakeSpriteLists()
    {
        DataSender ds = DataSender.instance;
        normalSprites.Add(ds.PrisonObjectImages[70]);
        normalSprites.Add(ds.PrisonObjectImages[71]);
        normalSprites.Add(ds.PrisonObjectImages[72]);
        normalSprites.Add(ds.PrisonObjectImages[73]);
        normalSprites.Add(ds.PrisonObjectImages[74]);
        seenSprites.Add(ds.PrisonObjectImages[157]);
        seenSprites.Add(ds.PrisonObjectImages[158]);
        seenSprites.Add(ds.PrisonObjectImages[159]);
        seenSprites.Add(ds.PrisonObjectImages[160]);
        seenSprites.Add(ds.PrisonObjectImages[161]);
        offSprites.Add(ds.PrisonObjectImages[168]);
        offSprites.Add(ds.PrisonObjectImages[169]);
        offSprites.Add(ds.PrisonObjectImages[170]);
        offSprites.Add(ds.PrisonObjectImages[171]);
        offSprites.Add(ds.PrisonObjectImages[172]);
    }
    private void MakeVectorLists()
    {
        vectors0.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 180f), Mathf.Sin(Mathf.Deg2Rad * -180f)));
        vectors0.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 162f), Mathf.Sin(Mathf.Deg2Rad * -162f)));
        vectors1.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 144f), Mathf.Sin(Mathf.Deg2Rad * -144f)));
        vectors1.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 126f), Mathf.Sin(Mathf.Deg2Rad * -126f)));
        vectors2.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 108f), Mathf.Sin(Mathf.Deg2Rad * -108f)));
        vectors2.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 90f), Mathf.Sin(Mathf.Deg2Rad * -90f)));
        vectors2.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 72f), Mathf.Sin(Mathf.Deg2Rad * -72f)));
        vectors3.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 54f), Mathf.Sin(Mathf.Deg2Rad * -54f)));
        vectors3.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 36f), Mathf.Sin(Mathf.Deg2Rad * -36f)));
        vectors4.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 18f), Mathf.Sin(Mathf.Deg2Rad * -18f)));
        vectors4.Add(new Vector2(Mathf.Cos(Mathf.Deg2Rad * 0f), Mathf.Sin(Mathf.Deg2Rad * -0f)));
    }
    private IEnumerator LookWait()
    {
        float rand = UnityEngine.Random.Range(0f, .1f);
        yield return new WaitForSeconds(rand);
        StartCoroutine(Scanning());
    }
    private IEnumerator Scanning()
    {
        while (true)
        {
            if(camMode != 0)
            {
                yield return null;
                continue;
            }
            
            switch (scanDir)
            {
                case 0:
                    currentVectors = vectors0;
                    break;
                case 1:
                    currentVectors = vectors1;
                    break;
                case 2:
                    currentVectors = vectors2;
                    break;
                case 3:
                    currentVectors = vectors3;
                    break;
                case 4:
                    currentVectors = vectors4;
                    break;
                default:
                    currentVectors = vectors2;
                    break;
            }

            foreach(Vector2 vector in currentVectors)
            {
                RaycastHit2D[] wallHits = Physics2D.RaycastAll(transform.position, vector, rangeOfSight);

                RaycastHit2D? wallHit = null;
                foreach (RaycastHit2D aHit in wallHits)
                {
                    if (aHit.collider.CompareTag("Wall"))
                    {
                        wallHit = aHit;
                        break;
                    }
                }

                if (wallHit.HasValue)
                {
                    Debug.DrawLine(transform.position, wallHit.Value.point, Color.red);
                }
                else
                {
                    Debug.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + vector * rangeOfSight, Color.green);
                }

                RaycastHit2D[] badHits;
                try
                {
                    badHits = Physics2D.RaycastAll(transform.position, wallHit.Value.point, wallHit.Value.distance);

                }
                catch
                {
                    badHits = Physics2D.RaycastAll(transform.position, vector, rangeOfSight);
                }

                RaycastHit2D? badHit = null;
                foreach (RaycastHit2D aHit in badHits)
                {
                    if (aHit.collider.CompareTag("BadObject"))
                    {
                        Debug.Log(aHit.collider.name);
                    }
                }
                foreach (RaycastHit2D aHit in badHits)
                {
                    if (aHit.collider.CompareTag("BadObject"))
                    {
                        if (availableBadObjects.Contains(aHit.collider.name))
                        {
                            badHit = aHit;
                            SeeBadAction(aHit.transform.gameObject);
                        }
                    }
                }

                if (badHit.HasValue)
                {
                    Debug.DrawLine(transform.position, badHit.Value.point, Color.red);
                }
                else
                {
                    Debug.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + vector * rangeOfSight, Color.green);
                }
            }
            yield return new WaitForSeconds(.1f);
        }
    }
    private IEnumerator Following()
    {
        while (true)
        {
            if(camMode != 1)
            {
                yield return null;
                continue;
            }

            if(player.position.y <= transform.position.y)
            {
                Vector2 dir = (player.position - transform.position).normalized;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; angleTest = angle;
                angle *= -1;
                angle = 180 - angle;
                if (angle >= 0 && angle < 36)
                {
                    followDir = 0;
                }
                else if (angle >= 36 && angle < 72)
                {
                    followDir = 1;
                }
                else if(angle >= 72 && angle < 108)
                {
                    followDir = 2;
                }
                else if(angle >= 108 && angle < 144)
                {
                    followDir = 3;
                }
                else if(angle >= 144 && angle <= 180)
                {
                    followDir = 4;
                }
            }
            else
            {
                if(player.position.x >= transform.position.x)
                {
                    followDir = 4;
                }
                else if(player.position.x < transform.position.x)
                {
                    followDir = 0;
                }
            }
            yield return null;
        }
    }
    private void SeeBadAction(GameObject badObject)
    {
        camMode = 1;
        
        BadObjectData data = badObject.GetComponent<BadObjectData>();

        //add heat
        int heatToAdd = 0;
        if (data.isMultiplied)
        {
            heatToAdd = data.heatGain * currentMap.npcLevel;
        }
        else
        {
            heatToAdd = data.heatGain;
        }
        if(heatToAdd > 0)
        {
            player.GetComponent<PlayerCollectionData>().playerData.heat += heatToAdd;
            StartCoroutine(statEffectsScript.MakeEffect(transform, "heat"));
        }

        if(data.heatSet != -1)
        {
            player.GetComponent<PlayerCollectionData>().playerData.heat = data.heatSet;
            StartCoroutine(statEffectsScript.MakeEffect(transform, "heat"));
        }
        //send guard

    }
    private IEnumerator CooldownWait()
    {
        float time = 0f;
        while (true)
        {
            if(camMode == 0)
            {
                time = 0f;
            }
            else if (camMode == 1)
            {
                if(Vector2.Distance(player.position, transform.position) >= 13)
                {
                    camMode = 0;
                    yield return null;
                    continue;
                }
                if (!pc.isPaused)
                {
                    time += Time.deltaTime;
                }
                if(time >= 5)
                {
                    camMode = 0;
                    yield return null;
                    continue;
                }
                yield return null;
            }
            yield return null;
        }
    }
    private IEnumerator ScanAnim()//1.017
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        while (true)
        {
            for(int i = 0; i < 8; i++)
            {
                int index = i;
                switch (i)
                {
                    case 5:
                        index = 3;
                        break;
                    case 6:
                        index = 2;
                        break;
                    case 7:
                        index = 1;
                        break;
                }
                scanDir = index;
                if (camMode == 0)
                {
                    sr.sprite = normalSprites[index];
                }
                float time = 0f;
                while(time < 1.017f)
                {
                    if (camMode != 0 || pc.isPaused)
                    {
                        yield return null;
                        continue;
                    }
                    time += Time.deltaTime;
                    yield return null;
                }
            }
        }
    }
    private IEnumerator FollowAnim()//.55
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        while (true)
        {
            float time = 0f;
            while (time < .55f)
            {
                if (camMode != 1 || pc.isPaused)
                {
                    yield return null;
                    continue;
                }
                sr.sprite = seenSprites[followDir];
                time += Time.deltaTime;
                yield return null;
            }
            time = 0f;
            while(time < .55f)
            {
                if(camMode != 1 || pc.isPaused)
                {
                    yield return null;
                    continue;
                }
                sr.sprite = normalSprites[followDir];
                time += Time.deltaTime;
                yield return null;
            }
            yield return null;
        }
    }
    public IEnumerator TurnOffCam(float time, bool infinite)
    {
        int lookDir = 2;
        if(camMode == 0)
        {
            lookDir = scanDir;
        }
        else if(camMode == 1)
        {
            lookDir = followDir;
        }

        camMode = 2;
        yield return new WaitForEndOfFrame();

        GetComponent<SpriteRenderer>().sprite = offSprites[lookDir];

        if (!infinite)
        {
            float aTime = 0f;
            while (aTime <= time)
            {
                if (pc.isPaused)
                {
                    yield return null;
                    continue;
                }
                aTime += Time.deltaTime;
                camTime = Mathf.FloorToInt(time) - Mathf.FloorToInt(aTime);
                yield return null;
            }
            TurnOnCam();
        }
    }
    public void TurnOnCam()
    {
        camMode = 0;
    }
}
