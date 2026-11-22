using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurtleGif : MonoBehaviour
{
    List<Sprite> frames = new List<Sprite>();
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void OnEnable()
    {
        LoadFrames();
        StartCoroutine(Anim());
    }
    private IEnumerator Anim()
    {
        Image img = GetComponent<Image>();
        int index = 0;
        while (true)
        {
            img.sprite = frames[index];
            index++;
            if (index == 34)
            {
                index = 0;
            }
            yield return new WaitForSeconds(.02f);
        }
    }
    private void LoadFrames()
    {
        if(frames.Count > 0)
        {
            frames.Clear();
        }
        
        for(int i = 0; i < 34; i++)
        {
            Sprite spr = Resources.Load<Sprite>("Main Menu Resources/Frames/frame" + i.ToString());
            frames.Add(spr); 
        }
    }
}
