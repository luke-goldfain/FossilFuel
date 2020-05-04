using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    Vector3 projectileSpawn { get; }
    string weaponName { get; }

    void RotateUp();
    void RotateDown();
    void Fire();
}
