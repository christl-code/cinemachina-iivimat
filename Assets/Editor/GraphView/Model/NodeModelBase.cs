using System;
using UnityEngine;

namespace iivimat
{
    [Serializable]
    public class NodeModelBase : ScriptableObject
    {
        public string guid { get; private set; }
        public Vector2 position;
        public string title;

        public string assetName { get { return "Node_" + guid; } private set { } }

        public NodeModelBase()
        {
            guid = Guid.NewGuid().ToString();
        }
    }
}