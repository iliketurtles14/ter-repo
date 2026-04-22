using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ToiletWater : MonoBehaviour
{
    public int waterLevel;
    private void OnEnable()
    {
        StartCoroutine(SpreadWater());
        StartCoroutine(WaterAnim());
    }
    private IEnumerator SpreadWater()
    {
        yield return new WaitForEndOfFrame();
        if(waterLevel == 0)
        {
            if (GetComponent<Rigidbody2D>() != null)
            {
                Destroy(GetComponent<Rigidbody2D>());
            }
            yield break;
        }

        float rand = UnityEngine.Random.Range(2f, 3f);
        yield return new WaitForSeconds(rand);

        //check for open spots
        for(int i = 0; i < 4; i++) //N,S,E,W
        {
            Vector2 pos = new Vector2(transform.position.x, transform.position.y);
            switch (i)
            {
                case 0:
                    pos += new Vector2(0, .8f);
                    break;
                case 1:
                    pos += new Vector2(0, -.8f);
                    break;
                case 2:
                    pos += new Vector2(.8f, 0);
                    break;
                case 3:
                    pos += new Vector2(-.8f, 0);
                    break;
            }
            
            GameObject toiletWater = Instantiate(Resources.Load<GameObject>("PrisonPrefabs/Objects/ToiletWater"));
            toiletWater.GetComponent<ToiletWater>().StopAllCoroutines();
            toiletWater.GetComponent<ToiletWater>().enabled = false;
            toiletWater.GetComponent<SpriteRenderer>().enabled = false;
            toiletWater.transform.parent = transform.parent;
            toiletWater.transform.position = pos;
            toiletWater.transform.position += new Vector3(0, 0, -1);
            toiletWater.name = "ToiletWater";
            yield return new WaitForFixedUpdate();

            Rigidbody2D rb = toiletWater.GetComponent<Rigidbody2D>();
            var cols = new List<Collider2D>();
            int count = rb.GetContacts(cols);

            bool shouldSpread = true;
            for(int j = 0; j < count; j++)
            {
                Collider2D col = cols[j];

                if((col.transform.parent.name == "Ground" && !col.CompareTag("Digable")) || col.name == "ToiletWater")
                {
                    shouldSpread = false;
                    break;
                }
            }

            if (!shouldSpread)
            {
                Destroy(toiletWater);
            }
            else
            {
                toiletWater.GetComponent<SpriteRenderer>().enabled = true;
                toiletWater.GetComponent<ToiletWater>().enabled = true;
                toiletWater.GetComponent<ToiletWater>().waterLevel = waterLevel - 1;
            }
        }
        if(GetComponent<Rigidbody2D>() != null)
        {
            Destroy(GetComponent<Rigidbody2D>());
        }
    }
    private IEnumerator WaterAnim()
    {
        DataSender ds = DataSender.instance;
        while (true)
        {
            GetComponent<SpriteRenderer>().sprite = ds.UIImages[132];
            yield return new WaitForSeconds(.533f);
            GetComponent<SpriteRenderer>().sprite = ds.UIImages[133];
            yield return new WaitForSeconds(.533f);
            GetComponent<SpriteRenderer>().sprite = ds.UIImages[134];
            yield return new WaitForSeconds(.533f);
            GetComponent<SpriteRenderer>().sprite = ds.UIImages[135];
            yield return new WaitForSeconds(.533f);
        }
    }
}
