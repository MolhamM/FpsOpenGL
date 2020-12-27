using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

//include GLM library
using GlmNet;
namespace Graphics
{
    class Shape
    {
        #region VARIABLES
        uint vertixBufferID;
        uint elementBufferID;
        bool useElementBuffer = false;
        float[] vertixPoints;
        ushort[] indecies;
        int type;
        int step; // step is how many steps do you take to export one complete value like positions so If you have 3 positions then 3 colors your steps is 1 because you have block of positions(3 values)  and then ignore block of colors (3 values) and so on 
        int axis = 3; // normally is set by 3 axis for example 3 floats describe position(xyz) and same for color (rgb)

        List<mat4> fullMovment;
        mat4 modelMatrix;
        
        vec3 center;
        vec3 position;
        vec3 rotationAngel;
        vec3 scale;
        #endregion

        #region CALLBACKS
        public Shape(float[] vertixPoints,int step,int type)
        {
            this.vertixPoints = vertixPoints;
            this.type = type;
            this.step = step;

            Initialize();

            vertixBufferID = GPU.GenerateBuffer(vertixPoints);
            
            CalculateCenter();
            Renderer.AssignShape(this);
        }
        public Shape(float[] vertixPoints, int step, int type,ushort[] indecies)
        {
            this.vertixPoints = vertixPoints;
            this.type = type;
            this.step = step;

            Initialize();

            useElementBuffer = true;
            vertixBufferID = GPU.GenerateBuffer(vertixPoints);
            this.indecies = indecies;
            elementBufferID = GPU.GenerateElementBuffer(indecies);
            
            CalculateCenter();
            Renderer.AssignShape(this);
        }

        public void Update()
        {
            UpdateModelMatrix();
        }
        public void Draw(int shaderModelMatrix)
        {
            Gl.glUniformMatrix4fv(shaderModelMatrix, 1, Gl.GL_FALSE, modelMatrix.to_array());
            BufferNewShape();

            if (useElementBuffer)
            {
                Gl.glDrawElements(type,indecies.Length,Gl.GL_UNSIGNED_SHORT,IntPtr.Zero);
            }
            else
            {
                Gl.glDrawArrays(type, 0, (vertixPoints.Length/(axis+(axis*step))));
            }
        }
        #endregion

        #region PUBLIC METHODS
        public void Rotate(float angel,vec3 rotationValue)
        {
            rotationAngel.x += angel * rotationValue.x;
            rotationAngel.y += angel * rotationValue.y;
            rotationAngel.z += angel * rotationValue.z;
        }
        public void Translate(vec3 addedPosition)
        {
            position += addedPosition;
        }
        public void Scale(vec3 addedScale)
        {
            scale += addedScale;
        }
        #region GETTERS AND SETTERS
        
        public void SetPosition(vec3 newPosition)
        {
            position = newPosition;
        }
        public mat4 GetModelMatrix()
        {
            return modelMatrix;
        }
        public uint GetBufferID()
        {
            return vertixBufferID;
        }
        public vec3 GetCenter()
        {
            return center;
        }
        public vec3 GetPosition()
        {
            return position;
        }
        #endregion
        #endregion

        #region PRIVATE METHODS
        private void Initialize()
        {
            fullMovment = new List<mat4>();
            modelMatrix = new mat4(1);

            position = new vec3(0, 0, 0);
            rotationAngel = new vec3(0, 0, 0);
            scale = new vec3(1, 1, 1);
        }
        private void UpdateModelMatrix()
        {
            //SetScale
            fullMovment.Add(glm.translate(new mat4(1), -1 * center));
            fullMovment.Add(glm.scale(new mat4(1), scale));
            fullMovment.Add(glm.translate(new mat4(1), center));
            // Set Rotation
            fullMovment.Add(glm.translate(new mat4(1), -1* center));
            fullMovment.Add(glm.rotate(rotationAngel.x, new vec3(1,0,0)));
            fullMovment.Add(glm.rotate(rotationAngel.y, new vec3(0,1,0)));
            fullMovment.Add(glm.rotate(rotationAngel.z, new vec3(0,0,1)));
            fullMovment.Add(glm.translate(new mat4(1), center));
            // Set Translation
            fullMovment.Add(glm.translate(new mat4(1), GetPosition()));

            modelMatrix = MathHelper.MultiplyMatrices(fullMovment);
            fullMovment.Clear();
        }
        private void CalculateCenter()
        {
            float averageX = 0;
            float averageY = 0;
            float averageZ = 0;
            
            if (useElementBuffer)
            {
                foreach(ushort i in indecies)
                {
                    int index;
                    if(i+2 < vertixPoints.Length)
                    {
                        index= i*( (step + 1)*axis ); 
                        averageX += vertixPoints[index];

                        index++;
                        averageY += vertixPoints[index];

                        index++;
                        averageZ += vertixPoints[index];
                    }
                }
            }
            else
            {

                for(int i = 0; i+2 < vertixPoints.Length; i += axis * (step+1))
                {
                    averageX += vertixPoints[i];
                    averageY += vertixPoints[i+1];
                    averageZ += vertixPoints[i+2];
                }

            }

            averageX /= (vertixPoints.Length / (axis + step * axis));
            averageY /= (vertixPoints.Length / (axis + step * axis));
            averageZ /= (vertixPoints.Length / (axis + step * axis));

            center = new vec3(averageX, averageY, averageZ);
        }
        private void BufferNewShape()
        {
            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, vertixBufferID);


            for (int i = 0; i < step + 1; i++)
            {
                Gl.glEnableVertexAttribArray(i);
                Gl.glVertexAttribPointer(i, axis, Gl.GL_FLOAT, Gl.GL_FALSE, axis * (step + 1) * sizeof(float), (IntPtr)(i * axis * sizeof(float)));
            }

        }
        #endregion
    }
}