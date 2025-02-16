using UnityEngine;

public class ChangeLane : MonoBehaviour
{
    public float laneDistance = 1f; // Khoảng cách giữa các lane

    public void PositionLane()
    {
        int randomLane = Random.Range(-1, 2); // Chọn ngẫu nhiên -1, 0, hoặc 1
        float targetX = randomLane * laneDistance; // Đặt vị trí x theo khoảng cách lane
        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
    }
}
