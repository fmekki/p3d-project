using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using PhysicsEng;
using Mogre.TutorialFramework;

namespace Mogre.Tutorials
{
    public class Projectile
    {
        private SceneNode projectileNode;
        private SceneManager mSceneMgr;
        private Entity projectileEntity;
        private PhysObj physObj;
        private static int count = 0;




        public Projectile(SceneManager mSceneMgr)
        {
            this.mSceneMgr = mSceneMgr;
            createPrjectile();
        }

        public void createPrjectile()
        {
            physObj = new PhysObj(mSceneMgr, 10, "sphere_PhysObj_" + count, 1, 0.6f, 0.05f);

            projectileEntity = mSceneMgr.CreateEntity("orb.mesh");

            projectileNode = mSceneMgr.CreateSceneNode();

            projectileNode.AttachObject(projectileEntity);
            projectileNode.Scale(new Vector3(2f, 2f, 2f));
            physObj.SceneNode.AddChild(projectileNode);
            projectileEntity.SetMaterialName("physobj");
            mSceneMgr.ShadowTechnique = ShadowTechnique.SHADOWTYPE_STENCIL_ADDITIVE;
            projectileEntity.GetMesh().BuildEdgeList();
            projectileEntity.CastShadows = true;

            physObj.addForceToList(new WeightForce(physObj.InvMass));
            physObj.addForceToList(new FrictionForce(physObj));


            count++;

        }

        public void setPos(Vector3 v)
        {
            physObj.Position = v;
        }
        public void setInitialDir(Vector3 v)
        {
            Vector3 initialDir = v;
            initialDir.Normalise();
            physObj.Velocity = 70 * initialDir;
        }
        public void attachToSceneGraph(SceneNode root)
        {
            root.AddChild(physObj.SceneNode);
        }

        public PhysObj ProjectilePhysObj
        {
            get { return physObj; }
        }

        public SceneNode ProjectileNode
        {
            get
            { return ProjectilePhysObj.SceneNode; }
        }


        public void remove()
        {
            projectileEntity.Dispose();
            projectileNode.Dispose();
            physObj.SceneNode.RemoveAndDestroyAllChildren();
            physObj.SceneNode.Dispose();
        }




    }
}
