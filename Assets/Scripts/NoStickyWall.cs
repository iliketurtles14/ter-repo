using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoStickyWall : MonoBehaviour
{

    private BoxCollider2D m_BoxCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        m_BoxCollider = GetComponent<BoxCollider2D>();

        m_BoxCollider.size = new Vector2(1.6f, 1.6f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
