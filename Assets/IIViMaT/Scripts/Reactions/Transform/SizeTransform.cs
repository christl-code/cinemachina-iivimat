﻿using UnityEngine;

namespace iivimat
{
    public class SizeTransform : ReactionComponent<Transform>
    {
        public enum RelativeToOptions
        {
            World,
            Self,
            Object,
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
                            transform.localScale = transformValues;
                            break;
                        case RelativeToOptions.Self:
                            transform.localScale += transformValues;
                            break;
                        case RelativeToOptions.Object:
                            transform.localScale = referenceObject.transform.localScale + transformValues;
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
            // Debug.Log("SizeTransform stopped");
        }
    }
}