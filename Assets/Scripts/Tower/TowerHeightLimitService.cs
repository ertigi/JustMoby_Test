using UnityEngine;

public sealed class TowerHeightLimitService
{
    public bool IsWithinHeightLimit(RectTransform towerArea, Vector2 lastCubeAnchoredPosition, Vector2 cubeSize)
    {
        var areaRect = towerArea.rect;

        float topEdge = lastCubeAnchoredPosition.y + cubeSize.y / 2f;

        return topEdge <= areaRect.yMax;
    }

    public bool IsStackedAboveLast(RectTransform towerArea, Vector2 lastCubeAnchoredPosition, Vector2 newCubeScreenPosition, Vector2 cubeSize)
    {
        Vector3 lastCubeWorldPos = towerArea.TransformPoint(lastCubeAnchoredPosition);
        Vector3 lastCubeLocalInArea = towerArea.InverseTransformPoint(lastCubeWorldPos);

        float lastTopLocalY = lastCubeLocalInArea.y + cubeSize.y * 0.5f;

        RectTransformUtility.ScreenPointToLocalPointInRectangle( towerArea, newCubeScreenPosition, null, out Vector2 localPoint);
        localPoint.y -= cubeSize.y * 0.5f;

        return localPoint.y > lastTopLocalY;
    }
}
