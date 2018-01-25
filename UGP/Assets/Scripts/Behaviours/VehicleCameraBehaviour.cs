﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


namespace UGP
{
    public class VehicleCameraBehaviour : NetworkBehaviour
    {
        private VehicleMovementBehaviour behaviour;
        private GameObject aimCamera;
        private GameObject followCamera;
        private Transform cam;

        private void Start()
        {
            cam = transform.Find("Camera");

            if (!isLocalPlayer)
                cam.gameObject.SetActive(false);
                return;

            cam = transform.Find("Camera");

            behaviour = GetComponentInParent<VehicleMovementBehaviour>();
            aimCamera = GameObject.Find("AimCamera");
            followCamera = GameObject.Find("FollowCamera");
        }

        private void LateUpdate()
        {
            if (!isLocalPlayer)
                cam.gameObject.SetActive(false);
                return;

            switch (behaviour.mode)
            {
                case VehicleState.DRIVE:
                    {
                        //SWITCH TO FOLLOW CAMERA
                        followCamera.SetActive(true);
                        aimCamera.SetActive(false);
                        break;
                    }

                case VehicleState.COMBAT:
                    {
                        //SWITCH TO AIM CAMERA
                        aimCamera.SetActive(true);
                        followCamera.SetActive(false);

                        var rot = aimCamera.transform.rotation;
                        rot[0] = 0.0f;
                        rot[2] = 0.0f;

                        
                        behaviour.Aim(rot);
                        break;
                    }
            }
        }
    }
}