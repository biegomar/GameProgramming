using System.Runtime.InteropServices;

namespace CellularAutomata.MaterialFlow;

[StructLayout(LayoutKind.Sequential, Size = 32)]
public record struct MaterialMovement(Material Source, Material? Destination);