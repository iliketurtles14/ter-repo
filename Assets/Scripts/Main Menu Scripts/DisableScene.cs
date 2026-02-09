using UnityEngine;
using UnityEngine.SceneManagement;

public class DisableScene : MonoBehaviour
{
    public void Start()
    {
        SceneManager.UnloadSceneAsync(1);
    }
}
