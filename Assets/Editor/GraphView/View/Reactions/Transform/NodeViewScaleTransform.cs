using UnityEditor;
using UnityEditor.UIElements;

namespace iivimat
{
    public class NodeViewScaleTransform : NodeViewReaction
    {
        SerializedObject so;

        SerializedProperty propTransformValues;
        SerializedProperty propRelativeTo;
        SerializedProperty propReferenceObject;

        PropertyField fieldTransformValues;
        PropertyField fieldRelativeTo;
        PropertyField fieldReferenceObject;

        public NodeViewScaleTransform(NodeModelReaction nodeModel) : base(nodeModel)
        {
            so = new SerializedObject(nodeModel.Reaction);

            propTransformValues = so.FindProperty("transformValues");
            propRelativeTo = so.FindProperty("relativeTo");
            propReferenceObject = so.FindProperty("referenceObject");

            fieldTransformValues = new PropertyField(propTransformValues, "Scale multiplier factor");
            fieldTransformValues.Bind(so);

            fieldRelativeTo = new PropertyField(propRelativeTo);
            fieldRelativeTo.Bind(so);

            fieldReferenceObject = new PropertyField(propReferenceObject);
            fieldReferenceObject.Bind(so);

            extensionContainer.Add(fieldTransformValues);
            extensionContainer.Add(fieldRelativeTo);
            // Display the object menu is the field is Object
            if(propRelativeTo.enumValueIndex == 2){
                extensionContainer.Add(fieldReferenceObject);
            }

            fieldRelativeTo.RegisterValueChangeCallback(evt =>
            {
                if (evt.changedProperty.enumValueIndex == 2)
                {
                    extensionContainer.Add(fieldReferenceObject);
                }
                else
                {
                    if(extensionContainer.Contains(fieldReferenceObject)){
                        extensionContainer.Remove(fieldReferenceObject);
                    }
                }
            });
        }
    }
}