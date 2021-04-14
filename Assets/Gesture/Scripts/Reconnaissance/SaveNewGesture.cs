using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace iivimat{

public class SaveNewGesture : MonoBehaviour
{
    public OVRSkeleton skeleton;
    public string nameNewGesture;
    public bool debug;

    private List<OVRBone> fingerBones;

    private string path = "Assets/Gesture/Gestures";

    void Start()
    {
        fingerBones = new List<OVRBone>();
        nameNewGesture = "New Gesture";
    }

    /// <summary>
    /// Constantly searching for a new gesture
    /// </summary>
    void Update()
    {
        if (fingerBones.Count == 0)
        {
            fingerBones = new List<OVRBone>(skeleton.Bones);
        }

        // Save a new movement
        if (debug && Input.GetKeyDown(KeyCode.Space))
        {
            Save();
        }
    }


    /// <summary>
    /// Allows to save a new gesture
    /// </summary>
    public void Save()
    {

        Gesture gest = new Gesture();
        gest.name = nameNewGesture;

        gest.fingerPositionDatas = GetDistancesBones();
        //gest.normal = GetNormal();

        string jsonGesture = JsonUtility.ToJson(gest);
        Debug.Log(jsonGesture);
        System.IO.File.WriteAllText(path + "/" + gest.name + ".json", jsonGesture);

        Debug.Log("New gesture added : " + gest.name);
    }

    private List<Vector3> GetDistancesBones()
    {
        List<Vector3> dataPosition = new List<Vector3>();

        foreach (var bone in fingerBones)
        {
            // Bone position
            Vector3 bonePosition = skeleton.transform.InverseTransformPoint(bone.Transform.position);

            // Position
            dataPosition.Add(bonePosition);

        }

        return dataPosition;
    }

    public Vector3 GetNormal()
    {
        // Get thumb trapezium bone
        Vector3 v1 = fingerBones[2].Transform.position - fingerBones[0].Transform.position;

        // Get pinky metacarpal bone

        Vector3 v2 = fingerBones[15].Transform.position - fingerBones[0].Transform.position;
        Vector3 normal = Vector3.Cross(v1, v2);
        return normal;
    }
}
}
