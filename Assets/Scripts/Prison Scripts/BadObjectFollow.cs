using UnityEngine;

public class BadObjectFollow : MonoBehaviour
{
    private Transform target = null;
    private void Update()
    {
        if(GetComponent<BadObjectData>().attachedObject == null)
        {
            Destroy(gameObject);
        }
        try
        {
            target = GetComponent<BadObjectData>().attachedObject.transform;
        }
        catch
        {

        }


        if (target != null)
        {
            transform.position = target.position;
            gameObject.layer = target.gameObject.layer;
            if(target.gameObject.name == "Player")
            {
                if(!Physics2D.GetIgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ground")))
                {
                    gameObject.layer = LayerMask.NameToLayer("Ground");
                }
                else
                {
                    gameObject.layer = LayerMask.NameToLayer("Underground");//just something that isnt the ground
                }
            }
        }
    }
}
