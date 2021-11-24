using UnityEngine;

public static class RegularPolygon
{
    public static float GetPolygonExtension(float distanceBetweenSides, int numberOfSides)
    {
        float polygonAngle = (numberOfSides - 2) * 180.0f / numberOfSides;
        float beta = polygonAngle / 2.0f;
        float alpha = 90 - beta;
        float b = distanceBetweenSides / 2.0f;
        // Sine rule
        float a = b * Mathf.Sin(Mathf.Deg2Rad * alpha) / Mathf.Sin(Mathf.Deg2Rad * beta);
        // Cos rule
        return Mathf.Sqrt(a * a - 2 * a * b * Mathf.Cos(Mathf.Deg2Rad * 90) + b * b);
    }

    public static float GetPolygonRadius(float apothem, int numberOfSides)
    {
        return apothem / Mathf.Cos(Mathf.Deg2Rad * (180.0f / numberOfSides));
    }

    public static float GetPolygonApothem(float sideLength, float numberOfSides)
    {
        return sideLength / (2.0f * Mathf.Tan(Mathf.Deg2Rad * (180.0f / numberOfSides)));
    }
}
