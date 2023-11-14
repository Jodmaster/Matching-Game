using UnityEngine;
using UnityEngine.EventSystems;

public interface IUsableItem 
{
    public enum itemType {Bomb, Concretion, Fragile, Color};

    public RectTransform trans { get; }

    public void OnBeginDrag(PointerEventData eventData) { }

    public void OnDrag(PointerEventData eventData) { }

    public void OnEndDrag(PointerEventData eventData) { }
}
