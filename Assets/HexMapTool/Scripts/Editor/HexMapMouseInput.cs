using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HexMapTool
{
    [CustomEditor(typeof(HexMesh))]
    public class HexMapMouseInput : Editor
    {
        SceneView view;
         void OnSceneGUI()
        {

            HexMesh script = (HexMesh)target;
            Event e = Event.current;
            switch (e.type)
            {
                case EventType.KeyDown:
                    {
                        if (e.keyCode == KeyCode.A)
                        {
                            Debug.Log("Input works");
                            //view = SceneView.lastActiveSceneView;
                            HandleInput();
                        }
                        break;
                    }
            }
        }

        void HandleInput()
        {
            Ray inputRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            Debug.DrawRay(inputRay.origin,inputRay.direction,Color.red,6);
            RaycastHit hit;
            if (Physics.Raycast(inputRay, out hit))
            {
                Debug.LogFormat("Touched At : {0} ",hit.point);
                //TouchCell(hit.point);
            }

        }
    }
}