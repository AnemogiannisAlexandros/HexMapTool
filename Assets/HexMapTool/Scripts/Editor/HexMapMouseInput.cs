using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HexMapTool
{
    /// <summary>
    /// Editor Interaction Window.
    /// this is how we interact with the tool at the scene
    /// </summary>
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
                        if (e.keyCode == KeyCode.Space)
                        {
                           Debug.Log("Working");
                            HandleInput();
                            SceneView.RepaintAll();
                        }
                        break;
                    }

                //case EventType.MouseDrag:
                //    {
                //        if (e.keyCode == KeyCode.Mouse0)
                //        {
                //            Debug.Log("Working");
                //            HandleInput();
                //            SceneView.RepaintAll();
                //        }
                //    }
                //    break;
                //case EventType.Layout:
                //    if (e.keyCode == KeyCode.Mouse0)
                //    {
                //        Debug.Log("Working");
                //        HandleInput();
                //        SceneView.RepaintAll();
                //    }
                //    HandleUtility.AddDefaultControl(0);

                   
                //    break;
            }
        }

        void HandleInput()
        {
            Ray inputRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            //Debug.DrawRay(inputRay.origin, inputRay.direction, Color.red, 6);
            RaycastHit hit;
            if (Physics.Raycast(inputRay, out hit))
            {
                Debug.Log("touched at Vector3 : " + hit.point);
                HexCoordinates coordinates = HexCoordinates.FromPosition(hit.point);
                Debug.Log("touched at HexCoordinates " + coordinates.ToString());
                HexGrid grid =  HexMapEditorWindow.grid;
                grid.TouchCell(hit.point,coordinates);
            }
        }
    }
}