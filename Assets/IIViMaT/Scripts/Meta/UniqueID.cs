using UnityEditor;
using UnityEngine;

namespace iivimat
{
    [ExecuteAlways]
    public class UniqueID : MonoBehaviour
    {
        [SerializeField]
        private string guid;
        public string Guid
        {
            get
            {
                if (string.IsNullOrEmpty(guid))
                    GenerateNewGUID();
                return guid;
            }
        }

        public void GenerateNewGUID()
        {
            guid = System.Guid.NewGuid().ToString();
        }

        #if UNITY_EDITOR
        // catch duplication of this GameObject
        // @source http://answers.unity.com/comments/1171089/view.html
        [SerializeField]
        int instanceID = 0;

        public delegate void Rename();
        public event Rename Renamed;

        public delegate void destruction();
        public event destruction Destroyed;

        private void Awake()
        {
            if (Application.isPlaying)
                return;

            if (instanceID == 0)
            {
                instanceID = GetInstanceID();
                return;
            }

            if (instanceID != GetInstanceID() && GetInstanceID() < 0)
            {
                //Debug.LogWarning("Detected Duplicate!");
                instanceID = GetInstanceID();
                GenerateNewGUID();
            }
        }

        private void OnEnable() => EditorApplication.hierarchyChanged += UpdateName;
        private void OnDisable() => EditorApplication.hierarchyChanged -= UpdateName;

        public void UpdateName()
        {
            Renamed?.Invoke();
        }

        private void OnDestroy()
        {
            Destroyed?.Invoke();
        }
        #endif
    }
}