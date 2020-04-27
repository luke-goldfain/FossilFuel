using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Grid
{
    public class GridMovableNode
    {
        public int XValue, ZValue;

        public GridMovableNode UpperNeighbor, LowerNeighbor, LeftNeighbor, RightNeighbor;

        public List<GridMovableNode> AllNeighborNodes;

        public GridMovableNode(int xVal, int zVal)
        {
            XValue = xVal;
            ZValue = zVal;

            AllNeighborNodes = new List<GridMovableNode>();
        }

        public void MatchUpNeighbors(GridMovableNode queryNode)
        {
            bool queryIsNeighbor = false;

            // Try to match up the node used for this method call as a neighbor of this node and vice versa
            if (this.RightNeighbor == null && queryNode.XValue == this.XValue + 1 && queryNode.ZValue == this.ZValue)
            {
                this.RightNeighbor = queryNode;
                //queryNode.LeftNeighbor = this;
                if (!AllNeighborNodes.Contains(RightNeighbor)) AllNeighborNodes.Add(RightNeighbor);
                queryIsNeighbor = true;
            }

            if (this.LeftNeighbor == null && queryNode.XValue == this.XValue - 1 && queryNode.ZValue == this.ZValue)
            {
                this.LeftNeighbor = queryNode;
                //queryNode.RightNeighbor = this;
                if (!AllNeighborNodes.Contains(LeftNeighbor)) AllNeighborNodes.Add(LeftNeighbor);
                queryIsNeighbor = true;
            }

            if (this.LowerNeighbor == null && queryNode.ZValue == this.ZValue + 1 && queryNode.XValue == this.XValue)
            {
                this.LowerNeighbor = queryNode;
                //queryNode.UpperNeighbor = this;
                if (!AllNeighborNodes.Contains(LowerNeighbor)) AllNeighborNodes.Add(LowerNeighbor);
                queryIsNeighbor = true;
            }

            if (this.UpperNeighbor == null && queryNode.ZValue == this.ZValue - 1 && queryNode.XValue == this.XValue)
            {
                this.UpperNeighbor = queryNode;
                //queryNode.LowerNeighbor = this;
                if (!AllNeighborNodes.Contains(UpperNeighbor)) AllNeighborNodes.Add(UpperNeighbor);
                queryIsNeighbor = true;
            }

            //if (queryIsNeighbor && !queryNode.AllNeighborNodes.Contains(this)) queryNode.AllNeighborNodes.Add(this);
        }
    }
}
