using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace priceSvg
{
    public class vertex
    {

        private int _vertexIdx;
        private double _coordX;
        private double _coordY;
        private double _coordZ;

        public vertex()
        { }

        public vertex(int vertexIdx, double coordX, double coordY, double coordZ)
        {
            _vertexIdx = vertexIdx;
            _coordX = coordX;
            _coordY = coordY;
            _coordZ = coordZ;
        }

        public int VertexIdx
        {
            get { return _vertexIdx; }
            set { _vertexIdx = value; }
        }

        public double X
        {
            get { return _coordX; }
            set { _coordX = value; }
        }

        public double Y
        {
            get { return _coordY; }
            set { _coordY = value; }
        }

        public double Z
        {
            get { return _coordZ; }
            set { _coordZ = value; }
        }



    }
}