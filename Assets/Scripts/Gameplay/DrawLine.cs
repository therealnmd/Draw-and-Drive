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
    private float currentLength = 0f; //tổng chiều dài đường vẽ

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

        //kiểm tra xem con trỏ chuột có đang ở chỗ UI hay k, nếu có thì k cho vẽ
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        //khi nhấn chuột trái thì bắt đầu vẽ
        if (Input.GetMouseButtonDown(0))
        {
            BeginDrawing();
        }

        if (Input.GetMouseButton(0) && isDrawing)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //tính khoảng cách từ điểm cuối cùng đã vẽ đến vị trí chuột hiện tại, nếu khoảng cách đủ lớn thì mới thêm điểm mới
            if (points.Count == 0 || Vector2.Distance(points[points.Count - 1], mousePos) >= minDistance)
            {
                float segmentLength = 0f; //đoạn đường vẽ
                if (points.Count > 0)
                {
                    segmentLength = Vector2.Distance(points[points.Count-1], mousePos);
                }
                else
                {
                    segmentLength = 0f;
                }
                //trừ đi lượng mực = chiều dài đoạn vừa vẽ
                if (InkSystem.Instance.UseInk(segmentLength))
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
            FindObjectOfType<GameManager>().OnFinishDrawing();
            
        }
    }

    //cho phép vẽ
    public void StartDrawing()
    {

        isDrawingEnabled = true;
        isDrawing = false;
    }

    public void StopDrawing()
    {
        isDrawingEnabled = false;
        isDrawing = false;
    }

    //bắt đầu vẽ
    private void BeginDrawing()
    {
        GameObject newLine = Instantiate(linePrefab);
        lineRenderer = newLine.GetComponent<LineRenderer>();
        edgeCollider = newLine.GetComponent<EdgeCollider2D>();
        //khởi tạo danh sách rỗng
        points = new List<Vector2>();
        //tổng chiều dài đường vẽ
        currentLength = 0f;

        lineRenderer.positionCount = 0; //chưa vẽ điểm nào 
        edgeCollider.points = new Vector2[0]; //chưa gán collider vào đâu cả

        isDrawing = true;
    }

    void UpdateLine()
    {
        lineRenderer.positionCount = points.Count;

        for (int i = 0; i < points.Count; i++)
            lineRenderer.SetPosition(i, points[i]);

        //ít nhất 2 điểm tạo 1 đường
        if (points.Count > 1)
        {
            Vector2[] colliderPoints = new Vector2[points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                colliderPoints[i] = lineRenderer.transform.InverseTransformPoint(points[i]);
            }
            //gán các điểm cho collider, để tạo va chạm
            edgeCollider.points = colliderPoints;
        }
    }
}

