namespace UI
{
    public class CartCreationUIController : BaseUIController
    {
        public void DisplayCalibrationStep1()
        {
            EnablePanelExclusive(UIConstants.Step1);
            EnableFalse(UIConstants.DoneButton);
        }

        public void DisplayCalibrationStep2()
        {
            EnablePanelExclusive(UIConstants.Step2);
        }

        public void DisplayCalibrationStep3()
        {
            EnablePanelExclusive(UIConstants.Step3);
            EnableTrue(UIConstants.DoneButton);
        }
    }
}
