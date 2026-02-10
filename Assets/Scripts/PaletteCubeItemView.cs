using UnityEngine;
using UnityEngine.UI;

public sealed class PaletteCubeItemView : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private RectTransform _rect;

    private CubeDescriptor _descriptor;
    private int _index;

    public void Bind(int index, CubeDescriptor descriptor)
    {
        _index = index;
        _descriptor = descriptor;

        _image.sprite = descriptor.Sprite;
    }
}