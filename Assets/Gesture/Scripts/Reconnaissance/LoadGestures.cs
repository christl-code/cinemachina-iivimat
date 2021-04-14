using UnityEngine;
using System.IO;
using System.Collections.Generic;

/// <summary>
/// Launch at the beginning of the Edit Mode to load the gestures
/// </summary>
namespace iivimat
{

    [ExecuteInEditMode]
    public class LoadGestures : MonoBehaviour
    {
        // Path where the JSon gestures are
        private string pathLoadGestures = "Assets/Gesture/Gestures/";


        // Loaded gestures
        private List<Gesture> gestures;

        public void Awake()
        {
            LoadAllGestures();
        }

        /// <summary>
        ///  Load every JSon gestures in the path
        ///  Give those gestures to the gestureDetector
        /// </summary>
        public void LoadAllGestures()
        {
            StreamReader reader = null;

            List<Gesture> loadGestures = new List<Gesture>();


            // For eeach file (JSon gesture)
            foreach (string file in Directory.GetFiles(pathLoadGestures))
            {
                reader = new StreamReader(file);

                try
                {
                    string text = reader.ReadToEnd();
                    Gesture g = JsonUtility.FromJson<Gesture>(text);
                    loadGestures.Add(g);
                }
                catch (System.Exception)
                {
                    // If it's not a Json gesture file
                }
            }

            reader.Close();
            GetComponent<GestureDetectorManager>().gestures = new List<Gesture>(loadGestures);
        }

    }
}
