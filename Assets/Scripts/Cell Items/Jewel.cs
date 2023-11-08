
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static GridItem_interface;

public class Jewel : MonoBehaviour, GridItem_interface {
    gridItemType GridItem_interface.itemType => gridItemType.Jewel;

    public Game_manager manager;

    public Color jewelColor;
    public SpriteRenderer rend;
    public Cell currentParent;

    public LayerMask jewelLayer;

    // Start is called before the first frame update
    void Start() {
        jewelLayer = LayerMask.GetMask("Jewel");
        manager = FindObjectOfType<Game_manager>();

        currentParent = transform.parent.gameObject.GetComponent<Cell>();
        name = "Jewel_" + currentParent.GetComponent<Cell>().cellNumber;

        rend = GetComponent<SpriteRenderer>();
        if(jewelColor == Color.red) { rend.sprite = manager.redSprite; } 
        else if  (jewelColor == Color.blue) {rend.sprite =  manager.blueSprite;} 
        else if (jewelColor == Color.green) { rend.sprite = manager.greenSprite;}
        else { rend.color = jewelColor; }
    }

    // Update is called once per frame
    void Update() {
        if(checkJewelBelow() && !manager.shouldFall.Contains(this)) {
            manager.shouldFall.Add(this);
        }
    }

    //raycasts down to the next cell to see if it should fall down to the next cell
    public bool checkJewelBelow() {

        Vector3 originOffset = new Vector3(0, 1, 0);

        //casts to cell below and sees how many colliders it hits 
        //needs to be done as an array because the ray will hit the origin jewel 
        RaycastHit2D jewel_check = Physics2D.Raycast(transform.position - originOffset, Vector2.down, 0.5f, LayerMask.GetMask("Jewel"));
        RaycastHit2D block_check = Physics2D.Raycast(transform.position - originOffset, Vector2.down, 0.5f, LayerMask.GetMask("Blocker"));

        Debug.DrawRay(transform.position - originOffset, Vector2.down, Color.red);

        if(!block_check){     
            if(!jewel_check){
                return true;
            } else { return false; }
        } else { return false; }
    }
      
}