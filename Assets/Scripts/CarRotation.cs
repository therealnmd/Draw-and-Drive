using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRotation : MonoBehaviour
{
    //[Header("Cấu hình raycast")]
    //public float rayLength = 1.5f;
    //public LayerMask groundLayer;

    //[Header("Cấu hình xoay mượt")]
    //public float rotationSpeed = 5f;

    //void Update()
    //{
    //    // Bắn ray từ vị trí xe xuống
    //    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, rayLength, groundLayer);

    //    if (hit.collider != null)
    //    {
    //        Vector2 normal = hit.normal;
    //        Vector2 tangent = new Vector2(normal.y, -normal.x);

    //        // Đảm bảo luôn hướng phải (không quay về trái)
    //        if (tangent.x < 0)
    //            tangent = -tangent;

    //        // Tính góc cần xoay quanh trục Z
    //        float angle = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg;

    //        // Tạo góc hiện tại giữ Y = 180, chỉ thay đổi Z
    //        Vector3 currentEuler = transform.eulerAngles;
    //        float newZ = Mathf.LerpAngle(currentEuler.z, angle, rotationSpeed * Time.deltaTime);
    //        transform.eulerAngles = new Vector3(0, 180, newZ); // giữ Y = 180
    //    }
    //    else
    //    {
    //        // Không chạm đất => vẫn giữ Y = 180, Z dần về 0
    //        Vector3 currentEuler = transform.eulerAngles;
    //        float newZ = Mathf.LerpAngle(currentEuler.z, 0f, rotationSpeed * Time.deltaTime);
    //        transform.eulerAngles = new Vector3(0, 180, newZ);
    //    }
    //}

    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(transform.position, transform.position + Vector3.down * rayLength);
    //}
    [Header("Cấu hình raycast")]
    public float rayLength = 1.5f; // Chiều dài raycast xuống
    public LayerMask groundLayer;
    public Transform rayOrigin;

    [Header("Cấu hình xoay")]
    public float rotationSpeed = 5f; // Tốc độ xoay

    void Update()
    {
        // Raycast xuống dưới để kiểm tra mặt đất
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin.position, Vector2.down, rayLength, groundLayer);

        if (hit.collider != null)
        {
            // Lấy normal của mặt đất
            Vector2 normal = hit.normal;

            // Tính góc Z để xe nghiêng theo địa hình
            float angleZ = Vector2.SignedAngle(Vector2.up, normal);

            // Nếu xe đang bị flip Y = 180, thì đảo góc Z lại để mũi xe hướng đúng
            if (Mathf.RoundToInt(transform.eulerAngles.y) == 180)
                angleZ = -angleZ;

            // Tạo rotation mới với góc Z phù hợp, giữ nguyên Y
            Quaternion targetRotation = Quaternion.Euler(0f, 180f, angleZ);

            // Xoay mượt
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            // Không chạm đất: quay về Z = 0, giữ Y = 180
            Quaternion targetRotation = Quaternion.Euler(0f, 180f, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    void OnDrawGizmos()
    {
        // Vẽ raycast để debug
        Gizmos.color = Color.red;
        Gizmos.DrawLine(rayOrigin.position, rayOrigin.position + Vector3.down * rayLength);
    }
}
