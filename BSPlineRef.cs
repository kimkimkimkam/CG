using UnityEngine;

public class BSplineRef : MonoBehaviour
{
    // Calculate a point on the B-spline curve
    public Vector3 CalculateBSplinePoint(Transform[] points, float t)
    {
        // Calculate the segment index based on t
        int segmentIndex = Mathf.FloorToInt(t * (points.Length - 3));
        float segmentT = (t * (points.Length - 3)) - segmentIndex;

        // Determine the index of the first control point for the segment
        int startIndex = segmentIndex;

        // Ensure that the startIndex does not exceed the range of the control points array
        if (startIndex < 0)
        {
            startIndex = 0;
        }
        else if (startIndex >= points.Length - 3)
        {
            startIndex = points.Length - 4;
        }

        // Coefficients for the B-spline calculation
        float[] coefficients = new float[] { 1f, 3f, 3f, 1f };

        // Calculate B-spline point
        Vector3 bsplinePoint = Vector3.zero;
        for (int j = 0; j < 4; j++)
        {
            bsplinePoint += points[startIndex + j].position * coefficients[j] * Mathf.Pow(segmentT, j);
        }

        // Apply normalization
        bsplinePoint /= 6f;

        return bsplinePoint;
    }
}
