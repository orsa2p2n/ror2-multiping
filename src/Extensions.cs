using System.Reflection;
using UnityEngine;

namespace Multiping;

public static class Extensions
{
    public static Color ToColor(this string colorString)
    {
        ColorUtility.TryParseHtmlString(colorString, out Color color);

        return color;
    }
        
    public static bool IsObjectVisible(this Camera camera, Transform targetObjectTransform)
    {
        Vector3 viewPos = camera.WorldToViewportPoint(targetObjectTransform.position);

        return ((viewPos.x is >= 0 and <= 1) && (viewPos.y is >= 0 and <= 1) && viewPos.z > 0);
    }
}