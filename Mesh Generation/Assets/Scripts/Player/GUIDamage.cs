using UnityEngine.UI;
using UnityEngine;

public class GUIDamage : MonoBehaviour
{

    private Image hurtImage;
    private bool flash = false;
    private int maxTime = 1;
    private float time = 0;

    void Start()
    {
        hurtImage = GameObject.Find("HurtImage").GetComponent<Image>();
    }

    void Update() {
        if(flash) {
            hurtImage.enabled = true;
            time += Time.deltaTime;
            if(time >= maxTime) {
                flash = false;
                time = 0;
            }
        } else {
            hurtImage.enabled = false;
        }
    }

    public void FlashImage() {
        flash = true;
    }

}
