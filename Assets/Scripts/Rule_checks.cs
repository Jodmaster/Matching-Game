using System.IO.IsolatedStorage;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;

public class Rule_checks : MonoBehaviour
{
    public bool canSwap;
    public bool canEliminate;
    
    public Game_manager manager;

    public bool colElim;
    public bool rowElim;
    public bool squareElim;

    public Cell[] threeInCol;
    public Cell[] threeInRow;
    public Cell[] sqaure;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindObjectOfType<Game_manager>();
        canSwap = false;
        canEliminate = false;
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

        //gets the cells that should be eliminated in a col or row around the origin and puts them in cell arrays for processing 
        int unbrokenStreak = 0;
        threeInCol = checkCanEliminateColumn(); 
        threeInRow = checkCanEliminateRow(); 

        void getColumn() {
            for(int i = 0; i < colToCheck.Length; i++) {
                colToCheck[i] = manager.getCellAtPosition(originCell.position[0] + i - 2, originCell.position[1]);
            }
        }

        void getRow() {
            for(int i = 0; i < rowToCheck.Length; i++) {
                rowToCheck[i] = manager.getCellAtPosition(originCell.position[0], originCell.position[1] + i - 2);
            }
        }

        Cell[] checkCanEliminateColumn() {

            //unbroken streak tracks how many cells of the right color have been found in a row 
            unbrokenStreak = 0;
            colElim = false;
            threeInCol = new Cell[3];

            //whole loop goes through colToCheck makes sure it's not an empty square and that it contains a jewel
            for(int i = 0; i < colToCheck.Length; i++) {
                if(colToCheck[i] != null) {
                    if(colToCheck[i].GetComponentInChildren<Jewel>() != null) {

                        //checks if the right color if yes then add it to the three in a row array and increase the unbroken streak counter
                        if(colToCheck[i].transform.GetChild(0).GetComponent<Jewel>().jewelColor == originJewel) {
                            threeInCol[unbrokenStreak] = colToCheck[i];

                            unbrokenStreak++;
                        } else {
                            //clears the three in row if there's a break in colors and resets the unbroken streak
                            for(int x = 0; x < threeInCol.Length; x++) { threeInCol[x] = null; }
                            unbrokenStreak = 0;
                        }
                    } else { unbrokenStreak = 0; }
                } else { unbrokenStreak = 0; }

                //if the unbroken streak reaches 3 break out of the loop 
                if(unbrokenStreak == 3) {
                    break;
                }
            }

            int threeInColCounter = 0;
            for(int i = 0; i < threeInCol.Length; i++) {
                if(threeInCol[i] != null) { threeInColCounter++; }
            }

            if(threeInColCounter == 3) { 
                canEliminate = true;
                colElim = true;
                return threeInCol; 
            } else { canEliminate = false; return null; }
        }

        Cell[] checkCanEliminateRow() {
            //resets unbroken streak for next four loop 
            unbrokenStreak = 0;
            rowElim = false;
            threeInRow = new Cell[3];

            for(int i = 0; i < rowToCheck.Length; i++) {
                if(rowToCheck[i] != null) {
                    if(rowToCheck[i].GetComponentInChildren<Jewel>() != null) {

                        //checks if the right color if yes then add it to the three in a col array and increase the unbroken streak counter
                        if(rowToCheck[i].transform.GetChild(0).GetComponent<Jewel>().jewelColor == originJewel) {
                            threeInRow[unbrokenStreak] = rowToCheck[i];
                            unbrokenStreak++;
                        } else {
                            //clears the three in row if there's a break in colors and resets the unbroken streak
                            for(int x = 0; x < threeInRow.Length; x++) { threeInRow[x] = null; }
                            unbrokenStreak = 0;
                        }
                    } else { unbrokenStreak = 0; }
                } else { unbrokenStreak = 0; }

                //if the unbroken streak reaches 3 break out of the loop 
                if(unbrokenStreak == 3) {
                    break;
                }
            }

            //eliminating columns takes priority over rows this can be changed //TODO play test and find out 
            if(!canEliminate) {
                int threeInRowCounter = 0;
                for(int i = 0; i < threeInRow.Length; i++) {
                    if(threeInRow[i] != null) { threeInRowCounter++; }
                }

                if(threeInRowCounter == 3) { canEliminate = true; } else { canEliminate = false; }
            }

            if(canEliminate) { rowElim = true; return threeInRow; } else { return null; }
        }
    }

    public Cell[] CheckSquare(Cell originCell){
        
        Cell[] squareToCheck = new Cell[9];
        
        int counter = 0;
        int colOffset = -1;
        int rowOffset = -1;
        

        for(int rowCount = 0; rowCount < 3; rowCount++) {
            for(int colCount = 0; colCount < 3; colCount++) {
                squareToCheck[counter] = manager.getCellAtPosition(originCell.position[0] + rowOffset, originCell.position[1] + (colOffset + colCount));
                counter++;
            }

            rowOffset++;
        }

        return (squareToCheck);
    }

    public Cell[] getSquareToEliminate(Cell originCell) {

        int[] startPoses = new int[4] {1, 2, 4, 5};
        Cell[] cellsToCheck = CheckSquare(originCell);
        Cell[] finalsquareCells = new Cell[4]; 

        for(int i = 0; i < 4; i++) {
            Cell[] current2by2 = getFourCells(startPoses[i]);
        }

        return finalsquareCells;

        Cell[] getFourCells(int startPos) {
            
            Cell[] currentSquare = new Cell[4];

            currentSquare[0] = cellsToCheck[startPos];
            currentSquare[1] = cellsToCheck[startPos + 1];
            currentSquare[2] = cellsToCheck[startPos + 3];
            currentSquare[3] = cellsToCheck[startPos + 4];

            return currentSquare;
        }
    }
}