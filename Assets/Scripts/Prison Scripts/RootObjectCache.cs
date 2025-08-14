using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class RootObjectCache : MonoBehaviour
{
    // Static dictionary accessible from anywhere
    private static Dictionary<string, GameObject> rootCache;

    void Awake()
    {
        if (rootCache != null) return; // Already initialized

        rootCache = new Dictionary<string, GameObject>();
        GameObject[] roots = SceneManager.GetActiveScene().GetRootGameObjects();

        foreach (GameObject root in roots)
        {
            if (!rootCache.ContainsKey(root.name))
                rootCache[root.name] = root;
            else
                Debug.LogWarning($"Duplicate root name detected: {root.name}");
        }
    }

    // Static method to get root objects from anywhere
    public static GameObject GetRoot(string name)
    {
        if (rootCache == null)
        {
            Debug.LogError("RootObjectCache has not been initialized yet!");
            return null;
        }

        if (rootCache.TryGetValue(name, out GameObject obj))
            return obj;

        Debug.LogWarning($"Root object '{name}' not found.");
        return null;
    }
}
