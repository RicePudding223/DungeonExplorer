namespace DungeonExplorer
{
    /// <summary>
    /// Represents a base class for all items in the game.
    /// This class stores the common property `Name` for all items.
    /// </summary>
    public abstract class Item
    {
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the Item class with the specified name.
        /// </summary>
        /// <param name="name"> The name of the item.</param>
        public Item(string name)
        {
            Name = name;
        }
    }
}
