using System.Collections;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]
    private Camera shakeCamera;

    public void StartShake(float duration, float magnitude) {
        StartCoroutine(RpcShake(duration, magnitude));
    }

    public IEnumerator RpcShake (float duration, float magnitude) {
        Vector3 originalPos = shakeCamera.transform.localPosition;
        float elapsed = 0f;
        while(elapsed < duration) {
            float x = Random.Range(-1f, 1) * magnitude;
            float y = Random.Range(-1f, 1) * magnitude;
            shakeCamera.transform.localPosition = new Vector3(x, y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        shakeCamera.transform.localPosition = originalPos;
    }
    
}
