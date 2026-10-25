using TMPro;
using UnityEngine;


public class ItemViewData
{
    public Sprite Sprite { get; set; }
    public string TextValue { get; set; }
    public Vector3 Position { get; set; }
}

public class ItemView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TextMeshPro text;

    public void SetData(ItemViewData viewData)
    {
        text.text = viewData.TextValue;
        spriteRenderer.sprite = viewData.Sprite;
        transform.position = viewData.Position;
    }

    public void SetText(string value) => text.text = value;
}