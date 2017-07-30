using System.Runtime.Remoting.Messaging;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

//[RequireComponent(typeof(ThirdPersonCharacter))]
public class UnitController : MonoBehaviour
{
   public GameObject formationPrefab;
   //[SerializeField] float walkMoveStopRadius = 0.1f; // TODO Rename to something with movement error tolerance, or something
   //ThirdPersonCharacter m_Character;   // A reference to the ThirdPersonCharacter on the object
   CameraRaycaster cameraRaycaster;
   Vector3 currentClickTarget;

   private List<GameObject> selected;

   private void Start()
   {
      cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
      //m_Character = GetComponent<ThirdPersonCharacter>();
      currentClickTarget = transform.position;
      selected = new List<GameObject>();
   }

   // Fixed update is called in sync with physics
   private void FixedUpdate()
   {
      if (Input.GetMouseButton(0))
      {
         // Debug.Log("Cursor raycast hit: " + cameraRaycaster.hit.collider.gameObject.name.ToString());
         switch (cameraRaycaster.currentLayerHit)
         {
            case Layer.Walkable:
               selected.Clear();
               break;
            case Layer.Enemy:
               // Debug.Log("Not interacting to enemy");

               break;
            case Layer.Human:
               GameObject hitHuman = cameraRaycaster.hit.collider.gameObject;
               if (!selected.Contains(hitHuman)){
                  selected.Add(hitHuman);
                  // Debug.Log("Human selected"); 
               }

               break;
            case Layer.RaycastEndStop:

               break;
            default:
               Debug.Log("SHOULDN'T BE HERE");

               break;
         }
      }
      if (Input.GetMouseButton(1))
      {
         Formation formation;
         switch (cameraRaycaster.currentLayerHit)
         {
            case Layer.Walkable:
               formation = FormFormation();
               formation.MoveTo(cameraRaycaster.hit.point);
               break;
            case Layer.Enemy:
               formation = FormFormation();
               GameObject attackTarget = cameraRaycaster.hit.collider.gameObject;
               if (attackTarget.GetComponent<Formation>())
               {
                  formation.Attack(attackTarget.GetComponent<Formation>());
               }else if(attackTarget.GetComponent<Human>())
               {
                  formation.Attack(attackTarget.GetComponent<Human>().formation);
               }
               break; 
            case Layer.Human:
               break;
            case Layer.RaycastEndStop:
               break;
            default:
               break;
         }
      }
         //Vector3 playerToClickPoint = currentClickTarget - transform.position;
         //if (playerToClickPoint.magnitude > walkMoveStopRadius)
         //{
         //   m_Character.Move(playerToClickPoint, false, false);
         //}
      }

   private Formation FormFormation()
   {
      // TODO use exsisting formation if there is any. Use settings of biggest group
      GameObject formation = Instantiate(formationPrefab, cameraRaycaster.hit.point, Quaternion.identity);

      foreach (GameObject selectedHuman in selected)
      {
         Human human = selectedHuman.GetComponent<Human>();
         human.JoinFormation(formation.GetComponent<Formation>());
      }

      if (formation.GetComponent<Formation>().members.Count <= 0)
      {
         Destroy(formation.gameObject);
      }

      return formation.GetComponent<Formation>();
   }

   private void OnDrawGizmos()
   {
      foreach(GameObject selectedHuman in selected)
      {
         Gizmos.color = Color.cyan;
         Vector3 pos = selectedHuman.transform.position;
         pos.y += 2;
         Gizmos.DrawSphere(pos,0.2f);
      }
   }
}

