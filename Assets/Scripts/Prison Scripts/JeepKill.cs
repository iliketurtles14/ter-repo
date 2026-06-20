using UnityEngine;

public class JeepKill : MonoBehaviour
{
    private Death deathScript;
    private void Start()
    {
        deathScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Death>();
    }
    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.name == "Player")
        {
            if (!otherCollider.GetComponent<PlayerCollectionData>().playerData.isDead)
            {
                deathScript.KillPlayer();
            }
        }
    }
}
