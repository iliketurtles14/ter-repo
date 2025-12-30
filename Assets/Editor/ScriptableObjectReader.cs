using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.IO;
using System;

public class ScriptableObjectReader : EditorWindow
{
    private string folderPath = "Assets/";
    private List<ScriptableObject> foundObjects = new List<ScriptableObject>();
    private Dictionary<ScriptableObject, Dictionary<string, object>> extractedData = new Dictionary<ScriptableObject, Dictionary<string, object>>();
    private Vector2 scrollPosition;
    private bool showValues = false;

    [MenuItem("Tools/Scriptable Object Reader")]
    public static void ShowWindow()
    {
        GetWindow<ScriptableObjectReader>("SO Reader");
    }

    private void OnGUI()
    {
        GUILayout.Label("Scriptable Object Reader", EditorStyles.boldLabel);
        
        EditorGUILayout.Space();
        
        // Folder path selection
        EditorGUILayout.BeginHorizontal();
        folderPath = EditorGUILayout.TextField("Folder Path:", folderPath);
        if (GUILayout.Button("Browse", GUILayout.Width(60)))
        {
            string path = EditorUtility.OpenFolderPanel("Select Folder", "Assets", "");
            if (!string.IsNullOrEmpty(path))
            {
                folderPath = "Assets" + path.Substring(Application.dataPath.Length);
            }
        }
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.Space();
        
        // Read button
        if (GUILayout.Button("Read ScriptableObjects", GUILayout.Height(30)))
        {
            ReadScriptableObjects();
        }
        
        EditorGUILayout.Space();
        
        // Display results
        if (foundObjects.Count > 0)
        {
            EditorGUILayout.LabelField($"Found {foundObjects.Count} ScriptableObject(s)", EditorStyles.boldLabel);
            
            showValues = EditorGUILayout.Toggle("Show Values", showValues);
            
            EditorGUILayout.Space();
            
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            
            foreach (var so in foundObjects)
            {
                if (so == null) continue;
                
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField($"Object: {so.name}", EditorStyles.boldLabel);
                EditorGUILayout.LabelField($"Type: {so.GetType().Name}");
                
                if (showValues && extractedData.ContainsKey(so))
                {
                    EditorGUI.indentLevel++;
                    foreach (var kvp in extractedData[so])
                    {
                        string valueStr = kvp.Value != null ? kvp.Value.ToString() : "null";
                        EditorGUILayout.LabelField($"{kvp.Key}: {valueStr}");
                    }
                    EditorGUI.indentLevel--;
                }
                
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }
            
            EditorGUILayout.EndScrollView();
            
            EditorGUILayout.Space();
            
            // Export buttons
            if (GUILayout.Button("Log All Data to Console"))
            {
                LogDataToConsole();
            }

            if (GUILayout.Button("Export to Desktop Text File"))
            {
                ExportToTextFile();
            }
            
            if (GUILayout.Button("Clear Results"))
            {
                foundObjects.Clear();
                extractedData.Clear();
            }
        }
    }

    private void ReadScriptableObjects()
    {
        foundObjects.Clear();
        extractedData.Clear();
        
        // Find all assets in the folder
        string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { folderPath });
        
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            ScriptableObject so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(assetPath);
            
            if (so != null)
            {
                foundObjects.Add(so);
                
                // Extract all public fields and properties
                Dictionary<string, object> data = new Dictionary<string, object>();
                System.Type type = so.GetType();
                
                // Get all public fields
                FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
                foreach (var field in fields)
                {
                    object value = field.GetValue(so);
                    data[field.Name] = value;
                }
                
                // Get all public properties with getters
                PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var property in properties)
                {
                    if (property.CanRead && property.GetIndexParameters().Length == 0)
                    {
                        try
                        {
                            object value = property.GetValue(so);
                            data[property.Name] = value;
                        }
                        catch
                        {
                            // Skip properties that throw exceptions
                        }
                    }
                }
                
                extractedData[so] = data;
            }
        }
        
        Debug.Log($"Found {foundObjects.Count} ScriptableObject(s) in {folderPath}");
    }

    private void LogDataToConsole()
    {
        foreach (var so in foundObjects)
        {
            if (so == null) continue;
            
            Debug.Log($"=== {so.name} ({so.GetType().Name}) ===");
            
            if (extractedData.ContainsKey(so))
            {
                foreach (var kvp in extractedData[so])
                {
                    string valueStr = kvp.Value != null ? kvp.Value.ToString() : "null";
                    Debug.Log($"  {kvp.Key}: {valueStr}");
                }
            }
        }
    }

    private void ExportToTextFile()
    {
        try
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string fileName = $"ScriptableObjectData_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";
            string filePath = Path.Combine(desktopPath, fileName);

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("===========================================");
                writer.WriteLine("SCRIPTABLE OBJECT DATA EXPORT");
                writer.WriteLine($"Export Date: {DateTime.Now}");
                writer.WriteLine($"Folder Path: {folderPath}");
                writer.WriteLine($"Total Objects Found: {foundObjects.Count}");
                writer.WriteLine("===========================================");
                writer.WriteLine();

                foreach (var so in foundObjects)
                {
                    if (so == null) continue;

                    writer.WriteLine($"=== {so.name} ===");
                    writer.WriteLine($"Type: {so.GetType().Name}");
                    writer.WriteLine($"Asset Path: {AssetDatabase.GetAssetPath(so)}");
                    writer.WriteLine();

                    if (extractedData.ContainsKey(so))
                    {
                        writer.WriteLine("Fields and Properties:");
                        foreach (var kvp in extractedData[so])
                        {
                            string valueStr = FormatValue(kvp.Value);
                            writer.WriteLine($"  {kvp.Key}: {valueStr}");
                        }
                    }

                    writer.WriteLine();
                    writer.WriteLine("-------------------------------------------");
                    writer.WriteLine();
                }

                writer.WriteLine("===========================================");
                writer.WriteLine("END OF EXPORT");
                writer.WriteLine("===========================================");
            }

            Debug.Log($"Successfully exported data to: {filePath}");
            EditorUtility.DisplayDialog("Export Successful", 
                $"Data exported to:\n{filePath}", "OK");
            
            // Open the file location
            EditorUtility.RevealInFinder(filePath);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to export data: {ex.Message}");
            EditorUtility.DisplayDialog("Export Failed", 
                $"Failed to export data:\n{ex.Message}", "OK");
        }
    }

    private string FormatValue(object value)
    {
        if (value == null)
        {
            return "null";
        }

        if (value is string)
        {
            return $"\"{value}\"";
        }

        if (value is System.Collections.IList list)
        {
            if (list.Count == 0)
            {
                return "[]";
            }
            return $"[{list.Count} items]";
        }

        return value.ToString();
    }
    
    // Helper method to get all values of a specific field across all ScriptableObjects
    public static List<T> GetAllValuesOfField<T>(List<ScriptableObject> objects, string fieldName)
    {
        List<T> values = new List<T>();
        
        foreach (var so in objects)
        {
            if (so == null) continue;
            
            FieldInfo field = so.GetType().GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);
            if (field != null && field.FieldType == typeof(T))
            {
                T value = (T)field.GetValue(so);
                values.Add(value);
            }
        }
        
        return values;
    }
}