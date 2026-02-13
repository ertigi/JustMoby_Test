using UnityEngine;

public sealed class EllipseHitTestService
{
    public bool ContainsPoint(RectTransform ellipseRect, Vector2 screenPoint)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(ellipseRect, screenPoint, null, out var local);

        var rect = ellipseRect.rect;
        float a = rect.width * 0.5f;
        float b = rect.height * 0.5f;

        float nx = local.x / a;
        float ny = local.y / b;
        return (nx * nx + ny * ny) <= 1f;
    }
}
