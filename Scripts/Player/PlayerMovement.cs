using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("移動相關參數")]
    [Tooltip("移動速度")]
    [SerializeField] float moveSpeed = 8;
    [Tooltip("按下Shift的加速度")]
    [SerializeField] float sprintSpeedModifier = 2;
    //[Tooltip("蹲下時的減速")]
    //[Range(0, 1)]
    //[SerializeField] float crouchedSpeedModifier = 0.5f;
    [Tooltip("角色旋轉時的速度")]
    [SerializeField] float rotateSpeedModifier = 5;
    [Tooltip("加速度百分比")]
    [SerializeField] float addSpeedRatio = 0.1f;

    [Header("跳躍參數")]
    [Tooltip("跳起來施加的力")]
    [SerializeField] float jumpForce = 15;
    [Tooltip("重力")]
    [SerializeField] float gravity = 50;
    [Tooltip("檢查與地面的距離")]
    [SerializeField] float distanceToGround = 0.1f;
    [SerializeField] Transform groundChecker;

    Vector3 targetMovement;//下一幀要移動到的目標位置
    Vector3 jumpDirection; //下一幀要跳到的位置
    [SerializeField] float lastFrameSpeed;//上一幀的移動速度

    bool isRun = false;

    InputController inputController;
    [SerializeField] CharacterController characterController;
    [SerializeField] Animator animator;
    [SerializeField] GameObject hand;

    private void Awake()
    {
        inputController = GameManagerSingle.Instance.InputController;
        //characterController = GetComponent<CharacterController>();
        //animator = GetComponent<Animator>();
        hand.SetActive(false);
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, -transform.up * distanceToGround, Color.red);

        if (inputController.SpeedUp())
        {
            isRun = !isRun;
        }
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("attack001") && !animator.GetCurrentAnimatorStateInfo(0).IsName("attack002") && !animator.GetCurrentAnimatorStateInfo(0).IsName("attack003"))
        {
            MoveBehaviour();
            JumpBehaviour();
        }
        else
        {
            characterController.Move(Vector3.zero);
        }
    }

    //移動的處理
    private void MoveBehaviour()
    {
        targetMovement = Vector3.zero;//先歸0避免偏移
        targetMovement += inputController.GetMoveInput().z * GetCurrentCameraForward();
        targetMovement += inputController.GetMoveInput().x * GetCurrentCameraRight();
        targetMovement = Vector3.ClampMagnitude(targetMovement, 1);//避免對角線超過1


        //按下Shift加速
        float nextFrameSpeed = 0;

        if (targetMovement == Vector3.zero)
        {
            nextFrameSpeed = 0;
            isRun = false;
        }
        else if (isRun)
        {
            nextFrameSpeed = 1;
            hand.SetActive(false);
            targetMovement *= sprintSpeedModifier;
            SmoothRotate(targetMovement);
        }
        else if (!isRun)
        {
            nextFrameSpeed = 0.5f;
            SmoothRotate(targetMovement);
        }

        if (lastFrameSpeed != nextFrameSpeed)
        {
            lastFrameSpeed = Mathf.Lerp(lastFrameSpeed, nextFrameSpeed, addSpeedRatio);
        }

        animator.SetFloat("Speed", lastFrameSpeed);

        characterController.Move(targetMovement * Time.deltaTime * moveSpeed);
    }

    private Vector3 GetCurrentCameraForward()
    {
        Vector3 cameraForward = Camera.main.transform.forward;//取得相機目前面向的方向
        cameraForward.y = 0f;//避免相機朝下(上)而導致人物向下(上)移動
        cameraForward.Normalize();//使cameraForward不超過1
        return cameraForward;
    }

    private Vector3 GetCurrentCameraRight()
    {
        Vector3 cameraRight = Camera.main.transform.right;//取得相機目前面向的方向
        cameraRight.y = 0f;//避免相機朝下(上)而導致人物向下(上)移動
        cameraRight.Normalize();//使cameraForward不超過1
        return cameraRight;
    }

    //平滑旋轉
    private void SmoothRotate(Vector3 targetMovement)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetMovement, Vector3.up), rotateSpeedModifier * Time.deltaTime);
    }

    //跳躍的處理
    private void JumpBehaviour()
    {
        //角色在地上並按下跳躍鍵就執行跳躍
        if (inputController.IsJumped() && IsGround())
        {
            jumpDirection = Vector3.zero;
            jumpDirection += jumpForce * Vector3.up;
            animator.SetTrigger("Jumped");
        }
        jumpDirection.y -= gravity * Time.deltaTime;
        jumpDirection.y = Mathf.Max(jumpDirection.y, -gravity);

        characterController.Move(jumpDirection * Time.deltaTime);
    }

    //角色是否在地上
    private bool IsGround()
    {
        return Physics.Raycast(groundChecker.position, -Vector3.up, distanceToGround);
    }
}
