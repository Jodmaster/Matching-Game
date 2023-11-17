
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static IGridItem;
using static IUsableItem;

public class Jewel : MonoBehaviour, IGridItem {
    gridItemType IGridItem.itemType => gridItemType.Jewel;
    public List<IUsableItem> usableItems = new List<IUsableItem>();

    public Game_manager manager;

    public Color jewelColor;
    public SpriteRenderer rend;
    public Cell currentParent;

    public Sprite sprite;
    public Sprite selectedSprite;

    public LayerMask jewelLayer;
    

    // Start is called before the first frame update
    void Start() {
        jewelLayer = LayerMask.GetMask("Jewel");
        manager = FindObjectOfType<Game_manager>();

        currentParent = transform.parent.gameObject.GetComponent<Cell>();
        name = "Jewel_" + currentParent.GetComponent<Cell>().cellNumber;

        //sets sprite based on jewel color
        rend = GetComponent<SpriteRenderer>();

        rend.sprite = sprite;
    }

    // Update is called once per frame
    void Update() {

        if(currentParent.isSelected) { rend.sprite = selectedSprite; } else { rend.sprite = sprite; }

        //checks if the jewel should fall and if it's already in the shouldfall array
        if(checkJewelBelow() && !manager.shouldFall.Contains(this)) {
            manager.shouldFall.Add(this);
        }

        //switches the position of items so they aren't overlayed on top of each other
        foreach(IUsableItem usableItem in usableItems) {

            switch(usableItems.Count) {
                case 1:
                    usableItems[0].trans.position = transform.position + new Vector3(-0.3f, 0.3f, 1f);                    
                    break;
                case 2:
                    usableItems[1].trans.position = transform.position + new Vector3(0.3f, 0.3f, 1f);
                    break;
                case 3:
                    usableItems[2].trans.position = transform.position + new Vector3(-0.3f, -0.3f, 1f);
                    break;
                case 4:
                    usableItems[3].trans.position = transform.position + new Vector3(0.3f, -0.3f, 1f);
                    break;
            }                           
        }
    }

    //raycasts down to the next cell to see if it should fall down to the next cell
    public bool checkJewelBelow() {

        Vector3 originOffset = new Vector3(0, 1, 0);

        //casts to cell below and sees how many colliders it hits 
        //needs to be done as an array because the ray will hit the origin jewel 
        RaycastHit2D jewel_check = Physics2D.Raycast(transform.position - originOffset, Vector2.down, 0.5f, LayerMask.GetMask("Jewel"));
        RaycastHit2D sand_check = Physics2D.Raycast(transform.position - originOffset, Vector2.down, 0.5f, LayerMask.GetMask("Sand"));
        RaycastHit2D block_check = Physics2D.Raycast(transform.position - originOffset, Vector2.down, 0.5f, LayerMask.GetMask("Blocker"));

        //if below it doesn't contain a blocker or jewel it should fall
        if (!jewel_check && !sand_check && !block_check) { return true; } 
        else { return false; }
    }
    
    public void setUsableItem(IUsableItem item) {
        usableItems.Add(item);
    }
}