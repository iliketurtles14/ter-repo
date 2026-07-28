using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour
{
    private Camera mainCamera;
    public float speed;
    public Transform blocker;
    public AudioSource audioSource;
    public AudioClip creditsClip;
    public bool canStart = true;
    private void Start()
    {
        mainCamera = Camera.main;
    }
    public IEnumerator StartCredits()
    {
        audioSource.Stop();
        blocker.GetComponent<Animator>().enabled = true;
        blocker.GetComponent<Animator>().Rebind();
        blocker.GetComponent<Animator>().Update(0f);
        blocker.GetComponent<Animator>().Play(0, 0, 0f);
        yield return new WaitForSeconds(1.1f);
        blocker.GetComponent<Animator>().enabled = false;
        audioSource.clip = creditsClip;
        audioSource.Play();
        audioSource.loop = false;
        yield return new WaitForSeconds(240f / 126f);
        mainCamera.transform.position = new Vector2(960, 518);
        float time = 0f;
        while(time < 240f / 126f)
        {
            blocker.Find("1").GetComponent<Image>().color = new Color(0, 0, 0, (-time * (126f / 240f)) + 1);
            blocker.Find("2").GetComponent<Image>().color = new Color(0, 0, 0, (-time * (126f / 240f)) + 1);
            time += Time.deltaTime;
            yield return null;
        }
        blocker.Find("1").localPosition = new Vector2(-1440, 0);
        blocker.Find("2").localPosition = new Vector2(1440, 0);
        while (true)
        {
            mainCamera.transform.position += new Vector3(0, speed * -Time.deltaTime);
            if (mainCamera.transform.position.y <= 348)
            {
                break;
            }
            yield return null;
        }
        mainCamera.transform.position = new Vector3(960, 348);
        yield return new WaitForSeconds(2.5f);
        blocker.Find("1").localPosition = new Vector2(-480, 0);
        blocker.Find("2").localPosition = new Vector2(480, 0);
        time = 0;
        while(time < 3)
        {
            blocker.Find("1").GetComponent<Image>().color = new Color(0, 0, 0, time * (1f / 3f));
            blocker.Find("2").GetComponent<Image>().color = new Color(0, 0, 0, time * (1f / 3f));
            time += Time.deltaTime;
            yield return null;
        }
        blocker.Find("1").GetComponent<Image>().color = new Color(0, 0, 0, 1);
        blocker.Find("2").GetComponent<Image>().color = new Color(0, 0, 0, 1);
        audioSource.Stop();
        audioSource.loop = true;
        audioSource.clip = DataSender.instance.MusicList[40];
        audioSource.Play();
        mainCamera.transform.position = new Vector2(960, 540);
        time = 0;
        while (time < 1)
        {
            blocker.Find("1").GetComponent<Image>().color = new Color(0, 0, 0, -time + 1);
            blocker.Find("2").GetComponent<Image>().color = new Color(0, 0, 0, -time + 1);
            time += Time.deltaTime;
            yield return null;
        }
        blocker.Find("1").GetComponent<Image>().color = new Color(0, 0, 0, 0);
        blocker.Find("2").GetComponent<Image>().color = new Color(0, 0, 0, 0);
        blocker.Find("1").localPosition = new Vector2(-1440, 0);
        blocker.Find("2").localPosition = new Vector2(1440, 0);
        blocker.Find("1").GetComponent<Image>().color = new Color(0, 0, 0, 1);
        blocker.Find("2").GetComponent<Image>().color = new Color(0, 0, 0, 1);

        canStart = true;
    }
}
