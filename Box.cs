using Mogre;
using Mogre.TutorialFramework;
using System;
using PhysicsEng;


namespace Mogre.Tutorials
{
    class Box : BaseApplication
    {
        Entity ent;
        SceneNode scene;


        public Box_WeaponUpgrade box_powerup;
        public PhysObj weaponBoxPhysObj;
        public SceneManager mSceneMgr;


        public Box(SceneManager mSceneMgr)
        {

            this.mSceneMgr = mSceneMgr;
            box_powerup = new Box_WeaponUpgrade(mSceneMgr);
            box_powerup.createWeaponUpgrade();

        }


    }
}
        