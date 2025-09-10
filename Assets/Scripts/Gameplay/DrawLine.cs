using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawLine : MonoBehaviour
{
    [Header("Ink")]
    public float maxInkLength = 20f;
    public float minDistance = 0.1f;

    [Header("Lines")]
    public GameObject linePrefab;
    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;
    private List<Vector2> points = new List<Vector2>();
    private float currentLength = 0f;

    private bool isDrawingEnabled = false;
    private bool isDrawing = false;
    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDrawingEnabled) return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

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
                if (InkSystem.Instance.currentInk - segmentLength >= 0)
                {
                    if (InkSystem.Instance.UseInk(segmentLength))
                    {
                        points.Add(mousePos);
                        currentLength += segmentLength;
                        UpdateLine();
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && isDrawing)
        {
            isDrawing = false;
            FindObjectOfType<GameManager>().OnFinishDrawing();
            
        }
    }

    public void StartDrawing()
    {

        isDrawingEnabled = true;
        isDrawing = false;
    }

    private void BeginDrawing()
    {
        GameObject newLine = Instantiate(linePrefab);
        lineRenderer = newLine.GetComponent<LineRenderer>();
        edgeCollider = newLine.GetComponent<EdgeCollider2D>();

        points = new List<Vector2>();
        currentLength = 0f;

        lineRenderer.positionCount = 0;
        edgeCollider.points = new Vector2[0];

        isDrawing = true;
    }

    public void StopDrawing()
    {
        isDrawingEnabled = false; // 🚫 Sau khi bấm Play thì tắt vẽ
    }

    void UpdateLine()
    {
        lineRenderer.positionCount = points.Count;

        for (int i = 0; i < points.Count; i++)
            lineRenderer.SetPosition(i, points[i]);

        // ✅ Cập nhật collider: cần ít nhất 2 điểm
        if (points.Count > 1)
        {
            Vector2[] colliderPoints = new Vector2[points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                colliderPoints[i] = lineRenderer.transform.InverseTransformPoint(points[i]);
            }
            edgeCollider.points = colliderPoints;
        }
    }
}

