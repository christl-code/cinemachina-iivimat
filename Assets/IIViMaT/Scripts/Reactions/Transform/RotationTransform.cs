using UnityEngine;

namespace iivimat
{
    public class RotationTransform : ReactionComponent<Transform>
    {
        public enum RelativeToOptions
        {
            World,
            Self,
            Object,
            Head
        }
        public Vector3 transformValues;

        public RelativeToOptions relativeTo;

        public GameObject referenceObject;

        public float angle;

        public override void OnEventRaised(Transform transform)
        {
            if(!playOnce || !finished)
            {
                if (Targets.Contains(transform))
                {
                    switch (relativeTo)
                    {
                        case RelativeToOptions.World:
                            transform.RotateAround(Vector3.zero, transformValues, angle);
                            break;
                        case RelativeToOptions.Self:
                            transform.RotateAround(transform.position, transformValues, angle);
                            break;
                        case RelativeToOptions.Object:
                            transform.RotateAround(referenceObject.transform.position, transformValues, angle);
                            break;
                        case RelativeToOptions.Head:
                            transform.RotateAround(Camera.main.transform.position, transformValues, angle);
                            break;
                        default:
                            break; //throw new ArgumentOutOfRangeException();
                    }
                    setFinished(true);
                }
            }
        }

        public override void OnEventStopped(Transform transform)
        {
            // Debug.Log("RotationTransform stopped");
        }
    }
}