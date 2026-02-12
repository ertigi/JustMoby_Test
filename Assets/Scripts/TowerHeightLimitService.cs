using UnityEngine;

public sealed class TowerHeightLimitService
{
    public bool CanAddNext(RectTransform towerArea, RectTransform stackContainer, float nextCubeHeight)
    {
        var areaRect = towerArea.rect;

        float currentTopY = GetCurrentTopY(stackContainer);
        float nextTopY = currentTopY + nextCubeHeight;

        return nextTopY <= areaRect.yMax;
    }

    private float GetCurrentTopY(RectTransform stackContainer)
    {
        float top = 0f;
        for (int i = 0; i < stackContainer.childCount; i++)
        {
            var child = stackContainer.GetChild(i) as RectTransform;
            if (child == null)
                continue;

            float childTop = child.anchoredPosition.y + child.rect.height;
            if (childTop > top)
                top = childTop;
        }
        return top;
    }
}
