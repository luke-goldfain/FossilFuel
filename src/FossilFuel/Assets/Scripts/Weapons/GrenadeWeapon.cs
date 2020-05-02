using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeWeapon : AbstractWeapon
{
    public GrenadeWeapon()
    {

    }

    private void Update()
    {
        projectileSpawn = this.transform.position + this.transform.right * 0.4f;
    }

    public override void Fire()
    {
        currentProjectile = Instantiate(projectilePrefab, this.transform);

        currentProjectile.transform.position = projectileSpawn;

        currentProjectile.transform.parent = null;

        currentProjectile.GetComponent<Rigidbody>().velocity = (ShootPower * this.transform.right);

        // Reset shoot power for next shot
        ShootPower = 0f;
    }
}
