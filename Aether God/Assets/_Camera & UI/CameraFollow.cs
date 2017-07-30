using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
   public float cameraRotationSensitivity = 0.5f;

   private GameObject target;
   private Vector3 mousePosLastUpdate;
   private bool isInFreelookMode = true;
   private bool isAxisInUse = false;

   // Use this for initialization
   void Start()
   {
      target = GameObject.FindGameObjectWithTag("Player");
      mousePosLastUpdate = new Vector3(0.0f, 0.0f, 0.0f);
      mousePosLastUpdate = Input.mousePosition;
   }

   // Update is called once per frame
   void LateUpdate()
   {
      if (Input.GetAxisRaw("RotateMainCamera") != 0)
      {
         if (isAxisInUse == false)
         {
            isInFreelookMode = !isInFreelookMode;
            if (isInFreelookMode)
            {
               mousePosLastUpdate = Input.mousePosition;
            }
            isAxisInUse = true;
         }
      }
      if (Input.GetAxisRaw("RotateMainCamera") == 0)
      {
         isAxisInUse = false;
      }
      HandleCameraMovement();
   }

   private void HandleCameraMovement()
   {
      transform.position = target.transform.position;
      if (isInFreelookMode)
      {
         float degrees = ((mousePosLastUpdate.x - Input.mousePosition.x) * cameraRotationSensitivity) * -1.0f;
         transform.Rotate(0.0f, degrees, 0.0f);
         mousePosLastUpdate = Input.mousePosition;
      }
   }
}
