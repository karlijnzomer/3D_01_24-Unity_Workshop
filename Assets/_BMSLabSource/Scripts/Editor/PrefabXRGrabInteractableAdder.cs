using UnityEditor;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PrefabXRGrabInteractableAdder : EditorWindow
{
    private SerializedObject _serializedObject;
    private SerializedProperty _prefabListSerialized;

    [MenuItem("Tools/Prefab XR Grab Interactable Adder")]
    private static void ShowWindow()
    {
        var window = GetWindow<PrefabXRGrabInteractableAdder>();
        window.titleContent = new GUIContent("XR Grab Interactable Adder");
        window.Show();
    }

    private void OnEnable()
    {
        _serializedObject = new SerializedObject(this);
        _prefabListSerialized = _serializedObject.FindProperty("_prefabList");
    }

    [SerializeField]
    private GameObject[] _prefabList;

    private void OnGUI()
    {
        _serializedObject.Update();
        EditorGUILayout.PropertyField(_prefabListSerialized, new GUIContent("Prefabs"), true);
        _serializedObject.ApplyModifiedProperties();

        if (GUILayout.Button("Add XRGrabInteractable to Prefabs"))
        {
            AddXRGrabInteractableToPrefabs();
        }

        if (GUILayout.Button("Clear Prefab List"))
        {
            ClearPrefabList();
        }
    }

    private void AddXRGrabInteractableToPrefabs()
    {
        foreach (var prefab in _prefabList)
        {
            if (prefab == null) continue;

            GameObject prefabInstance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            if (prefabInstance.GetComponent<XRGrabInteractable>() == null)
            {
                prefabInstance.AddComponent<XRGrabInteractable>();
                PrefabUtility.ApplyPrefabInstance(prefabInstance, InteractionMode.UserAction);
            }
            DestroyImmediate(prefabInstance);
        }

        AssetDatabase.SaveAssets();
    }

    private void ClearPrefabList()
    {
        _prefabList = new GameObject[0];
        _serializedObject.Update();
    }
}
