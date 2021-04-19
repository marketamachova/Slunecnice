namespace UI
{
    public class MultiviewDisplayStrategy : DisplayStrategy
    {
        public MultiviewDisplayStrategy(UIControllerMobile uiController) : base(uiController)
        {
        }

        protected override void DisplaySelfPortrait()
        {
            UIController.EnablePanelExclusive(UIConstants.MultiviewScreenPortrait);
            UIController.ActivateExclusive(UIConstants.MultiviewButton);
        }

        protected override void DisplaySelfLandscape()
        {
            UIController.EnablePanelExclusive(UIConstants.MultiviewScreenLandscape);
            UIController.ActivateExclusive(UIConstants.MultiviewButton);
        }
    }
}