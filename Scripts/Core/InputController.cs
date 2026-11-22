using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public float vertical;
    public float horizontal;

    [SerializeField] PlayerInput playerInput;

    private void Awake()
    {
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
    }

    private void Update()
    {
        CheckCursorState();
    }


    public Vector3 GetMoveInput()
    {
        //Vector3 move = new Vector3(joystick.Horizontal, 0, joystick.Vertical);//取得水平、前後移動(非上下(Y軸))
        //Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));//取得水平、前後移動(非上下(Y軸))

        Vector3 move = new Vector3(playerInput.actions["Move"].ReadValue<Vector2>().x, 0, playerInput.actions["Move"].ReadValue<Vector2>().y);//取得水平、前後移動(非上下(Y軸))
        move = Vector3.ClampMagnitude(move, 1);//限制move最大只能為1，避免對角線(向斜前(後)方移動)過大導致速度太快

        return move;
    }

    public bool CanProcessInput()
    {
        return true;//當游標鎖住就回傳true
    }

    public float GetMouseXAxis()
    {
        if (CanProcessInput())
        {
            //return Input.GetAxis("Mouse X")
            return playerInput.actions["Look"].ReadValue<Vector2>().x;
        }
        return 0;
    }

    public float GetMouseYAxis()
    {
        if (CanProcessInput())
        {
            //return Input.GetAxis("Mouse Y");
            return playerInput.actions["Look"].ReadValue<Vector2>().y;
        }
        return 0;
    }

    public float GetMouseScrollWheelAxis()
    {
        if (CanProcessInput())
        {
            return Input.GetAxis("Mouse ScrollWheel");
        }
        return 0;
    }
    //處理按下加速
    public bool SpeedUp()
    {
        if (CanProcessInput())
        {
            return playerInput.actions["Run"].triggered;
        }

        return false;
    }

    //是否按下跳躍
    public bool IsJumped()
    {
        if (CanProcessInput())
        {
            return playerInput.actions["Jump"].triggered;
        }
        return false;
    }
    #region 射擊相關
    //是否按下滑鼠右鍵
    public bool GetAimInput()
    {
        if (CanProcessInput())
        {
            return Input.GetMouseButtonDown(1);
        }
        return false;
    }

    //是否按下滑鼠左鍵
    public bool GetFireDown()
    {
        if (CanProcessInput())
        {
            return playerInput.actions["Attack"].triggered;
        }
        return false;
    }

    //是否持續按下滑鼠左鍵
    public bool GetFireHold()
    {
        if (CanProcessInput())
        {
            return Input.GetMouseButton(0);
        }
        return false;
    }

    //是否放開滑鼠左鍵
    public bool GetFireUp()
    {
        if (CanProcessInput())
        {
            return Input.GetMouseButtonUp(0);
        }
        return false;
    }

    //是否按下R鍵
    public bool GetReloadInput()
    {
        if (CanProcessInput())
        {
            return Input.GetKeyDown(KeyCode.R);
        }
        return false;
    }

    //是否按下切換武器
    public int GetSwitchWeapon()
    {
        if (CanProcessInput())
        {
            if (Input.GetKeyDown(KeyCode.E))//按下E切到下一樣武器(編號+1)
            {
                return 1;
            }
            else if (Input.GetKeyDown(KeyCode.Q))//按下Q切到上一樣武器(編號-1)
            {
                return -1;
            }
        }
        return 0;
    }

    #endregion

    private void CheckCursorState()
    {
        if (Input.GetKeyDown(KeyCode.Escape))//按下ESC
        {
            if (Cursor.lockState == CursorLockMode.None)//如果游標不是鎖住的
            {
                Cursor.lockState = CursorLockMode.Locked;//就鎖住
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;//反之
            }
        }
    }
}
