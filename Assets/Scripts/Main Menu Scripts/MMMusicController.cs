using UnityEngine;

public class MMMusicController : MonoBehaviour
{
    public bool canStartMusic;
    private bool hasStarted;
    private void Update()
    {
        if (canStartMusic && !hasStarted)
        {
            hasStarted = true;
            GetComponent<AudioSource>().clip = DataSender.instance.MusicList[40];
            GetComponent<AudioSource>().Play();
        }
    }
}
