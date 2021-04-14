using UnityEditor;
using UnityEditor.UIElements;

namespace iivimat
{
    public class NodeViewPositionTransform : NodeViewReaction
    {
        SerializedObject so;

        SerializedProperty propTransformValues;
        SerializedProperty propRelativeTo;
        SerializedProperty propReferenceObject;
        SerializedProperty propRelativeHeading;

        PropertyField fieldTransformValues;
        PropertyField fieldRelativeTo;
        PropertyField fieldReferenceObject;
        PropertyField fieldRelativeHeading;

        public NodeViewPositionTransform(NodeModelReaction nodeModel) : base(nodeModel)
        {
            so = new SerializedObject(nodeModel.Reaction);

            propTransformValues = so.FindProperty("transformValues");
            propRelativeTo = so.FindProperty("relativeTo");
            propReferenceObject = so.FindProperty("referenceObject");

            fieldTransformValues = new PropertyField(propTransformValues,"Vector position");
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
                    extensionContainer.Insert(4,fieldReferenceObject);
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