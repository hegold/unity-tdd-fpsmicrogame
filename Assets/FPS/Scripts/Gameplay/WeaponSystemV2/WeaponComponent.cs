using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponComponent : MonoBehaviour
{
    public Transform MuzzleTransform { get; set; }

    public void Fire()
    {
        var projectileObj = new GameObject("Projectile", typeof(ProjectileComponent));
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
