using Mogre;
using Mogre.TutorialFramework;
using System;


namespace Mogre.Tutorials
{
    class BulletBall
    {
        private Entity projectileEntity;
        private SceneManager mSceneMgr;
        private SceneNode projectileNode;

        public BulletBall(SceneManager mSceneMgr)
        {

            createBulletBall();

        }

        public void createBulletBall()
        {
            

        }
        public SceneNode ProjectileNode
        {
            get
            {
                return projectileNode;
            }
        }
        public Entity ProjectileEntity
        {
            get
            {
                return projectileEntity;
            }
        }

    }

     
        
    
    
}
