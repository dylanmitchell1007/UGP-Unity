﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;

namespace UGP
{
    public class MirrorRotateBehaviour : MonoBehaviour
    {
        public Button Leftbutton;
        public Button Rightbutton;
        public Transform model;
        public float turnspeed;

        public void RotateLeft()
        {
            if(Input.GetKey(KeyCode.LeftArrow))
            {
            var left = new Vector3(0.0f, -1 * turnspeed, 0.0f);
            model.Rotate(left * Time.deltaTime);
            }
          

        }
        public void RotateRight()
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                var right = new Vector3(0.0f, 1 * turnspeed, 0.0f);
                model.Rotate(right * Time.deltaTime);
            }
        }
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            RotateLeft();
            RotateRight();
        }
    }
}