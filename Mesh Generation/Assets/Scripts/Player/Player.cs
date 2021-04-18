using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private CameraShake cameraShake;
    [SerializeField]
    private GUIDamage guiDamage;
    [SerializeField]
    private int maxHealth = 100;
    private int currHealth;

    void Awake() {
        SetDefaults();
    }

    public void SetDefaults() {
        currHealth = maxHealth;
    }

    public void Hit(PlayerWeapon weapon) {//tint edges red
        guiDamage.FlashImage();
        cameraShake.StartShake(.05f, .01f);
        TakeDamage(weapon.getDamage());
    }

    private void TakeDamage(int damage) {
        currHealth -= damage;
        Debug.Log(transform.name + " now has " + currHealth + " health.");
        if(currHealth <= 0) {
            //die
        }
    }
}
