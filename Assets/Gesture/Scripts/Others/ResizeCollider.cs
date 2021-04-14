using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeCollider : MonoBehaviour
{
    OVRSkeleton skeleton;
    List<OVRBone> fingerBones;
    OVRHand hand;

    BoxCollider box;

    float minX,maxX,minY,maxY,minZ,maxZ;

    void Start(){
        box = this.GetComponent<BoxCollider>();
        hand = this.GetComponent<OVRHand>();
        skeleton = this.GetComponent<OVRSkeleton>();

        ResetValue();
    }

    // Update is called once per frame
    void Update()
    {


        if(hand.IsTracked){

            fingerBones = new List<OVRBone>(skeleton.Bones);
            GetLimitesCoords();
            ResizeBox();
        }
    }

    private void GetLimitesCoords(){
        ResetValue();

        foreach(OVRBone bone in fingerBones){
            Vector3 pos = bone.Transform.position;

            // X
            if(pos.x < minX){
                minX = pos.x;
            }

            if(pos.x > maxX){
                maxX = pos.x;
            }

            // Y
            if(pos.y < minY){
                minY = pos.y;
            }

            if(pos.y > maxY){
                maxY = pos.y;
            }

            // Z
            if(pos.z < minZ){
                minZ = pos.z;
            }

            if(pos.z > maxZ){
                maxZ = pos.z;
            }
        }
    }

    private void ResizeBox(){
        float Sizex = Mathf.Abs(maxX - minX);
        float Sizey = Mathf.Abs(maxY - minY);
        float Sizez = Mathf.Abs(maxZ - minZ);
        Vector3 size = new Vector3(Sizey, Sizez, Sizex);
        box.size = size;
        box.transform.position = this.transform.position ;
    }

    private void ResetValue(){
        minX = float.MaxValue;
        minY = float.MaxValue;
        minZ = float.MaxValue;

        maxX = -float.MaxValue;
        maxY = -float.MaxValue;
        maxZ = -float.MaxValue;
    }
}
