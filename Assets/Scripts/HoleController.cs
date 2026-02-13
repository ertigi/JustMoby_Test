using UnityEngine;

public sealed class HoleController
{
    private readonly RectTransform _holeRect;
    private readonly EllipseHitTestService _ellipseHitTest;

    public HoleController(SceneReferences sceneReferences, EllipseHitTestService ellipseHitTest)
    {
        _holeRect = sceneReferences.HoleRect;
        _ellipseHitTest = ellipseHitTest;
    }

    public bool IsDroppedIntoHole(Vector2 screenPoint)
    {
        return _ellipseHitTest.ContainsPoint(_holeRect, screenPoint);
    }
}
