using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShowerOutfit : MonoBehaviour
{
    private PlayerIDInv playerIDInvScript;
    private SpriteRenderer outfitSR;
    public bool isShowering;
    private void Start()
    {
        outfitSR = transform.Find("Outfit").GetComponent<SpriteRenderer>();
        playerIDInvScript = RootObjectCache.GetRoot("MenuCanvas").transform.Find("PlayerMenuPanel").GetComponent<PlayerIDInv>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.name == "InmateShower")
        {
            isShowering = true;
            outfitSR.enabled = false;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.name == "InmateShower")
        {
            isShowering = false;
            if(playerIDInvScript.idInv[0].itemData != null)
            {
                outfitSR.enabled = true;
            }
        }
    }
}
