using UnityEngine;

public class GroundSpriteSetter : MonoBehaviour
{
    public Transform grounds;
    public GroundSizeSet sizeScript;
    public void SetGround(Sprite groundSprite, bool isTiled)
    {
        grounds.Find("Ground").GetComponent<SpriteRenderer>().sprite = groundSprite;
        Texture2D groundTex = groundSprite.texture;
        if (isTiled)
        {
            grounds.Find("Ground").GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Tiled;
        }
        else
        {
            grounds.Find("Ground").GetComponent<SpriteRenderer>().drawMode = SpriteDrawMode.Sliced;
        }
        sizeScript.SetSize();
    }
}
