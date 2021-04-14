using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace iivimat
{
    /// <summary>
    /// Saves all actions and reactions in the IIVIMAT graph
    /// </summary>
    public class InteractionsSaver : MonoBehaviour
    {
        public List<Action> actions = new List<Action>();
        public List<Reaction> reactions = new List<Reaction>();
        public List<Information> informations = new List<Information>();

        public List<string> actualGUIDS = new List<string>();

        private void OnEnable()
        {
            Clean();
        }

        
        public void AddAction(Action action)
        {
            if (!actions.Contains(action))
            {
                Clean();
                actions.Add(action);
            }
        }

        public void AddReaction(Reaction reaction)
        {
            if (!reactions.Contains(reaction))
            {
                Clean();
                reactions.Add(reaction);
            }
        }

        public void AddInformation(Information information){
            if(!informations.Contains(information)){
                Clean();
                informations.Add(information);
            }
        }

        public void RemoveAction(Action action)
        {
            if (actions.Contains(action))
            {
                actions.Remove(action);
                Clean();
            }
        }

        public void RemoveReaction(Reaction reaction)
        {
            if (reactions.Contains(reaction))
            {
                reactions.Remove(reaction);
                Clean();
            }
        }


        public void RemoveInformation(Information information){
            if(informations.Contains(information)){
                informations.Remove(information);
                Clean();
            }
        }

        public void RemoveGuid(List<string> guids){
            foreach(string guid in guids){
                foreach(Action a in actions){
                    a.ObjectIDs.Remove(guid);
                    a.Clean();
                }
            }
        }

        public void Clean()
        {
            for (int i = 0; i < actions.Count; i++)
            {
                if (actions[i] == null)
                    actions.RemoveAt(i);
            }
            for (int i = 0; i < reactions.Count; i++)
            {
                if (reactions[i] == null)
                    reactions.RemoveAt(i);
            }
            for (int i = 0; i < informations.Count; i++)
            {
                if (informations[i] == null)
                    informations.RemoveAt(i);
            }
        }

        public void Clear(){
            reactions.Clear();
            actions.Clear();
            informations.Clear();
        }
    }
}