
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using static GridItem_interface;

public class Sand : MonoBehaviour, GridItem_interface
{
    gridItemType GridItem_interface.itemType => gridItemType.Sand;

    public Cell currentParent;
    Game_manager manager;
    LayerMask layer;

    public bool fallLeft;
    public bool fallRight;
    public bool fallDown;

    // Start is called before the first frame update
    void Start()
    {
        layer = LayerMask.NameToLayer("Sand");
        currentParent = transform.parent.gameObject.GetComponent<Cell>();
        manager = FindObjectOfType<Game_manager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(shouldFall() && !manager.sandToFall.Contains(this)) {
            manager.sandToFall.Add(this);
        }
        
    }

    
    public bool shouldFall() {
        
        Vector3 sideOffset = new Vector3(2, 0, 0);
        Vector3 downOffset = new Vector3(0, 1, 0);
        Vector3 diagonalOffset = new Vector3(1.5f, 0, 0);

        LayerMask[] masks = new LayerMask[3] { LayerMask.GetMask("Jewel"), LayerMask.GetMask("Blocker"), LayerMask.GetMask("Sand") };

        for(int i = 0; i < masks.Length; i++) { Debug.Log("Mask is: " + LayerMask.LayerToName(masks[i])); }

        //down check 
        for(int i = 0; i < masks.Length; i++) {
            RaycastHit2D down = Physics2D.Raycast(transform.position - downOffset, Vector2.down, 1.5f, masks[i]);
            if(down) { fallDown = false; } else if (i == masks.Length - 1 && !down) { fallDown = true; }
        }

        //left check 
        for(int i = 0; i < masks.Length; i++) {
            
            RaycastHit2D left = Physics2D.Raycast(transform.position - sideOffset, Vector2.left, 1.5f, masks[i]);

            if(!left) {

                RaycastHit2D down = Physics2D.Raycast(transform.position - downOffset - diagonalOffset, Vector2.down, 1.5f, masks[i]);

                if(down) {
                    fallLeft = false;
                } else {
                    fallLeft = true;
                }

            } else { fallLeft = false; }

        }

        //right
        for(int i = 0; i < masks.Length; i++) {

            RaycastHit2D right = Physics2D.Raycast(transform.position + sideOffset, Vector2.right, 1.5f, masks[i]);

            if(!right) {

                RaycastHit2D down = Physics2D.Raycast(transform.position - downOffset + diagonalOffset, Vector2.down, 1.5f, masks[i]);

                if(down) {
                    fallRight = false;
                } else {
                    fallRight = true;
                }

            } else { fallRight = false; }

        }


        if(fallLeft || fallRight || fallDown) {
            Debug.Log("fallLeft " + fallLeft.ToString() + " fallright " + fallRight.ToString() + " fallDown " + fallDown.ToString());
            return true;
        }

        return false;
    }
    
}
