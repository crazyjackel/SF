using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static class UIHelper
{
    public static Vector2 ToScreenPoint(this Vector3 vec)
    {
        var camera = Camera.main;
        var resolution = Screen.currentResolution;
        Vector2 vector = 2 * new Vector2(vec.x / resolution.width, 1 - (vec.y / resolution.height)) - Vector2.one;
        Vector2 vector2 = new Vector2(vector.x * camera.orthographicSize * camera.aspect, vector.y * camera.orthographicSize);
        Vector2 vector3 = vector2 + new Vector2(camera.transform.position.x, camera.transform.position.y);
        return vector3;
    }

    public static void SetBorderColor(this IStyle style, Color color)
    {
        style.borderBottomColor = color;
        style.borderTopColor = color;
        style.borderLeftColor = color;
        style.borderRightColor = color;
    }
}
