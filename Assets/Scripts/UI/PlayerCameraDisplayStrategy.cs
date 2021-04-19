namespace UI
{
    public class PlayerCameraDisplayStrategy : DisplayStrategy
    {
        public PlayerCameraDisplayStrategy(UIControllerMobile uiController) : base(uiController)
        {
        }

        protected override void DisplaySelfPortrait()
        {
            UIController.EnablePanelExclusive(UIConstants.WatchScreenPortrait);
            UIController.ActivateExclusive(UIConstants.PlayerCameraButton);
            UIController.EnableCameraView(UIConstants.PlayerCameraRT);
        }

        protected override void DisplaySelfLandscape()
        {
            UIController.EnablePanelExclusive(UIConstants.WatchScreenLandscape);
            UIController.ActivateExclusive(UIConstants.PlayerCameraButton);
            UIController.EnableCameraView(UIConstants.PlayerCameraRTLandscape);
        }
    }
}