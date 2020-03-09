using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Grid
{
    public class GridMovableNode
    {
        public GridMovableNode UpperNeighbor, LowerNeighbor, LeftNeighbor, RightNeighbor;

        public void MatchUpNeighbors()
        {
            if (this.UpperNeighbor != null && this.UpperNeighbor.LowerNeighbor == null)
            {
                this.UpperNeighbor.LowerNeighbor = this;
            }

            if (this.LowerNeighbor != null && this.LowerNeighbor.UpperNeighbor == null)
            {
                this.LowerNeighbor.UpperNeighbor = this;
            }

            if (this.LeftNeighbor != null && this.LeftNeighbor.RightNeighbor == null)
            {
                this.LeftNeighbor.RightNeighbor = this;
            }

            if (this.RightNeighbor != null && this.RightNeighbor.LeftNeighbor == null)
            {
                this.RightNeighbor.LeftNeighbor = this;
            }
        }
    }
}
