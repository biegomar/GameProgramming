using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mazes.Contracts
{
    public interface IMazeGenerator
    {
        public Cell[,] Generate(Cell[,] rawMaze);
    }
}
