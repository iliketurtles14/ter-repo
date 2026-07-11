using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class DisableTmpRaycastTargets
{
    [MenuItem("Tools/TMP/Disable Raycast Target (Open Scenes + Prefabs)")]
    public static void DisableEverywhere()
    {
        if (!EditorUtility.DisplayDialog(
                "Disable TMP Raycast Target",
                "Set `raycastTarget = false` on all TextMeshProUGUI in open scenes and all prefabs?",
                "Yes",
                "No"))
        {
            return;
        }

        int changed = 0;

        changed += DisableInOpenScenes();
        changed += DisableInAllPrefabs();

        AssetDatabase.SaveAssets();
        Debug.Log($"Done. Updated {changed} TextMeshProUGUI component(s).");
    }

    private static int DisableInOpenScenes()
    {
        int changed = 0;

        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (!scene.isLoaded) continue;

            foreach (GameObject root in scene.GetRootGameObjects())
            {
                TextMeshProUGUI[] texts = root.GetComponentsInChildren<TextMeshProUGUI>(true);
                foreach (TextMeshProUGUI text in texts)
                {
                    if (!text.raycastTarget) continue;

                    Undo.RecordObject(text, "Disable TMP Raycast Target");
                    text.raycastTarget = false;
                    EditorUtility.SetDirty(text);
                    changed++;
                }
            }
        }

        return changed;
    }

    private static int DisableInAllPrefabs()
    {
        int changed = 0;
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab");

        foreach (string guid in prefabGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefabRoot = PrefabUtility.LoadPrefabContents(path);

            bool prefabChanged = false;
            TextMeshProUGUI[] texts = prefabRoot.GetComponentsInChildren<TextMeshProUGUI>(true);

            foreach (TextMeshProUGUI text in texts)
            {
                if (!text.raycastTarget) continue;

                text.raycastTarget = false;
                prefabChanged = true;
                changed++;
            }

            if (prefabChanged)
            {
                PrefabUtility.SaveAsPrefabAsset(prefabRoot, path);
            }

            PrefabUtility.UnloadPrefabContents(prefabRoot);
        }

        return changed;
    }
}