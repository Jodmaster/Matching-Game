using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static IUsableItem;

public class Bomb : MonoBehaviour, IUsableItem, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    itemType type => itemType.Bomb;

    public RectTransform trans => GetComponent<RectTransform>();

    private Canvas canvas;
    private CanvasGroup group;
    private Game_manager manager;

    void Start() {
        canvas = FindObjectOfType<Canvas>();
        manager = FindObjectOfType<Game_manager>();
        
        group = GetComponent<CanvasGroup>();        
    }

    void Update() {

    }

    public void OnBeginDrag(PointerEventData eventData) {
        group.alpha = 0.5f;
        group.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData) {
        //gets current mouse pos and then sets the bomb to that position accounting for the scale factor of the canvas
        Vector3 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 posToMove = new Vector3(currentMousePos.x, currentMousePos.y, -0.15f);

        trans.position = posToMove / canvas.scaleFactor;
    }
   
    public void OnEndDrag(PointerEventData eventData) {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1000, LayerMask.GetMask("Cell"));

        if(hit != false) {
            Vector3 cellPos = hit.transform.position;
            Vector3 posToGoTo = new Vector3(cellPos.x, cellPos.y, -0.15f);

            Cell cell = hit.transform.GetComponent<Cell>();

            trans.position = posToGoTo;

            if(cell.GetComponentInChildren<Jewel>() != null) {
                transform.SetParent(cell.GetComponentInChildren<Jewel>().transform);
                GetComponentInParent<Jewel>().setUsableItem(this);
            } else { Destroy(gameObject); }
            

        } else{ Destroy(gameObject); }

    }
}
