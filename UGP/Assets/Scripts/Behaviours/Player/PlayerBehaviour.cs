﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


namespace UGP
{
    public class PlayerBehaviour : NetworkBehaviour
    {
        public Player player;
        [HideInInspector] public Player p;

        [SyncVar] public bool isDriving;
        public VehicleBehaviour vehicle;
        public InGamePlayerMovementBehaviour playerMovement;
        public PlayerInteractionBehaviour interaction;

        private void Awake()
        {
            if (!localPlayerAuthority)
            {
                enabled = false;
                return;
            }
        }

        private void Start()
        {
            if (!localPlayerAuthority)
            {
                enabled = false;
                return;
            }

            p = player;
            p.Alive = true;
        }

        private void FixedUpdate()
        {
            if (!localPlayerAuthority)
            {
                enabled = false;
                return;
            }

            if (vehicle == null)
            {
                isDriving = false;
            }
            else
            {
                isDriving = true;
            }

            var model1 = transform.Find("Model-Head");
            var model2 = transform.Find("Model-Body");
            var rb = GetComponent<Rigidbody>();

            if (isDriving)
            {
                vehicle.enabled = true;
                playerMovement.enabled = false;
                interaction.enabled = false;

                var ammoBox = p.ammo;
                vehicle._v.ammunition = ammoBox;


                //DISABLE THE PLAYER COLLIDER(S) IF DRIVING
                model1.gameObject.SetActive(false);
                model2.gameObject.SetActive(false);

                transform.position = vehicle.seat.position;
                transform.rotation = vehicle.seat.rotation;

                rb.isKinematic = true;

                //NEEDS WORK
                if (Input.GetKeyDown(KeyCode.F))
                {
                    //GET OUT OF VEHICLE
                    vehicle._v.ammunition = null;
                    vehicle.SetVehicleActive(false);
                    vehicle = null;
                    isDriving = false;
                }
            }
            else
            {
                vehicle = null;
                playerMovement.enabled = true;
                interaction.enabled = true;

                //ENABLE THE PLAYER COLLIDER(S) IF NOT DRIVING
                model1.gameObject.SetActive(true);
                model2.gameObject.SetActive(true);

                rb.isKinematic = false;
            }
        }
    }
}