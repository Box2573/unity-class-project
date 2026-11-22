using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowSmooth : MonoBehaviour
{
    [Tooltip("相機跟隨目標")]
    [SerializeField] Transform player;
    [Tooltip("距離目標最大距離")]
    [SerializeField] float distanceToTarget;
    [Tooltip("起始高度")]
    [SerializeField] float startHieght;
    [Tooltip("移動時間")]
    [SerializeField] float smoothTime;
    [Tooltip("滾輪軸靈敏度")]
    [SerializeField] float sensitivityOffset_Z;
    [Tooltip("最小 Y Offset")]
    [SerializeField] float minOffset_Y;
    [Tooltip("最大 Y Offset")]
    [SerializeField] float maxOffset_Y;
    [Tooltip("目前Y軸高度")]

    Vector3 smoothPosition = Vector3.zero;//下一幀要移動到的目標位置
    Vector3 currentVelocity = Vector3.zero;//下一幀跳躍到的方向
    float lastFrameSpeed;//上一幀移動速度
    float nextFrameSpeed = 0;//下一幀移動速度
    float offset_Y;

    InputController inputController;

    private void Awake()
    {
        inputController = GameManagerSingle.Instance.InputController;
        transform.position = player.position + Vector3.up * startHieght;
        offset_Y = startHieght;
    }

    private void LateUpdate()
    {
        if (inputController.GetMouseScrollWheelAxis() != 0)
        {
            offset_Y -= inputController.GetMouseScrollWheelAxis() * sensitivityOffset_Z;
            offset_Y = Mathf.Clamp(offset_Y, minOffset_Y, maxOffset_Y);
            Vector3 offsetY = player.position + player.up * offset_Y;
            transform.position = Vector3.Lerp(transform.position, offsetY, smoothTime);
        }
        if (CheckDistance())
        {
            smoothPosition = Vector3.SmoothDamp(transform.position, player.position + Vector3.up * offset_Y,
                                                                                ref currentVelocity, smoothTime);
            transform.position = smoothPosition;
        }
    }

    //檢測目標與相機距離用來判斷相機是否要去跟隨目標
    private bool CheckDistance()
    {
        return Vector3.Distance(transform.position, player.position) > distanceToTarget;
    }
}
