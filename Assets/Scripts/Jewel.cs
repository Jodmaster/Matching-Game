
using UnityEngine;
using static GridItem_interface;

public class Jewel : MonoBehaviour, GridItem_interface
{
    gridItemType GridItem_interface.itemType => gridItemType.Jewel;
    
    public Color jewelColor;
    public SpriteRenderer rend;
    public Cell currentParent;

    // Start is called before the first frame update
    void Start()
    {
        Cell currentParent = GetComponentInParent<Cell>();
        this.name = "Jewel_" + currentParent.cellNumber;
        rend = GetComponent<SpriteRenderer>();
        rend.color = jewelColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //raycasts down to the next cell to see if it should fall down to the next cell
    public bool checkJewelBelow() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, LayerMask.GetMask("Jewel"));

        if(hit.collider != null) {
            Debug.Log("Collider: " + hit.collider.gameObject.name); 
            Debug.Log("False");
            return false;
        } else { Debug.Log("True"); return true; }
    }
}
