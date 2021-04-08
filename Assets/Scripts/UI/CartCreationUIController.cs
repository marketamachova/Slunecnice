namespace UI
{
    public class CartCreationUIController : BaseUIController
    {
        public void DisplayCalibrationStep1()
        {
            EnablePanelExclusive("Step1");
            EnableFalse("DoneButton");
        }

        public void DisplayCalibrationStep2()
        {
            EnablePanelExclusive("Step2");
        }

        public void DisplayCalibrationStep3()
        {
            EnablePanelExclusive("Step3");
            EnableTrue("DoneButton");
        }
    }
}
