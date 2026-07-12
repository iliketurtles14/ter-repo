using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextFix : EditorWindow
{
    private static readonly System.Collections.Generic.Dictionary<string, float> fontWordSpacingMap =
        new System.Collections.Generic.Dictionary<string, float>
        {
            { "Reincarcerated", -12.5f },
            { "Reincarcerated Outline", 12.5f },
            { "Reincarcerated Bold", 0f },
            { "Reincarcerated Bold Outline", 25f }
        };

    [MenuItem("Tools/TextMeshPro/Fix Word Spacing (Open Scenes + Prefabs)")]
    public static void FixWordSpacing()
    {
        if (!EditorUtility.DisplayDialog(
                "Fix TMP Word Spacing",
                "Set word spacing on all TextMeshProUGUI based on font?\n\n" +
                "Reincarcerated: -12.5\n" +
                "Reincarcerated Outline: 12.5\n" +
                "Reincarcerated Bold: 0\n" +
                "Reincarcerated Bold Outline: 25",
                "Yes",
                "No"))
        {
            return;
        }

        int changed = 0;

        changed += FixInOpenScenes();
        changed += FixInAllPrefabs();

        AssetDatabase.SaveAssets();
        Debug.Log($"Done. Updated {changed} TextMeshProUGUI component(s).");
    }

    private static int FixInOpenScenes()
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
                    if (UpdateWordSpacing(text))
                    {
                        changed++;
                        EditorUtility.SetDirty(text);
                    }
                }
            }
        }

        return changed;
    }

    private static int FixInAllPrefabs()
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
                if (UpdateWordSpacing(text))
                {
                    prefabChanged = true;
                    changed++;
                }
            }

            if (prefabChanged)
            {
                PrefabUtility.SaveAsPrefabAsset(prefabRoot, path);
            }

            PrefabUtility.UnloadPrefabContents(prefabRoot);
        }

        return changed;
    }

    private static bool UpdateWordSpacing(TextMeshProUGUI text)
    {
        if (text.font == null) return false;

        string fontName = text.font.name;

        if (fontWordSpacingMap.TryGetValue(fontName, out float spacing))
        {
            if (text.wordSpacing != spacing)
            {
                text.wordSpacing = spacing;
                return true;
            }
        }

        return false;
    }
}