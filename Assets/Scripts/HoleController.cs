using UnityEngine;

public sealed class HoleController
{
    private RectTransform _holeRect;
    private RectTransform _holeMaskRect;
    private EllipseHitTestService _ellipseHitTest;
    private HoleViewModel _holeViewModel;
    private GameConfigSO _config;

    public HoleController(
        SceneReferences sceneReferences, 
        EllipseHitTestService ellipseHitTest, 
        HoleViewModel holeViewModel,
        GameConfigSO gameConfigSO)
    {
        _holeRect = sceneReferences.HoleRect;
        _holeMaskRect = sceneReferences.HoleRect;
        _ellipseHitTest = ellipseHitTest;
        _holeViewModel = holeViewModel;
        _config = gameConfigSO;
    }

    public bool IsDroppedIntoHole(Vector2 screenPoint)
    {
        return _ellipseHitTest.ContainsPoint(_holeRect, screenPoint);
    }

    public void SpawnHoleCube(CubeDescriptor descriptor, Vector2 screenPoint)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_holeMaskRect, screenPoint, null, out var local);

        var data = new HoleCubeSpawnData(descriptor, _config.CubeSize, local);

        _holeViewModel.OnCubeSpawnRequested.OnNext(data);
    }
}
