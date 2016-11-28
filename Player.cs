using Mogre;
using Mogre.TutorialFramework;
using System;
using PhysicsEng;


namespace Mogre.Tutorials
{
    class Player
    {

        public SceneNode playerNode;
        public Cannon cannon;
        public SceneManager mSceneMgr;
        private Vector3 fireDirection;

        private PhysObj playerPhysObj;
        int count;

        public Player(SceneManager mSceneMgr)
        {
            this.mSceneMgr = mSceneMgr;


            createPlayer();

        }

        public void createPlayer()
        {
            playerNode = mSceneMgr.CreateSceneNode();

            cannon = new Cannon(mSceneMgr);

            playerNode.AddChild(cannon.BaseNode);

            playerPhysObj = new PhysObj(mSceneMgr, 10, "cannon_PhysObj_" + count, 0.001f, 0.6f, 0.05f);
            playerPhysObj.SceneNode.AddChild(playerNode);
            PlayerPhysObj.Position = new Vector3(0, 40, 30);
            playerPhysObj.Translate(new Vector3(10, 30, 200));

            cannon.cannonNode.Translate(0, 300, 0);
            cannon.baseNode.Translate(0, 0, 100);
            cannon.cannonNode.Translate(0, 0, -300);
            cannon.baseNode.Translate(0, -10, -40);

            cannon.baseNode.Scale(0.7f, 0.7f, 0.7f);
            cannon.cannonNode.Scale(1.2f, 1.25f, 1.7f);
            cannon.baseNode.Scale(0.8f, 0.8f, 0.8f);


        }

        public void attachToSceneGraph(SceneNode root)
        {
            root.AddChild(playerPhysObj.SceneNode);

        }
        public SceneNode PlayerNode
        {
            get
            {
                return playerPhysObj.SceneNode;
            }
        }
        public PhysObj PlayerPhysObj
        {
            get { return playerPhysObj; }
        }


        public Vector3 FireDirection
        {
            get
            {
                fireDirection = -playerNode.LocalAxes * cannon.ForewardDirection;
                fireDirection.Normalise();
                return fireDirection;
            }
        }
        public void Move(Vector3 displacements)
        {
            Vector3 restrict = PlayerPhysObj.Position + displacements;
            if (restrict.x > -50 && restrict.x < 50)
            {
                PlayerPhysObj.Translate(displacements);
            }
        }
        public void Rotate(Vector3 axisX, Vector3 axisY, Radian anglesX, Radian anglesY, Node.TransformSpace x, Node.TransformSpace Y)
        {

            Vector2 anglesUpdate = new Vector2(new Degree(anglesX).ValueDegrees, new Degree(anglesY).ValueDegrees);
            float pitch = cannon.cannonNode.Orientation.Pitch.ValueDegrees + anglesUpdate.x;
            float yaw = playerNode.Orientation.Yaw.ValueDegrees + anglesUpdate.y;

            if (pitch < 70 && pitch > -20)
            {
                cannon.CannonNode.Rotate(axisX, anglesX, x);

            }
            if (yaw < 60 && yaw > -60)
            {

                playerNode.Rotate(axisY, anglesY, Y);

            }

        }
    }
}
