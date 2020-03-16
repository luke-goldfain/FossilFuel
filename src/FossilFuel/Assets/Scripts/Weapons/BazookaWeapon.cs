using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaWeapon : AbstractWeapon
{
    public BazookaWeapon()
    {
       
    }

    private void Update()
    {
        projectileSpawn = this.transform.position + this.transform.right * 0.4f;
    }

    public void ChargeShot()
    {
        if (shootPower < maxShootPower)
        {
            shootPower += shootPowerIncrement;
        }
    }

    public override void Fire()
    {
        currentProjectile = Instantiate(projectilePrefab, this.transform);

        currentProjectile.transform.position = projectileSpawn;

        currentProjectile.transform.parent = null;

        currentProjectile.GetComponent<Rigidbody>().velocity = (shootPower * this.transform.right); // May only shoot horizontally
    }
}
