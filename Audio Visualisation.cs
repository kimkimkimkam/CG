using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class AudioVisualisation : MonoBehaviour
{
    public Transform[] controlPoints = new Transform[9];//control points array
    public Vector3[] initialPositions;
    public AudioSource[] songs = new AudioSource[3];//songs array
   
    public LineRenderer lineRenderer;
    AudioSource currentSong;
    //public float movementSpeed = 5.0f; // point moving speed
    public float movementAmplitude = 6.0f; // points moving amplitude
    public float sensitivity = 5.0f; //use to adjust the influence of audio amplitude on the vertical movement of the control points

    private float[] timeOffsets;//set the time offsets for each points

    public CurveButton curvebutton;
    public BezierCurve beziercurve;

    public BSplineRef bspline;
    public LineRenderer lineRendererPrefab; // Prefab of the LineRenderer for the curve
    public int numberOfPoints = 100; // Number of points to sample on the curve

    [Header("UI")]
    public TMP_Dropdown songDropdown;

    void Start()
    {
        StartCoroutine(timeOffset());
        currentSong = songs[0];
        currentSong.Play();
        songDropdown.onValueChanged.AddListener(OnSongDropdownChanged);

        //to document the initial positions
        initialPositions = new Vector3[controlPoints.Length];

        
        for (int i = 0; i < controlPoints.Length; i++)
        {
            initialPositions[i] = controlPoints[i].position;
        }
    }

    IEnumerator timeOffset()
    {
        yield return new WaitForSeconds(0.05f);
        MoveControlPoints();


        DrawCurve();


        StartCoroutine(timeOffset());
    }

    public void SetMovementAmplitude(float value)
    {
        movementAmplitude = value;
    }

    //play with the control points
    //color, scale, position
    public void MoveControlPoints()
    {
        //if no button is clicked, the points will not move
        if (!curvebutton.isBezier && !curvebutton.isBSpline)
        {
            return;
        }

        float[] spectrumData = new float[256];
        //use drop down ui button to choose the current song, so this line will get thhe spectrum data of the current song
        GetCurrentSong().GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);//to get the spectrum data of the song

        
        

        for (int i = 0; i < controlPoints.Length; i++)
        {
                float amplitude = spectrumData[i % spectrumData.Length];
            //------------------------------------------------------------------------------------
            //color

                Color color = Color.Lerp(UnityEngine.Color.black, Color.white, amplitude*10);
                //change the color of control point
                controlPoints[i].GetComponent<SpriteRenderer>().color = color;
            //------------------------------------------------------------------------------------
            //scale
                //get the sprite renderer of control points
                SpriteRenderer spriteRenderer = controlPoints[i].GetComponent<SpriteRenderer>();
                //link scale of control points to amplitude
                float scaleFactor = 1.0f + amplitude;
                //rescale points
                spriteRenderer.size *= scaleFactor;

            //------------------------------------------------------------------------------------
            // Movement

                float verticalMovement = amplitude * sensitivity;
                controlPoints[i].position = new Vector3(controlPoints[i].position.x, verticalMovement * movementAmplitude, controlPoints[i].position.z);
           
        
    }
    }

    void DrawCurve()
    {
        //click button to choose which curve do you want to draw
        //if you click bezier button

        if (curvebutton.isBezier)
        {
            //draw bezier curve
            int resolution = 100;
            var points = new List<Vector3>();

            for (int i = 0; i <= resolution; i++)
            {
                float t = i / (float)resolution;



                Vector3 point = beziercurve.CalculateBezierPoint(controlPoints, t);
                points.Add(point);
            }

            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPositions(points.ToArray());
        }
        //if you click BSpline button

        if (curvebutton.isBSpline)
        {
            // Draw B-spline curve
            int resolution = 100;
            var points = new List<Vector3>();

            for (int i = 0; i <= resolution; i++)
            {
                float t = i / (float)resolution;
                Vector3 point = bspline.CalculateBSplinePoint(controlPoints, t);
                points.Add(point);
            }
            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPositions(points.ToArray());

        }

    }

    public AudioSource GetCurrentSong()
    {
        int selectedIndex = songDropdown.value;
        return songs[selectedIndex];
    }

    public void OnSongDropdownChanged(int index)
    {
        currentSong.Stop();//stop the current song

        currentSong = GetCurrentSong();//assign new song to current song

        currentSong.Play();//play new current song
    }

}
