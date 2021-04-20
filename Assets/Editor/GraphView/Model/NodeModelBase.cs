using System;
using UnityEngine;

namespace iivimat
{
    /// <summary>
    /// Mother Class of node model
    /// </summary>
    [Serializable]
    public class NodeModelBase : ScriptableObject
    {
        public string guid { get; private set; }
        public Vector2 position;
        public string title;

        public string assetName { get { return "Node_" + guid; } private set { } }
        /// <summary>
        /// NodelModeBase Constructor
        /// Allocates GUID to NodeModelBase objects
        /// </summary>
        public NodeModelBase()
        {
            guid = Guid.NewGuid().ToString();
        }
    }
}