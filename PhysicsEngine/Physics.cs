using Mogre;
using System;
using System.Collections.Generic;

namespace PhysicsEng
{
    public class Physics
    {
		// ************************************ DO NOT TOUCH THIS SECTION **************************************************************
		// ************************** THE METHODS YOU MUST IMPLEMENT ARE IN THE NEXT SECTION (Line 40) *******************************************
		// --------------------- Fields ---------------------
        private const float MINSPEED = 0.00000001f;

        private Timer simTime;
        private float dt;
        // --------------------- Attributes ---------------------
        public void startSimTimer()
        {
            simTime = new Timer();
        }
        
        public void stopSimTimer()
        {
            simTime.Dispose();
        }
        
        public void resetSimTimer()
        {
            simTime.Reset();
        }

        public float Milliseconds
        {
            get { return simTime.Milliseconds; }
        }
		//*********************************************************************************************************

        // ********************************** METHODS TO IMPLEMENT **************************************
        
        private Vector3 velVerletSolver(PhysObj obj, float Dt)
        {
            Vector3 newPos = new Vector3();
            Vector3 finalVel = new Vector3();
            Vector3 oldResForces = new Vector3();
            
            oldResForces = obj.ResForces; 	//Froces at this frame

            // ***---> YOUR POSITION UPDATE IMPLEMENTATION HERE! <---***
            obj.Position = obj.Position + (obj.Velocity * Dt) + (Dt * Dt * 0.5f * obj.InvMass) * obj.ResForces;
            newPos = obj.Position;
            
			obj.updateResFor(newPos);		//Forces at next frame (access thorugh obj.ResForces)
			
			// ***---> YOUR VELOCITY UPDATE IMPLEMENTATION HERE! <---***
            obj.Velocity = (obj.Velocity) + (Dt * 0.5f * obj.InvMass) * (oldResForces + obj.ResForces);
            finalVel = obj.Velocity;
            
			return finalVel;
        }

        private Vector3 impulse(PhysObj obj, PhysObj obj1)
        {
            // ***---> YOUR IMPLEMENTATION HERE! <---***
            Vector3 impluse = -(obj.Restitution + 1) / (obj1.InvMass + obj.InvMass) * (obj.Velocity - obj1.Velocity);

            return impluse;
        }
		//*********************************************************************************************************
		
		//*********************************** DO NOT TOUCH THIS SECTION *******************************************
        //---------------------------------------- Update Method -----------------------------------------
        public void updatePhysics(float frequency, List<PhysObj> physObjList, List<Plane> boundPlanes)
        {
            dt = frequency;
            
            if (Milliseconds > frequency)
            {   
                updateObjsCollisionList(physObjList);                                                                   //Generate Collision lists

                foreach (PhysObj obj in physObjList)
                {
                    List<PhysObj> objCollisionList = new List<PhysObj>();

                    getCollidingObjects(obj, physObjList, objCollisionList);

                    if (objCollisionList.Count != 0)
                    {
                        collisionReactions(obj, objCollisionList);                                                      //Compute collisions reactions between objects    
                    }
                }
                
				foreach (PhysObj obj in physObjList)                                                                    //For each physics object in the simulation
                {
                    obj.Velocity = velVerletSolver(obj, frequency);                                                     //Update object velocity
                    foreach (Plane plane in boundPlanes)
                    {
                        planeCollision(obj, plane);
                    }
                    obj.updatePostion(frequency);                                                                       //Update object position
				}
				
                resetSimTimer();                                                                                        //Reset the simulation timer
            }
        }
        
        //---------------------------------------- Auxiliary Methods -----------------------------------------

        private void planeCollision(PhysObj obj, Plane plane)
        {
            float normalSpeed =  obj.Velocity.DotProduct(plane.normal);
            float potential = obj.Position.DotProduct(plane.normal);
            if(normalSpeed <0)
            {
                float t = (obj.Radius - potential - plane.d) / normalSpeed;
                if (t <= dt)
                {
					obj.Velocity -= (1 + obj.Restitution) * normalSpeed * plane.normal;
                }
            }
        }

        private void updateObjsCollisionList(List<PhysObj> physObjList)
        {
            for (int i = 0; i < physObjList.Count - 1; i++)
            {
                physObjList[i].CollisionList = new List<Contacts>();
                PhysObj obj = physObjList[i];
                for (int j = i + 1; j < physObjList.Count; j++)
                {
                    PhysObj potCollObj = physObjList[j];

                    float[] times = new float[2];
                    
                    if (sphereCollision(obj, potCollObj, ref times))
                    {
                        physObjList[i].Hit = true;
                        physObjList[j].Hit = true;
                        Contacts contatct = new Contacts();
                        contatct.collisionID = potCollObj.ID;
                        contatct.penetrationTimes = times;
                        
                        if (times[1] >= 0 && times[0] <= dt)
                        {
                            contatct.contactNormal = (potCollObj.Position - obj.Position) + times[0] * (potCollObj.Velocity - obj.Velocity);
                        }

                        obj.CollisionList.Add(contatct);
                    }
                }
            }
        }

        private bool sphereCollision(PhysObj obj, PhysObj potCollObj,ref float[] times)
        {
            times[0] = dt + 1;
            times[1] = -1;

            bool collision = false;

            Vector3 c = potCollObj.Position - obj.Position;

            Vector3 v = potCollObj.Velocity - obj.Velocity;

            float r = obj.Radius + potCollObj.Radius;

            float cDotV = c.DotProduct(v);

            float sqrNormV = v.SquaredLength;

            float numTerm = c.SquaredLength - r * r;

            float delta = cDotV * cDotV - sqrNormV * numTerm;

            if (delta >= 0)
            {
                float sqrtDelta = Mogre.Math.Sqrt(delta);
                float time1 = (-cDotV + sqrtDelta) / sqrNormV;
                float time2 = -(cDotV + sqrtDelta) / sqrNormV;
                
                if (time2 < time1)
                {
                    times[0] = time2;
                    times[1] = time1;
                }
                else
                {
                    times[0] = time1;
                    times[1] = time2;
                }
            }

            if (times[1] >= 0 && times[0]<=dt)
            {   
                collision = true;
            }

            return collision;
        }

        private void getCollidingObjects(PhysObj obj, List<PhysObj> physObjList, List<PhysObj> collisionList)
        {
            foreach (Contacts contObj in obj.CollisionList)
            {
                PhysObj collObj = physObjList.Find(el => el.ID == contObj.collisionID);

                if (collObj != null)
                    collisionList.Add(collObj);
            }
        }

        private void solveBSConpenetrations(PhysObj obj, List<PhysObj> collisionList)
        {
            Sphere BSO = obj.getBoundingSphere();
            
            Vector3 translVector = new Vector3();
            
            foreach (PhysObj collObj in collisionList)
            {
                Sphere BSCO = collObj.getBoundingSphere();
                
                float displacement = 0;

                Vector3 direction = BSO.Center - BSCO.Center;

                float distance = direction.Normalise();
                
                float minDist = BSO.Radius + BSCO.Radius;

                if (distance < minDist)
                {
                    displacement = minDist - distance;
                    translVector += displacement * direction;
                }
            }
            obj.Translate(translVector);
        }

        private void collisionReactions(PhysObj obj, List<PhysObj> collisionList)
        {
            for(int i=0; i<collisionList.Count; i++)
            {

                Vector3 imp = impulse(obj, collisionList[i]);

                obj.Velocity += imp * obj.InvMass;

                collisionList[i].Velocity -= imp * collisionList[i].InvMass;
            }
        }
		//*********************************************************************************************************
    }
}