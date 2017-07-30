using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
   [SerializeField] float walkMoveStopRadius = 0.2f; // TODO Rename to something with movement error tolerance, or something
   ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
   CameraRaycaster cameraRaycaster;

   public static bool isInDirectMode = true;

   private bool isAutoRunning = false;
   private Vector3 currentDestination;
   private Vector3 clickPoint;

   private String letter;

   private void Start()
   {
      cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
      thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
      currentDestination = transform.position;
   }

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.G))// TODO key mapping
      {
         isInDirectMode = !isInDirectMode;
         currentDestination = transform.position;
      }
      if (Input.GetKeyDown(KeyCode.Mouse3))// TODO key mapping
      {
         isAutoRunning = !isAutoRunning;
      }
   }

   // Fixed update is called in sync with physics
   private void FixedUpdate()
   {
      if (isInDirectMode)
      {
         ProcessDirectMovement();
      }
      else
      {
         ProcessMouseMovement();
      }
   }

   private void ProcessDirectMovement()
   {
      float h = Input.GetAxis("Horizontal");
      float v = Input.GetAxis("Vertical");
      if (isAutoRunning)
      {
         v = 1;
      }

      // calculate camera relative direction to move:
      Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
      Vector3 movement = v * cameraForward + h * Camera.main.transform.right;

      thirdPersonCharacter.Move(movement, false, false);
   }

   private void ProcessMouseMovement()
   {
      if (Input.GetMouseButton(0)) // TODO key mapping
      {
         // Debug.Log("Cursor raycast hit: " + cameraRaycaster.hit.collider.gameObject.name.ToString());
         clickPoint = cameraRaycaster.hit.point;
         switch (cameraRaycaster.currentLayerHit)
         {
            case Layer.Walkable:
               currentDestination = ShortDestination(clickPoint, walkMoveStopRadius);  // So not set in default case
               break;
            case Layer.Enemy:
               // Debug.Log("Not moving to enemy");
               break;
            case Layer.RaycastEndStop:
               break;
            default:
               Debug.Log("SHOULDN'T BE HERE");
               break;
         }
      }
      WalkToDestination();
   }

   private void WalkToDestination()
   {
      Vector3 currentRelativeDestination = currentDestination - transform.position;
      if (currentRelativeDestination.magnitude >= walkMoveStopRadius)
      {
         thirdPersonCharacter.Move(currentRelativeDestination, false, false);
      }
      else
      {
         thirdPersonCharacter.Move(Vector3.zero, false, false);
      }
   }

   private Vector3 ShortDestination(Vector3 destination, float shortening)
   {
      Vector3 reductionVector = (destination - transform.position).normalized * shortening;
      return destination - reductionVector;
   }

   private void OnDrawGizmos()
   {
      Gizmos.color = Color.black;
      Gizmos.DrawSphere(currentDestination, 0.2f);
      Gizmos.DrawSphere(clickPoint, 0.3f);
   }
}

