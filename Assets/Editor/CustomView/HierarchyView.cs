using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

namespace iivimat
{
    /// <summary>
    /// HierarchyView checks project to create tags
    /// and apply 3 scripts on every GameObjects child of Environment
    /// (to make them interactable)
    /// </summary>
    [InitializeOnLoad]
    public static class HierarchyView
    {
        private static GameObject environment;

        /// <summary>
        /// Hierarchy Constructor
        /// Launches method at the opening of a scene
        /// and at hierarchy change
        /// </summary>
        static HierarchyView()
        {
            CheckTags();
            EditorSceneManager.sceneOpened += OnSceneOpened;
            OnSceneOpened(EditorSceneManager.GetActiveScene(), OpenSceneMode.Single);
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
        }

        // @source http://answers.unity.com/answers/917823/view.html
        /// <summary>
        /// Checks if meta, feedback and environment tags exist in tags List
        /// otherwise it creates and add them to the list
        /// </summary>
        static void CheckTags()
        {
            // Open tag manager
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty tagsProp = tagManager.FindProperty("tags");

            // For Unity 5 we need this too
            // SerializedProperty layersProp = tagManager.FindProperty("layers");

            // Adding a Tag
            string tagMeta = "Meta";
            string tagEnvironment = "Environment";
            string tagFeedback = "Feedback";

            // First check if it is not already present
            bool foundMeta = false;
            bool foundEnvironment = false;
            bool foundFeedback = false;
            for (int i = 0; i < tagsProp.arraySize; i++)
            {
                SerializedProperty t = tagsProp.GetArrayElementAtIndex(i);
                if (t.stringValue.Equals(tagMeta)) { foundMeta = true; }
                if (t.stringValue.Equals(tagEnvironment)) { foundEnvironment = true; }
                if (t.stringValue.Equals(tagFeedback)) { foundFeedback = true; }
            }

            // if not found, add it
            if (!foundMeta)
            {
                tagsProp.InsertArrayElementAtIndex(0);
                SerializedProperty n = tagsProp.GetArrayElementAtIndex(0);
                n.stringValue = tagMeta;
            }
            if (!foundEnvironment)
            {
                tagsProp.InsertArrayElementAtIndex(0);
                SerializedProperty n = tagsProp.GetArrayElementAtIndex(0);
                n.stringValue = tagEnvironment;
            }
            if (!foundFeedback)
            {
                tagsProp.InsertArrayElementAtIndex(0);
                SerializedProperty n = tagsProp.GetArrayElementAtIndex(0);
                n.stringValue = tagFeedback;
            }

            /*
            // Setting a Layer (Let's set Layer 10)
            string layerName = "the_name_want_to_give_it";

            // --- Unity 4 ---
            SerializedProperty sp = tagManager.FindProperty("User Layer 10");
            if (sp != null) sp.stringValue = layerName;

            // --- Unity 5 ---
            SerializedProperty sp = layersProp.GetArrayElementAtIndex(10);
            if (sp != null) sp.stringValue = layerName;
            */

            // and to save the changes
            tagManager.ApplyModifiedProperties();
        }

        /// <summary>
        /// Initialises environment and calls OnHierarchyChanged 
        /// at the opening of the scene
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        static void OnSceneOpened(Scene scene, OpenSceneMode mode)
        {
            environment = GameObject.FindWithTag("Environment");
            //OnHierarchyChanged();
        }

        // Delegate called when hierarchy is changed, to add our essential components to environment children
        /// <summary>
        /// Update environment children's components when hierarchy is changed
        /// </summary>
        static void OnHierarchyChanged()
        {
            if (environment == null)
                return;

            // Naive way ?
            List<Transform> children = environment.GetComponentsInChildren<Transform>().Except(new[] { environment.transform }).ToList();

            //Actual GUID of objects in new scene
            List<string> newGuids = new List<string>();

            foreach (Transform child in children)
            {
                GameObject go = child.gameObject;

                if (go.CompareTag("Feedback"))
                    continue;

                // Components check
                if (go.GetComponent<LocalActions>() == null) go.AddComponent<LocalActions>();
                if (go.GetComponent<LocalCoroutineHandler>() == null) go.AddComponent<LocalCoroutineHandler>();
                if (go.GetComponent<UniqueID>() == null) go.AddComponent<UniqueID>();

                newGuids.Add(go.GetComponent<UniqueID>().Guid);
            }

            List<string> missingGuids = InteractionsUtility.GetInteractionsSaver().actualGUIDS.Except(newGuids).ToList();

            InteractionsUtility.GetInteractionsSaver().RemoveGuid(missingGuids);
            InteractionsUtility.GetInteractionsSaver().actualGUIDS = newGuids;
        }
    }
}