using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Grid
{
    public interface IGridManager
    {
        int gridDepth { get; set; }
        int gridWidth { get; set; }

        float gridPieceD { get; set; }
        float gridPieceW { get; set; }
    }
}
