using System;
using UnityEditor;
using UnityEngine;

namespace iivimat{

    [Serializable]
    public class NodeModelInformation : NodeModelBase
    {
        [SerializeField]
        private Information information;

        public Information Information
        {
            get
            {
                if (information == null)
                    information = InteractionsUtility.FindInformationByGuid(informationID);
                return information;
            }
        }

        public string informationID;

        /// <summary>
        /// Initialises an information and saves it
        /// </summary>
        /// <param name="informationType"></param>
        public void SetupInformation(string informationType)
        {
            // Create the information asset.
            information = ScriptableObject.CreateInstance(informationType) as Information;
            information.name = information.assetName;
            information.Title = "" + information.GetType() + information.GetInstanceID();
            informationID = information.Guid;

            //Save it to the scene.
            InteractionsUtility.GetInteractionsSaver().AddInformation(information);
        }

        /// <summary>
        /// On Destruction information are removed from the scene model
        /// </summary>
        private void OnDestroy()
        {
            if (information != null)
            {
                Undo.RegisterCompleteObjectUndo(InteractionsUtility.GetInteractionsSaver(), "");
                InteractionsUtility.GetInteractionsSaver().RemoveInformation(information);
                Undo.DestroyObjectImmediate(information);
            }
        }
    }
}
