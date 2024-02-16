using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurveButton : MonoBehaviour
{
    public bool isBezier = false;
    public bool isBSpline = false;
    public AudioVisualisation audioV;

    public void Bezier()
    {
        isBSpline = false;
        isBezier = true;
        ResetControlPoints();
    }

    public void BSpline()
    {
        isBezier = false;
        isBSpline = true;
        Debug.Log(isBSpline);
        ResetControlPoints();
    }

    private void ResetControlPoints()
    {
        
        if (audioV.initialPositions == null || audioV.initialPositions.Length != audioV.controlPoints.Length)
        {
            Debug.LogError("Initial positions array is not properly initialized.");
            return;
        }

        // reset the position of control points
        for (int i = 0; i < audioV.controlPoints.Length; i++)
        {
            audioV.controlPoints[i].position = audioV.initialPositions[i];
        }
    }

}
