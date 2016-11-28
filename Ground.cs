using Mogre;
using Mogre.TutorialFramework;
using System;


namespace Mogre.Tutorials
{
    class Ground
    {
        public Plane ground, waterplatform, waterplatform2;
        private Entity groundEntity, platformEnt, platformEnt2;
        private SceneNode groundNode, platformNode, platformNode2;
        private SceneManager mSceneMgr;
        private Cube cube, cube2;


        public Ground(SceneManager mSceneMgr)
        {
            this.mSceneMgr = mSceneMgr;
            ground = new Plane(Vector3.UNIT_Y, 0);
            waterplatform = new Plane(Vector3.UNIT_Y, 10);
            waterplatform2 = new Plane(Vector3.UNIT_Y, 10);


            MeshManager.Singleton.CreatePlane("Ground",
                ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
                ground, 6000, 6000, 200, 200, true,
                1, 30, 30, Vector3.UNIT_Z);

            MeshManager.Singleton.CreatePlane("WaterPlatform",
                ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
                waterplatform, 100, 100, 10, 10, true,
                1, 1, 1, Vector3.UNIT_Z);
            MeshManager.Singleton.CreatePlane("WaterPlatform2",
                ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
                waterplatform, 100, 100, 10, 10, true,
                1, 1, 1, Vector3.UNIT_Z);



            groundEntity = mSceneMgr.CreateEntity("Ground_Entity", "Ground");
            groundNode = mSceneMgr.RootSceneNode.CreateChildSceneNode("Ground_Node");
            groundEntity.SetMaterialName("ground");
            groundNode.AttachObject(groundEntity);


            cube = new Cube(mSceneMgr);
            cube.makeCube("box", "waterWallTexture", 100, 50, 100, 100f, 0f, 250f);
            cube.attachToSceneGraph(mSceneMgr.RootSceneNode);
            cube2 = new Cube(mSceneMgr);
            cube2.makeCube("box2", "waterWallTexture", 100, 50, 100, -100f, 0f, 250f);
            cube2.attachToSceneGraph(mSceneMgr.RootSceneNode);



            platformEnt = mSceneMgr.CreateEntity("platform_Entity", "WaterPlatform");
            platformNode = mSceneMgr.RootSceneNode.CreateChildSceneNode("platform_Node");
            platformEnt.SetMaterialName("waterAnimated");
            platformNode.AttachObject(platformEnt);
            platformNode.Translate(new Vector3(98, 22, 250));

            platformEnt2 = mSceneMgr.CreateEntity("platform_Entity2", "WaterPlatform2");
            platformNode2 = mSceneMgr.RootSceneNode.CreateChildSceneNode("platform_Node2");
            platformEnt2.SetMaterialName("waterAnimated");
            platformNode2.AttachObject(platformEnt2);
            platformNode2.Translate(new Vector3(-98, 22, 250));

        }

    }
}
