
using System.Collections;

namespace iivimat
{
    /// <summary>
    /// 
    /// </summary>
    public interface ILocalLoopCoroutine
    {
        Action GetAction();
        IEnumerator GetCoroutine();
    }
}