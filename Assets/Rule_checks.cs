using UnityEngine;

public class Rule_checks : MonoBehaviour
{
    public bool canSwap;
    public Game_manager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<Game_manager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool canSwapJewels(Cell cell1, Cell cell2)
    {
        Cell[] potentialSwaps = validCellSwaps(cell1);
        return false;
    }

    public Cell[] validCellSwaps(Cell cell) {

        //intialises return array
        Cell[] validCells = new Cell[4];

        //arrays for checking the rows and columns
        int[] validRows = checkRows(cell);
        int[] validCols = checkCols(cell);

        //gets the valid swap cells
        validCells[0] = manager.getCellAtPosition(cell.position[0], validCols[0]);
        validCells[1] = manager.getCellAtPosition(cell.position[0], validCols[1]);
        validCells[2] = manager.getCellAtPosition(validRows[0], cell.position[1]);
        validCells[3] = manager.getCellAtPosition(validRows[1], cell.position[1]);

        return validCells;

        int[] checkRows(Cell celly)
        {

            int[] validRowPos = new int[2];

            //gets the possible rows 
            int[] possibleRowPos = new int[2]{ celly.position[0] + 1, celly.position[0] - 1 };

            //checks that the possible rows are valid and then adds them to the validRows array
            for (int i = 0; i < possibleRowPos.Length; i++){
                if(possibleRowPos[i] > 0 || possibleRowPos[i] < manager.numOfRows){
                    validRowPos[i] = possibleRowPos[i];
                }
            }
            
            //returns the array
            return validRowPos;
        }

        int[] checkCols(Cell celly)
        {
            int[] validColPos = new int[2];

            //gets the possible cols
            int[] possibleColPos = new int[2]{celly.position[1] + 1, celly.position[1] - 1};

            //checks that the possible cols are valid and then adds them to validCols array
            for (int i = 0; i < possibleColPos.Length; i++){
                if (possibleColPos[i] > 0 || possibleColPos[i] < manager.numOfCols){
                    validColPos[i] = possibleColPos[i];
                }
            }

            //returns the array
            return validColPos;
        }
    }
}