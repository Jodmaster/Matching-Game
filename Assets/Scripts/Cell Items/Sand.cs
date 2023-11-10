
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using static GridItem_interface;

public class Sand : MonoBehaviour, GridItem_interface
{
    gridItemType GridItem_interface.itemType => gridItemType.Sand;

    public Cell currentParent;
    Game_manager manager;
    LayerMask layer;

    public bool fallLeft = false;
    public bool fallRight = false;
    public bool fallDown = false;

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
        
        Vector3 sideOffset = new Vector3(1, 0, 0);
        Vector3 downOffset = new Vector3(0, 1, 0);
        Vector3 diagonalOffset = new Vector3(1.5f, 0, 0);

        int[] layermaskValues = new int[] { 3, 6, 8 };

        LayerMask[] masks = {LayerMask.NameToLayer("Sand"), LayerMask.NameToLayer("Blocker"), LayerMask.NameToLayer("Jewel") };

        Debug.DrawRay(transform.position + sideOffset, Vector2.right, Color.red);
        Debug.DrawRay(transform.position - sideOffset, Vector2.left, Color.red);
        Debug.DrawRay(transform.position - downOffset, Vector2.down, Color.blue);
        Debug.DrawRay(transform.position - downOffset + diagonalOffset, Vector2.down, Color.black);
        Debug.DrawRay(transform.position - downOffset - diagonalOffset, Vector2.down, Color.black);

        
        //down check 
        for (int i = 0; i < masks.Length; i++) {
            RaycastHit2D down = Physics2D.Raycast(transform.position - downOffset, Vector2.down, 1.5f, masks[i]);
            if (down) { fallDown = false ; break; } else { fallDown = true; }
        }

        //right
        for (int i = 0; i < masks.Length; i++) {

            RaycastHit2D right = Physics2D.Raycast(transform.position + sideOffset, Vector2.right, 1.5f, masks[i]);

            if (right) {fallRight = false; break; } else {
                RaycastHit2D down = Physics2D.Raycast(transform.position - downOffset + diagonalOffset, Vector2.down, 1.5f, masks[i]);
                if (down) { fallRight = false; break; } else { fallRight = true; }
            }
        }

        //left check 
        for (int i = 0; i < masks.Length; i++) {
            
            RaycastHit2D left = Physics2D.Raycast(transform.position - sideOffset, Vector2.left, 1.5f, masks[i]);

            if(left) { fallLeft = false; break; } else {
                RaycastHit2D down = Physics2D.Raycast(transform.position - downOffset - diagonalOffset, Vector2.down, 1.5f, masks[i]);
                if (down) { fallLeft = false; break; } else { fallLeft = true; }
            }
        }

        
        if(fallLeft || fallRight || fallDown) {
            Debug.Log("Sand" + name + "fallLeft: " + fallLeft.ToString() + ", fallright: " + fallRight.ToString() + ", fallDown: " + fallDown.ToString());
            return true;
        }

        return false;
    }
    
}
