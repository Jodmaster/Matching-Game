public interface IGridItem
{
    public enum gridItemType { Jewel, Blocker, Sand };
    public gridItemType itemType { get; }
}
