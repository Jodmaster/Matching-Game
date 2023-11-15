using UnityEngine;
using UnityEngine.EventSystems;
using static IUsableItem;

public class Colour_Bomb : MonoBehaviour, IUsableItem, IDragHandler, IBeginDragHandler, IEndDragHandler {

    itemType type => itemType.Color;
    
    public RectTransform trans => GetComponent<RectTransform>();

    private Canvas canvas;
    private CanvasGroup group;
    private Game_manager manager;

    // Start is called before the first frame update
    void Start() {
        canvas = FindObjectOfType<Canvas>();
        manager = FindObjectOfType<Game_manager>();

        group = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void OnBeginDrag(PointerEventData eventData) {
        //makes the dragged sprite slightly transparent and allows raycasts to go through the item
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
        
        //Ray casts for a cell beneath the bomb
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 1000, LayerMask.GetMask("Cell"));

        //if found a cell, check if the cell contains a jewel if yes, set it as parent and match transform
        //else destroy this gameobject
        if(hit != false) {
            Vector3 cellPos = hit.transform.position;
            Vector3 posToGoTo = new Vector3(cellPos.x, cellPos.y, -0.15f);
            group.alpha = 1f;

            Cell cell = hit.transform.GetComponent<Cell>();
            Debug.Log(cell.name);

            trans.position = posToGoTo;

            //we check if there's a jewel 
            if(cell.GetComponentInChildren<Jewel>() != null ) {
                transform.SetParent(cell.GetComponentInChildren<Jewel>().transform);
                GetComponentInParent<Jewel>().setUsableItem(this);
                manager.colorBombsUsed++;
            } else { Destroy(gameObject); }

        } else { Destroy(gameObject); }
   
    }
   
}
