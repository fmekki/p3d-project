using Mogre;
using Mogre.TutorialFramework;
using System;
using PhysicsEng;


namespace Mogre.Tutorials
{
    public class Cannon
    {
        public SceneNode baseNode;
        public SceneNode cannonNode;
        private SceneManager mSceneMgr;
        private Entity baseEntity;
        private Entity cannonEntity;
        private Vector3 forewardDirection;





        public Cannon(SceneManager mSceneMgr)
        {
            this.mSceneMgr = mSceneMgr;
            createCannon();
            mSceneMgr.ShadowTechnique = ShadowTechnique.SHADOWTYPE_STENCIL_ADDITIVE;
            cannonEntity.GetMesh().BuildEdgeList();
            baseEntity.GetMesh().BuildEdgeList();

            cannonEntity.CastShadows = true;
            baseEntity.CastShadows = true;
        }


        public void createCannon()
        {
            cannonEntity = mSceneMgr.CreateEntity("cannon_Entity", "cannontop.mesh");

            baseEntity = mSceneMgr.CreateEntity("base_Entity", "basecannon.mesh");

            baseNode = mSceneMgr.CreateSceneNode("base_Node");
            cannonNode = baseNode.CreateChildSceneNode("cannon_Node");
            cannonNode.Translate(new Vector3(-75, 0, 0));

            baseNode.AttachObject(baseEntity);

            cannonNode.AttachObject(cannonEntity);

            baseNode.Scale(0.25f, 0.25f, 0.25f);
        }
        public void moveCannon(int x, int y, int z)
        {
            baseNode.Translate(x, y, z);
        }
        public SceneNode BaseNode
        {
            get
            {
                return baseNode;
            }

        }
        public SceneNode CannonNode
        {
            get
            {
                return cannonNode;
            }
        }


        public Vector3 ForewardDirection
        {
            get
            {
                forewardDirection = cannonNode.LocalAxes.GetColumn(2);
                return forewardDirection;
            }
        }


    }
}
