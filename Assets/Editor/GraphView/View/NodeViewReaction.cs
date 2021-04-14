using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace iivimat
{
    public class NodeViewReaction : Node
    {

        Color backgroundColor = Color.red;
        SerializedObject so;
        protected Type targetType;

        protected SerializedProperty propTargets;
        protected SerializedProperty propPlayOnce;

        protected PropertyField fieldPlayOnce;

        protected int maxTargets = 15;
        protected int actualNumberOfTargets;


        public NodeViewReaction(NodeModelReaction nodeModel)
        {
            // SETUP
            title = nodeModel.Reaction.GetType().Name;
            capabilities |= Capabilities.Movable;
            viewDataKey = nodeModel.guid;
            name = nodeModel.guid;
            userData = nodeModel;
            SetPosition(new Rect(nodeModel.position, Vector2.zero));

            actualNumberOfTargets = 0;
            mainContainer.style.backgroundColor = backgroundColor;

            // Ports
            var inputPort = Port.Create<Edge>(Orientation.Horizontal, Direction.Input, Port.Capacity.Multi, typeof(NodeModelAction));
            inputPort.portName = "Action(s)";
            inputPort.name = "input";

            inputContainer.Add(inputPort);

            // Parameters
            so = new SerializedObject( ((NodeModelReaction)userData).Reaction );

            propPlayOnce = so.FindProperty("playOnce");
            fieldPlayOnce = new PropertyField(propPlayOnce, "Play once");

            fieldPlayOnce.Bind(so);

            extensionContainer.Add(fieldPlayOnce);
            extensionContainer.Add(BuildTargets());

            RefreshExpandedState();
        }

        public virtual VisualElement BuildTargets()
        {
            targetType = ((Reaction)so.targetObject).GetType().BaseType.GetGenericArguments()[0];

            propTargets = so.FindProperty("targets");
            
            // Loads and clones our VisualTree (eg. our UXML structure) inside the root.
            VisualElement root = new VisualElement();
            VisualTreeAsset quickToolVisualTree = Resources.Load<VisualTreeAsset>("UI/HelpBox");
            quickToolVisualTree.CloneTree(root);
            VisualElement visualElement = root.Q<VisualElement>("container");

            // Retrieve targets and display them as rows
            VisualElement targetsContainer = new VisualElement();
            targetsContainer.style.backgroundColor = mainContainer.style.backgroundColor;
            visualElement.Add(targetsContainer);
            for (int i = 0; i < propTargets.arraySize; i++)
            {
                RetrieveRow(targetsContainer, i);
            }

            // Add row button
            Button addRow = new Button();
            visualElement.Add(addRow);
            addRow.text = "Add target";
            addRow.clicked += () =>
            {
                if(actualNumberOfTargets < maxTargets){
                    CreateEmptyRow(targetsContainer);
                }
            };

            return visualElement;
        }

        protected void RetrieveRow(VisualElement container, int index)
        {
            VisualElement row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;

            ObjectField objField = new ObjectField() { allowSceneObjects = true, objectType = targetType };
            objField.BindProperty(propTargets.GetArrayElementAtIndex(index));
            row.Add(objField);
            // objField.RegisterCallback<MouseDownEvent>(evt => DisplayParameters(objField.value));

            Button btn = new Button();
            row.Add(btn);
            btn.text = "Del.";
            btn.clicked += (() =>
            {
                so.Update();
                propTargets.DeleteArrayElementAtIndex(index);
                so.ApplyModifiedProperties();
                ((Reaction)so.targetObject).Clean();
                container.Remove(row);
            });

            container.Add(row);
        }

        // TODO: Empty row created with value of last row ???
        protected void CreateEmptyRow(VisualElement container)
        {
            VisualElement row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;

            ObjectField objField = new ObjectField() { allowSceneObjects = true, objectType = targetType };
            so.Update();
            propTargets.InsertArrayElementAtIndex(propTargets.arraySize);
            so.ApplyModifiedProperties();
            objField.BindProperty(propTargets.GetArrayElementAtIndex(propTargets.arraySize - 1));
            row.Add(objField);

            Button btn = new Button();
            row.Add(btn);
            btn.text = "Del.";
            btn.clicked += (() =>
            {
                actualNumberOfTargets -= 1;
                so.Update();
                propTargets.DeleteArrayElementAtIndex(container.IndexOf(row));
                so.ApplyModifiedProperties();
                ((Reaction)so.targetObject).Clean();
                container.Remove(row);
            });

            container.Add(row);
            actualNumberOfTargets += 1;
        }
    }
}