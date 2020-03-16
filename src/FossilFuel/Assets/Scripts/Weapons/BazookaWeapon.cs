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
        projectileSpawn = new Vector3(this.transform.position.x + 0.3f, this.transform.position.y + 0.3f, this.transform.position.z);
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
