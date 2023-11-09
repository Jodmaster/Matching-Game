
using UnityEngine;
using static GridItem_interface;

public class Sand : MonoBehaviour, GridItem_interface
{
    gridItemType GridItem_interface.itemType => gridItemType.Sand;

    public Cell currentParent;
    Game_manager manager;

    public bool fallLeft;
    public bool fallRight;
    public bool fallDown;

    // Start is called before the first frame update
    void Start()
    {
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
        
        Vector3 sideOffset = new Vector3(2f, 0, 0);
        Vector3 downOffset = new Vector3(0, 1, 0);

        RaycastHit2D right = Physics2D.Raycast(transform.position + sideOffset, Vector2.left, 1.5f);
        RaycastHit2D left = Physics2D.Raycast(transform.position - sideOffset, Vector2.right, 1.5f);
        RaycastHit2D down = Physics2D.Raycast(transform.position - downOffset, Vector2.down, 1.5f);

        Debug.DrawRay(transform.position + sideOffset, Vector2.left, Color.red);
        Debug.DrawRay(transform.position - sideOffset, Vector2.right, Color.blue);
        Debug.DrawRay(transform.position - downOffset, Vector2.down, Color.green);

        if (!down) { fallDown = true; return true; }

        if (!left) {
            RaycastHit2D checkDown = Physics2D.Raycast(transform.position - sideOffset - downOffset, Vector2.down, 1.5f);
            if (!checkDown) {
                fallLeft = true;
                return true;
            }
        }

        if (!right) {
            RaycastHit2D checkDown = Physics2D.Raycast(transform.position - sideOffset - downOffset, Vector2.down, 1.5f);
            if (!checkDown) {
                fallRight = true;
                return true;
            }
        }

        return false;
    }
}
