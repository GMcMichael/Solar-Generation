//using Mirror;
using System.Collections;
using UnityEngine;


public class NetworkedCameraShake : MonoBehaviour
{
    /*[SerializeField]
    private Camera camera;

    [ClientRpc]
    public void StartShake(float duration, float magnitude)
    {
        StartCoroutine(RpcShake(duration, magnitude));
    }

    public IEnumerator RpcShake(float duration, float magnitude)
    {
        Vector3 originalPos = camera.transform.localPosition;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1) * magnitude;
            float y = Random.Range(-1f, 1) * magnitude;
            camera.transform.localPosition = new Vector3(x, y, originalPos.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
        camera.transform.localPosition = originalPos;
    }*/
}
