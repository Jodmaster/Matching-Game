using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_1_setup : MonoBehaviour
{
    public int[,] itemToContain;

    public void Awake()
    {
        itemToContain = new int[6, 6] {
            {0,0,0,0,0,0},
            {0,0,1,0,0,0},
            {0,0,0,1,0,0},
            {0,0,0,0,0,0},
            {0,0,0,0,0,0},
            {0,0,1,0,0,0}
        };
    }
}

    
