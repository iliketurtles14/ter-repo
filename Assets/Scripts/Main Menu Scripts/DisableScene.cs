using UnityEngine;
using UnityEngine.SceneManagement;

public class DisableScene : MonoBehaviour
{
    public void Start()
    {
        SceneManager.UnloadSceneAsync(1);
        SceneManager.UnloadSceneAsync(2);
    }
}
