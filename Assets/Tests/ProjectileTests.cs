using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using Unity.FPS.Game;
using Unity.FPS.Gameplay;

public class ProjectileTests
{
    // A Test behaves as an ordinary method
    //[Test]
    //public void ProjectileTestsSimplePasses()
    //{
    //    // Use the Assert class to test conditions
    //}

    //// A UnityTest allows `yield return null;` to skip a frame.
    //[UnityTest]
    //public IEnumerator NewTestScriptWithEnumeratorPasses()
    //{
    //    // Use the Assert class to test conditions.
    //    // Use yield to skip a frame.
    //    yield return null;
    //}

    // A test with the [RequiresPlayMode] tag ensures that the test is always run inside PlayMode.
    [UnityTest]
    [RequiresPlayMode]
    public IEnumerator NewTestScriptInPlayModeWithEnumeratorPasses()
    {
        // array of collides to ignore
        // PlayerWeaponsManager - weapon camra transform
        var fakePlayer = new GameObject("FakePlayer");
        var camera = fakePlayer.AddComponent<Camera>();
        fakePlayer.AddComponent < PlayerInputHandler>();
        fakePlayer.AddComponent<GameFlowManager>();
        fakePlayer.AddComponent<Health>();
        var actor = fakePlayer.AddComponent<Actor>();
        actor.AimPoint = fakePlayer.transform;
        fakePlayer.AddComponent<ActorsManager>();
        var audioManager = fakePlayer.AddComponent<AudioManager>();
        audioManager.AudioMixers = new UnityEngine.Audio.AudioMixer[0];
        var playerCharacterController = fakePlayer.AddComponent < PlayerCharacterController>();
        playerCharacterController.PlayerCamera = camera;
        var weaponsManager = fakePlayer.AddComponent<PlayerWeaponsManager>();
        weaponsManager.WeaponParentSocket = fakePlayer.transform;
        weaponsManager.WeaponCamera = camera;

        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/FPS/Prefabs/Projectiles/Projectile_Blaster.prefab");
        var obj = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity);
        var projectile = obj.GetComponent<ProjectileBase>();

        projectile.Shoot(fakePlayer, Vector3.right, 1.0f);

        Assert.IsNotNull(obj);

        // TODO: Extension method for Vec3 compare
        //Assert.AreEqual(Vector3.right, obj);

        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;

        

    }
}
