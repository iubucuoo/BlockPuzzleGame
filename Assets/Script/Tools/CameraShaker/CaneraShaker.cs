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
    Transform UIRoot;
    Vector3 originalV3;
    Vector3 changeV3;
    // Start is called before the first frame update
    void Start()
    {
        UIRoot = UIManager.Inst.UIRoot;
        originalV3 = UIRoot.position;
    }

    IEnumerator Shake(float duration, float magnitude)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            changeV3.x = Random.Range(-1f, 1f) * magnitude;
            changeV3.y = Random.Range(-1f, 1f) * magnitude;
            UIRoot.position = changeV3;
            elapsed += Time.deltaTime;
            yield return 0;
        }
        UIRoot.position = originalV3;
    }
    public void PlayShake()
    {
        Debug.LogError("shake");
        StartCoroutine(Shake(.15f, 20));
    }
}
