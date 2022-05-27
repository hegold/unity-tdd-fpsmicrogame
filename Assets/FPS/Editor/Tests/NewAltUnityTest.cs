using NUnit.Framework;
using Altom.AltUnityDriver;
using Unity.FPS.Gameplay;
using System.Linq;

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
        //driver.WaitForObject(By.COMPONENT, "");
        //driver.HoldButton(new AltUnityVector2(), 10.0f);
        //driver.PressKey(AltUnityKeyCode.W, duration: 10.0f);
        var obj = driver.FindObject(By.COMPONENT, "Unity.FPS.Gameplay.PlayerWeaponsManager");
        obj.CallComponentMethod<string>("Unity.FPS.Gameplay.PlayerWeaponsManager", "HACK_ShootWeapon", new object[0]);
    }

}