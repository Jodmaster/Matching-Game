using System.IO.IsolatedStorage;
using UnityEngine;

public class Rule_checks : MonoBehaviour
{
    public bool canSwap;
    public Game_manager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<Game_manager>();
        canSwap = false;
    }

    public void canSwapJewels(Cell cell1, Cell cell2)
    {
        Cell[] potentialSwaps = validCellSwaps(cell1);

        for(int i = 0; i < potentialSwaps.Length; i++) {
            if(potentialSwaps[i] == cell2) {
                canSwap = true;
            }
        }


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

    public void CheckThreeInARow(Cell originCell) {

        //getting origin jewel info
        Color originJewel = originCell.transform.GetChild(0).GetComponent<Jewel>().jewelColor;
        
        //arrays to hold all the cells to check
        Cell[] colToCheck = new Cell[5];
        Cell[] rowToCheck = new Cell[5];

        //gets the potential cells on the cols and rows that should be searched for 3s
        getColumn();
        getRow();

        //unbroken streak tracks how many cells of the right color have been found in a row 
        int unbrokenStreak = 0;
        Cell[] threeInCol = new Cell[3];
        
        //whole loop goes through colToCheck makes sure it's not an empty square and that it contains a jewel
        for (int i = 0; i < colToCheck.Length; i++) {
           if(colToCheck[i] != null) {
                if (colToCheck[i].GetComponentInChildren<Jewel>() != null) { 
   
                    //checks if the right color if yes then add it to the three in a row array and increase the unbroken streak counter
                    if (colToCheck[i].transform.GetChild(0).GetComponent<Jewel>().jewelColor == originJewel) {
                        threeInCol[unbrokenStreak] = colToCheck[i];
                        
                        unbrokenStreak++;
                    } else {
                        //clears the three in row if there's a break in colors and resets the unbroken streak
                        for (int x = 0; x < threeInCol.Length; x++) { threeInCol[x] = null; }                      
                        unbrokenStreak = 0;
                    }
                } else { unbrokenStreak = 0;}
           } else { unbrokenStreak = 0;}

           //if the unbroken streak reaches 3 break out of the loop 
           if (unbrokenStreak == 3) {
                break;
           }
        }

        //resets unbroken streak for next four loop 
        unbrokenStreak = 0;
        Cell[] threeInRow = new Cell[3];

        for (int i = 0; i < rowToCheck.Length; i++) {
            if (rowToCheck[i] != null) {
                if (rowToCheck[i].GetComponentInChildren<Jewel>() != null) {

                    //checks if the right color if yes then add it to the three in a col array and increase the unbroken streak counter
                    if (rowToCheck[i].transform.GetChild(0).GetComponent<Jewel>().jewelColor == originJewel) {
                        threeInRow[unbrokenStreak] = rowToCheck[i];
                        unbrokenStreak++;
                    } else {
                        //clears the three in row if there's a break in colors and resets the unbroken streak
                        for (int x = 0; x < threeInRow.Length; x++) { threeInRow[x] = null; }
                        unbrokenStreak = 0;
                    }
                } else { unbrokenStreak = 0; }
            } else { unbrokenStreak = 0; }

            //if the unbroken streak reaches 3 break out of the loop 
            if (unbrokenStreak == 3) {
                break;
            }
        }

        for(int x = 0; x < threeInRow.Length; x++)
        {
            Debug.Log("Three in col " + x + ": " + threeInRow[x]);
        }

        void getColumn() {
            for (int i = 0; i < colToCheck.Length; i++) {
                colToCheck[i] = manager.getCellAtPosition(originCell.position[0] + i - 2, originCell.position[1]);
            }
        }

        void getRow() {
            for (int i = 0; i < rowToCheck.Length; i++) {
                rowToCheck[i] = manager.getCellAtPosition(originCell.position[0], originCell.position[1] + i - 2);
            }
        }
    }

    public void CheckSquare(Cell originCell){

    }
}