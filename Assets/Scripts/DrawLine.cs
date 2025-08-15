using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    public float maxInkLength = 10f;
    public float minDistance = 0.1f;

    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;
    private List<Vector2> points = new List<Vector2>();
    private float currentLength = 0f;
    private bool isDrawingEnabled = false; // ✅ Được phép vẽ chưa
    private bool isDrawing = false;
    private bool hasDrawn = false; // ✅ Đã vẽ xong rồi thì không cho vẽ nữa
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDrawingEnabled || hasDrawn) return;

        if (Input.GetMouseButtonDown(0))
        {
            BeginDrawing();
        }
           
        if (Input.GetMouseButton(0) && isDrawing)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (points.Count == 0 || Vector2.Distance(points[points.Count - 1], mousePos) >= minDistance)
            {
                float segmentLength = points.Count > 0 ? Vector2.Distance(points[points.Count - 1], mousePos) : 0;
                if (currentLength + segmentLength <= maxInkLength)
                {
                    points.Add(mousePos);
                    currentLength += segmentLength;
                    UpdateLine();
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && isDrawing)
        {
            isDrawing = false;
            hasDrawn = true;
            FindObjectOfType<GameManager>().OnFinishDrawing();
        }
    }

    public void StartDrawing()
    {
        if (hasDrawn)
            return; // 🚫 Không reset line nếu đã vẽ xong

        isDrawingEnabled = true;
        isDrawing = false;

        points.Clear();
        lineRenderer.positionCount = 0;
        edgeCollider.points = new Vector2[0];
        currentLength = 0f;
    }

    private void BeginDrawing()
    {
        isDrawing = true;
        points.Clear();
        currentLength = 0f;
        lineRenderer.positionCount = 0;
        edgeCollider.points = new Vector2[0];
    }

    void UpdateLine()
    {
        lineRenderer.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
            lineRenderer.SetPosition(i, points[i]);
        edgeCollider.points = points.ToArray();
    }
}

