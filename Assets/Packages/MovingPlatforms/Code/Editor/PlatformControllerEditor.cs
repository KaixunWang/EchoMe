using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Bundos.MovingPlatforms
{
    [CustomEditor(typeof(PlatformController))]
    public class PlatformControllerEditor : Editor
    {
        PlatformController platformController;

        SelectionInfo selectionInfo;

        private bool needsRepaint = false;
        private GUIStyle style = new GUIStyle();

        private void OnEnable()
        {
            platformController = target as PlatformController;
            selectionInfo = new SelectionInfo();
            style.normal.textColor = Color.black;
        }
        private void OnSceneGUI()
        {
            if (platformController.editing)
            {
                HandleEvents();
                HandleUI();
            }
        }

        private void HandleUI()
        {
            Handles.BeginGUI();
            {
                GUILayout.BeginArea(new Rect(10, 10, 200, 70));
                {
                    if (Event.current.modifiers == EventModifiers.Control)
                        GUILayout.Label("Removing points", style);
                    else
                        GUILayout.Label("Adding points", style);
                }
                GUILayout.EndArea();
            }
            Handles.EndGUI();
        }

        private void HandleEvents()
        {
            Event guiEvent = Event.current;
            switch (guiEvent.type)
            {
                case EventType.Repaint:
                    Draw();
                    break;
                case EventType.Layout:
                    HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
                    break;
                default:
                    HandleInput(guiEvent);

                    if (needsRepaint)
                    {
                        HandleUtility.Repaint();
                        needsRepaint = false;
                    }
                    break;
            }
        }

        private void Draw()
        {
            for (int i = 0; i < platformController.waypoints.Count; i++)
            {
                Vector3 nextPoint = platformController.waypoints[(i + 1) % platformController.waypoints.Count];

                if (platformController.pathType == WaypointPathType.Closed)
                {
                    // Draw Edges
                    if (i == selectionInfo.lineIndex)
                    {
                        Handles.color = Color.red;
                        Handles.DrawLine(platformController.waypoints[i], nextPoint, 1f);
                    }
                    else
                    {
                        Handles.color = Color.black;
                        Handles.DrawDottedLine(platformController.waypoints[i], nextPoint, 1f);
                    }
                }
                else if (i != platformController.waypoints.Count - 1)
                {
                    // Draw Edges
                    if (i == selectionInfo.lineIndex)
                    {
                        Handles.color = Color.red;
                        Handles.DrawLine(platformController.waypoints[i], nextPoint, 1f);
                    }
                    else
                    {
                        Handles.color = Color.black;
                        Handles.DrawDottedLine(platformController.waypoints[i], nextPoint, 1f);
                    }
                }

                // Draw Points
                if (i == selectionInfo.pointIndex)
                {
                    Handles.color = selectionInfo.pointIsSelected ? Color.black : Color.red;
                }
                else
                {
                    Handles.color = Color.white;
                }

                Handles.DrawSolidDisc(platformController.waypoints[i], Vector3.back, platformController.handleRadius);

                Handles.color = Color.black;
                Handles.Label(platformController.waypoints[i], i.ToString(), style);
            }

            needsRepaint = false;
        }

        void HandleInput(Event guiEvent)
        {
            Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
            float dstToDrawPlane = (0 - mouseRay.origin.z) / mouseRay.direction.z;
            Vector3 mousePosition = SnapToGrid(mouseRay.GetPoint(dstToDrawPlane));

            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.modifiers == EventModifiers.Control)
            {
                HandleLeftMouseDownDelete(mousePosition);
                return;
            }

            if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.modifiers == EventModifiers.None)
            {
                HandleLeftMouseDown(mousePosition);
            }

            if (guiEvent.type == EventType.MouseUp && guiEvent.button == 0 && guiEvent.modifiers == EventModifiers.None)
            {
                HandleLeftMouseUp(mousePosition);
            }

            if (guiEvent.type == EventType.MouseDrag && guiEvent.button == 0 && guiEvent.modifiers == EventModifiers.None)
            {
                HandleLeftMouseDrag(mousePosition);
            }

            if (!selectionInfo.pointIsSelected)
            {
                UpdateMouseOverInfo(mousePosition);
            }
        }

        void HandleLeftMouseDownDelete(Vector3 mousePosition)
        {
            Undo.RecordObject(platformController, "Remove waypoint");
            platformController.waypoints.RemoveAt(selectionInfo.pointIndex);
            selectionInfo.pointIndex = -1;
            needsRepaint = true;
        }

        void HandleLeftMouseDown(Vector3 mousePosition)
        {
            if (!selectionInfo.mouseIsOverPoint)
            {
                int newPointIndex = (selectionInfo.mouseIsOverLine) ? selectionInfo.lineIndex + 1 : platformController.waypoints.Count;
                Undo.RecordObject(platformController, "Add waypoint");
                platformController.waypoints.Insert(newPointIndex, mousePosition);
                selectionInfo.pointIndex = newPointIndex;
            }

            selectionInfo.pointIsSelected = true;
            selectionInfo.positionAtStartOfDrag = mousePosition;
            needsRepaint = true;
        }

        void HandleLeftMouseUp(Vector3 mousePosition)
        {
            if (selectionInfo.pointIsSelected)
            {
                platformController.waypoints[selectionInfo.pointIndex] = selectionInfo.positionAtStartOfDrag;
                Undo.RecordObject(platformController, "Move point");
                platformController.waypoints[selectionInfo.pointIndex] = mousePosition;

                selectionInfo.pointIsSelected = false;
                selectionInfo.pointIndex = -1;
                needsRepaint = true;
            }
        }

        void HandleLeftMouseDrag(Vector3 mousePosition)
        {
            if (selectionInfo.pointIsSelected)
            {
                platformController.waypoints[selectionInfo.pointIndex] = mousePosition;
                needsRepaint = true;
            }
        }

        void UpdateMouseOverInfo(Vector3 currMousePosition)
        {
            int mouseOverPointIndex = -1;
            for (int i = 0; i < platformController.waypoints.Count; i++)
            {
                if (Vector3.Distance(currMousePosition, platformController.waypoints[i]) < platformController.handleRadius)
                {
                    mouseOverPointIndex = i;
                    break;
                }
            }

            if (mouseOverPointIndex != selectionInfo.pointIndex)
            {
                selectionInfo.pointIndex = mouseOverPointIndex;
                selectionInfo.mouseIsOverPoint = mouseOverPointIndex != -1;

                needsRepaint = true;
            }

            if (selectionInfo.mouseIsOverPoint)
            {
                selectionInfo.mouseIsOverLine = false;
                selectionInfo.lineIndex = -1;
            }
            else
            {
                int mouseOverLineIndex = -1;
                float closestLineDistance = platformController.handleRadius;
                for (int i = 0; i < platformController.waypoints.Count; i++)
                {
                    Vector3 nextPointInShape = platformController.waypoints[(i + 1) % platformController.waypoints.Count];
                    float dstFromMouseToLine = HandleUtility.DistancePointToLineSegment(currMousePosition, platformController.waypoints[i], nextPointInShape);
                    if (dstFromMouseToLine < closestLineDistance)
                    {
                        closestLineDistance = dstFromMouseToLine;
                        mouseOverLineIndex = i;
                    }
                }

                if (selectionInfo.lineIndex != mouseOverLineIndex)
                {
                    selectionInfo.lineIndex = mouseOverLineIndex;
                    selectionInfo.mouseIsOverLine = mouseOverLineIndex != -1;
                    needsRepaint = true;
                }
            }
        }

        Vector3 SnapToGrid(Vector3 position)
        {
            float snappedX = Mathf.Round(position.x / platformController.snappingSettings.x) * platformController.snappingSettings.x;
            float snappedY = Mathf.Round(position.y / platformController.snappingSettings.y) * platformController.snappingSettings.y;

            return new Vector3(snappedX, snappedY, position.z);
        }

        public void DrawText(string text, Vector3 worldPos, Vector2 screenOffset = default, Color? color = default, int alignment = 0)
        {
            Handles.BeginGUI();

            var restoreColor = GUI.color;
            if (color.HasValue) GUI.color = color.Value;

            var view = UnityEditor.SceneView.currentDrawingSceneView;
            var screenPos = view.camera.WorldToScreenPoint(worldPos);
            screenPos += new Vector3(screenOffset.x, screenOffset.y, 0f);

            if (screenPos.y < 0f || screenPos.y > Screen.height || screenPos.x < 0f || screenPos.x > Screen.width || screenPos.z < 0f)
            {
                GUI.color = restoreColor;

            }
            else
            {
                var size = GUI.skin.label.CalcSize(new GUIContent(text));

                if (alignment == 0)
                {
                    screenPos.x -= (size.x / 2f) + 1f;
                }
                else if (alignment < 0)
                {
                    screenPos.x -= size.x - 2f;
                }
                else
                {
                    screenPos.x -= 4f;
                }

                GUI.Label(new Rect(screenPos.x, -screenPos.y + view.position.height + 4f, size.x, size.y), text);
                GUI.color = restoreColor;

            }

            Handles.EndGUI();
        }

        public class SelectionInfo
        {
            public int pointIndex = -1;
            public bool mouseIsOverPoint = false;
            public bool pointIsSelected = false;
            public Vector3 positionAtStartOfDrag;

            public int lineIndex = -1;
            public bool mouseIsOverLine = false;
        }
    }
}