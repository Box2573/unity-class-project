using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("相機跟隨目標")]
    [SerializeField] Transform target;

    [Header("靈敏度")]
    [Tooltip("水平軸靈敏度")]
    [SerializeField] float sensivity_X = 2;
    [Tooltip("垂直軸靈敏度")]
    [SerializeField] float sensivity_Y = 2;
    [Tooltip("滾輪靈敏度")]
    [SerializeField] float sensivity_Z = 5;

    [Header("玩家能向上(下)看到的最大(小)角度")]
    [Tooltip("玩家能看到最低的角度")]
    [SerializeField] float minVerticalAngle = -10;
    [Tooltip("玩家能看到最高的角度")]
    [SerializeField] float maxVerticalAngle = 85;

    [Header("距離")]
    [Tooltip("相機與目標距離")]
    [SerializeField] float cameraToTargetDistance = 10;
    [Tooltip("相機與目標最小距離")]
    [SerializeField] float minDistance = 2;
    [Tooltip("相機與目標最大距離")]
    [SerializeField] float maxDistance = 25;
    [SerializeField] Vector3 offset;


    float mouse_X = 0;//視角左右移動(預設看向前方)
    float mouse_Y = 30;//視角(相機)上下移動(預設相機(視角)微往下看)
    bool isPause;

    InputController inputController;

    private void Awake()
    {
        inputController = GameManagerSingle.Instance.InputController;
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }
    private void LateUpdate()
    {
        //原本 + 滑鼠移動 * 移動靈敏度 可以得到最後要把視角移到哪裡
        mouse_X += inputController.GetMouseXAxis() * sensivity_X;
        mouse_Y += inputController.GetMouseYAxis() * sensivity_Y;
        mouse_Y = Mathf.Clamp(mouse_Y, minVerticalAngle, maxVerticalAngle);//限制垂直角度


        //進行旋轉視角轉動x為上下移動因此x要放mouse_y(視角上下移動), 轉動y則為左右移動因此要放mouse_x(視角左右移動)
        transform.rotation = Quaternion.Euler(mouse_Y, mouse_X, 0);//z軸不做旋轉
                                                                   //相機位置 = 旋轉後的位置  * 相機距離玩家距離(相機要在玩家後方因此為負) + 玩家現在座標 這樣相機會一直保持在玩家後方
        transform.position = Quaternion.Euler(mouse_Y, mouse_X, 0)
                             * new Vector3(0, 0, -cameraToTargetDistance) + target.position + Vector3.up * offset.y;

        cameraToTargetDistance += inputController.GetMouseScrollWheelAxis() * sensivity_Z;
        cameraToTargetDistance = Mathf.Clamp(cameraToTargetDistance, minDistance, maxDistance);

    }
}
