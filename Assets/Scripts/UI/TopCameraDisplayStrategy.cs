namespace UI
{
    public class TopCameraDisplayStrategy : DisplayStrategy
    {
        public TopCameraDisplayStrategy(UIControllerMobile uiController) : base(uiController)
        {
        }

        protected override void DisplaySelfPortrait()
        {
            UIController.EnablePanelExclusive(UIConstants.WatchScreenPortrait);
            UIController.ActivateExclusive(UIConstants.TopCameraButton);
            UIController.EnableCameraView(UIConstants.TopCameraRT);
        }

        protected override void DisplaySelfLandscape()
        {
            UIController.EnablePanelExclusive(UIConstants.WatchScreenLandscape);
            UIController.ActivateExclusive(UIConstants.TopCameraButton);
            UIController.EnableCameraView(UIConstants.TopCameraRTLandscape);
        }
    }
}