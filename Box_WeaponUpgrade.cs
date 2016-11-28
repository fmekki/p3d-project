using Mogre;
using Mogre.TutorialFramework;
using System;
using PhysicsEng;



namespace Mogre.Tutorials
{
    public class Box_WeaponUpgrade
    {

        private Entity weaponBoxEnt,physEnt;
        private SceneNode weaponBoxNode,physScene;
        private PhysObj weaponPhysObj;
        private SceneManager mSceneMgr;
        private Plane face;
        

        public Box_WeaponUpgrade(SceneManager mSceneMgr)
        {
            this.mSceneMgr = mSceneMgr;
        }
        public void createWeaponUpgrade()
        {
            face = new Plane(Vector3.UNIT_Y, 0);

            MeshManager.Singleton.CreatePlane("Ground",
                ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME,
                face, 200, 200, 10, 10, true,
                1, 1, 1, Vector3.UNIT_Z);

            weaponBoxEnt = mSceneMgr.CreateEntity("asddsa", "face");
            weaponBoxNode = mSceneMgr.RootSceneNode.CreateChildSceneNode("Weapon_Box_Node");
            //groundEntity.SetMaterialName("waterAnimated");
            weaponBoxEnt.SetMaterialName("testMaterial1");
            weaponBoxNode.AttachObject(weaponBoxEnt);   


        }
        public void attachToSceneGraph(SceneNode root)
        {
            root.AddChild(weaponBoxNode);
        }       
    }
}