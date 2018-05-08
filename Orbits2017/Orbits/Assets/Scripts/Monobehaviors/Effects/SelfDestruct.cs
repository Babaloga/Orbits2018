using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour
{
    public float countdownTime;

    void Start()
    {
        StartCoroutine(Camera.main.GetComponent<CameraEffects>().Shake(5f, 1f));
        StartCoroutine("SelfDestructSequence");
    }
    IEnumerator SelfDestructSequence()
    {
        yield return new WaitForSeconds(countdownTime);
        Destroy(gameObject);
    }
}