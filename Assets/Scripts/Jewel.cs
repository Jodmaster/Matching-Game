
using UnityEngine;
using static GridItem_interface;

public class Jewel : MonoBehaviour, GridItem_interface
{
    gridItemType GridItem_interface.itemType => gridItemType.Jewel;
    
    public Color jewelColor;
    public SpriteRenderer rend;
    public Cell currentParent;
    
    public bool jewelBelow;

    public LayerMask jewelLayer;

    // Start is called before the first frame update
    void Start()
    {
        jewelLayer = LayerMask.GetMask("Jewel");

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
    public void checkJewelBelow() {
        
        //casts to cell below and sees how many colliders it hits 
        //needs to be done as an array because the ray will hit the origin jewel 
        RaycastHit2D[] hit = Physics2D.RaycastAll(this.transform.position, Vector2.down, 1.5f, LayerMask.GetMask("Jewel"));

        //checks to see if there's more than one collider in the hits if yes there is a jewel below otherwise there is not 
        if(hit.Length > 1) {
            jewelBelow = true;
        } else {
            jewelBelow = false;
        }
    }
}
