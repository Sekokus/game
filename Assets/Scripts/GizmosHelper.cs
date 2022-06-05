using System.Collections.Generic;
using UnityEngine;

public static class GizmosHelper
{
    private static readonly Stack<Color> ColorHistory = new Stack<Color>();
    private static readonly Stack<Matrix4x4> MatrixHistory = new Stack<Matrix4x4>();

    public static void PushColor(Color color)
    {
        ColorHistory.Push(Gizmos.color);
        Gizmos.color = color;
    }
        
    public static void PushMatrix(Matrix4x4 matrix)
    {
        MatrixHistory.Push(Gizmos.matrix);
        Gizmos.matrix = matrix;
    }
        
    public static void PopColor()
    {
        Gizmos.color = ColorHistory.Pop();
    }
        
    public static void PopMatrix()
    {
        Gizmos.matrix = MatrixHistory.Pop();
    }
}