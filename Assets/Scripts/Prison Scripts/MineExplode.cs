using System.Collections;
using UnityEngine;

public class MineExplode : MonoBehaviour
{
    private Transform player;
    private Death deathScript;
    private PlayerCollectionData pData;
    private Particles particlesScript;
    private FightEffects fightFX;

    private Sprite s1;
    private Sprite s2;
    private Sprite s3;
    private Sprite s4;
    private Sprite s5;
    private Sprite s6;
    private void Start()
    {
        player = RootObjectCache.GetRoot("Player").transform;
        pData = player.GetComponent<PlayerCollectionData>();
        deathScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Death>();
        particlesScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Particles>();
        fightFX = RootObjectCache.GetRoot("ScriptObject").GetComponent<FightEffects>();

        s1 = PadSpriteToSizeBottom(DataSender.instance.UIImages[363], 49, 76); //.033, .167
        s2 = PadSpriteToSizeBottom(DataSender.instance.UIImages[364], 49, 76);
        s3 = PadSpriteToSizeBottom(DataSender.instance.UIImages[365], 49, 76);
        s4 = PadSpriteToSizeBottom(DataSender.instance.UIImages[366], 49, 76);
        s5 = PadSpriteToSizeBottom(DataSender.instance.UIImages[367], 49, 76);
        s6 = PadSpriteToSizeBottom(DataSender.instance.UIImages[362], 49, 76);
    }
    private void Update()
    {
        if(!pData.playerData.isDead && !pData.playerData.inGodMode && player.GetComponent<CapsuleCollider2D>().IsTouching(GetComponent<BoxCollider2D>()))
        {
            deathScript.KillPlayer();
            StartCoroutine(Explode());
        }
    }
    private IEnumerator Explode()
    {
        PSoundController.PlaySound("explode");
        GameObject explosionObj = new GameObject("Explosion");
        explosionObj.AddComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
        explosionObj.GetComponent<SpriteRenderer>().sprite = s1;
        //49x76 245x380 190
        explosionObj.GetComponent<SpriteRenderer>().size = new Vector2(4.9f, 7.6f);
        explosionObj.transform.parent = transform.parent;
        explosionObj.transform.position = transform.position;
        explosionObj.transform.position += new Vector3(0, 0, -1);
        explosionObj.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;
        explosionObj.GetComponent<SpriteRenderer>().sortingLayerName = GetComponent<SpriteRenderer>().sortingLayerName;

        fightFX.MakeScreenShake();
        particlesScript.CreateDust(transform.position, 2, player.GetComponent<SpriteRenderer>().sortingLayerName);

        yield return new WaitForSeconds(.033f);
        explosionObj.GetComponent<SpriteRenderer>().sprite = s2;
        yield return new WaitForSeconds(.167f);
        explosionObj.GetComponent<SpriteRenderer>().sprite = s3;
        yield return new WaitForSeconds(.167f);
        explosionObj.GetComponent<SpriteRenderer>().sprite = s4;
        yield return new WaitForSeconds(.167f);
        explosionObj.GetComponent<SpriteRenderer>().sprite = s5;
        yield return new WaitForSeconds(.167f);
        explosionObj.GetComponent<SpriteRenderer>().sprite = s6;
        yield return new WaitForSeconds(.167f);

        Destroy(explosionObj);
    }
    private static Sprite PadSpriteToSizeBottom(Sprite source, int targetWidth, int targetHeight)
    {
        Texture2D sourceTex = source.texture;

        Rect rect = source.textureRect;

        // Extract the sprite's pixels from its texture
        Texture2D cropped = new Texture2D(
            (int)rect.width,
            (int)rect.height,
            TextureFormat.RGBA32,
            false);

        cropped.SetPixels(
            sourceTex.GetPixels(
                (int)rect.x,
                (int)rect.y,
                (int)rect.width,
                (int)rect.height));

        cropped.filterMode = FilterMode.Point;
        cropped.Apply();

        // Create transparent destination texture
        Texture2D result = new Texture2D(
            targetWidth,
            targetHeight,
            TextureFormat.RGBA32,
            false);

        Color[] pixels = new Color[targetWidth * targetHeight];
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = Color.clear;

        result.SetPixels(pixels);

        // Bottom-aligned, centered horizontally
        int xOffset = (targetWidth - cropped.width) / 2;
        int yOffset = 0;

        result.SetPixels(
            xOffset,
            yOffset,
            cropped.width,
            cropped.height,
            cropped.GetPixels());

        result.filterMode = FilterMode.Point;
        result.Apply();

        // Create sprite with same pixels-per-unit as original
        return Sprite.Create(
            result,
            new Rect(0, 0, result.width, result.height),
            new Vector2(0.5f, 0f), // pivot at bottom center
            source.pixelsPerUnit);
    }
}
