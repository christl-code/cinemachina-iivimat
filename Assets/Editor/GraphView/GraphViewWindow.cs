using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace iivimat
{
    /// <summary>
    /// 
    /// </summary>
    public class GraphViewWindow : EditorWindow, ISearchWindowProvider
    {
        public static readonly string pluginName = "IIViMaT";
        public static readonly string nameSpace = "iivimat";
        public static readonly string inspecPrefix = "NodeView";

        private static GraphViewWindow window;
        public static GraphViewWindow Window
        {
            get
            {
                if (window == null)
                    window = GetWindow<GraphViewWindow>(false, pluginName, true);
                return window;
            }
            private set { window = value; }
        }

        private GraphModel graphModel;
        public GraphModel GraphModel
        {
            get
            {

                // Load the asset.

                //string currentgraphModelPath = "Assets/Resources/IIVIMATAssets/Models" + pluginName + "/Models/" + currentSceneGUID;
                string directoryPath = "Assets/Resources/IIVIMATAssets/Models";
                string currentgraphModelPath = directoryPath + "/" +  currentSceneGUID;

                try
                {
                    var folder = Directory.CreateDirectory(directoryPath);
                }
                catch (UnityException)
                {
                }

                UnityEngine.Object[] assets;
                assets = AssetDatabase.LoadAllAssetsAtPath(currentgraphModelPath + ".asset");

                if (assets == null || assets.Length == 0)
                {
                    ClearAll();

                    // Create and save the new graphModel
                    graphModel = ScriptableObject.CreateInstance<GraphModel>();
                    AssetDatabase.CreateAsset(graphModel, currentgraphModelPath + ".asset");
                    AssetDatabase.SaveAssets();
                }
                else
                {
                    // Find the asset.
                    foreach (var asset in assets)
                    {
                        if (asset is GraphModel)
                        {
                            graphModel = asset as GraphModel;
                        }
                    }
                }
                return graphModel;
            }
        }

        private CustomGraphView customGraphView;
        public CustomGraphView CustomGraphView
        {
            get
            {
                if (customGraphView == null)
                {
                    customGraphView = new CustomGraphView(GraphModel);

                    customGraphView.RegisterCallback<AttachToPanelEvent>(evt => customGraphView.nodeCreationRequest += OnRequestNodeCreation);
                    customGraphView.RegisterCallback<DetachFromPanelEvent>(evt => customGraphView.nodeCreationRequest -= OnRequestNodeCreation);

                    // (this).GetRootVisualContainer().Add(customGraphView);
                    rootVisualElement.Add(customGraphView);
                }
                return customGraphView;
            }
        }

        private string currentSceneGUID;
        private static GameObject environment;

        internal struct NodeEntry
        {
            public Type type;
            public string title;
            public object data;
        }

        [MenuItem("Tools/IIViMaT _%#I")]
        public static void ShowWindow() {
            Window = GetWindow<GraphViewWindow>(false, pluginName, true);
        }



        [MenuItem("Tools/Clear _%#U")]
        public static void ClearAllButton() {
            if(EditorUtility.DisplayDialog("Clear ", "Do you really want to remove every IIViMaT objects ?\nThis action is irreversible.", "Yes", "No")){

                    string directoryPath = "Assets/Resources/IIVIMATAssets/Models";
                    string path = directoryPath + "/" + GameObject.FindWithTag("Meta").GetComponent<UniqueID>().Guid;

                    try
                    {
                        var folder = Directory.CreateDirectory(directoryPath);
                    }
                    catch (UnityException)
                    {
                    }

                    ClearAll();

                    GraphModel graphModel = ScriptableObject.CreateInstance<GraphModel>();
                    AssetDatabase.CreateAsset(graphModel, path + ".asset");
                    AssetDatabase.SaveAssets();
                }
        }

        /*
        [MenuItem("Tools/Clean references _%#O")]
        public static void CleanReferencesButton() {
            string directoryPath = "Assets/Resources/IIVIMATAssets/Models";
            string path = directoryPath + "/" + GameObject.FindWithTag("Meta").GetComponent<UniqueID>().Guid;
            
            UnityEngine.Object[] assets;
            assets = AssetDatabase.LoadAllAssetsAtPath(path + ".asset");
            foreach (var asset in assets)
                    {
                        if (asset is GraphModel)
                        {
                            GraphModel graphModel = asset as GraphModel;
                            graphModel.Clean();
                        }
                    }
        }*/


        // OnEnable and OnDisable are the perfect places to (un)subscribe to delegates
        private void OnEnable()
        {
            EditorSceneManager.sceneOpened += OnSceneOpened;
            Undo.undoRedoPerformed += Reload;

            Reload();
        }

        private void OnDisable()
        {
            EditorSceneManager.sceneOpened -= OnSceneOpened;
            Undo.undoRedoPerformed -= Reload;
            graphModel.Clean();
        }

        // Delegate called when a scene is opened, to update our graph model & view
        protected void OnSceneOpened(Scene scene, OpenSceneMode mode) => Reload();

        protected void OnRequestNodeCreation(NodeCreationContext context) => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), this);

        /// <summary>
        /// Create the research tree when you right click on the windows
        /// </summary>
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>();

            tree.Add(new SearchTreeGroupEntry(new GUIContent("Create Node"), 0));

            tree.Add(new SearchTreeGroupEntry(new GUIContent("Object Node"), 1));
            if (environment != null)
            {
                List<Transform> children = environment.GetComponentsInChildren<Transform>().Except(new[] { environment.transform }).ToList();
                foreach (Transform child in children)
                {
                    GameObject go = child.gameObject;
                    if (!go.tag.Equals("Feedback"))
                    {
                        NodeEntry nodeEntry = new NodeEntry
                        {
                            type = typeof(NodeModelObject),
                            title = go.name,
                            data = go
                        };
                        tree.Add(new SearchTreeEntry(new GUIContent(go.name)) { level = 2, userData = nodeEntry });
                    }
                }
            }

            string defaultPath = "Assets/" + pluginName + "/Scripts/";

            // Create recursivly the tree to select an action or a reaction.
            tree.Add(new SearchTreeGroupEntry(new GUIContent("Action Node"), 1));
            createSearchTree(typeof(NodeModelAction), defaultPath + "Actions" + "/", 2, tree);
            tree.Add(new SearchTreeGroupEntry(new GUIContent("Reaction Node"), 1));
            createSearchTree(typeof(NodeModelReaction), defaultPath + "Reactions" + "/", 2, tree);
            tree.Add(new SearchTreeGroupEntry(new GUIContent("Information Node"), 1));
            createSearchTree(typeof(NodeModelInformation), defaultPath + "Informations" + "/", 2, tree);
            return tree;
        }

        /// <summary>
        /// Create the research tree when you right click on the windows
        /// </summary>
        /// <param><c>t</c> type for the nodes</param>
        /// <param><c>path</c> path to create the hierarchy</param>
        /// <param><c>level</c> deep level in the tree</param>
        /// <param><c>tree</c> general tree</param>
        public void createSearchTree(Type t, string path, int level, List<SearchTreeEntry> tree){

            string[] DirectoriesPaths = Directory.GetDirectories(path);
            foreach(string s in DirectoriesPaths){
                string name = s.Replace(@"\","/");
                name = name.Substring(name.LastIndexOf("/")+1);
                tree.Add(new SearchTreeGroupEntry(new GUIContent(name), level));
                createSearchTree(t, s, level + 1, tree);
            }
            
            string[] FilesPaths = Directory.GetFiles(path, "*.cs", SearchOption.TopDirectoryOnly);
            foreach (string scriptPath in FilesPaths)
            {
                MonoScript script = AssetDatabase.LoadAssetAtPath(scriptPath, typeof(MonoScript)) as MonoScript;
                string name = scriptPath.Substring(scriptPath.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                name = name.Substring(0, name.LastIndexOf("."));
                NodeEntry nodeEntry = new NodeEntry
                {
                    type = t,
                    title = name,
                    data = script.GetClass()
                };
                tree.Add(new SearchTreeEntry(new GUIContent(name)) { level = level, userData = nodeEntry });

            }
        }

        public bool OnSelectEntry(SearchTreeEntry entry, SearchWindowContext context)
        {
            if (entry is SearchTreeGroupEntry)
                return false;

            bool success = false;

            NodeEntry nodeEntry = (NodeEntry)entry.userData;

            var windowRoot = Window.rootVisualElement;
            var windowMousePosition = windowRoot.ChangeCoordinatesTo(windowRoot.parent, context.screenMousePosition - Window.position.position);
            var graphMousePosition = CustomGraphView.contentViewContainer.WorldToLocal(windowMousePosition);

            if (nodeEntry.type == typeof(NodeModelObject))
            {
                // Create node asset
                NodeModelObject nodeModel = ScriptableObject.CreateInstance(nodeEntry.type) as NodeModelObject;
                Undo.RegisterCreatedObjectUndo(nodeModel, "node creation");

                // Setup it
                nodeModel.name = nodeModel.assetName;
                nodeModel.SetupGO( (GameObject)nodeEntry.data, GraphModel );
                nodeModel.title = nodeModel.Go.name;
                nodeModel.position = graphMousePosition;

                GraphModel.Add(nodeModel);

                // Create and setup node UI
                CreateNode(nodeModel);

                success = true;
            }
            else if (nodeEntry.type == typeof(NodeModelAction))
            {
                // Create node asset
                NodeModelAction nodeModel = ScriptableObject.CreateInstance(nodeEntry.type) as NodeModelAction;
                Undo.RegisterCreatedObjectUndo(nodeModel, "node creation");

                // Setup it
                nodeModel.name = nodeModel.assetName;
                nodeModel.SetupAction( nodeEntry.data.ToString() );
                nodeModel.title = nodeModel.Action.Title;
                nodeModel.position = graphMousePosition;

                GraphModel.Add(nodeModel);

                // Create and setup node UI
                CreateNode(nodeModel);

                success = true;
            }
            else if (nodeEntry.type == typeof(NodeModelReaction))
            {
                // Create node asset
                NodeModelReaction nodeModel = ScriptableObject.CreateInstance(nodeEntry.type) as NodeModelReaction;
                Undo.RegisterCreatedObjectUndo(nodeModel, "node creation");

                // Setup it
                nodeModel.name = nodeModel.assetName;
                nodeModel.SetupReaction( nodeEntry.data.ToString() );
                nodeModel.title = nodeModel.Reaction.Title;
                nodeModel.position = graphMousePosition;

                GraphModel.Add(nodeModel);

                // Create and setup node UI
                CreateNode(nodeModel);

                success = true;
            }
            else if(nodeEntry.type == typeof(NodeModelInformation)){
                // Create node asset
                NodeModelInformation nodeModel = ScriptableObject.CreateInstance(nodeEntry.type) as NodeModelInformation;
                Undo.RegisterCreatedObjectUndo(nodeModel, "node information");

                // Setup it
                nodeModel.name = nodeModel.assetName;
                nodeModel.SetupInformation( nodeEntry.data.ToString() );
                nodeModel.title = nodeModel.Information.Title;
                nodeModel.position = graphMousePosition;

                GraphModel.Add(nodeModel);

                // Create and setup node UI
                CreateNode(nodeModel);

                success = true;
            }
            else
            {
                Debug.Log("Error while creating node");
            }

            return success;
        }

        private Node CreateNode(NodeModelBase nodeModel)
        {
            Node nodeView = null;

            if (nodeModel is NodeModelObject nodeModelObject)
            {
                if(nodeModelObject.Go != null){
                    nodeView = new NodeViewObject(nodeModelObject);
                }
            }
            else if (nodeModel is NodeModelAction nodeModelAction)
            {
                try
                {
                    // Try to create a custom action node with the given type
                    string classToCreate = nameSpace + "." + inspecPrefix + nodeModelAction.Action.GetType().Name;
                    nodeView = (Node)Activator.CreateInstance(Type.GetType(classToCreate), new[] { nodeModelAction });
                }
                catch
                {
                    // If there is no one, then create a basic node
                    nodeView = new NodeViewAction(nodeModelAction);
                }
            }
            else if (nodeModel is NodeModelReaction nodeModelReaction)
            {
                try
                {
                    // Try to create a custom reaction node with the given type
                    string classToCreate = nameSpace + "." + inspecPrefix + nodeModelReaction.Reaction.GetType().Name;
                    nodeView = (Node)Activator.CreateInstance(Type.GetType(classToCreate), new[] { nodeModelReaction });
                }
                catch
                {
                    // If there is no one, then create a basic node
                    nodeView = new NodeViewReaction(nodeModelReaction);
                }
            }
            else if(nodeModel is NodeModelInformation nodeModelInformation){
                try
                {
                    // Try to create a custom reaction node with the given type
                    string classToCreate = nameSpace + "." + inspecPrefix + nodeModelInformation.Information.GetType().Name;
                    nodeView = (Node)Activator.CreateInstance(Type.GetType(classToCreate), new[] { nodeModelInformation });
                }
                catch
                {
                    // If there is no one, then create a basic node
                    nodeView = new NodeViewInformation(nodeModelInformation);
                }
            }
            else
            {
                Debug.LogError("Node Model " + nodeModel + " not recognized");
            }

            if (nodeView != null)
                CustomGraphView.AddElement(nodeView);

            return nodeView;
        }

        private void CreateEdge(EdgeModel edgeModel)
        {
            if(edgeModel != null){
                try{
                    var inputGuid = edgeModel.input.guid;
                    var outputGuid = edgeModel.output.guid;

                    var inputNode = CustomGraphView.GetNodeByGuid(inputGuid);
                    var outputNode = CustomGraphView.GetNodeByGuid(outputGuid);

                    var inputPort = inputNode.inputContainer.Q<Port>(name: "input");
                    var outputPort = outputNode.outputContainer.Q<Port>(name: "output");

                    var edge = inputPort.ConnectTo(outputPort);
                    edge.viewDataKey = edgeModel.guid;
                    edge.userData = edgeModel;

                    CustomGraphView.AddElement(edge);
                }catch(NullReferenceException){
                    
                }
            }
            
            
        }

        public void Reload()
        {
            GameObject meta = GameObject.FindWithTag("Meta");
            environment = GameObject.FindWithTag("Environment");

            if (meta == null || environment == null)
            {
                Debug.LogWarning("Current scene is not an " + pluginName + " scene");
                return;
            }

            currentSceneGUID = meta.GetComponent<UniqueID>().Guid;

            // Clean everything
            //GraphModel.Clean();
            CustomGraphView.Clean();

            // And recreate them based on GraphModel lists
            foreach (var nodeModel in GraphModel.nodes)
                CreateNode(nodeModel);
            foreach (var edgeModel in GraphModel.edges)
                CreateEdge(edgeModel);
        }

        /// <summary>
        /// Clear all IIVimat actions / reactions / objets related to IIVimat in the GameObjects in the scene
        /// </summary>
        public static void ClearAll(){
            // Clear all actions in the objects in meta
            InteractionsUtility.GetInteractionsSaver().Clear();
            InteractionsUtility.GetGlobalActions().Clear();
            InteractionsUtility.GetEventManager().Clear();
            InteractionsUtility.GetEventEndHandler().Clear();


            // Clear all local actions
            LocalActions[] components = Resources.FindObjectsOfTypeAll<LocalActions>();
            foreach (LocalActions la in components){
                la.Clear();
            }

            // Clear all LocalCoroutines
            LocalCoroutineHandler[] components_coroutines = Resources.FindObjectsOfTypeAll<LocalCoroutineHandler>();
            foreach(LocalCoroutineHandler lc in components_coroutines){
                lc.Clear();
            }


            // Clear all proximity spheres
            GameObject[] spheres = GameObject.FindGameObjectsWithTag("Feedback");
            foreach(GameObject go in spheres){
                DestroyImmediate(go);
            }

        }
    }
}