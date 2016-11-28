using Mogre;
using Mogre.TutorialFramework;
using System;
using PhysicsEng;
using System.Collections.Generic;


namespace Mogre.Tutorials
{
    class Game : BaseApplication
    {
        //--InstanceImports--//
        private Player player;
        private Ground groundClass;
        private Enemy enemy;
        private Wall wall;
        private Environment environment;
        private HUD hud;
        private Cube cube;
        private Boolean powerup, healthremoved, exitloop;
        private Projectile ball, ball2, ball3;
       
        //--InputImports--//
        protected MOIS.InputManager mInputMgr;
        protected MOIS.Keyboard mKeyboard;
        protected MOIS.Mouse mMouse;
        //-------------------------//
        
        //--PhyImports--//
        private Physics phys;
        private List<PhysObj> physObjList;
        private List<Wall> wallList;
        private List<Plane> boundariesList;
        private List<Projectile> projectileList;
        private List<Enemy> enemyList;
        private List<Cube> cubeListAmmo;
        private List<Cube> cubeListHealth;
        private List<Cube> cubeListPowerUp;

        private static int count, boxtimer,levelNum = 1;
        private int ammocount = 30, healthcount = 100, enemyTimer = 0, score, x, boxNo, ammoboxtimer; 
        private int timer, globaltimer,walltimer,enemylistsize, projlistsize,enemyspeed=300,enemyloopInt=1;
        private float wallPos, wallPos2;
        private Random random = new Random();
        private Boolean ammoremoved,levelup;
        private Boolean ammoincrease=false;
        //---------------//


//---------------------------------//
//---MAIN----------------//
        public static void Main()
        {
            new Game().Go();
        }

//---------------------------------//
//---INITIATION----------------//
        protected override void CreateScene()
        {
            player = new Player(mSceneMgr);
            player.attachToSceneGraph(mSceneMgr.RootSceneNode);
            hud = new HUD(mSceneMgr);
            environment = new Environment(mSceneMgr);
            enemyList = new List<Enemy>();
            wallList = new List<Wall>();
            projectileList = new List<Projectile>();
            groundClass = new Ground(mSceneMgr);
            Random random = new Random();
            cubeListPowerUp = new List<Cube>();
            cubeListHealth = new List<Cube>();
            cubeListAmmo = new List<Cube>();
            wall = new Wall(mSceneMgr);
            int listsize = enemyList.Count;
            setPhys();
            
            physObjList.Add(player.PlayerPhysObj);
            physObjList.Add(wall.WallPhysObj);
            
            wall.attachToSceneGraph(mSceneMgr.RootSceneNode);
            wallList.Add(wall);
            wall.WallPhysObj.Translate(new Vector3(-200, 0, 120));
            wallPos = wall.WallPhysObj.Position.x;
            wallPos2 = wall.WallPhysObj.Position.x - 200;                
        }

        protected void setPhys()
        {
            boundariesList = new List<Plane>();
            boundariesList.Add(groundClass.ground);
            physObjList = new List<PhysObj>();
            phys = new Physics();
            phys.startSimTimer();
            count = 0;
        }


//---------------------------------//
//---UPDATE----------------//
        protected override void UpdateScene(FrameEvent evt)
        {
            hud.setScoreCaption(score);
            hud.setAmmoCaption(ammocount);
            hud.setHealthCaption(healthcount);
            hud.setLevelCaption(levelNum);
            int randX = random.Next(-200, 0);
            walltimer++;
            timer++;
            enemyTimer++;
            globaltimer++;
            boxtimer++;
            ammoboxtimer++;
            enemylistsize = enemyList.Count;
            projlistsize = projectileList.Count;
            int phylistsize = physObjList.Count;
    //---------------------------------//
    //---WALLS----------------//
            if (globaltimer < 1000)
            {
                if (walltimer == 20 && wallPos<200)
                {
                    int wallx = random.Next(-200, 200);
                    wall = new Wall(mSceneMgr);
                    physObjList.Add(wall.WallPhysObj);
                    wall.attachToSceneGraph(mSceneMgr.RootSceneNode);
                    wallList.Add(wall);
                    wall.WallPhysObj.Translate(new Vector3(wallPos+30, 0, 130));
                    wallPos = wall.WallPhysObj.Position.x;
                }
                if (walltimer == 20 && wallPos2 < 350)
                {
                    wall = new Wall(mSceneMgr);
                    physObjList.Add(wall.WallPhysObj);
                    wall.attachToSceneGraph(mSceneMgr.RootSceneNode);
                    wallList.Add(wall);
                    wall.WallPhysObj.Translate(new Vector3(wallPos2 + 30, 0, 80));
                    wallPos2 = wall.WallPhysObj.Position.x;
                    walltimer = 0;
                }
            }
            for (int i = 0; i < wallList.Count; i++)
            {
                if (wallList[i].WallPhysObj.Hit == true)
                {
                    wallList[i].WallPhysObj.Hit = false;
                    score--;
                    wallList[i].remove();
                    physObjList.Remove(wallList[i].WallPhysObj);
                    wallList.Remove(wallList[i]);
                    healthcount = healthcount - 5;
                    break;
                }
            }
    //---------------------------------//
    //---ENEMIES----------------//
            if (enemyTimer == 1000)
                {
                    for (int i = 0; i < enemyloopInt; i++)
                    {
                    int z = random.Next(-2000, -1500);
                    int x = random.Next(-200, 200);
                    enemy = new Enemy(mSceneMgr);
                    enemy.EnemyPhysObj.Translate(new Vector3(x, 0, z));
                    enemy.attachToSceneGraph(mSceneMgr.RootSceneNode);
                    physObjList.Add(enemy.EnemyPhysObj);
                    enemyTimer = 0;

                    enemyList.Add(enemy);
                }
            }
            for (int i = 0; i < enemylistsize; i++)
            {
                
                enemy.setSpeeding(enemyspeed);
                enemyList[i].animate(evt);
                if (enemyList[i].EnemyPhysObj.Hit == true)
                {
                    enemyList[i].remove();
                    physObjList.Remove(enemyList[i].EnemyPhysObj);
                    
                    enemyList[i].EnemyPhysObj.Hit = false;
                }

                if (enemyList[i].EnemyPhysObj.Position.z > 400)
                {
                    enemyList[i].remove();
                    physObjList.Remove(enemyList[i].EnemyPhysObj);
                }
            }
            for (int p = 0; p < projlistsize; p++)
            {
                if (projectileList[p].ProjectilePhysObj.Hit == true)
                {
                    score++;
                    levelup = true;
                    physObjList.Remove(projectileList[p].ProjectilePhysObj);
                    projectileList[p].remove();
                    projectileList[p].ProjectilePhysObj.Hit = false;
                }
                if (projectileList[p].ProjectilePhysObj.Position.z < -1000)
                {
                    physObjList.Remove(projectileList[p].ProjectilePhysObj);
                    projectileList[p].remove();
                }
            }
            //--Scores And Level Checks--//
            if (score.Equals(5)&& levelup == true)
            {
                levelNum = 2;
                enemyspeed = 500;
                enemyloopInt = 2;
                enemy.setSpeeding(enemyspeed);
                levelup = false;
            }
            if (score.Equals(10) && levelup == true)
            {
                levelNum = 3;
                enemyloopInt = 3;
                enemyspeed = 700;
                enemy.setSpeeding(enemyspeed);
                
                levelup = false;
            }

            if (score.Equals(15) && levelup == true)
            {
                levelNum = 4;
                enemyloopInt = 4;
                enemyspeed = 900;
                enemy.setSpeeding(enemyspeed);

                levelup = false;
            }
            if (score.Equals(20) && levelup == true)
            {
                levelNum = 5;
                enemyspeed = 1000;
                enemy.setSpeeding(enemyspeed);

                levelup = false;
            }
            if (score.Equals(25) && levelup == true)
            {
                levelNum = 6;
                enemyspeed = 1200;
                enemy.setSpeeding(enemyspeed);

                levelup = false;
            }
    //---------------------------------//
    //---PICKUPS----------------//
            //int randX = random.Next(-200, 200);
            if (globaltimer ==700)
            {
                boxtimer = 0;
                ammoboxtimer = 0;
                //--health--//
                cube = new Cube(mSceneMgr);
                cube.makeCube("boxhealth" + count, "healthBoxTexture", 50f, 50f, 50f, randX-100, 700f, -100f);
                cube.attachToSceneGraph(mSceneMgr.RootSceneNode);
                cubeListHealth.Add(cube);
                physObjList.Add(cube.CubePhysObj);
                physObjList.Add(cube.CubeEl);
                cube.CubeEl.Hit = false;
            }

            if (globaltimer == 800)
            {
                //--ammo--//
                cube = new Cube(mSceneMgr);
                cube.makeCube("boxammo" + count, "ammoBoxTexture", 50, 50, 50, randX + 100, 700f, -100f);
                cube.attachToSceneGraph(mSceneMgr.RootSceneNode);
                cubeListAmmo.Add(cube);
                cube.CubePhysObj.Hit = false;
                physObjList.Add(cube.CubePhysObj);
                physObjList.Add(cube.CubeEl);
                healthremoved = false;
            }

           int boxX = random.Next(-200, 0);
           //--Healthbox Sequential Creation & Check--//
            if (boxtimer == 10 && healthremoved == true)
            {
                cube = new Cube(mSceneMgr);
                cube.makeCube("boxes" + boxNo, "healthBoxTexture", 50, 50, 50, boxX - 100, 700f, -100f);
                cube.attachToSceneGraph(mSceneMgr.RootSceneNode);
                cubeListHealth.Add(cube);
                physObjList.Add(cube.CubePhysObj);  
            }
            //--Ammo Sequential Creation & Check--//
            if (ammoboxtimer == 10 && ammoremoved == true)
            {
                int ammoboxX = random.Next(-200, 200);
                cube = new Cube(mSceneMgr);
                cube.makeCube("boxammo" + boxNo, "ammoBoxTexture", 50, 50, 50, boxX + 100, 700f, -100f);
                cube.attachToSceneGraph(mSceneMgr.RootSceneNode);
                cubeListAmmo.Add(cube);
                physObjList.Add(cube.CubePhysObj);
            }
            
            for (int ammu = 0; ammu < cubeListAmmo.Count; ammu++)
            {
                if (cubeListAmmo[ammu].CubePhysObj.Hit == true)
                {
                    physObjList.Remove(cubeListAmmo[ammu].CubePhysObj);
                    physObjList.Remove(cubeListAmmo[ammu].CubeEl);
                    
                    cubeListAmmo[ammu].remove();
                    ammoremoved = true;
                    ammoboxtimer = 0;
                    ammoincrease = true;
                    
                    cubeListAmmo[ammu].CubePhysObj.Hit = false;
                    cubeListAmmo[ammu].CubeEl.Hit = false;

                }
            }
            for (int bb = 0; bb < cubeListHealth.Count; bb++)
            {
                if (cubeListHealth[bb].CubePhysObj.Hit == true)
                {
                    physObjList.Remove(cubeListHealth[bb].CubePhysObj);
                    physObjList.Remove(cubeListHealth[bb].CubeEl);
                    cubeListHealth[bb].remove();
                    healthremoved = true;
                    boxtimer = 0;
                    healthcount = healthcount + 5;
                    cubeListHealth[bb].CubePhysObj.Hit = false;
                    cubeListHealth[bb].CubeEl.Hit = false;
                }
            }
            if (levelNum > 3)
            {
                powerup = true;
            }
            else
            {
                powerup = false;
            }

            if (ammoincrease == true)
            {
                ammocount = ammocount+10;
                enemyTimer = 900;
                ammoincrease = false;
            }
            if (healthcount < 5)
            {
                hud.gameOver();
                CompositorManager.Singleton.AddCompositor(mCamera.Viewport, "fragmentShader");
                CompositorManager.Singleton.SetCompositorEnabled(mCamera.Viewport, "fragmentShader", true);
            }

            if (player.PlayerPhysObj.Hit == true)
            {
                Console.WriteLine("PLAYER HIT");
                player.PlayerPhysObj.Hit = false;
            }
            phys.updatePhysics(0.04f, physObjList, boundariesList);
        }

//---------------------------------//
//---VIEWING----------------//
        protected override void CreateCamera()
        {
            mCamera = mSceneMgr.CreateCamera("PlayerCam");
            mCamera.Position = new Vector3(0, 200, 500);
            mCamera.LookAt(new Vector3(0, 200, 0));
            mCamera.NearClipDistance = 5;
            mCamera.FarClipDistance = 1000;
            mCamera.FOVy = new Degree(75);
            mCameraMan = new CameraMan(mCamera);
            mCameraMan.Freeze = true;
           
        }
        
        protected override void CreateViewports()
        {
            Viewport viewport = mWindow.AddViewport(mCamera);
            mCamera.AspectRatio = viewport.ActualWidth / viewport.ActualHeight;
            //--FOG--//
            ColourValue fadeColour = new ColourValue(0f, 0f, 0f);
            //mSceneMgr.SetFog(FogMode.FOG_EXP2, fadeColour, 0.0005f, 100, -200);
            mSceneMgr.SetFog(FogMode.FOG_LINEAR, fadeColour, 0.00008f, 800, 2500);
            mWindow.GetViewport(0).BackgroundColour = fadeColour;
            mCamera.FocalLength = 300f;
            
        }

//---------------------------------//
//---INPUTS----------------//

        protected override void CreateFrameListeners()
        {
            base.CreateFrameListeners();
            mRoot.FrameRenderingQueued +=
                new FrameListener.FrameRenderingQueuedHandler(ProcessInput);
        }
        protected override void InitializeInput()
        {
            base.InitializeInput();
            int windowHandle;
            mWindow.GetCustomAttribute("WINDOW", out windowHandle);
            mInputMgr = MOIS.InputManager.CreateInputSystem((uint)windowHandle);
            mKeyboard = (MOIS.Keyboard)mInputMgr.CreateInputObject(MOIS.Type.OISKeyboard, true);
            mMouse = (MOIS.Mouse)mInputMgr.CreateInputObject(MOIS.Type.OISMouse, false);
            mKeyboard.KeyPressed += new MOIS.KeyListener.KeyPressedHandler(OnMyKeyPressed);
        }

        bool ProcessInput(FrameEvent evt)
        {
            Vector3 displacements = Vector3.ZERO;
            Vector3 angles = Vector3.ZERO;
            float sensitivity = 0.07f;
            mKeyboard.Capture();
            mMouse.Capture();
            

            if (mKeyboard.IsKeyDown(MOIS.KeyCode.KC_A))
                displacements.x += -0.2f;
            if (mKeyboard.IsKeyDown(MOIS.KeyCode.KC_D))
                displacements.x += 0.2f;

            player.Move(displacements);

            if (mKeyboard.IsKeyDown(MOIS.KeyCode.KC_Q))
                angles.y += sensitivity;
            if (mKeyboard.IsKeyDown(MOIS.KeyCode.KC_E))
                angles.y += -sensitivity;
            if (mKeyboard.IsKeyDown(MOIS.KeyCode.KC_S))
                angles.x += -sensitivity;
            if (mKeyboard.IsKeyDown(MOIS.KeyCode.KC_W))
                angles.x += sensitivity;

            if (mMouse.MouseState.ButtonDown(MOIS.MouseButtonID.MB_Left))
            {
                angles.y = -mMouse.MouseState.X.rel/1.6f;
                angles.x = -mMouse.MouseState.Y.rel/1.6f;
            }
            player.Rotate(Vector3.UNIT_X, Vector3.UNIT_Y, angles.x * sensitivity, angles.y * sensitivity, Node.TransformSpace.TS_PARENT, Node.TransformSpace.TS_WORLD);
            return true;
        } 

        protected bool OnMyKeyPressed(MOIS.KeyEvent arg)
        {
            Vector3 displacements = Vector3.ZERO;
            switch (arg.key)
            {
                case MOIS.KeyCode.KC_SPACE:
                    {
                        if (ammocount <1)
                        {
                            hud.NoAmmo();
                            break;   
                        }
                        else
                        {
                            ball = new Projectile(mSceneMgr);
                            physObjList.Add(ball.ProjectilePhysObj);
                            player.FireDirection.Normalise();
                            Vector3 position = player.PlayerPhysObj.Position + player.cannon.CannonNode.Position * 0.1f + player.FireDirection * 50;
                            ball.setPos(position);
                            ball.setInitialDir(player.FireDirection);
                            projectileList.Add(ball);
                            ball.attachToSceneGraph(mSceneMgr.RootSceneNode);


                            if (powerup == true)
                            {
                                Matrix3 rot = new Matrix3();
                                rot.FromAxisAngle(Vector3.UNIT_Y, new Radian(new Degree(-10)));
                                ball2 = new Projectile(mSceneMgr);
                                physObjList.Add(ball2.ProjectilePhysObj);
                                player.FireDirection.Normalise();
                                Vector3 changeDir2 = rot * player.FireDirection;
                                Vector3 position2 = new Vector3(ball2.ProjectilePhysObj.Radius + 10, 0, 0) + player.PlayerPhysObj.Position + player.cannon.CannonNode.Position * 0.1f + changeDir2 * 50;
                                ball2.setPos(position2);
                                ball2.setInitialDir(changeDir2);
                                projectileList.Add(ball2);
                                ball2.ProjectileNode.Scale(0.8f, 0.8f, 0.8f);
                                ball2.attachToSceneGraph(mSceneMgr.RootSceneNode);

                                rot.FromAxisAngle(Vector3.UNIT_Y, new Radian(new Degree(10)));
                                ball3 = new Projectile(mSceneMgr);
                                physObjList.Add(ball3.ProjectilePhysObj);
                                player.FireDirection.Normalise();
                                Vector3 changeDir3 = rot * player.FireDirection;
                                Vector3 position3 = new Vector3(-ball3.ProjectilePhysObj.Radius - 10, 0, 0) + player.PlayerPhysObj.Position + player.cannon.CannonNode.Position * 0.1f + changeDir3 * 50;
                                ball3.setPos(position3);
                                ball3.setInitialDir(changeDir3);
                                projectileList.Add(ball3);
                                ball3.ProjectileNode.Scale(0.8f, 0.8f, 0.8f);
                                ball3.attachToSceneGraph(mSceneMgr.RootSceneNode);
                            }
                            count++;
                            ammocount--;
                            break;
                        }
                    }
                    
                case MOIS.KeyCode.KC_ESCAPE:
                    {
                        return false;
                    }
            }
            return true;
        }
    }
}
    
