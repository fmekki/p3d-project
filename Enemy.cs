using Mogre;
using Mogre.TutorialFramework;
using System;
using PhysicsEng;


namespace Mogre.Tutorials
{
    class Enemy : BaseApplication
    {

        private Robot robot;
        private PhysObj enPhysObj;
        private static int count = -1, speedInt;
        private float x, y, z;
        private static int robotcount = 1;
        private Vector3 enemyposition;



        public Enemy(SceneManager mSceneMgr)
        {

            this.mSceneMgr = mSceneMgr;
            createEnemy();
        }

        public void createEnemy()
        {

            enPhysObj = new PhysObj(mSceneMgr, 40, "robot_PhysObj_" + count, 0.01f, 0.6f, 0.05f);
            count++;
            robot = new Robot(mSceneMgr);

            enPhysObj.SceneNode.AddChild(robot.robotNode);

            enPhysObj.Translate(new Vector3(0, 300, 0));
            robot.robotNode.Translate(new Vector3(6, -40, 0));
            robotcount++;
            enPhysObj.Hit = false;

            //-------Visualise the PhysObj-----------//
            //ent = mSceneMgr.CreateEntity("orb.mesh");
            //scene = mSceneMgr.CreateSceneNode();
            //scene.AttachObject(ent);

            robot.robotNode.Scale(1.4f, 1.4f, 1.4f);
            enPhysObj.addForceToList(new WeightForce(enPhysObj.InvMass));
            enPhysObj.addForceToList(new FrictionForce(enPhysObj));
        }

        public void attachToSceneGraph(SceneNode root)
        {
            root.AddChild(enPhysObj.SceneNode);

        }
        public SceneNode EnemyNode
        {
            get { return EnemyPhysObj.SceneNode; }
        }
        public int RobotCount
        {
            get { return robotcount; }
        }
        public PhysObj EnemyPhysObj
        {
            get { return enPhysObj; }
        }
        public int Speeding
        {
            get { return speedInt; }
        }
        public void setSpeeding(int i)
        {
            speedInt = i;
        }
        public void animate(FrameEvent evt)
        {
            robot.animate(evt);
            enPhysObj.Translate(Speeding * evt.timeSinceLastFrame * Vector3.UNIT_Z);
            enemyposition = enPhysObj.Position;
        }
        public void remove()
        {

            robot.robotNode.Dispose();
            enPhysObj.SceneNode.RemoveAndDestroyAllChildren();
            enPhysObj.SceneNode.Dispose();
        }

    }
}
