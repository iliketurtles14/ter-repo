using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMSoundController : MonoBehaviour
{
    private Dictionary<string, int> soundDict = new Dictionary<string, int>
    {
        { "door", 0 },
        { "step", 1 },
        { "hit", 2 },
        { "pickup", 3 },
        { "throw", 4 },
        { "open", 5 },
        { "close", 6 },
        { "en_hit", 7 },
        { "lose", 8 },
        { "accolade", 9 },
        { "hp", 10 },
        { "buy", 11 },
        { "bell", 12 },
        { "hit1", 13 },
        { "hit2", 14 },
        { "rumble", 15 },
        { "plip", 16 },
        { "toilet", 17 },
        { "shoot", 18 },
        { "generator", 19 },
        { "electric", 20 },
        { "punch_new", 21 },
        { "explode", 22 },
        { "jeep", 23 },
        { "TANK_BOOM", 24 }
    };

    public void PlaySound(string clipName)
    {
        int clipNum = soundDict[clipName];
        GetComponent<AudioSource>().PlayOneShot(DataSender.instance.SoundList[clipNum]);
    }


    /*
    //private void Update()
    //{
    //    //if (Input.GetKeyDown(KeyCode.F5))
    //    //{
    //    //    StartCoroutine(PlaySounds());
    //    //}
    //}
    //private IEnumerator PlaySounds()
    //{
    //    int i = 0;
    //    foreach(AudioClip clip in DataSender.instance.SoundList)
    //    {
    //        Debug.Log("Playing clip number " + i.ToString() + ".");
            
    //        GetComponent<AudioSource>().clip = clip;
    //        GetComponent<AudioSource>().Play();

    //        while (GetComponent<AudioSource>().isPlaying)
    //        {
    //            yield return null;
    //        }

    //        yield return new WaitForSeconds(1.5f);
    //        i++;
    //    }
    //}*/
}
