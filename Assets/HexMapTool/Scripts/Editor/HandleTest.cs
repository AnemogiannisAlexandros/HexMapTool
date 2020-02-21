//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEditor;
//using System.IO;

//namespace HexMapTool
//{

//    [CustomEditor(typeof(HexMesh))]
//    public class HandleTest : Editor
//    { 
//        private void OnSceneGUI()
//        {
//            Handles.BeginGUI();
//            ///THIS DRAW OVER AN OBJECT IN THE SCENE
//            //Vector3 point = new Vector3();
//            ////point = ((MonoBehaviour)target).transform.TransformPoint(point);
//            //point = ((MonoBehaviour)target).transform.position;
//            //point = Camera.main.WorldToScreenPoint(point);
//            //GUI.Label(new Rect(point.x, point.y, 300, 30), "Hello World");

//            ///THIS IS SOME WEIRD ASS SHIT
//            Ray inputRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
//            RaycastHit hit;
//            if (Physics.Raycast(inputRay, out hit))
//            {
//                Vector3 drawPoint = Handles.PositionHandle(hit.point, Quaternion.identity);
//                Handles.SphereHandleCap(0, drawPoint/2, Quaternion.identity, 50, EventType.Repaint);
//                Handles.color = Color.red;
//            }
//            //Vector3 point = new Vector3();
//            //point = ((MonoBehaviour)target).transform.TransformPoint(point);
//            //point = ((MonoBehaviour)target).transform.position;
//            //point = Camera.main.WorldToScreenPoint(point);

//            Handles.EndGUI();
      
//        }
//    }
//}
