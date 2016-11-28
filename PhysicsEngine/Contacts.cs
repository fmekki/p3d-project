using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using Mogre.TutorialFramework;

namespace PhysicsEng
{
    public class Contacts
    {
        public String collisionID;
        public float[] penetrationTimes;

        public Vector3 contactNormal;
        
        public Contacts()
        {
            contactNormal = new Vector3();
            penetrationTimes = new float[2];
        }
    }
}
