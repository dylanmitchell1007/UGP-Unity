﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;


namespace Trent
{


    public enum PlayerState
    {
        idle = 0,
        move = 1,
        driving = 2,
        sitting = 3,
        standing = 4,
        viewing = 5,
    }

    public class PlayerMovementBehaviour : NetworkBehaviour
    {

        public PlayerState state;

        public float WalkSpeed = 1.0f;
        public float RunSpeed = 1.0f;
        public float JumpStrength = 1.0f;
        public float TurnSpeed = 1.0f;

        private bool CanMove;
        public bool IsSitting = false;
        public bool IsStanding = false;
        public bool IsViewing = false;

        private bool HasJumped;
        private bool HasSprinted;
        private float SprintTimer = 1.0f;
        private float JumpTimer = 1.0f;
        private Rigidbody rb;
        // Use this for initialization
        void Start()
        {
            state = PlayerState.idle;
            CanMove = true;
            rb = GetComponent<Rigidbody>();
            if (!rb)
            {
                rb = gameObject.AddComponent<Rigidbody>();
            }
        }
        public override void OnStartLocalPlayer()
        {
            GetComponent<MeshRenderer>().material.color = Color.white;
        }
        void OnTriggerEnter(Collider other)
        {
            var player = FindObjectsOfType<PlayerMovementBehaviour>();

            if (other.tag == "ChairCollider")
            {
                state = PlayerState.sitting;
                transform.position = other.GetComponent<Transform>().position;
                transform.rotation = other.GetComponent<Transform>().rotation;
                Debug.Log("Test");
                CanMove = false;
                rb.isKinematic = true;
                IsSitting = true;
            }
            if(other.tag == "ToolCollider")
            {
                state = PlayerState.standing;
                transform.position = other.GetComponent<Transform>().position;
                transform.rotation = other.GetComponent<Transform>().rotation;
                Debug.Log("Test");
                CanMove = false;
                rb.isKinematic = true;
                IsStanding = true;
            }
            if (other.tag == "HoverCraftCollider")
            {
                state = PlayerState.viewing;
                transform.position = other.GetComponent<Transform>().position;
                transform.rotation = other.GetComponent<Transform>().rotation;
                Debug.Log("Test");
                CanMove = false;
                rb.isKinematic = true;
                IsViewing = true;
            }
            if (other.tag == "DockCollider")
            {
                state = PlayerState.viewing;
                transform.position = other.GetComponent<Transform>().position;
                transform.rotation = other.GetComponent<Transform>().rotation;
                Debug.Log("Test");
                CanMove = false;
                rb.isKinematic = true;
                IsSitting = true;
            }
        }

        void FixedUpdate()
        {
            if(IsSitting)
                if (Input.GetKeyDown(KeyCode.F))
                {
                    state = PlayerState.move;
                    //GET OUT OF CHAIR
                    Vector3 getOutPosition = new Vector3(2.5f, 0, 0);
                    transform.position = transform.position + getOutPosition;
                    IsSitting = false;
                    CanMove = true;
                    rb.isKinematic = false;
                }
            if (IsStanding)
                if (Input.GetKeyDown(KeyCode.F))
                {
                    state = PlayerState.move;
                    //GET OUT OF ToolBelt
                    Vector3 getOutPosition = new Vector3(-1.5f, 0, 0);
                    transform.position = transform.position + getOutPosition;
                    IsStanding = false;
                    CanMove = true;
                    rb.isKinematic = false;
                }
            if (IsViewing)
                if (Input.GetKeyDown(KeyCode.F))
                {
                    state = PlayerState.move;
                    //GET OUT OF HoverCraftView
                    Vector3 getOutPosition = new Vector3(1.5f, 0, 0);
                    transform.position = transform.position + getOutPosition;
                    IsViewing = false;
                    CanMove = true;
                    rb.isKinematic = false;
                }
            if (state == PlayerState.move)
            {
                IsStanding = false;
                IsSitting = false;
            }
           

            if (CanMove == true)
            {
                IsSitting = false;
                state = PlayerState.move;
                if (!isLocalPlayer)
                {
                  
                    return;
                }
                //OnStartLocalPlayer();
                var Forward = Input.GetAxis("Vertical");
                var TurnHead = Input.GetAxis("Horizontal");

                Vector3 movementVector = new Vector3(TurnHead * TurnSpeed, 0.0f, Forward * WalkSpeed);

                var deltaX = Input.GetAxis("Mouse X"); //GET THE MOUSE DELTA X
                var deltaY = Input.GetAxis("Mouse Y"); //GET THE MOUSE DELTA Y

                Vector3 rotX = new Vector3(-deltaY, 0, 0);
                Vector3 rotY = new Vector3(0, deltaX, 0);

                Quaternion rot = Quaternion.Euler(rotX); //CREATE A QUATERNION ROTATION FROM A EULER ANGLE ROTATION
                transform.rotation = Quaternion.Slerp(transform.rotation, transform.rotation * rot, 1.0f); //INTERPOLATE BETWEEN THE CURRENT ROTATION AND THE NEW ROTATION
                rot = Quaternion.Euler(rotY); //CREATE A QUATERNION ROTATION FROM A EULER ANGLE ROTATION
                transform.rotation = Quaternion.Slerp(transform.rotation, transform.rotation * rot, 1.0f); //INTERPOLATE BETWEEN THE CURRENT ROTATION AND THE NEW ROTATION

                if (Input.GetKeyDown(KeyCode.Space)) //JUMP
                {
                    Vector3 jumpVector = new Vector3(0, 1 * JumpStrength, 0);
                    //rb.AddForce(jumpVector);
                    HasJumped = true;
                    transform.Translate(jumpVector);
                    JumpTimer++;
                }

                if (JumpTimer >= 2.0f)
                {
                    JumpTimer = 0.0f;
                    HasJumped = false;
                }


                if (HasJumped == true)
                {
                    JumpTimer += Time.deltaTime;
                }



                transform.Translate(movementVector);
                //rb.AddForce(movementVector);

                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    Vector3 sprintVector = new Vector3(Forward, WalkSpeed * RunSpeed);
                    HasSprinted = true;
                    //rb.AddForce(sprintVector);
                    transform.Translate(sprintVector);
                    SprintTimer++;
                }

                if (SprintTimer >= 2.0f)
                {
                    SprintTimer = 0.0f;
                    HasSprinted = false;
                }


                if (HasSprinted == true)
                {
                    SprintTimer += Time.deltaTime;
                }


            }
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}