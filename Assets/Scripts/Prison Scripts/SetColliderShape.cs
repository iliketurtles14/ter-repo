using Unity.VisualScripting;
using UnityEngine;

public class SetColliderShape : MonoBehaviour
{
    public Sprite sprite;
    private void Start()
    {
        bool hasTransparency = false;
        Texture2D tex = sprite.texture;
        Rect rect = sprite.textureRect;
        Color[] pixels = tex.GetPixels(
            (int)rect.x,
            (int)rect.y,
            (int)rect.width,
            (int)rect.height);
        
        foreach(Color c in pixels)
        {
            if(c.a == 0)
            {
                hasTransparency = true;
                break;
            }
        }

        if (hasTransparency)
        {
            bool isTrigger = GetComponent<BoxCollider2D>().isTrigger;
            Destroy(GetComponent<BoxCollider2D>());
            this.AddComponent<SpriteRenderer>().sprite = sprite;
            this.AddComponent<PolygonCollider2D>().isTrigger = isTrigger;
            Destroy(GetComponent<SpriteRenderer>());
            PolygonCollider2D poly = GetComponent<PolygonCollider2D>();
            for (int pathIndex = 0; pathIndex < poly.pathCount; pathIndex++)// scale the collider by 10x (this js needs to happen idk why prolly cuz sprite too small or sum)
            {
                Vector2[] points = poly.GetPath(pathIndex);

                for (int i = 0; i < points.Length; i++)
                {
                    points[i] *= 10f;
                }

                poly.SetPath(pathIndex, points);
            }
            poly.sharedMaterial = Resources.Load<PhysicsMaterial2D>("PrisonResources/NoFriction");
        }
    }
}
