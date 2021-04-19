namespace UI
{
    public abstract class DisplayStrategy
    {
        protected readonly UIControllerMobile UIController;
        
        protected DisplayStrategy(UIControllerMobile uiController)
        {
            UIController = uiController;
        }

        public void DisplaySelf(bool portrait)
        {
            if (portrait)
            {
                DisplaySelfPortrait();
            }
            else
            {
                DisplaySelfLandscape();
            }
        }
        
        protected virtual void DisplaySelfPortrait(){}
        protected virtual void DisplaySelfLandscape(){}
    }
}