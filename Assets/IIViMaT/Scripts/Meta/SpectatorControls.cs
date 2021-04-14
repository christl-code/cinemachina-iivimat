using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

namespace iivimat
{
    /// <summary>
    /// Switch between keyboard/mouse controls and 
    /// VR headset controls if VR headset is connected or not
    /// </summary>
    public class SpectatorControls : MonoBehaviour
    {
        // Mouse parameters
        // Please note that this adjusting mouse speed by code is not too relevant (as those speeds are incremntal ?)
        // We also have to change the sensitivity in Edit > Project Settings > Input Manager > [Mouse X; Mouse Y; Mouse ScrollWheel] > Sensitivity
        public float mouseRotationSpeed = 25;

        // Keyboard parameters
        public float keyboardTranslationSpeed = 2;



        private void Start()
        {
            if (!isPresent())
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        private void Update()
        {
            if (!isPresent())
            {
                Mouse();
                Keyboard();
            }
        }

        private void Mouse()
        {
            transform.RotateAround(transform.position, Vector3.up, Input.GetAxis("Mouse X") * mouseRotationSpeed * Time.deltaTime);

            Vector3 rotation = transform.eulerAngles;
            rotation.x -= Input.GetAxis("Mouse Y") * mouseRotationSpeed * Time.deltaTime;
            rotation.x = rotation.x < 180 ? Mathf.Min(rotation.x, 90) : Mathf.Max(rotation.x, 270);
            transform.rotation = Quaternion.Euler(rotation);
        }

        private void Keyboard()
        {
            // Keys movement
            transform.Translate(
                Vector3.right * Input.GetAxis("Horizontal") * keyboardTranslationSpeed * Time.deltaTime
                + Vector3.forward * Input.GetAxis("Vertical") * keyboardTranslationSpeed * Time.deltaTime);
        }

        private bool isPresent()
        {
            var xrDisplaySubsystems = new List<XRDisplaySubsystem>();
            SubsystemManager.GetInstances<XRDisplaySubsystem>(xrDisplaySubsystems);
            foreach (var xrDisplay in xrDisplaySubsystems)
            {
                if (xrDisplay.running)
                {
                    return true;
                }
            }
            return false;
        }   
    }
}