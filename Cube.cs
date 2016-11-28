using Mogre;
using Mogre.TutorialFramework;
using System;
using PhysicsEng;
using System.Collections.Generic;


namespace Mogre.Tutorials
{
    class Cube
    {
        private ManualObject manual;
        private SceneManager mSceneMgr;
        private Entity cubeEntity;
        private SceneNode cubeNode;
        private static int countC;
        private PhysObj cubePhysObj, cubeElasticObj;
        private SceneNode scene;
        private Entity ent;

        public Cube(SceneManager mSceneMgr)
        {
            this.mSceneMgr = mSceneMgr;

        }

        public void makeCube(string cubeName, string materialName, float width, float height, float depth, float posx, float posy, float posz)
        {

            //--MANUAL--//
            manual = mSceneMgr.CreateManualObject(cubeName + "_ManObj_" + countC);
            countC++;
            manual.Begin(materialName, RenderOperation.OperationTypes.OT_TRIANGLE_LIST);

            // --- Fills the Vertex buffer and define the texture coordinates for each vertex ---

            //--- Vertex 0 ---
            manual.Position(new Vector3(.5f * width, .5f * height, .5f * depth));
            manual.Normal(Vector3.UNIT_Z);
            manual.TextureCoord(0, 0);//Texture coordinates here!

            //--- Vertex 1 ---
            manual.Position(new Vector3(.5f * width, -.5f * height, .5f * depth));
            manual.Normal(Vector3.UNIT_Z);
            manual.TextureCoord(0, 1);//Texture coordinates here!

            //--- Vertex 2 ---
            manual.Position(new Vector3(.5f * width, .5f * height, -.5f * depth));
            manual.Normal(Vector3.UNIT_Z);
            manual.TextureCoord(1, 0);//Texture coordinates here!

            //--- Vertex 3 ---
            manual.Position(new Vector3(.5f * width, -.5f * height, -.5f * depth));
            manual.Normal(Vector3.UNIT_Z);
            manual.TextureCoord(1, 1);//Texture coordinates here!


            //--- Vertex 4 ---
            manual.Position(new Vector3(-.5f * width, .5f * height, .5f * depth));
            manual.Normal(Vector3.UNIT_Z);
            manual.TextureCoord(1, 1);//Texture coordinates here!

            //--- Vertex 5 ---
            manual.Position(new Vector3(-.5f * width, -.5f * height, .5f * depth));
            manual.Normal(Vector3.UNIT_Z);
            manual.TextureCoord(1, 0);//Texture coordinates here!

            //--- Vertex 6 ---
            manual.Position(new Vector3(-.5f * width, .5f * height, -.5f * depth));
            manual.Normal(Vector3.UNIT_Z);
            manual.TextureCoord(0, 1);//Texture coordinates here!

            //--- Vertex 7 ---
            manual.Position(new Vector3(-.5f * width, -.5f * height, -.5f * depth));
            manual.Normal(Vector3.UNIT_Z);
            manual.TextureCoord(0, 0);//Texture coordinates here!


            // --- Fills the Index Buffer ---
            //--------Face 1----------
            manual.Index(0);
            manual.Index(1);
            manual.Index(2);

            manual.Index(2);
            manual.Index(1);
            manual.Index(3);

            //--------Face 2----------
            manual.Index(4);
            manual.Index(6);
            manual.Index(5);

            manual.Index(6);
            manual.Index(7);
            manual.Index(5);

            //--------Face 3----------
            manual.Index(0);
            manual.Index(4);
            manual.Index(1);

            manual.Index(1);
            manual.Index(4);
            manual.Index(5);

            //--------Face 4----------
            manual.Index(0);
            manual.Index(6);
            manual.Index(4);

            manual.Index(0);
            manual.Index(2);
            manual.Index(6);

            //--------Face 5----------
            manual.Index(6);
            manual.Index(2);
            manual.Index(3);

            manual.Index(6);
            manual.Index(3);
            manual.Index(7);

            //--------Face 5----------
            manual.Index(3);
            manual.Index(1);
            manual.Index(7);

            manual.Index(1);
            manual.Index(5);
            manual.Index(7);

            manual.End();
            manual.ConvertToMesh(cubeName);



            //--ATTACH MANUAL TO ENTITY&SCENENODE--//
            cubeEntity = mSceneMgr.CreateEntity(cubeName);
            cubeNode = mSceneMgr.CreateSceneNode();
            cubeNode.AttachObject(cubeEntity);
            cubeEntity.SetMaterialName(materialName);
            cubeNode.Rotate(Vector3.UNIT_Y, new Degree(270));
            //----------------------------------------------//


            //--PHYS OBJ--//
            cubePhysObj = new PhysObj(mSceneMgr, 30, "cubeEntity" + countC, 50f);
            cubePhysObj.SceneNode.AddChild(cubeNode);
            cubePhysObj.Translate(new Vector3(posx, posy, posz));
            //-------------------------------------------------//

            //-------------ELASTIC OBJECT------------------------------------------//
            cubeElasticObj = new PhysObj(mSceneMgr, 3, "cubeElastic" + countC, 0);
            ent = mSceneMgr.CreateEntity("orb.mesh");
            cubeElasticObj.Translate(new Vector3(posx + 10, posy + 100, posz));
            cubePhysObj.addForceToList(new ElasticForce(cubeElasticObj, cubePhysObj, restLenght: 500f, elastiConstant: 0.003f));
            cubePhysObj.addForceToList(new WeightForce(cubePhysObj.InvMass));
            //-----------------//
        }

        public void attachToSceneGraph(SceneNode root)
        {
            root.AddChild(cubePhysObj.SceneNode);
            root.AddChild(cubeElasticObj.SceneNode);
        }

        public void rotate()
        {
            cubeNode.Rotate(Vector3.UNIT_Y, new Degree(245));
            cubeNode.Rotate(Vector3.UNIT_Z, new Degree(10));
        }

        public void remove()
        {
            cubeEntity.Dispose();
            cubeNode.RemoveChild(cubeNode);
            cubeNode.Dispose();
            cubeElasticObj.Hit = false;
            cubePhysObj.Hit = false;
        }

        public PhysObj CubePhysObj
        {
            get { return cubePhysObj; }
        }

        public PhysObj CubeEl
        {
            get { return cubeElasticObj; }
        }



    }
}
