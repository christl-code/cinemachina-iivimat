using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace iivimat
{
    /// <summary>
    /// 
    /// </summary>
    public class NodeViewReactionString : Node
    {
        Color backgroundColor = Color.red;

        SerializedObject so;

        SerializedProperty propTargets;

        protected int maxTargets = 15;
        protected int actualNumberOfTargets;

        public NodeViewReactionString(NodeModelReaction nodeModel)
        {
            // SETUP
            title = nodeModel.Reaction.Title;
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
            extensionContainer.Add(BuildTargets());
            RefreshExpandedState();
        }

        public VisualElement BuildTargets()
        {
            so = new SerializedObject( ((NodeModelReaction)userData).Reaction );

            propTargets = so.FindProperty("targets");

            // Loads and clones our VisualTree (eg. our UXML structure) inside the root.
            VisualElement root = new VisualElement();
            VisualTreeAsset quickToolVisualTree = Resources.Load<VisualTreeAsset>("UI/HelpBox");
            quickToolVisualTree.CloneTree(root);
            VisualElement visualElement = root.Q<VisualElement>("container");
            // Retrieve targets and display them as rows
            VisualElement targetsContainer = new VisualElement();
            visualElement.Add(targetsContainer);
            for (int i = 0; i < propTargets.arraySize; i++)
            {
                RetrieveRow(targetsContainer, i);
            }

            // Add row button
            Button addRow = new Button();
            visualElement.Add(addRow);
            addRow.text = "Ajouter cible";
            addRow.clicked += () =>
            {
                if(actualNumberOfTargets < maxTargets){
                    CreateEmptyRow(targetsContainer);
                }            
            };

            return visualElement;
        }

        private void RetrieveRow(VisualElement container, int index)
        {
            VisualElement row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;

            TextField textField = new TextField() { value = "Veuillez entrer votre texte ici..." };
            textField.BindProperty(propTargets.GetArrayElementAtIndex(index));
            row.Add(textField);
            // objField.RegisterCallback<MouseDownEvent>(evt => DisplayParameters(objField.value));

            Button btn = new Button();
            row.Add(btn);
            btn.text = "Supp.";
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
        private void CreateEmptyRow(VisualElement container)
        {
            VisualElement row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;

            TextField textField = new TextField() { value = "Veuillez entrer votre texte ici..." };
            so.Update();
            propTargets.InsertArrayElementAtIndex(propTargets.arraySize);
            so.ApplyModifiedProperties();
            textField.BindProperty(propTargets.GetArrayElementAtIndex(propTargets.arraySize - 1));
            row.Add(textField);

            Button btn = new Button();
            row.Add(btn);
            btn.text = "Supp.";
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