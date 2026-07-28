using UnityEngine;

public class SpotlightCatch : MonoBehaviour
{
    private Solitary solitaryScript;
    private void Start()
    {
        solitaryScript = RootObjectCache.GetRoot("ScriptObject").GetComponent<Solitary>();
    }
    private void Update()
    {
        transform.localPosition = Vector2.zero;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            string outfit = col.GetComponent<OutfitController>().outfit;
            bool sendToSolitary = false;
            switch (outfit)
            {
                case "Inmate":
                case "POW":
                case "Elf":
                case "Tux":
                case "Prisoner":
                    sendToSolitary = true;
                    break;
            }
            if (!col.transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled)
            {
                sendToSolitary = true;
            }

            if (sendToSolitary)
            {
                StartCoroutine(solitaryScript.GoToSolitary("CaughtSpotlight"));
            }
        }
    }
}
