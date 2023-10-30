using UnityEngine;

public class Rule_checks : MonoBehaviour
{
    public bool canSwap;
    public Game_manager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponent<Game_manager>();
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

        Debug.Log("pos is: " + cell.position[0] + ", " + validCols[0]);
        

        validCells[0] = manager.getCellAtPosition(0,0);
        

        return validCells;

        int[] checkRows(Cell celly)
        {
            int rowPos = celly.position[0];
            int[] validRowPos = new int[2];

            int[] possibleRowPos = new int[2]{rowPos + 1, rowPos -1};

            for (int i = 0; i < possibleRowPos.Length; i++){
                if(possibleRowPos[i] > 0){
                    validRowPos[i] = possibleRowPos[i];
                }
            }

            return validRowPos;
        }

        int[] checkCols(Cell celly)
        {
            int colPos = celly.position[1];
            int[] validColPos = new int[2];

            int[] possibleColPos = new int[2]{colPos + 1, colPos - 1 };


            for (int i = 0; i < possibleColPos.Length; i++)
            {
                if (possibleColPos[i] > 0)
                {
                    validColPos[i] = possibleColPos[i];
                }
            }

            return validColPos;
        }

    }
}