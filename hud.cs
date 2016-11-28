using System;
using Mogre;
using Mogre.TutorialFramework;


namespace Mogre.Tutorials
{
    public class HUD
    {


        private Overlay overlay;
        private OverlayElement scoreUI, ammoUI, healthUI, gameover, bg, levelUI, noAmmo;

        public HUD(SceneManager mSceneMgr)
        {
            overlay = OverlayManager.Singleton.GetByName("Overlay");
            overlay.Show();
            bg = OverlayManager.Singleton.GetOverlayElement("BG");
            scoreUI = OverlayManager.Singleton.GetOverlayElement("ScoreOverlay");
            noAmmo = OverlayManager.Singleton.GetOverlayElement("NoAmmo");
            noAmmo.Caption = "NO AMMO!";
            noAmmo.Hide();
            levelUI = OverlayManager.Singleton.GetOverlayElement("LevelOverlay");
            ammoUI = OverlayManager.Singleton.GetOverlayElement("AmmoOverlay");
            healthUI = OverlayManager.Singleton.GetOverlayElement("HealthOverlay");
            gameover = OverlayManager.Singleton.GetOverlayElement("GameOver");
            gameover.Caption = "GAME OVER!";
            gameover.Hide();
        }
        public void NoAmmo()
        {
            noAmmo.Show();
        }
        public void gameOver()
        {
            gameover.Show();
        }
        public void setHealthCaption(int health)
        {
            healthUI.Caption = "" + health + "%";
        }
        public void setScoreCaption(int score)
        {
            scoreUI.Caption = "" + score;
        }
        public void setLevelCaption(int level)
        {
            levelUI.Caption = "Level: " + level;
        }
        public void setAmmoCaption(int ammocount)
        {
            ammoUI.Caption = "" + ammocount;
        }
    }
}
