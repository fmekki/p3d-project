using Mogre;
using Mogre.TutorialFramework;
using System;



namespace Mogre.Tutorials
{
    public class Robot
    {

        public Entity robotEntity, ent;
        public SceneNode robotNode, scene;
        public SceneManager mSceneMgr;
        public AnimationState idleAnimeationState;
        private Vector3 walkDirection;
        public AnimationState walkAnimationState;
        public AnimationState shootAnimationState;
        public float x, y, z;
        public float angle;
        private static int counterRobot = 1;


        public Robot(SceneManager mSceneMgr)
        {
            this.mSceneMgr = mSceneMgr;
            createRobot();
            mSceneMgr.ShadowTechnique = ShadowTechnique.SHADOWTYPE_STENCIL_ADDITIVE;
            robotEntity.CastShadows = true;
        }
        public void createRobot()
        {
            robotEntity = mSceneMgr.CreateEntity("robot.mesh");

            robotNode = mSceneMgr.CreateSceneNode("robotnode_" + counterRobot);
            counterRobot++;

            robotNode.Rotate(Vector3.UNIT_Y, new Degree(270));

            robotNode.AttachObject(robotEntity);

            walkAnimationState = robotEntity.GetAnimationState("Walk");
            walkAnimationState.Loop = true;
            walkAnimationState.Enabled = true;
            walkDirection = Vector3.UNIT_X;
            angle = 0;
            shootAnimationState = robotEntity.GetAnimationState("Shoot");
            shootAnimationState.Loop = true;
            shootAnimationState.Enabled = true;

        }

        public void animate(FrameEvent evt)
        {
            walkAnimationState.AddTime(evt.timeSinceLastFrame * 2);
            shootAnimationState.AddTime(evt.timeSinceLastFrame / 2);
        }

        public void attachToSceneGraph(SceneNode root)
        {
            root.AddChild(robotNode);
        }
        public Vector3 WalkingDir
        {
            get
            {
                angle += Mogre.Math.PI / 10000;
                walkDirection = new Vector3(Mogre.Math.Cos(angle), 0, -Mogre.Math.Sin(angle));
                return walkDirection;
            }
        }

    }
}