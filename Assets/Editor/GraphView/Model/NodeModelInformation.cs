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

        private void OnDestroy()
        {
            if (information != null)
            {
                Undo.RegisterCompleteObjectUndo(InteractionsUtility.GetInteractionsSaver(), "");
                InteractionsUtility.GetInteractionsSaver().RemoveInformation(information);
                // DestroyImmediate(action, true);
                Undo.DestroyObjectImmediate(information);
            }
        }
    }
}
