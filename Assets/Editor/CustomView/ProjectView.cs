using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace iivimat
{
    [InitializeOnLoad]
    public class ProjectView
    {
        /// <summary>
        /// Adds a menu item to create a new IIVimat scene (Project)
        /// </summary>
        [MenuItem("Assets/Create/Scene IIViMaT", false, 201)]
        static void CreateScene()
        {
            // Duplicates our template scene
            string dstScenePath = AssetDatabase.GenerateUniqueAssetPath(AssetDatabase.GetAssetPath(Selection.activeObject) + "/New " + GraphViewWindow.pluginName + " Scene.unity");

            if (AssetDatabase.CopyAsset("Assets/" + GraphViewWindow.pluginName + "/Scenes/Templates/Template Scene.unity", dstScenePath))
            {
                Scene scene = EditorSceneManager.OpenScene(dstScenePath);
                GameObject.FindWithTag("Meta").GetComponent<UniqueID>().GenerateNewGUID();
                EditorSceneManager.SaveScene(scene);
                SceneManager.SetActiveScene(scene);
            }
            else
            {
                Debug.LogError("Could not create " + GraphViewWindow.pluginName + " scene.");
            }
        }

        /// <summary>
        /// Adds a menu item to create a new IIVimat Oculus scene (Project)
        /// </summary>
        [MenuItem("Assets/Create/Scene Oculus IIViMaT", false, 201)]
        static void CreateSceneOculus()
        {
            // Duplicates our template scene
            string dstScenePath = AssetDatabase.GenerateUniqueAssetPath(AssetDatabase.GetAssetPath(Selection.activeObject) + "/New " + GraphViewWindow.pluginName + " Oculus Scene.unity");

            if (AssetDatabase.CopyAsset("Assets/" + GraphViewWindow.pluginName + "/Scenes/Templates/Template Oculus Scene.unity", dstScenePath))
            {
                Scene scene = EditorSceneManager.OpenScene(dstScenePath);
                GameObject.FindWithTag("Meta").GetComponent<UniqueID>().GenerateNewGUID();
                EditorSceneManager.SaveScene(scene);
                SceneManager.SetActiveScene(scene);
            }
            else
            {
                Debug.LogError("Could not create " + GraphViewWindow.pluginName + " scene.");
            }
        }

        /// <summary>
        /// Add a menu item to create a new sphere 360 (Hierarchy)
        /// </summary>
        [MenuItem("GameObject/Video/Sphere 360", false, 10)]        
        static void CreateSphere360Prefab()
        {
            string path = "Assets/Resources/Prefabs/360Video/360VideoSphere.prefab";
            UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)); 
            UnityEngine.Object newObject = PrefabUtility.InstantiatePrefab(prefab, GameObject.FindWithTag("Environment").gameObject.transform);   
            Selection.activeObject = newObject; 
        }
    }
}