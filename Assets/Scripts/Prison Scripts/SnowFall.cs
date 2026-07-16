using System.Collections;
using UnityEngine;

public class SnowFall : MonoBehaviour
{
    public float speed = -1;
    public float horizontalMovement;
    private void Start()
    {
        StartCoroutine(Fall());
    }
    private IEnumerator Fall()
    {
        while (true)
        {
            if(speed == -1)
            {
                yield return null;
                continue;
            }
            
            transform.position += new Vector3(horizontalMovement * Time.deltaTime, -1 * speed * Time.deltaTime, 0);
            if(transform.position.y <= 0)
            {
                Destroy(gameObject);
                yield break;
            }
            yield return null;
        }
    }
}
