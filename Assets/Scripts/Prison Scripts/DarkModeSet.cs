using System.IO;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DarkModeSet : MonoBehaviour
{
    private IniFile data;
    private Light2D globalLight;
    private LightController lightCtrl;
    private Transform undergroundLight;
    private Transform player;
    private void Start()
    {
        data = new IniFile(Path.Combine(Application.streamingAssetsPath, "UserData.ini"));
        globalLight = RootObjectCache.GetRoot("GlobalLight").GetComponent<Light2D>();
        lightCtrl = RootObjectCache.GetRoot("GlobalLight").GetComponent<LightController>();
        player = RootObjectCache.GetRoot("Player").transform;
        undergroundLight = RootObjectCache.GetRoot("UndergroundLight").transform;
    }
    private void Update()
    {
        if(data.Read("DarkMode", "Settings") == "True")
        {
            globalLight.intensity = 0;
            lightCtrl.enabled = false;
            undergroundLight.gameObject.SetActive(true);
            undergroundLight.position = player.position;
        }
    }
}
