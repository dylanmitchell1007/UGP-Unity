﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


namespace Trent
{


    public class PlayerMovementBehaviour : MonoBehaviour
    {
        public float WalkSpeed = 1.0f;
        public float RunSpeed = 1.0f;
        public float JumpStrength = 1.0f;
        public float TurnSpeed = 1.0f;

        private bool HasJumped;
        private bool HasSprinted;
        private float SprintTimer = 1.0f;
        private float JumpTimer = 1.0f;
        private Rigidbody rb;
        // Use this for initialization
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            if(!rb)
            {
                rb = gameObject.AddComponent<Rigidbody>();
            }
        }
       
        void FixedUpdate()
        {
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
        
            if(Input.GetKeyDown(KeyCode.Space)) //JUMP
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
        // Update is called once per frame
        void Update()
        {

        }
    }
}