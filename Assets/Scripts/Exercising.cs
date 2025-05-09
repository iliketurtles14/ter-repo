using System.Collections;
using UnityEngine;

public class Exercising : MonoBehaviour
{
    public MouseCollisionOnItems mcs;
    public ItemBehaviours itemBehavioursScript;
    private GameObject barLine;
    public GameObject actionBarPanel;
    private GameObject currentEquipment;
    private bool onEquipment = false;
    private bool onQ = true;
    private bool onE = false;
    private int amountOfBars = 0;
    public void Start()
    {
        barLine = Resources.Load<GameObject>("BarLine");
        StartCoroutine(BarLoop());
    }
    public void Update()
    {
        if (mcs.isTouchingEquipment && Input.GetMouseButtonDown(0) && !onEquipment)
        {
            float distance = Vector2.Distance(transform.position, mcs.touchedEquipment.transform.position);
            if(distance <= 2.4f)
            {
                currentEquipment = mcs.touchedEquipment;
                onEquipment = true;
                StartCoroutine(ClimbEquipment());
            }
        }
    }
    public IEnumerator ClimbEquipment()
    {
        currentEquipment.GetComponent<BoxCollider2D>().enabled = false;
        
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        GetComponent<PlayerCtrl>().enabled = false;
        while (Vector2.Distance(transform.position, currentEquipment.transform.position) > .1f)
        {
            transform.position += 5f * Time.deltaTime * (currentEquipment.transform.position - transform.position).normalized;
            yield return null;
        }
        transform.position = currentEquipment.transform.position;
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        GetComponent<PlayerCtrl>().enabled = true;

        if (currentEquipment.name.StartsWith("BenchPress"))
        {
            StartCoroutine(BenchPress());
        }
    }
    public IEnumerator BenchPress()
    {
        StopCoroutine(ClimbEquipment());
        StartCoroutine(itemBehavioursScript.DrawActionBar(false, false));
        GetComponent<PlayerAnimation>().enabled = false;
        itemBehavioursScript.CreateActionText("asdf");
        BodyController bodyController = GetComponent<BodyController>();
        OutfitController outfitController = GetComponent<OutfitController>();

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Q) && onQ)
            {
                onQ = false;
                onE = true;

                amountOfBars += 10;
            }
            else if(Input.GetKeyDown(KeyCode.E) && onE)
            {
                onQ = true;
                onE = false;

                amountOfBars += 10;
            }

            if(amountOfBars > 49)
            {
                amountOfBars = 49;
            }
            
            foreach(Transform barLine in actionBarPanel.transform)
            {
                Destroy(barLine.gameObject);
            }
            for(int i = 0; i < amountOfBars; i++)
            {
                Instantiate(barLine, actionBarPanel.transform);
            }

            if(amountOfBars >= 0 && amountOfBars < 16)
            {
                GetComponent<SpriteRenderer>().sprite = bodyController.characterDict[bodyController.character][8][0];
                if(transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled == true)
                {
                    transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = outfitController.outfitDict[outfitController.outfit][8][0];
                }
            }
            else if(amountOfBars >= 16 && amountOfBars < 32)
            {
                GetComponent<SpriteRenderer>().sprite = bodyController.characterDict[bodyController.character][8][1];
                if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled == true)
                {
                    transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = outfitController.outfitDict[outfitController.outfit][8][1];
                }
            }
            else if(amountOfBars >= 32)
            {
                GetComponent<SpriteRenderer>().sprite = bodyController.characterDict[bodyController.character][8][2];
                if (transform.Find("Outfit").GetComponent<SpriteRenderer>().enabled == true)
                {
                    transform.Find("Outfit").GetComponent<SpriteRenderer>().sprite = outfitController.outfitDict[outfitController.outfit][8][2];
                }
            }

            yield return null;
        }
    }
    public IEnumerator BarLoop()
    {
        while (true)
        {
            if (amountOfBars > 0)
            {
                amountOfBars--;
                yield return new WaitForSeconds(.015f);
            }
            yield return null;
        }
    }
}
