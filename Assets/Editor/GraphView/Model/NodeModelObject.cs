using System;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace iivimat
{
    [Serializable]
    public class NodeModelObject : NodeModelBase
    {
        [SerializeField]
        UniqueID uniqueID;
        public string objectID;
        GraphModel graphModel;

        private GameObject go;
        public GameObject Go
        {
            get
            {
                if (go == null)
                {
                    go = InteractionsUtility.FindGameObjectByGuid(objectID);
                    uniqueID = go.GetComponent<UniqueID>();
                }
                return go;
            }
        }

        public delegate void Repaint();
        public event Repaint Renamed;

        public delegate void Destruction();
        public event Destruction Destroyed;

        public void SetupGO(GameObject go, GraphModel graphModel)
        {
            this.graphModel = graphModel;

            uniqueID = go.GetComponent<UniqueID>();
            uniqueID.Renamed += OnRename;
            objectID = uniqueID.Guid;
            //EditorSceneManager.sceneLoaded += OnSceneLoaded;
            //EditorApplication.hierarchyChanged += OnHierarchyChanged;
        }

        private void OnRename()
        {
            this.title = Go.name;
            Renamed?.Invoke();
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode sceneMode) => CheckIfDestroyed();

        void OnHierarchyChanged() => CheckIfDestroyed();

        private void CheckIfDestroyed()
        {
            if (Go == null)
            {
                graphModel.Remove(this);
                Destroyed?.Invoke();
            }
        }

        private void OnDestroy()
        {
            uniqueID.Renamed -= OnRename;
            //EditorSceneManager.sceneLoaded -= OnSceneLoaded;
            //EditorApplication.hierarchyChanged -= OnHierarchyChanged;

            if (Go != null)
            {
                Debug.Log("OnDestroy called from " + title);
                LocalActions localActions = Go.GetComponent<LocalActions>();
                for (int i = 0; i < localActions.Actions.Count; i++)
                {
                    localActions.Actions[i].RemoveGameObject(Go);
                }
            }
        }
    }
}