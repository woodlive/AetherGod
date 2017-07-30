using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraRaycaster))]
public class CursorAffordances : MonoBehaviour {

   [SerializeField] Texture2D walkableCursor = null;
   [SerializeField] Texture2D enemyCursor = null;
   [SerializeField] Texture2D humanCursor = null;
   [SerializeField] Texture2D raycastEndStopCursor = null;

   [SerializeField] Vector2 cursorHotspot = new Vector2(0f, 0f);

   CameraRaycaster cameraRaycaster;

   // Use this for initialization
   void Start () {
      cameraRaycaster = GetComponent<CameraRaycaster>();
      cameraRaycaster.onLayerChange += CursorLayerChangeHandler;
   }

   void CursorLayerChangeHandler ( Layer newLayer) {
      switch (newLayer)
      {
         case Layer.Walkable:
            Cursor.SetCursor(walkableCursor, new Vector2(0f,30f), CursorMode.Auto);
            break;
         case Layer.Enemy:
            Cursor.SetCursor(enemyCursor, new Vector2(0f, 0f), CursorMode.Auto);
            break;
         case Layer.Human:
            Cursor.SetCursor(humanCursor, new Vector2(0f, 0f), CursorMode.Auto);
            break;
         case Layer.RaycastEndStop:
            Cursor.SetCursor(raycastEndStopCursor, cursorHotspot, CursorMode.Auto);
            break;
         default:
            break;
      }
   }

// TODO consider de-registering OnLayerChange on leaving all sceenes
}
