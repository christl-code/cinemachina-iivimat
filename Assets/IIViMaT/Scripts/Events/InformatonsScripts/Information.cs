using System;
using System.Collections.Generic;
using UnityEngine;

namespace iivimat{
    [Serializable]
    public class Information : ScriptableObject
    {
        
        [SerializeField]
        private string guid;
        public string Guid { get { return guid; } private set { guid = value; } }
        [SerializeField]
        private string title;
        public string Title { get { return title; } set { title = value; } }
        public string assetName { get { return "Information_" + guid; } private set { } }

        public Information()
        {
            if (String.IsNullOrEmpty(Guid)) Guid = System.Guid.NewGuid().ToString();
        }
    }
}
