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
        return false;
    }

    public Cell[] validCellSwaps(Cell cell) {

        Cell[] validCells = new Cell[4];

        int[] validRows = checkRows(cell);
        int[] validCols = checkCols(cell);

        validCells[0] = manager.getCellAtPosition(cell.position[0], validCols[0]);
        validCells[1] = manager.getCellAtPosition(cell.position[0], validCols[1]);
        validCells[2] = manager.getCellAtPosition(validRows[0], cell.position[1]);
        validCells[3] = manager.getCellAtPosition(validRows[1], cell.position[1]);

        for(int i = 0; i < validCells.Length; i++) {
            Debug.Log("valid cell " + i + " : " + validCells[i]);
        }
        

        return validCells;

        int[] checkRows(Cell celly)
        {

            //gets the row position of the passed cell
            int rowPos = celly.position[0];
            int[] validRowPos = new int[2];

            //gets the possible rows 
            int[] possibleRowPos = new int[2]{rowPos + 1, rowPos -1};

            //checks that the possible rows are valid and then adds them to the validRows array
            for (int i = 0; i < possibleRowPos.Length; i++){
                if(possibleRowPos[i] > 0){
                    validRowPos[i] = possibleRowPos[i];
                }
            }
            
            //returns the array
            return validRowPos;
        }

        int[] checkCols(Cell celly)
        {
            //gets the col position of the passed dcell
            int colPos = celly.position[1];
            int[] validColPos = new int[2];

            //gets the possible cols
            int[] possibleColPos = new int[2]{colPos + 1, colPos - 1 };

            //checks that the possible cols are valid and then adds them to validCols array
            for (int i = 0; i < possibleColPos.Length; i++){
                if (possibleColPos[i] > 0){
                    validColPos[i] = possibleColPos[i];
                }
            }

            //returns the array
            return validColPos;
        }

    }
}