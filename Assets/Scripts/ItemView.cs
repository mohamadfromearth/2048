using DG.Tweening;
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

    public int Value { get; set; }

    public void SetData(ItemViewData viewData)
    {
        text.text = viewData.TextValue;
        spriteRenderer.sprite = viewData.Sprite;
        transform.position = viewData.Position;
    }

    public void SetText(string value) => text.text = value;

    public void SetPosition(Vector3 position) => transform.position = position;

    public void MoveTo(Vector3 position, float dur, TweenCallback complete = null)
    {
        transform.DOMove(position, dur).onComplete = complete;
    }

    public void SetScale(Vector3 scale) => transform.localScale = scale;

    public void ScaleTo(Vector3 scale, float duration) => transform.DOScale(scale, duration);
}