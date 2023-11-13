using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bomb : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{

    private Canvas canvas;
    private RectTransform rectTrans;
    private CanvasGroup group;

    void Start() {
        canvas = FindObjectOfType<Canvas>();
        group = GetComponent<CanvasGroup>();
        rectTrans = GetComponent<RectTransform>();
    }


    public void OnBeginDrag(PointerEventData eventData) {
        group.alpha = 0.5f;
        group.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) {
        //gets current mouse pos and then sets the bomb to that position accounting for the scale factor of the canvas
        Vector3 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 posToMove = new Vector3(currentMousePos.x, currentMousePos.y, -0.15f);

        rectTrans.position = posToMove / canvas.scaleFactor;
    }

    
    public void OnEndDrag(PointerEventData eventData) {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1000, LayerMask.GetMask("Cell"));

        if(hit != false) {
            Vector3 cellPos = hit.transform.position;
            Vector3 posToGoTo = new Vector3(cellPos.x, cellPos.y, -0.15f);

            rectTrans.position = posToGoTo;

        } else{ Destroy(this.gameObject); }

    }
}
