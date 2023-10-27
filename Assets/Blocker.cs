using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocker : MonoBehaviour, GridItem_interface
{
    public GridItem_interface.gridItemType itemType => GridItem_interface.gridItemType.Blocker;
    public int[,] posInGrid => throw new System.NotImplementedException();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
