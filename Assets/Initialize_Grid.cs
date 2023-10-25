using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Awake()
    {
        numOfRows = manager.numOfRows;
        numOfCols = manager.numOfCols;

        //gets the original transform of the empty controlling grid intialization
        trans = GetComponent<Transform>();
        OriginalTrans = trans.position;
        currentSpawnPos = trans;   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GridInitilization()
    {
        //loops through each row and offsets the new spawn position each time 
        for (int rowCount = 0; rowCount < numOfRows; rowCount++)
        {
            for (int columnCount = 0; columnCount < numOfCols - 1; columnCount++)
            {
                //instantiates the cell prefab and then adjusts spawn point
                Instantiate(cellPrefab, currentSpawnPos.position, trans.rotation);
                currentSpawnPos.position = new Vector3(currentSpawnPos.position.x + columnGap, currentSpawnPos.position.y, 0);
            }

            //moves up a row 
            Instantiate(cellPrefab, currentSpawnPos.position, trans.rotation);
            currentSpawnPos.position = new Vector3(OriginalTrans.x, currentSpawnPos.position.y + rowGap, 0);
        }
    }
}
