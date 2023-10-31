using UnityEngine;

public class Initialize_Grid : MonoBehaviour
{
    public Game_manager manager;

    [SerializeField] private float columnGap;
    [SerializeField] private float rowGap;

    public Cell cellPrefab;

    private Transform trans;
    private Vector3 OriginalTrans;
    private Transform currentSpawnPos;

    private int numOfRows;
    private int numOfCols;

    //using awake instead of start as the info is needed for filling in game_manager start()
    void Awake()
    {
        numOfRows = manager.numOfRows;
        numOfCols = manager.numOfCols;

        //gets the original transform of the empty controlling grid intialization
        trans = GetComponent<Transform>();
        OriginalTrans = trans.position;
        currentSpawnPos = trans;   
    }

    //sets up all the cells and adds grid items
    public void GridInitilization()
    {
        //needed for renaming the cells as they are created
        int cellNumber = 0;
        //col count is needed for assigning the cell its position 
        int colCount = 0;
        Cell newCell;

        //loops through each row and offsets the new spawn position each time 
        for (int rowCount = 0; rowCount < numOfRows; rowCount++){
            for (int columnCount = 0; columnCount < numOfCols - 1; columnCount++){
                
                //instantiates the cell prefab and then adjusts spawn point
                instantiateCell(rowCount, colCount);
                currentSpawnPos.position = new Vector3(currentSpawnPos.position.x + columnGap, currentSpawnPos.position.y, 0);
                colCount++;
            }

            //moves up a row 
            instantiateCell(rowCount, colCount);
            currentSpawnPos.position = new Vector3(OriginalTrans.x, currentSpawnPos.position.y + rowGap, 0);
            colCount = 0;
        }

        void instantiateCell(int row, int col)
        {   
            //instantiates the the cell and adds a cell number
            //renames the cell to make for easier debugging and developing 
            newCell = Instantiate(cellPrefab, currentSpawnPos.position, trans.rotation);
            newCell.setCellNumber(cellNumber);
            newCell.position = new int[] {row, col};
            newCell.name = "Cell_" + cellNumber;
            cellNumber++;
        }
    }
}
