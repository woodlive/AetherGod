using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
[RequireComponent(typeof(ThirdPersonCharacter))]
public class Human : MonoBehaviour
{
   [SerializeField] float walkMoveStopRadius = 0.5f; // TODO Rename to something with movement error tolerance, or something
   [SerializeField] float attackMoveStopRadius = 5f;
   ThirdPersonCharacter thirdPersonCharacter;
   public GameObject waypointPrefab;

   private Waypoint wayPoint;

   public Formation formation { get; set; }

   [SerializeField] float maxHealth = 100f;

   private float currentHealthPoints = 100f;

   public float healthAsPercentage
   {
      get { return currentHealthPoints / maxHealth; }
   }

   void Start()
   {
      thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
      formation = null;
      wayPoint = null;
   }

   private void FixedUpdate()
   {
      WalkToDestination();
   }

   private Vector3 ShortDestination(Vector3 destination, float shortening)
   {
      Vector3 reductionVector = (destination - transform.position).normalized * shortening;
      return destination - reductionVector;
   }

   private void WalkToDestination()
   {
      if (wayPoint != null)
      {
         Vector3 directionToWaypoint = wayPoint.transform.position - transform.position;
         if (directionToWaypoint.magnitude > walkMoveStopRadius)
         {
            thirdPersonCharacter.Move(directionToWaypoint, false, false);
         }
         else
         {
            thirdPersonCharacter.Move(Vector3.zero, false, false);
         }
      }
   }

   public void SetWaypoint(Vector3 position)
   {
      GameObject newWayPoint = Instantiate(waypointPrefab, position, Quaternion.identity);

      if (wayPoint != null)
      {
         Destroy(wayPoint.gameObject);
      }
      wayPoint = newWayPoint.GetComponent<Waypoint>();
      GetComponent<AICharacterControl>().target = wayPoint.gameObject.transform;
   }

   public void JoinFormation(Formation newFormation)
   {
      if (formation != null)
      {
         formation.leave(this);
      }

      newFormation.join(this);
      formation = newFormation;
   }

public void AttackHuman(Human target){
      SetWaypoint( ShortDestination(target.transform.position, attackMoveStopRadius));
   }

   private void OnDrawGizmos()
   {
      if (wayPoint != null)
      {
         Gizmos.color = Color.red;
         Gizmos.DrawLine(transform.position, wayPoint.gameObject.transform.position);
      }
   }
}
