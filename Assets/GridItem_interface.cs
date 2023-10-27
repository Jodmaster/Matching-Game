
public interface GridItem_interface
{
    
    public enum gridItemType { Jewel, Blocker, Sand };
    public gridItemType itemType { get; }
    public int[,] posInGrid { get; }

}
