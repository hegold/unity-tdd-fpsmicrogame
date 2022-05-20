using NUnit.Framework;
using Altom.AltUnityDriver;

public class NewAltUnityTest
{
    public AltUnityDriver driver;
    //Before any test it connects with the socket
    [OneTimeSetUp]
    public void SetUp()
    {
        driver =new AltUnityDriver();
    }

    //At the end of the test closes the connection with the socket
    [OneTimeTearDown]
    public void TearDown()
    {
        driver.Stop();
    }

    [Test]
    public void TestStartButton()
    {
        driver.LoadScene("IntroMenu");
        driver.FindObject(By.NAME, "StartButton").Click();
        driver.WaitForCurrentSceneToBe("MainScene");
    }

}