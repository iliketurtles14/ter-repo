using UnityEngine;
using UnityEditor;

public class CopyGameObjectNames
{
    [MenuItem("Tools/Copy Selected GameObject Names")]
    private static void CopyNames()
    {
        var selected = Selection.gameObjects;
        if (selected.Length == 0)
        {
            Debug.Log("No GameObjects selected.");
            return;
        }

        string names = "";
        foreach (var obj in selected)
        {
            names += obj.name + "\n";
        }

        EditorGUIUtility.systemCopyBuffer = names;
        Debug.Log("Copied names to clipboard!");
    }
}
