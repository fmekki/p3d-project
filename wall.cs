using Mogre;
using Mogre.TutorialFramework;
using System;
using PhysicsEng;
using System.Collections.Generic;


namespace Mogre.Tutorials
{
    class Wall
    {

        private SceneManager mSceneMgr;
        private ManualObject manual;
        private Entity manualEntity;
        private SceneNode manualNode;
        private Boolean tooshort;
        private StaticGeometry manyQuad;
        private Plane skyPlane;
        private Light pointLight;
        private Light directionalLight;
        private Light spotLight;
        private PhysObj envPhysObj2;
        private PhysObj wallPhysObj;
        private static int countW = 0;
        private Entity ent;
        private Entity ent2;
        private SceneNode scene;
        private SceneNode scene2;
        private List<PhysObj> physObjList;




        public Wall(SceneManager mSceneMgr)
        {
            this.mSceneMgr = mSceneMgr;
            wallPhysObj = new PhysObj(mSceneMgr, 10, "wall_entity_" + countW, 0.0001f);

            //#/---Manual Objects--/#//
            manual = mSceneMgr.CreateManualObject("manualQuad" + countW);
            manual.Begin("void", RenderOperation.OperationTypes.OT_TRIANGLE_LIST);
            manual.Position(new Vector3(50, 50, 0)); //Vertex 0 (1,1)
            manual.Normal(Vector3.UNIT_Z);
            manual.TextureCoord(0, 0);
            manual.Position(new Vector3(-50, 50, 0)); // Vertex 1 (-1,1)
            manual.Normal(Vector3.UNIT_Z);
            manual.TextureCoord(0, 1);
            manual.Position(new Vector3(50, -50, 0)); // Vertex 2 (1, -1)
            manual.Normal(Vector3.UNIT_Z);
            manual.TextureCoord(1, 0);
            manual.Position(new Vector3(-50, -50, 0)); // Vertex 3 (-1, -1)
            manual.Normal(Vector3.UNIT_Z);
            manual.TextureCoord(1, 1);

            manual.Index(0);
            manual.Index(2);
            manual.Index(1);
            manual.Index(3);
            manual.Index(1);
            manual.Index(2);
            manual.End();
            manual.ConvertToMesh("Quad" + countW);

            manualEntity = mSceneMgr.CreateEntity("Quad_Entity" + countW, "Quad" + countW);
            manualNode = mSceneMgr.CreateSceneNode("Quad_Node" + countW);
            manualNode.AttachObject(manualEntity);
            manualEntity.SetMaterialName("wallTexture");
            manualNode.Scale(new Vector3(0.3f, 0.8f, 0.8f));
            countW++;
            manualNode.Translate(new Vector3(3.5f, 0, 60));
            wallPhysObj.SceneNode.AddChild(manualNode);

        }




        public void attachToSceneGraph(SceneNode root)
        {
            root.AddChild(wallPhysObj.SceneNode);
        }

        public PhysObj WallPhysObj
        {
            get { return wallPhysObj; }
        }


        public void reduce()
        {
            manualNode.Scale(new Vector3(1, 0.8f, 1));
        }

        public void remove()
        {
            manualEntity.Dispose();
            manualNode.Parent.RemoveChild(manualNode);
            manualNode.Dispose();
            wallPhysObj.SceneNode.Parent.RemoveChild(wallPhysObj.SceneNode);
            wallPhysObj.SceneNode.Dispose();

        }

    }



}
