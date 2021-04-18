using UnityEngine;

[System.Serializable]
public class PlayerWeapon//MAKE THE DEFAULT WEAPON THE LASER GUN, HAVE CHARGE UP TIME AND WHILE CHARGING SHOW GROWING LASER LINE
{
    
    [SerializeField]
    private string name = "Pistol";
    [SerializeField]
    private int damage = 10;
    [SerializeField]
    private float range = 100f;
    [SerializeField]
    private int fireRate = 1;
    private bool canShoot = true;

    public void Shot() {
        canShoot = false;
    }

    public void Cooldown() {
        canShoot = true;
    }

    public string getName() {
        return name;
    }

    public int getDamage() {
        return damage;
    }

    public float getRange() {
        return range;
    }

    public int getFireRate() {
        return fireRate;
    }

    public bool getCanShoot() {
        return canShoot;
    }

}
