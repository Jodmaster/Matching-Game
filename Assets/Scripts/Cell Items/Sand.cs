
using UnityEngine;
using static IGridItem;

public class Sand : MonoBehaviour, IGridItem
{
    gridItemType IGridItem.itemType => gridItemType.Sand;

    public Cell currentParent;
    Game_manager manager;

    //for tracking which direction the sand should fall
    public bool fallLeft = false;
    public bool fallRight = false;
    public bool fallDown = false;

    // Start is called before the first frame update
    void Start()
    {
        //get parent; set name for debugging purposes
        currentParent = transform.parent.gameObject.GetComponent<Cell>();
        name = "sand_" + currentParent.cellNumber;

        manager = FindObjectOfType<Game_manager>();
    }

    // Update is called once per frame
    void Update()
    {       
        //if should fall is true and it's not already in the array add it to the sand to fall array
        if(shouldFall() && !manager.sandToFall.Contains(this)) { 
            manager.sandToFall.Add(this);
        }
        
    }


    public bool shouldFall() {
        if(!manager.isLerping) {
            //offsets for the origin of the raycast that the sand does
            Vector3 downOffset = new Vector3(0, 1, 0);
            Vector3 diagonalOffset = new Vector3(1.5f, 0, 0);

            //checks for jewels 
            RaycastHit2D leftJewelCheck = Physics2D.Raycast(transform.position - diagonalOffset, Vector2.down, 2f, LayerMask.GetMask("Jewel"));
            RaycastHit2D rightJewelCheck = Physics2D.Raycast(transform.position + diagonalOffset, Vector2.down, 2f, LayerMask.GetMask("Jewel"));
            RaycastHit2D downJewelCheck = Physics2D.Raycast(transform.position - downOffset, Vector2.down, 1f, LayerMask.GetMask("Jewel"));

            //checks for sand
            RaycastHit2D leftSandCheck = Physics2D.Raycast(transform.position - diagonalOffset, Vector2.down, 2f, LayerMask.GetMask("Sand"));
            RaycastHit2D rightSandCheck = Physics2D.Raycast(transform.position + diagonalOffset, Vector2.down, 2f, LayerMask.GetMask("Sand"));
            RaycastHit2D downSandCheck = Physics2D.Raycast(transform.position - downOffset, Vector2.down, 1f, LayerMask.GetMask("Sand"));

            //checks for blockers
            RaycastHit2D leftBlockerCheck = Physics2D.Raycast(transform.position - diagonalOffset, Vector2.down, 2f, LayerMask.GetMask("Blocker"));
            RaycastHit2D rightBlockerCheck = Physics2D.Raycast(transform.position + diagonalOffset, Vector2.down, 2f, LayerMask.GetMask("Blocker"));
            RaycastHit2D downBlockerCheck = Physics2D.Raycast(transform.position - downOffset, Vector2.down, 1f, LayerMask.GetMask("Blocker"));

            /**
             * Tried a nicer for loop implementation but broke it might be worth another try but this works for now  
            */

            //if a route was found make the appropriate bool
            if(!downSandCheck && !downJewelCheck && !downBlockerCheck) { fallDown = true; } else { fallDown = false; }
            if(!rightSandCheck && !rightJewelCheck && !rightBlockerCheck) { fallRight = true; } else { fallRight = false; }
            if(!leftSandCheck && !leftJewelCheck && !leftBlockerCheck) { fallLeft = true; } else { fallLeft = false; }

            if(fallDown || fallRight || fallLeft) { return true; } else { return false; }
        } return false;
    }
}
