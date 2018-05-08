using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryPredict: MonoBehaviour
{
    public Color32 lineColor;
    LineRenderer sightLine;
    public int segmentCount = 200;
    public string materialName = "ShipTrajectory";
    public float fadeRate = 10;
    public LayerMask tMask;

    // Length scale for each segment (a measure of distance)
    public float segmentScale = 1;

    public void SimulatePath(Vector2 shipPosition, Vector2 shipVelocity, int length, int renderPoints)
    {
        StopCoroutine("FadeOut");

        sightLine = GetComponent<LineRenderer>();

        segmentCount = length;
        Vector2[] segments = new Vector2[segmentCount];
        Material lineMat = (Material)Resources.Load(materialName, typeof(Material));
        
        Color startColor = lineColor;
        Color endColor = startColor;
        startColor.a = 1;
        endColor.a = 0;
        sightLine.startColor = startColor;
        sightLine.endColor = endColor;
        sightLine.material = lineMat;      

        segments[0] = shipPosition;

        // The initial velocity
        Vector2 segVelocity = shipVelocity;

        for (int i = 1; i < segmentCount; i++)
        {
            float segTime = (segVelocity.sqrMagnitude != 0) ? segmentScale / segVelocity.magnitude : 0; //essetially segment distance over segment time = ship velocity 

            //float segTime = Time.fixedDeltaTime * segmentScale;

            segVelocity += GravityHandler.SumFieldStrength(segments[i - 1]) * segTime;

            RaycastHit2D hit = Physics2D.Raycast(segments[i - 1], segVelocity, segmentScale, tMask);
            if (hit)
            {

                segments[i] = segments[i - 1] + segVelocity.normalized * hit.distance;
                segVelocity = segVelocity - GravityHandler.SumFieldStrength(segments[i - 1]) * (segmentScale - hit.distance) / segVelocity.magnitude;
                segVelocity = Vector2.zero;

            }

            else
            {
                
                segments[i] = segments[i - 1] + segVelocity * segTime;
            }
            //print(i + " " + segments.Length);
        }


        sightLine.positionCount = segmentCount / renderPoints;
        for (int i = 0; i < sightLine.positionCount; i++)
            sightLine.SetPosition(i, segments[i * renderPoints]);

        StartCoroutine("FadeOut");
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(0.25f);
        while (sightLine.startColor.a > 0)
        {
            sightLine.startColor = new Color(sightLine.startColor.r, sightLine.startColor.b, sightLine.startColor.g, sightLine.startColor.a - 0.1f);
            yield return new WaitForSeconds(1 / fadeRate);
        }
    }
}