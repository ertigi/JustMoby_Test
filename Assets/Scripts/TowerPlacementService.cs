using UnityEngine;

public sealed class TowerPlacementService
{
    public Vector2 GetFirstPlacement(RectTransform towerArea, Vector2 screenPoint)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(towerArea, screenPoint, null, out Vector2 localPoint);
        return localPoint;
    }

    public Vector2 GetStackPlacementForPalette(RectTransform towerArea, Vector2 lastCubeAnchoredPosition, Vector2 screenPoint, Vector2 cubeSize)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(towerArea, screenPoint, null, out Vector2 localPoint);

        float x = localPoint.x;
        float y = lastCubeAnchoredPosition.y + cubeSize.y;

        return new Vector2(x, y);
    }

    public Vector2 GetStackPlacementForTower(Vector2 lastCubeAnchoredPosition, Vector2 currentCubeAnchoredPosition, Vector2 cubeSize)
    {
        float x = currentCubeAnchoredPosition.x;
        float y = lastCubeAnchoredPosition.y + cubeSize.y;

        return new Vector2(x, y);
    }

    public bool IsPointInsideArea(RectTransform towerArea, Vector2 screenPoint)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(towerArea, screenPoint);
    }

    public bool IsPointOverExistingStackForPaletteCube(RectTransform stackContainer, Vector2 lastCubeAnchoredPosition, Vector2 screenPoint, Vector2 cubeSize)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(stackContainer, screenPoint, null, out Vector2 localPoint);
        return Mathf.Abs(lastCubeAnchoredPosition.x - localPoint.x) <= cubeSize.x / 2f;
    }

    public bool IsPointOverExistingStackForTowerCube(Vector2 lastCubeAnchoredPosition, Vector2 currentCubeAnchoredPosition, Vector2 cubeSize)
    {
        return Mathf.Abs(lastCubeAnchoredPosition.x - currentCubeAnchoredPosition.x) <= cubeSize.x / 2f;
    }
}
