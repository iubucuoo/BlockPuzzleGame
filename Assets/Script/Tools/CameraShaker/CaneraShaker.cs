using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaneraShaker : MonoBehaviour
{
    public static CaneraShaker Inst;
    private void Awake()
    {
        Inst = this;
    }
    public GameObject mainCamera;
    // Start is called before the first frame update
    void Start()
    {

    }

    int fd = 50;
    IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 orignalPosition = mainCamera.transform.position;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float x = Random.Range(-1f*fd, 1f * fd) * magnitude;
            float y = Random.Range(-1f * fd, 1f * fd) * magnitude;
            mainCamera.transform.position = new Vector3(x, y, 0);
            elapsed += Time.deltaTime;
            yield return 0;
        }
        mainCamera.transform.position = orignalPosition;
    }
   public void PlayShake()
    {
        Debug.LogError("shake");
        StartCoroutine(Shake(.15f, .4f));
    }
}
