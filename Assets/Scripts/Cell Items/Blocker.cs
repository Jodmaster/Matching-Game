
using UnityEngine;
using static IGridItem;

public class Blocker : MonoBehaviour, IGridItem
{
    public gridItemType itemType => gridItemType.Blocker;

}
