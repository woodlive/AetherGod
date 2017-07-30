using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Formation : MonoBehaviour
{
   private List<Human> memberHumans;
   public float formationSpread = 0.70f;

   public List<Human> members
   {
      get { return memberHumans; }
      set { memberHumans = value; }
   }

   // Use this for initialization
   void Awake()
   {
      memberHumans = new List<Human>();
   }

   // Update is called once per frame
   void Update()
   {

   }

   public void MoveTo(Vector3 destination)
   {
      transform.position = destination;
      RefreshPositions();
   }

   internal void leave(Human human)
   {
      memberHumans.Remove(human);
      if (memberHumans.Count <= 0)
      {
         Destroy(gameObject);
      }
      else
      {
         RefreshPositions();
      }
   }

   internal void join(Human human)
   {
      memberHumans.Add(human);
      RefreshPositions();
   }

   private void RefreshPositions()
   {
      float degreePerMember = 360 / members.Count;
      for (int i = 0; i < members.Count; i++)
      {
         Vector3 pos = transform.position;
         pos += Quaternion.AngleAxis(degreePerMember * i, Vector3.up) * new Vector3(formationSpread, 0.0f, 0.0f);
         members[i].SetWaypoint(pos);
      }
   }

   private void OnDrawGizmos()
   {
      Gizmos.color = Color.yellow;
      Gizmos.DrawSphere(transform.position, 0.1f);
      foreach (Human human in memberHumans)
      {
         Gizmos.DrawLine(transform.position, human.gameObject.transform.position);
      }
   }

   internal void Attack(Formation attackTarget)
   {
      MoveTo(attackTarget.transform.position);
      for (int i = 0; i < memberHumans.Count; i++)
      {
         if(i < attackTarget.members.Count){
            members[i].AttackHuman(attackTarget.members[i]);
         }
      }
   }
}
