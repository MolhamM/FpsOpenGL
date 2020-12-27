using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Tao.OpenGl;

//include GLM library
using GlmNet;

using System.IO;
using System.Diagnostics;

namespace Graphics
{
    
    class Renderer
    {
        static List<Shape> avialableShapes = new List<Shape>();
        static List<ShapeTextures> avialableShapes2 = new List<ShapeTextures>();
        Shader sh;

        ShapeTextures triangleShape;
        Shape xyzAxisShape;
        ShapeTextures cubeShape;
        Shape doubletriangle;
        ShapeTextures shapeTexture;
        ShapeTextures triangleWithTex;
        ShapeTextures trinagleWithTex2;
        //3D Drawing
        mat4 ViewMatrix;
        mat4 ProjectionMatrix;

        int ShaderModelMatrixID;
        int ShaderViewMatrixID;
        int ShaderProjectionMatrixID;

        Stopwatch timer = Stopwatch.StartNew();
        float deltaTime;
        public void Initialize()
        {
            string projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
            sh = new Shader(projectPath + "\\Shaders\\SimpleVertexShader.vertexshader", projectPath + "\\Shaders\\SimpleFragmentShader.fragmentshader");
            Gl.glClearColor(0, 0, 0.4f, 1);

            float[] cube =
            {
                1.0f , 1.0f , 1.0f, //0
                1.0f , 0.0f , 0.0f, //color
                -1.0f , 1.0f , 1.0f, //1
                0.0f , 1.0f , 0.0f, //color
                -1.0f , -1.0f , 1.0f,//2
                0.0f , 0.0f , 1.0f, // color
                1.0f , -1.0f , 1.0f,//3
                1.0f , 1.0f , 1.0f, //color

                1.0f , 1.0f , -1.0f,//4
                1.0f , 0.0f , 1.0f, // color
                -1.0f , 1.0f , -1.0f,//5
                0.0f , .5f , .2f, //color
                -1.0f , -1.0f , -1.0f,//6
                .8f , 0.6f , .4f, //color
                1.0f , -1.0f , -1.0f,//7
                .3f , 1.0f , .5f, // color


                1.0f , 1.0f , -1.0f,//8 dup of 4
                .2f , .5f , .2f, //color
                1.0f , 1.0f , 1.0f, // 9  duplicate 0
                .9f , .3f , .7f, // color
                1.0f , -1.0f , 1.0f, // 10 dup 3
                .3f , .7f , .5f, // color
                1.0f , -1.0f , -1.0f, // 11 dup 7
                .5f , .7f , .5f, //color

                -1.0f , 1.0f , -1.0f, //12 dup of 5
                .7f , .8f , .2f, // color
                -1.0f , 1.0f , 1.0f, //13 duplicate 1
                .5f , .7f , .3f, //color
                -1.0f , -1.0f , 1.0f, // 14 duplicate 2
                .4f , .7f , .7f, // color
                -1.0f , -1.0f , -1.0f, // 15 dup 6
                .2f , .5f , 1.0f,//color 

                1.0f , 1.0f , 1.0f, //16 duplicate 0
                .6f , 1.0f , .7f, //color
                1.0f , 1.0f , -1.0f, // 17 dup of 4
                .6f , .4f , .8f,//color
                -1.0f , 1.0f , -1.0f, //18 dup of 5
                .2f , .8f , .7f, //color
                -1.0f , 1.0f , 1.0f, // 19 duplicate 1
                .2f , .7f , 1.0f,//color

                1.0f , -1.0f , 1.0f, // 20 dup 3
                .8f , .3f , .7f, //color
                1.0f , -1.0f , -1.0f, //21 dup7
                .8f , .9f , .5f,//color
                -1.0f , -1.0f , -1.0f,  // 22 dup 6
                .5f , .8f , .5f, //color
                -1.0f , -1.0f , 1.0f, //23 dup 2
                .9f , 1.0f , .2f //color
            };
            float[] vertices_Triangle = { 
		        // T1
		        5.0f,  0.0f, -5.0f, 1.0f, 0.0f, 0.0f,
                15.0f, 0.0f, -5.0f, 0.0f, 1.0f, 0.0f,
                10f,  21.0f, -5.0f, 0.0f, 0.0f, 1.0f,  //B
            };
            Vertex[] triangleModelVertices = { 
		        // T1
		        new Vertex(new vec3(5.0f,  0.0f, -5.0f), new vec3(1.0f, 0.0f, 0.0f), new vec3(0.0f,0.0f,0.0f)),
                new Vertex(new vec3(15.0f, 0.0f, -5.0f), new vec3(0.0f, 1.0f, 0.0f), new vec3(0.0f,0.0f,0.0f)),
                new Vertex(new vec3(10f,  21.0f, -5.0f), new vec3(0.0f, 0.0f, 1.0f), new  vec3(0.0f,0.0f,0.0f)),
            };
            float[] triangleWithTexture =
            {
                5.0f,  0.0f, -5.0f,              1.0f, 0.0f, 0.0f,           0,0,0,
                15.0f, 0.0f, -5.0f,              0.0f, 1.0f, 0.0f,           0,1,0,
                10f,  21.0f, -5.0f,              0.0f, 0.0f, 1.0f,           1,0,0
            };
            float[] doubleTriangle =
            {
                0.5f,0.0f,0.0f, //00
                1,0,0, // red
                0.0f,1.0f,0.0f, //1
                0,1,0, // green
                0.0f,-1.0f,0.0f, //2
                0,0,1, //blue

                -.5f,0.0f,0.0f,//3
                1,1,0
            };
            ushort[] doubleTriangleIndecies = { 0, 1, 2, 1, 2, 3 };
            float[] vertices_XYZ = { 
                //x
		        0.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, //R
		        100.0f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f, //R
		        //y
	            0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f, //G
		        0.0f, 100.0f, 0.0f, 0.0f, 1.0f, 0.0f, //G
		        //z
	            0.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f,  //B
		        0.0f, 0.0f, -100.0f, 0.0f, 0.0f, 1.0f,  //B
            };
            Texture groundtex = new Texture(Texture.texturesPaths[(int)Texture.Textures.Ground],0);
            Texture Catle = new Texture(Texture.texturesPaths[(int)Texture.Textures.Crate], 1);
            // Initialize SHapes
            //triangleShape = new ShapeTextures(triangleWithTexture, 2,Gl.GL_TRIANGLES,groundtex);
            //xyzAxisShape = new Shape(vertices_XYZ,1 , Gl.GL_LINES);
            //cubeShape = new ShapeTextures(cube, 1,Gl.GL_POLYGON,Catle);
            //doubletriangle = new Shape(doubleTriangle, 1, Gl.GL_TRIANGLES, doubleTriangleIndecies);
            //doubletriangle.Scale(new vec3(20, 20, 20));
            //shapeTexture = new ShapeTextures(triangleWithTexture, 2, Gl.GL_TRIANGLES, groundtex);

            triangleWithTex = new ShapeTextures(triangleWithTexture, 2, Gl.GL_TRIANGLES, groundtex);
            trinagleWithTex2 = new ShapeTextures(vertices_Triangle, 1, Gl.GL_TRIANGLES, groundtex);

            trinagleWithTex2.Translate(new vec3(10, 0, 0));
            // View matrix 
            ViewMatrix = glm.lookAt(
                        new vec3(30, 50, 50), 
                        new vec3(0, 0, 0), 
                        new vec3(0, 1, 0)  
                );

            ProjectionMatrix = glm.perspective(45.0f, 4.0f / 3.0f, 0.1f, 100.0f);
           
            sh.UseShader();

            ShaderModelMatrixID = Gl.glGetUniformLocation(sh.ID, "modelMatrix");
            ShaderViewMatrixID = Gl.glGetUniformLocation(sh.ID, "viewMatrix");
            ShaderProjectionMatrixID = Gl.glGetUniformLocation(sh.ID, "projectionMatrix");
           
            Gl.glUniformMatrix4fv(ShaderViewMatrixID, 1, Gl.GL_FALSE, ViewMatrix.to_array());
            Gl.glUniformMatrix4fv(ShaderProjectionMatrixID, 1, Gl.GL_FALSE, ProjectionMatrix.to_array());

            timer.Start();
        }

        public void Draw()
        {
            sh.UseShader();
            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT);

            //xyzAxisShape.Draw(ShaderModelMatrixID);
            //triangleShape.Draw(ShaderModelMatrixID);
            //cubeShape.Draw(ShaderModelMatrixID);
            //doubletriangle.Draw(ShaderModelMatrixID);
            //shapeTexture.Draw(ShaderModelMatrixID);
            triangleWithTex.Draw(ShaderModelMatrixID);
            trinagleWithTex2.Draw(ShaderModelMatrixID);
        }
        private void BufferNewShape(uint vertixBufferID,int step)
        {
            int axisCount = 3;
            Gl.glBindBuffer(Gl.GL_ARRAY_BUFFER, vertixBufferID);

            Gl.glEnableVertexAttribArray(0);
            Gl.glVertexAttribPointer(0, axisCount, Gl.GL_FLOAT, Gl.GL_FALSE, axisCount*step * sizeof(float), (IntPtr)0);
           
            if(step > 1)
            {
                Gl.glEnableVertexAttribArray(1);
                Gl.glVertexAttribPointer(1, axisCount, Gl.GL_FLOAT, Gl.GL_FALSE, axisCount*step * sizeof(float), (IntPtr)(axisCount * sizeof(float)));
            }

        }
        const float rotationSpeed=5;
        const float movementSpeed = 50;
        const float expandingRatio = 5;
        public void Update()
        {
            CalcDeltaTime();
            UpdateShapes();
        }
        
        public void UpdateShapes()
        {
            foreach (Shape shape in avialableShapes)
            {
                shape.Update();
            }
            foreach (ShapeTextures shape in avialableShapes2)
            {
                shape.Update();
            }
        }
        public void CleanUp()
        {
            sh.DestroyShader();
        }
       
        public void TranslateShape(vec3 addedPosition)
        {
            addedPosition *= (movementSpeed* deltaTime);
            triangleWithTex.Translate(addedPosition);
        }
        public void RotateShape(vec3 rotationDirection )
        {
            triangleShape.Rotate(rotationSpeed * deltaTime, rotationDirection);
        }
        public void ScaleShape(vec3 rotationAxis)
        {
            rotationAxis *= expandingRatio * deltaTime;
            doubletriangle.Scale(rotationAxis);
        }
        public static void AssignShape(Shape shapeToAssign)
        {
            avialableShapes.Add(shapeToAssign);
        }
        public static void AssignShape2(ShapeTextures shapeToAssign)
        {
            avialableShapes2.Add(shapeToAssign);
        }

        #region PRIVATE METHODS
        private void CalcDeltaTime()
        {
            timer.Stop();
            deltaTime = timer.ElapsedMilliseconds / 1000.0f;
            
            timer.Reset();
            timer.Start();
        }
        #endregion
    }
}
