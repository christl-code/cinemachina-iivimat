using UnityEngine;

namespace iivimat
{
    public class PositionTransform : ReactionComponent<Transform>
    {
        public enum RelativeToOptions
        {
            World,
            Self,
            Object,
            Head
        }
        public Vector3 transformValues;

        public RelativeToOptions relativeTo = RelativeToOptions.World;

        public GameObject referenceObject;

        public override void OnEventRaised(Transform transform)
        {
            if(!playOnce || !finished)
            {
                if (Targets.Contains(transform))
                {

                    switch (relativeTo)
                    {
                        case RelativeToOptions.World :
                            transform.position = transformValues;
                            break;

                        case RelativeToOptions.Self :
                            transform.position = transform.position + transformValues;
                            break;

                        case RelativeToOptions.Object :
                            transform.position = referenceObject.transform.position + transformValues;
                            break;

                        case RelativeToOptions.Head :
                            transform.position = Camera.main.transform.position + transformValues;
                            break;

                        default:
                            break;
                    }
                }
            }
        }

        public override void OnEventStopped(Transform transform)
        {
            // Debug.Log("PositionTransform stopped");
        }
    }
}