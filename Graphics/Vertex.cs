using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//include GLM library
using GlmNet;

namespace Graphics
{
    class Vertex
    {
        public static int size = 3*3*sizeof(float);
        public static int attributesCount = 3;

        vec3 position;
        vec3 color;
        vec3 textCoord;
        public Vertex(vec3 position , vec3 color , vec3 textCoord)
        {
            this.position = position;
            this.color = color;
            this.textCoord = textCoord;
        }

        public vec3 Position   
        {
            get { return position; }   
            set { position = value; } 
        }
        public vec3 Color
        {
            get { return color; }
            set { color = value; }
        }
        public vec3 TextureCoordinate
        {
            get { return textCoord; }
            set { textCoord = value; }
        }
    }
}
