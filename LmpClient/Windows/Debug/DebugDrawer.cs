﻿using UnityEngine;

namespace LmpClient.Windows.Debug
{
    public partial class DebugWindow
    {
        protected override void DrawWindowContent(int windowId)
        {
            GUI.DragWindow(MoveRect);

            ScrollPos = GUILayout.BeginScrollView(ScrollPos, GUILayout.Width(WindowWidth), GUILayout.Height(WindowHeight));
            DrawDebugButtons();
            GUILayout.EndScrollView();
        }

        private static void DrawDebugButtons()
        {
            GUILayout.BeginVertical();

            _displayFast = GUILayout.Toggle(_displayFast, "Fast debug update");

            _displayVectors = GUILayout.Toggle(_displayVectors, "Display vessel vectors");
            if (_displayVectors)
                GUILayout.Label(_vectorText);

            _displayPositions = GUILayout.Toggle(_displayPositions, "Display active vessel positions");
            if (_displayPositions)
                GUILayout.Label(_positionText);

            _displayVesselsPositions = GUILayout.Toggle(_displayVesselsPositions, "Display other vessel positions");
            if (_displayVesselsPositions)
                GUILayout.Label(_positionVesselsText);

            _displaySubspace = GUILayout.Toggle(_displaySubspace, "Display subspace statistics");
            if (_displaySubspace)
                GUILayout.Label(_subspaceText);

            _displayTimes = GUILayout.Toggle(_displayTimes, "Display time clocks");
            if (_displayTimes)
                GUILayout.Label(_timeText);

            _displayConnectionQueue = GUILayout.Toggle(_displayConnectionQueue, "Display connection statistics");
            if (_displayConnectionQueue)
                GUILayout.Label(_connectionText);

            GUILayout.EndVertical();
        }
    }
}
