using Mogre;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhysicsEng
{
    public class PhysObj
    {
		//******************** DO NOT MODIFY! **************************
        //---------------- Fields ------------------
        private string id;
        private float invMass;
        private float restitution;
        private float frictionCoeff;
        private float radius;

        private bool hit=false;

        private Vector3 resForces;
        private Vector3 velocity;
        private List<Force> forceList;
        private Vector3 position;

        // --- Collision Parameters ---
        private List<Contacts> collisionList;

        // --- Bounding Volumes ---
        private Sphere sphere;
        private AxisAlignedBox aabb;

        // --- Scene Node ---
        private SceneNode sceneNode;

        //--------------- Properties ---------------

        public bool Hit
        {
            set { hit = value; }
            get { return hit; }
        }

        public Vector3 Position
        {
            get { return position; }
            set 
            {
                position = value;
                sceneNode.SetPosition(position.x, position.y, position.z) ;
            }
        }

        public float FrictionCoeff
        {
            get { return frictionCoeff; }
            set { frictionCoeff = value; }
        }

        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        public Vector3 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }
        
        public float InvMass
        {
            get { return invMass; }
            set { invMass = value; }
        }

        public float Restitution
        {
            get { return restitution; }
            set { restitution = value; }
        }

        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        public Vector3 ResForces
        {
            get { return resForces; }
            set { resForces = value; }
        }

        public AxisAlignedBox AABB
        {
            get { return aabb; }
            set { aabb = value; }
        }

        public List<Contacts> CollisionList
        {
            get { return collisionList; }
            set { collisionList = value; }
        } 
        
        public List<Force> ForceList
        {
            get { return forceList; }
        }

        public SceneNode SceneNode
        {
            get { return sceneNode; }
        }

        //------------ Constructors --------------
        
        public PhysObj() { }

        public PhysObj(SceneManager mSceneMgr, float radius, string id, float invMass = 1.0f, float restitution = 0.99f, float frictionCoeff = 0.1f)
        {
            this.invMass = invMass;
            this.restitution = restitution;
            this.id = id;
            this.radius = radius;
            this.frictionCoeff = frictionCoeff;

            this.resForces = new Vector3();
            this.velocity = new Vector3();
            this.forceList = new List<Force>();
            this.sphere = new Sphere();
            this.aabb = new AxisAlignedBox();

            this.collisionList = new List<Contacts>();

            this.sceneNode = mSceneMgr.CreateSceneNode(id + "_Node");
        }

        // ------------- Methods -----------------
        
        public void Translate(Vector3 displacement)
        {
            sceneNode.Translate(displacement);
            position = sceneNode.Position;
        }

        public AxisAlignedBox getBoundingBox()
        {
            aabb.SetExtents(new Vector3(position.x - radius, position.y - radius, position.z - radius), new Vector3(position.x + radius, position.y + radius, position.z + radius));
            return aabb;
        }

        public void updateAABB()
        {
            aabb.TransformAffine(new Matrix4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, position.x, position.y, position.z, 0.1f));
        }

        public Sphere getBoundingSphere()
        {
            sphere.Center = position;
            sphere.Radius = radius;
            return sphere;
        }

        public void addForceToList(Force force)
        {
            Force frc = new Force();
            frc = forceList.Find(f => f.ID == force.ID);
            if (frc==null)
                forceList.Add(force);
        }

        public void removeForceFromList(string ID)
        {
            forceList.RemoveAll(obj=> obj.ID == ID);
        }

        public void updateResFor(Vector3 Position)
        {
            resForces = Vector3.ZERO;
            foreach (Force f in forceList)
            {
                f.compute(Position);
                resForces += f.ForceVect;
            }
        }

        public void updatePostion(float dt)
        {
            Translate(dt * velocity);
        }
    }
}
