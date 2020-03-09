using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Grid
{
    public class GridMovableNode
    {
        public int XValue, ZValue;

        public GridMovableNode UpperNeighbor, LowerNeighbor, LeftNeighbor, RightNeighbor;

        public GridMovableNode(int xVal, int zVal)
        {
            XValue = xVal;
            ZValue = zVal;
        }

        public void MatchUpNeighbors(GridMovableNode queryNode)
        {
            // Try to match up the node used for this method call as a neighbor of this node and vice versa
            if (this.RightNeighbor == null && queryNode.XValue == this.XValue + 1)
            {
                this.RightNeighbor = queryNode;
                queryNode.LeftNeighbor = this;
            }

            if (this.LeftNeighbor == null && queryNode.XValue == this.XValue - 1)
            {
                this.LeftNeighbor = queryNode;
                queryNode.RightNeighbor = this;
            }

            if (this.LowerNeighbor == null && queryNode.ZValue == this.ZValue + 1)
            {
                this.LowerNeighbor = queryNode;
                queryNode.UpperNeighbor = this;
            }

            if (this.UpperNeighbor == null && queryNode.ZValue == this.ZValue - 1)
            {
                this.UpperNeighbor = queryNode;
                queryNode.LowerNeighbor = this;
            }
        }
    }
}
