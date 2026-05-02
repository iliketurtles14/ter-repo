using System.Collections;
using UnityEngine;

public class GeneratorAnimation : MonoBehaviour
{
    private GeneratorController genController;
    private Sprite genOn1;
    private Sprite genOn2;
    private Sprite genOff1;
    private Sprite genOff2;

    private void Start()
    {
        genController = RootObjectCache.GetRoot("ScriptObject").GetComponent<GeneratorController>();
        StartCoroutine(StartWait());
    }
    private IEnumerator StartWait()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        DataSender ds = DataSender.instance;
        genOn1 = ds.PrisonObjectImages[165];
        genOn2 = ds.PrisonObjectImages[164];
        genOff1 = ds.PrisonObjectImages[162];
        genOff2 = ds.PrisonObjectImages[163];

        StartCoroutine(Anim());
    }
    private IEnumerator Anim()
    {
        while (true)
        {
            if (genController.genIsOff)
            {
                GetComponent<SpriteRenderer>().sprite = genOff1;
                float time = 0;
                while (time <= .55f && genController.genIsOff)
                {
                    yield return null;
                    time += Time.deltaTime;
                }
                GetComponent<SpriteRenderer>().sprite = genOff2;
                time = 0;
                while (time <= .55f && genController.genIsOff)
                {
                    yield return null;
                    time += Time.deltaTime;
                }
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = genOn1;
                float time = 0;
                while (time <= .55f && !genController.genIsOff)
                {
                    yield return null;
                    time += Time.deltaTime;
                }
                GetComponent<SpriteRenderer>().sprite = genOn2;
                time = 0;
                while (time <= .55f && !genController.genIsOff)
                {
                    yield return null;
                    time += Time.deltaTime;
                }
            }
            yield return null;
        }
    }
}
