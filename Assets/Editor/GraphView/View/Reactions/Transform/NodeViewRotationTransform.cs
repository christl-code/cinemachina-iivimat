using UnityEditor;
using UnityEditor.UIElements;

namespace iivimat
{
    public class NodeViewRotationTransform : NodeViewReaction
    {
        SerializedObject so;

        SerializedProperty propTransformValues;
        SerializedProperty propRelativeTo;
        SerializedProperty propReferenceObject;
        SerializedProperty propRelativeHeading;
        SerializedProperty propAngle;

        PropertyField fieldTransformValues;
        PropertyField fieldRelativeTo;
        PropertyField fieldReferenceObject;
        PropertyField fieldRelativeHeading;
        PropertyField fieldAngle;

        public NodeViewRotationTransform(NodeModelReaction nodeModel) : base(nodeModel)
        {
            so = new SerializedObject(nodeModel.Reaction);

            propTransformValues = so.FindProperty("transformValues");
            propRelativeTo = so.FindProperty("relativeTo");
            propReferenceObject = so.FindProperty("referenceObject");
            propAngle = so.FindProperty("angle");

            fieldTransformValues = new PropertyField(propTransformValues, "Rotate around axis");
            fieldTransformValues.Bind(so);

            fieldRelativeTo = new PropertyField(propRelativeTo,"Reference point");
            fieldRelativeTo.Bind(so);

            fieldReferenceObject = new PropertyField(propReferenceObject,"Reference object");
            fieldReferenceObject.Bind(so);

            fieldAngle = new PropertyField(propAngle);
            fieldAngle.Bind(so);

            extensionContainer.Add(fieldTransformValues);
            extensionContainer.Add(fieldAngle);
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