using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootProjectile : MonoBehaviour
{
    public Transform GunPoint;
    public GameObject[] _pfBullet;
    public Upgradeables upgrades;

    public float baseSpread = 15;
    public float spreadFactor;

    public MousePosition mousepos;

    bool isCriticalHit;

    private void Start()
    {
        upgrades.GetComponent<Upgradeables>();
    }

    public void ComponentShoot()
    {
        if (upgrades._fireRate == 0)
        {
            //ShootProjectiles(upgrades.NumberOfProjectile);
            Shoot(upgrades.NumberOfProjectile);
        }
        else{
            if (Time.time > upgrades._nextFire && upgrades._fireRate > 0)
            {
                upgrades._nextFire = Time.time + upgrades._fireRate;
                Shoot(upgrades.NumberOfProjectile);
                //StartCoroutine(ShootProjectiles(upgrades.NumberOfProjectile));
            }
 
        }
            
    }

    void Shoot(int numberOfProjectiles)
    {
        spreadFactor = upgrades.SpreadFactor;
        if(numberOfProjectiles >= 2)
        {
            upgrades.SpreadFactor -= ((numberOfProjectiles / 2) + 1) * spreadFactor;
        }
        else
        {
            upgrades.SpreadFactor = 0;
        }
        for (int i = 0; i < numberOfProjectiles; i++)
        {

            GameObject bullet = Instantiate(_pfBullet[0], GunPoint.position, Quaternion.identity);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            bulletScript._upgrades = upgrades;

            //changes value of the bullets before sending it
            bulletScript.Damage = CalculateDamage();
            bulletScript.Speed = upgrades.projectileSpeed;
            bulletScript.isCritical = isCriticalHit;
            bulletScript.isRicochet = upgrades.Ricochet;
            bulletScript.explosiveArea = upgrades.ExplosionArea;
            bulletScript.freezeTime = upgrades.FreezeTime;
            bulletScript._playerHealth = upgrades.pHealth;

            //set bonus bools
            bulletScript.isArmorPiercer = upgrades.ArmorPiercer;
            bulletScript.isMegaRicochet = upgrades.MegaRicochet;
            bulletScript.isExplosionMagnet = upgrades.ExplosionMagnet;
            bulletScript.isSeeking = upgrades.Seeking;
            bulletScript.isLifeSteal = upgrades.LifeSteal;
            bulletScript.isUltraFreeze = upgrades.UltraFreeze;

            bullet.transform.localScale = upgrades.ProjectileSize;

            //sends bullet in the direction the bullet is facing, bullet is facing towards cursor when fired
            

            bullet.transform.LookAt(mousepos.WorldPosition);

            bullet.transform.Rotate(0, upgrades.SpreadFactor, 0);

            Vector3 ShootDirection = bullet.transform.forward;
            bullet.GetComponent<Rigidbody>().AddForce(ShootDirection * upgrades.projectileSpeed, ForceMode.VelocityChange);

            upgrades.SpreadFactor += spreadFactor;

            //destorys bulet after its lifetime has passed
            Destroy(bullet, upgrades.ProjectileLifeTime);
            
        }
        upgrades.SpreadFactor = spreadFactor;
    }


    private float CalculateDamage()
    {
        float calCritChance = Random.Range(1, 100);
        if(upgrades.critChance >= calCritChance)
        {
            upgrades.projectileDamage = Mathf.RoundToInt(upgrades.projectileDamage * upgrades.critDamage);
            isCriticalHit = true;
        }
        else
        {
            upgrades.projectileDamage = upgrades.projectileBaseDamage;
            isCriticalHit = false;
        }
            

        return upgrades.projectileDamage;
    }
}
