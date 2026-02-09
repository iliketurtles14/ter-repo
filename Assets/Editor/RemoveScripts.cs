using UnityEngine;
using UnityEditor;

public class RemoveMissingScriptsByTag : MonoBehaviour
{
    [MenuItem("Tools/Remove Missing Scripts from 'Item' Prefabs")]
    private static void RemoveMissingScriptsFromTaggedPrefabs()
    {
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab");
        int removedCount = 0;

        foreach (string guid in prefabGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (prefab != null && prefab.CompareTag("Item"))
            {
                // Instantiate the prefab, clean it up, then save it back
                GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                if (instance != null)
                {
                    removedCount += GameObjectUtility.RemoveMonoBehavioursWithMissingScript(instance);
                    PrefabUtility.SaveAsPrefabAsset(instance, path);
                    DestroyImmediate(instance);
                }
            }
        }

        Debug.Log($"Removed {removedCount} missing script components from 'Item' prefabs.");
    }
}
