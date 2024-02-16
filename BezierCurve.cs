using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : MonoBehaviour
{
    public Vector3 CalculateBezierPoint(Transform[] points, float t)
    {
        int n = points.Length - 1;
        Vector3 result = Vector3.zero;

        for (int i = 0; i <= n; i++)
        {

            float coeff = BinomialCoefficient(n, i) * Mathf.Pow(1 - t, n - i) * Mathf.Pow(t, i);
            result += points[i].position * coeff;
        }

        return result;
    }

    //Binomial Coefficient: C(n, k) = n! / (k! * (n - k)!)
    int BinomialCoefficient(int n, int k)
    {
        int result = 1;

        for (int i = 1; i <= k; i++)
        {
            result *= (n - i + 1);
            result /= i;
        }

        return result;
    }
}
