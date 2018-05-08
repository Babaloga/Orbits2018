using UnityEngine;
using System.Collections;

public class GravityHandler : MonoBehaviour {
    public float gravConstant = -100f;
    public static Attractor[] attractors;

    public static GravityHandler instance;

    private void Start()
    {
        instance = this;
        attractors = FindObjectsOfType<Attractor>();
    }

    public static Vector2 SumFieldStrength(Vector3 objectPos)
    {      
        Vector2 sumFieldStrength = Vector2.zero;

        foreach(Attractor a in attractors)
        {
            float mass = a.mass;
            Vector2 direction = objectPos - a.transform.position;
            float magnitude = direction.magnitude;

            float fieldStrengthScalar = 0;

            if (magnitude != 0)
                fieldStrengthScalar = (instance.gravConstant * mass) / Mathf.Pow(magnitude, 2);
            
            Vector2 fieldStrength = fieldStrengthScalar * direction.normalized;

            sumFieldStrength += fieldStrength;
        }
        return sumFieldStrength;
    }

    public static void RefreshAttractors()
    {
        attractors = FindObjectsOfType<Attractor>();
    }

}
