//using Mirror;
using UnityEngine;

public class NetworkedPlayer : MonoBehaviour
{
    /*[SerializeField]
    private CameraShake cameraShake;
    [SerializeField]
    private GUIDamage guiDamage;
    [SerializeField]
    private int maxHealth = 100;
    [SyncVar]//everytime the var changes its pushed to everyone
    private int currHealth;

    void Awake()
    {
        SetDefaults();
    }

    public void SetDefaults()
    {
        currHealth = maxHealth;
    }

    public void Hit(PlayerWeapon weapon)
    {//tint edges red
        guiDamage.FlashImage();
        cameraShake.StartShake(.05f, .01f);
        TakeDamage(weapon.getDamage());
    }

    private void TakeDamage(int damage)
    {
        currHealth -= damage;
        Debug.Log(transform.name + " now has " + currHealth + " health.");
        if (currHealth <= 0)
        {
            //die
        }
    }*/
}
