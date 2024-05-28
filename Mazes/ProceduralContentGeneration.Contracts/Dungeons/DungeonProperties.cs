using Mazes.Contracts.Cells;

namespace Mazes.Contracts.Dungeons
{
    public struct DungeonProperties
    {
        public CellVector PlaygroundDimension { get; set; }
        public CellVector GridDimension { get; set; }
        public byte MaxGoneRooms { get; set; }

        public DungeonProperties(CellVector playgroundDimension, CellVector gridDimension, byte maxGoneRooms)
        {
            this.PlaygroundDimension = playgroundDimension;
            this.GridDimension = gridDimension;
            this.MaxGoneRooms = maxGoneRooms;
        }
    }
}