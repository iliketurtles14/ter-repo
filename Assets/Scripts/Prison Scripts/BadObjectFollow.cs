using UnityEngine;

public class BadObjectFollow : MonoBehaviour
{
    private Transform target = null;
    private void Update()
    {
        target = GetComponent<BadObjectData>().attachedObject.transform;
        
        if(target != null)
        {
            transform.position = target.position;
        }
    }
}
