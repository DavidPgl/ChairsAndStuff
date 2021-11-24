using System;
using UnityEngine;

public class ChairVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject seat;
    [SerializeField] private LineRenderer circleRenderer;
    [SerializeField] private LineRenderer polygonRenderer;

    [Header("Settings")] [SerializeField] private bool fixSeats = true;
    [SerializeField] [Range(0, 20)] private int numberOfSeats = 5;
    [SerializeField] [Range(0, 2)] private float seatWidth = 1.0f;
    [SerializeField] [Range(0, 2)] private float distance = 1.0f;
    [SerializeField] [Range(0, 2)] private float radius = 1.0f;

    [SerializeField] private float minRadius = 0.0f;


    private GameObject[] _seats = new GameObject[0];

    private void OnDisable()
    {
        foreach (GameObject toDelete in _seats)
        {
            DestroyImmediate(toDelete);
        }
    }
    
    private void Update()
    {
        if (!circleRenderer || !polygonRenderer)
            return;

        if (numberOfSeats < 3 || seatWidth < 0 || distance < 0 || radius < 0)
            return;

        float polygonRadius = 0.0f;
        float polygonApothem = 0.0f;


        if (fixSeats)
        {
            float extension = RegularPolygon.GetPolygonExtension(distance, numberOfSeats);
            // Required polygon side length
            float sideLength = extension + extension + seatWidth;
            float minPolygonApothem = RegularPolygon.GetPolygonApothem(sideLength, numberOfSeats);

            polygonApothem = Mathf.Max(minPolygonApothem, radius);
            // Calculate radius from new apothem
            polygonRadius = RegularPolygon.GetPolygonRadius(polygonApothem, numberOfSeats);
        }
        else
        {
            int possibleNumberOfSeats = 3;
            while (true)
            {
                float extension = RegularPolygon.GetPolygonExtension(distance, possibleNumberOfSeats + 1);
                // Required polygon side length
                float sideLength = extension + extension + seatWidth;
                // Calculate minimum apothem and check if it exceeds required radius
                float minPolygonApothem = RegularPolygon.GetPolygonApothem(sideLength, possibleNumberOfSeats + 1);
                if (minPolygonApothem > radius)
                    break;

                ++possibleNumberOfSeats;
            }

            polygonApothem = radius;
            polygonRadius = RegularPolygon.GetPolygonRadius(radius, possibleNumberOfSeats);
            numberOfSeats = possibleNumberOfSeats;
        }
        
        minRadius = polygonApothem;

        DrawPolygon(polygonRadius, numberOfSeats);
        DrawCircle(radius, 30);
        DrawSeats(polygonApothem, numberOfSeats);
    }
    
    private void DrawCircle(float circleRadius, int segments)
    {
        circleRenderer.positionCount = 0;
        circleRenderer.startWidth = 0.01f;
        circleRenderer.endWidth = 0.01f;
        
        float deltaTheta = 2.0f * Mathf.PI / segments;
        float theta = 0.0f;
        for (int i = 0; i < segments + 1; i++)
        {
            ++circleRenderer.positionCount;

            float x = circleRadius * Mathf.Cos(theta);
            float y = circleRadius * Mathf.Sin(theta);

            circleRenderer.SetPosition(i, new Vector3(x, y, 0.0f));

            theta += deltaTheta;
        }
    }

    private void DrawPolygon(float polygonRadius, int segments)
    {
        polygonRenderer.positionCount = 0;
        polygonRenderer.startWidth = 0.01f;
        polygonRenderer.endWidth = 0.01f;
        
        float deltaTheta = 2.0f * (Mathf.PI / segments);
        float theta = 0.0f;
        for (int i = 0; i < segments + 1; i++)
        {
            ++polygonRenderer.positionCount;

            float x = polygonRadius * Mathf.Cos(theta);
            float y = polygonRadius * Mathf.Sin(theta);

            polygonRenderer.SetPosition(i, new Vector3(x, y, 0.0f));

            theta += deltaTheta;
        }
    }

    private void DrawSeats(float polygonApothem, int seats)
    {
        foreach (GameObject toDelete in _seats)
        {
            DestroyImmediate(toDelete);
        }

        _seats = new GameObject[seats];

        float deltaTheta = 2.0f * (Mathf.PI / seats);
        float theta = deltaTheta / 2.0f;
        for (int i = 0; i < seats; i++)
        {
            float x = polygonApothem * Mathf.Cos(theta);
            float y = polygonApothem * Mathf.Sin(theta);

            GameObject newSeat = Instantiate(seat, this.transform);
            newSeat.transform.Rotate(0, 0, Mathf.Rad2Deg * theta);
            newSeat.transform.localScale = new Vector3(seatWidth, seatWidth, 1.0f);
            newSeat.transform.position = new Vector3(x, y, 0);
            _seats[i] = newSeat;

            theta += deltaTheta;
        }
    }
}