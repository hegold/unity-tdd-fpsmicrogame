using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class WeaponSystemTests
{
    [Test]
    public void CanCreateWeapon()
    {
        GameObject weaponObj = new GameObject("Weapon", typeof(WeaponComponent));

        GameObject foundObj = GameObject.Find("Weapon");

        Assert.AreEqual(weaponObj, foundObj);
        Assert.AreEqual(weaponObj.GetComponent<WeaponComponent>(), foundObj.GetComponent<WeaponComponent>());
    }

    [Test]
    public void ShouldCreateProjectileWhenFired()
    {
        GameObject weaponObj = new GameObject("Weapon", typeof(WeaponComponent));

        weaponObj.GetComponent<WeaponComponent>().Fire();
        
        GameObject foundObj = GameObject.Find("Projectile");
        Assert.IsNotNull(foundObj);
        Assert.IsNotNull(foundObj.GetComponent<ProjectileComponent>());
    }

    [Test]
    public void ShouldCreateProjectileAtWeaponMuzzle()
    {
        Transform muzzleTransform = new GameObject("Muzzle").transform;
        muzzleTransform.SetPositionAndRotation(new Vector3(1, 2, 3), Quaternion.Euler(1, 2, 3));
        GameObject weaponObj = new GameObject("Weapon", typeof(WeaponComponent));
        weaponObj.GetComponent<WeaponComponent>().MuzzleTransform = muzzleTransform;

        weaponObj.GetComponent<WeaponComponent>().Fire();

        GameObject foundObj = GameObject.Find("Projectile");
        Assert.AreEqual(muzzleTransform.position, foundObj.transform.position);
        Assert.AreEqual(muzzleTransform.rotation, foundObj.transform.rotation);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator WeaponSystemTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
