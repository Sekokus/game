using System;
using UnityEngine;

namespace Sekokus
{
    public static class GizmosHelper
    {
        public static void DrawWithColor(Color color, Action drawAction)
        {
            var previousColor = Gizmos.color;
            Gizmos.color = color;

            drawAction();

            Gizmos.color = previousColor;
        }
    }
}