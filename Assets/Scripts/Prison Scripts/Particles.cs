using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{
    private List<Sprite> dustSprites = new List<Sprite>();
    private List<Sprite> dirtSprites = new List<Sprite>();
    private Transform tiles;
    private void Start()
    {
        tiles = RootObjectCache.GetRoot("Tiles").transform;
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        MakeLists();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            StartCoroutine(CreateDust(Camera.main.ScreenToWorldPoint(Input.mousePosition), 1));
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            StartCoroutine(CreateDirtParticles(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
        }
    }
    private void MakeLists()
    {
        DataSender ds = DataSender.instance;

        //dust
        for(int i = 17; i <= 24; i++)
        {
            dustSprites.Add(ds.UIImages[i]);
        }
        //dirt
        for(int i = 12; i >= 8; i--)
        {
            dirtSprites.Add(ds.UIImages[i]);
        }
    }
    public IEnumerator CreateDust(Vector2 pos, float sizeScale)
    {
        GameObject dust = new GameObject();
        dust.AddComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
        dust.GetComponent<SpriteRenderer>().sortingOrder = 100;
        dust.name = "Dust";
        dust.transform.parent = tiles.Find("GroundObjects"); //just doing this to make it organized
        dust.transform.position = pos;

        Vector2 size = new Vector2(1.6f * sizeScale, 1.6f * sizeScale);
        SpriteRenderer sr = dust.GetComponent<SpriteRenderer>();
        for(int i = 0; i < dustSprites.Count; i++)
        {
            sr.sprite = dustSprites[i];
            sr.size = size;
            yield return new WaitForSeconds(.04375f);//one frame where 45 frames is 1 second
        }

        Destroy(dust);
    }
    public IEnumerator CreateDirtParticles(Vector2 pos)
    {
        GameObject dirt = new GameObject();
        dirt.AddComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
        dirt.GetComponent<SpriteRenderer>().sortingOrder = 101;
        dirt.name = "DirtParticles";
        dirt.transform.parent = tiles.Find("GroundObjects");
        dirt.transform.position = pos;

        SpriteRenderer sr = dirt.GetComponent<SpriteRenderer>();
        Vector2 size = new Vector2(1.6f, 1.6f);
        for(int i = 0; i < dirtSprites.Count; i++)
        {
            sr.sprite = dirtSprites[i];
            sr.size = size;
            yield return new WaitForSeconds(.07f);
        }

        Destroy(dirt);
    }
}
