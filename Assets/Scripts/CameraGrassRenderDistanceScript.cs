using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteAlways]
public class CameraGrassRenderDistanceScript : MonoBehaviour
{
    [SerializeField]
    float grassRenderDistance = 30f;

    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Application.isPlaying)
        {
            float[] distances = new float[32];
            distances[14] = grassRenderDistance;
            cam.layerCullDistances = distances;
            
        }
        //else
        //{
        //    Camera cam = SceneView.GetAllSceneCameras()[0];
        //    float[] d = new float[32];
        //    d[14] = grassRenderDistance;
        //    cam.layerCullDistances = d;
        //}
    }

}
