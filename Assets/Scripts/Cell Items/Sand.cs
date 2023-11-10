
using JetBrains.Annotations;
using UnityEditor;
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
        name = "sand_" + currentParent.cellNumber;

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

        Vector3 downOffset = new Vector3(0, 1, 0);
        Vector3 diagonalOffset = new Vector3(1.5f, 0, 0);

        Debug.DrawRay(transform.position + diagonalOffset, Vector2.down * 2, Color.yellow);

        RaycastHit2D leftJewelCheck = Physics2D.Raycast(transform.position - diagonalOffset, Vector2.down, 2f, LayerMask.GetMask("Jewel"));
        RaycastHit2D rightJewelCheck = Physics2D.Raycast(transform.position + diagonalOffset, Vector2.down, 2f, LayerMask.GetMask("Jewel"));
        RaycastHit2D downJewelCheck = Physics2D.Raycast(transform.position - downOffset, Vector2.down, 1f, LayerMask.GetMask("Jewel"));

        RaycastHit2D leftSandCheck = Physics2D.Raycast(transform.position - diagonalOffset, Vector2.down, 2f, LayerMask.GetMask("Sand"));
        RaycastHit2D rightSandCheck = Physics2D.Raycast(transform.position + diagonalOffset, Vector2.down, 2f, LayerMask.GetMask("Sand"));
        RaycastHit2D downSandCheck = Physics2D.Raycast(transform.position - downOffset, Vector2.down, 1f, LayerMask.GetMask("Sand"));

        RaycastHit2D leftBlockerCheck = Physics2D.Raycast(transform.position - diagonalOffset, Vector2.down, 2f, LayerMask.GetMask("Blocker"));
        RaycastHit2D rightBlockerCheck = Physics2D.Raycast(transform.position + diagonalOffset, Vector2.down, 2f, LayerMask.GetMask("Blocker"));
        RaycastHit2D downBlockerCheck = Physics2D.Raycast(transform.position - downOffset, Vector2.down, 1f, LayerMask.GetMask("Blocker"));

        if(!downSandCheck && !downJewelCheck && !downBlockerCheck) { fallDown = true; } else { fallDown = false; }
        if(!rightSandCheck && !rightJewelCheck && !rightBlockerCheck) { fallRight = true; } else { fallRight = false; }
        if(!leftSandCheck && !leftJewelCheck && !leftBlockerCheck) { fallLeft = true; } else { fallLeft = false; }

        if(fallDown || fallRight || fallLeft) { return true; } else { return false; }

    }
}
