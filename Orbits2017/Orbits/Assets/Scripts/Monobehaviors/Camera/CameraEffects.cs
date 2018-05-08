using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;
using UnityEngine.PostProcessing;

public class CameraEffects : MonoBehaviour {

    public float brightness = 1f;
    public float contrast = 1f;
    public float intensity = 0f;
    public float blur = 0f;

    Vector3 moveRecord;
    BrightnessEffect bright;
    BloomOptimized bloom;


    // Use this for initialization
    void Start () {
        bright = GetComponent<BrightnessEffect>();
        bloom = GetComponent<BloomOptimized>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        bright._Brightness = brightness;
        bright._Contrast = contrast;
        bloom.intensity = intensity;
        bloom.blurSize = blur;

        ChromaticAberrationModel.Settings c = GetComponent<PostProcessingBehaviour>().profile.chromaticAberration.settings;
        if (c.intensity > 0)
        {
            c.intensity -= 5f * Time.fixedDeltaTime;
            Camera.main.GetComponent<PostProcessingBehaviour>().profile.chromaticAberration.settings = c;
        }
    }

    public IEnumerator Shake(float magnitude, float duration)
    {
        print("shake " + magnitude);
        magnitude = magnitude / 10;
        duration = duration / 10;
        float elapsed = 0.0f;

        print(magnitude);

        moveRecord = Vector3.zero;

        bool everyOther = true;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            Vector3 move = Random.insideUnitSphere * magnitude;
            //print(move.magnitude);
            Quaternion rot = Quaternion.Euler(0, 0, transform.rotation.z + (Random.Range(-2, 2) * 0.5f *  magnitude));
            move.z = 0;

            if (everyOther)
                move = move * -1;

            everyOther = !everyOther;

            if (moveRecord.magnitude > magnitude)
            {
                move = -moveRecord.normalized * magnitude * 1.5f;
            }

            float rate = 0;
            while (rate <= 1)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, transform.localPosition + move, rate);
                transform.rotation = Quaternion.Lerp(transform.rotation, rot, rate);
                rate += 0.5f;
                yield return new WaitForSeconds(0.001f);
            }

            moveRecord += move;           
        }
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.localPosition = new Vector3(0, 0, -10);
        yield return null;
    }
}
