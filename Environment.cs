using Mogre;
using Mogre.TutorialFramework;
using System;
using PhysicsEng;
using System.Collections.Generic;


namespace Mogre.Tutorials
{
    class Environment
    {

        private SceneManager mSceneMgr;
        private ManualObject manual;
        private Entity manualEntity;
        private SceneNode manualNode;
        private StaticGeometry manyQuad;
        private Plane skyPlane;
        private Light pointLight;
        private Light directionalLight;
        private Light spotLight, spotLight2;
        private PhysObj envPhysObj2;
        private PhysObj envPhysObj;
        private static int countW = 0;
        private Entity ent;
        private Entity ent2;
        private SceneNode scene;
        private SceneNode scene2;
        private List<PhysObj> physObjList;



        public Environment(SceneManager mSceneMgr)
        {
            //------SkyPlane
            this.mSceneMgr = mSceneMgr;
            skyPlane.d = 300;
            skyPlane.normal = Vector3.NEGATIVE_UNIT_Y;
            mSceneMgr.SetSkyPlane(true, skyPlane, "testMaterialSky", 100, 20, true, 0f, 50, 50);
            mSceneMgr.AmbientLight = new ColourValue(0.1f, 0.1f, 0.1f);

            //------DirectionalLight
            directionalLight = mSceneMgr.CreateLight("directionalLight");
            directionalLight.Type = Light.LightTypes.LT_DIRECTIONAL;
            directionalLight.Position = new Vector3(500, 500, 500);
            directionalLight.Direction = Vector3.NEGATIVE_UNIT_Z + Vector3.NEGATIVE_UNIT_Y;
            directionalLight.SetAttenuation(100000, 0.01f, 0, 0);

            //------SpotLight1
            spotLight = mSceneMgr.CreateLight("spotLight");
            spotLight.Type = Light.LightTypes.LT_SPOTLIGHT;
            spotLight.DiffuseColour = ColourValue.Red;
            spotLight.Direction = (Vector3.NEGATIVE_UNIT_Z) + Vector3.NEGATIVE_UNIT_Y;
            spotLight.Position = new Vector3(0, 200, 500);

            //------Spotlight2
            spotLight2 = mSceneMgr.CreateLight("spotLight2");
            spotLight2.Type = Light.LightTypes.LT_SPOTLIGHT;
            spotLight2.DiffuseColour = ColourValue.Blue;
            spotLight2.Direction = (Vector3.NEGATIVE_UNIT_Z) + Vector3.NEGATIVE_UNIT_Y;
            spotLight2.Position = new Vector3(0, 200, 500);


            //------Spotlight Settings
            spotLight.SetSpotlightRange(new Degree(0), new Degree(35));
            spotLight.SetAttenuation(8000, 1, 0, 0);
            spotLight.SpotlightFalloff = 1;
            spotLight2.SetSpotlightRange(new Degree(0), new Degree(35));
            spotLight2.SetAttenuation(8000, 1, 0, 0);
            spotLight2.SpotlightFalloff = 1;

        }



    }
}
