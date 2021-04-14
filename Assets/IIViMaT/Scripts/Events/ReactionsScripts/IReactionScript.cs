using UnityEngine;

namespace iivimat
{
    /// <summary>
    /// Interface used to be execute in the reaction LaunchScript.
    /// A script needs to implement this interface to be call in a LaunchScript Reaction.
    /// </summary>    
    public interface IReactionScript{

        void ReactionToScript();
    }
}