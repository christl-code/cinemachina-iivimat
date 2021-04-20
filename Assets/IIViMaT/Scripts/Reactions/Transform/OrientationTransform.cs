using UnityEngine;

namespace iivimat
{
    public class OrientationTransform : ReactionComponent<Transform>
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
                        case RelativeToOptions.World:
                            transform.rotation = Quaternion.Euler(transformValues);
                            break;
                        case RelativeToOptions.Self:
                            transform.rotation = transform.rotation * Quaternion.Euler(transformValues);
                            break;
                        case RelativeToOptions.Object:
                            transform.rotation = referenceObject.transform.rotation * Quaternion.Euler(transformValues);
                            break;
                        case RelativeToOptions.Head:
                            transform.rotation = Camera.main.transform.rotation * Quaternion.Euler(transformValues);
                            break;
                        default:
                            break; //throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }

        public override void OnEventStopped(Transform transform)
        {
            // Debug.Log("OrientationTransform stopped");
        }
    }
}