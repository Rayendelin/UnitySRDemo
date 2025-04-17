using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;
using UnityEngine.EventSystems;

public class CamControl : MonoBehaviour
{
    public static CamControl Instance;//设置该脚本为单例脚本
    public GameObject camControl;//相机跟随拍摄的主角
    public GameObject camStandard;//摄像机的基准点
    public GameObject theCamera;//摄像机本身
    public GameObject camRefPoint;//要拍摄主角的参考点（头部）
    public float distance = 2.2f;//控制摄像机的拍摄距离
    public float rot_HorizontaiSpeed = 1;//摄像机水平旋转速度
    public float rot_VerticalSpeed = 4;//摄像机垂直旋转速度
    public float limit_RotAngle_Min = -90;//摄像机垂直方向最小旋转角度
    public float limit_RotAngle_Max = 90;//摄像机垂直方向最大旋转角度
    public bool isFingerTouchOnUI = false;//判断当前手指是否点击到了UI上
    public float moveSpeed = 8;//相机移动速度
    private Vector3 targetPos;
    public float offset = 1;
    public GameObject Player;
    public float rotationSpeed = 0f; // 默认旋转速度
    private int speedIndex = 0; // 速度档位索引
    private float[] speeds = { 0f, 20.0f, 60.0f }; // 三档速度

    private Vector3 camControlUp= Vector3.zero;


    // //public Transform player; // 玩家对象的Transform
    // //public float initialDistance = 5.0f; // 初始相机到玩家的距离
    // public float avoidanceSpeed = 2.0f; // 相机避障时的速度

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if(speedIndex > 0)
        {
            camControlUp = camControl.transform.position;//定义旋转轴
            camControl.transform.RotateAround(camControlUp, Vector3.up, rotationSpeed * Time.deltaTime);//绕Y旋转

        }
        
    }

    private void FixedUpdate()
    {
        CtrlCamFollowHero();
        CtrlOneHandRot();
        CtrlTowHandRot();

    //     // 获取相机与玩家间的距离
    //     float distanceToPlayer = Vector3.Distance(transform.position, theCamera.transform.position);

    //     // 生成射线方向，始终射向相机
    //     Vector3 rayDirection = (theCamera.transform.position - transform.position).normalized;

    //     // 发射射线
    //     RaycastHit hit;
    //     if (Physics.Raycast(transform.position, rayDirection, out hit, Mathf.Infinity-0.1f))
    //     {
    //         // 如果射线的碰撞不是相机
    //         if (hit.collider.gameObject != gameObject)
    //         {
    //             // 获取射线碰撞点到玩家的距离
    //             float distanceToHitPoint = Vector3.Distance(hit.point, theCamera.transform.position);

    //             // 如果这个距离小于玩家到相机的距离，表示中间有障碍物，拉近相机
    //             if (distanceToHitPoint < distanceToPlayer)
    //             {
    //                 float step = avoidanceSpeed * Time.deltaTime;
    //                 theCamera.transform.position = hit.point;
    //             }
    //         }
    //     }

    //     // 没有障碍物时，相机回到初始距离
    //     else
    //     {
    //         float step = avoidanceSpeed * Time.deltaTime;
    //         transform.position = Vector3.MoveTowards(transform.position, theCamera.transform.position - transform.forward * distance, step);
    //     }

    }

    void CtrlCamFollowHero() 
    {
        targetPos = camRefPoint.transform.position;
        targetPos.z += offset;
        camControl.transform.position = Vector3.Lerp(camControl.transform.position, targetPos, moveSpeed * Time.deltaTime);

        // camControl.transform.position = camRefPoint.transform.position;
        // theCamera.transform.position = camControl.transform.position + (camStandard.transform.position - camControl.transform.position).normalized * distance;
    }

    public void ViewSwitching()
    {
        Vector3 targetCam;
        if(offset<=0)
        {
            offset = Mathf.Lerp(offset, 1, 50*Time.deltaTime);
            targetCam = camRefPoint.transform.position;
            targetCam.y -= 1f;
            camRefPoint.transform.position = Vector3.Lerp(camRefPoint.transform.position, targetCam, 50 * Time.deltaTime);
            Debug.Log("11");
        }
        else
        {
            offset = Mathf.Lerp(offset, (float)-0.5, 50*Time.deltaTime);
            targetCam = camRefPoint.transform.position;
            targetCam.y += 1f;
            camRefPoint.transform.position = Vector3.Lerp(camRefPoint.transform.position, targetCam, 50 * Time.deltaTime);
            Debug.Log("22");
        }
    }

    public void ViewRotate()
    {
        speedIndex = (speedIndex + 1) % speeds.Length; // 切换速度档位
        rotationSpeed = speeds[speedIndex]; // 应用新的旋转速度
    }


    void CtrlOneHandRot()
    {
        if (Input.touchCount == 1)
        {
            if (isFingerTouchOnUI == true)
            {
                return;
            }

            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                //如果当前手指点击在UIButton中，则摄像机不进行旋转
                if(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    return;
                }
            }

            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                //如果当前手指点击在UIButton中，则摄像机不进行旋转
                if (EventSystem.current.IsPointerOverGameObject(Input .GetTouch(0).fingerId))
                {
                    return;
                }
            }

            //用于储存手指的滑动量XY
            Vector2 touchDeltaPosition = Vector2.zero;
            float rotX = 0;
            float rotY = 0;
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                touchDeltaPosition = Input.GetTouch(0).deltaPosition;
                rotX = touchDeltaPosition.x;
                rotY = touchDeltaPosition.y;
            }
            //控制相机水平方向上的旋转
            camControl.transform.Rotate(Vector3.up, rotX * rot_HorizontaiSpeed * Time.deltaTime);
            //控制相机垂直方向上的旋转
            float tempEularX = camControl.transform.eulerAngles.x;//获取摄像机X轴旋转角度
            tempEularX = tempEularX - rotY * rot_VerticalSpeed * Time.deltaTime;//计算摄像机旋转角度-单指滑动值
            //tempEularX = Mathf.Clamp(tempEularX, limit_RotAngle_Min, limit_RotAngle_Max);//限制最大最小值
            camControl.transform.eulerAngles = new Vector3(tempEularX, camControl.transform.eulerAngles.y, 0);//应用旋转值
        }
    }


    //用于判定是否可以使用碰到屏幕的第一个手指进行摄像机旋转
    bool isRotFinger_00 = false;
    //用于判定是否可以使用碰到屏幕的第二个手指进行摄像机旋转
    bool isRotFinger_01 = false;

    //处理双指旋转的函数
    void CtrlTowHandRot()
    {
        //如果当前点击的手指少于2根，不进行双指旋转
        if (Input.touchCount <2)
        {
            return;
        }

        //使用isFingerTouchUI判断当前是否有手指点击在UI上，如果两根手指都没有点击在UI上，那么我们也不进行旋转
        if (isFingerTouchOnUI)
        {
            //检测碰到屏幕的第一根手指是否点击到了UI
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    isRotFinger_00 = false;
                }
                else
                {
                    isRotFinger_00 = true;
                }
            }

            //如果结束滑动
            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                //将第一根手指设置为不能旋转摄像机
                isRotFinger_00 = false;
            }

            //检测碰到屏幕的第二根手指是否点击到了UI
            if (Input.GetTouch(1).phase == TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(1).fingerId))
                {
                    isRotFinger_01 = false;
                }
                else
                {
                    isRotFinger_01 = true;
                }
            }

            //如果结束滑动
            if (Input.GetTouch(1).phase == TouchPhase.Ended)
            {
                //将第一根手指设置为不能旋转摄像机
                isRotFinger_01 = false;
            }

            //如果两根手指都无法控制相机旋转，那就不继续执行后面的旋转代码
            if (isRotFinger_00 == false && isRotFinger_01 == false)
            {
                return;
            }

            Vector2 touchDeltaiPosition = Vector2.zero;
            float rotX = 0;
            float rotY = 0;

            //这段代码用来处理双指旋转
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                if (isRotFinger_00)
                {
                    touchDeltaiPosition = Input.GetTouch(0).deltaPosition;
                    rotX = touchDeltaiPosition.x;
                    rotY = touchDeltaiPosition.y;
                }
            }

            if (Input.GetTouch(1).phase == TouchPhase.Moved)
            {
                if (isRotFinger_01)
                {
                    touchDeltaiPosition = Input.GetTouch(1).deltaPosition;
                    rotX = touchDeltaiPosition.x;
                    rotY = touchDeltaiPosition.y;
                }
                //控制相机水平方向上的旋转
            camControl.transform.Rotate(Vector3.up, rotX * rot_HorizontaiSpeed * Time.deltaTime);
            //控制相机垂直方向上的旋转
            float tempEularX = camControl.transform.eulerAngles.x;//获取摄像机X轴旋转角度
            tempEularX = tempEularX - rotY * rot_VerticalSpeed * Time.deltaTime;//计算摄像机旋转角度-单指滑动值
            //tempEularX = Mathf.Clamp(tempEularX, limit_RotAngle_Min, limit_RotAngle_Max);//限制最大最小值
            camControl.transform.eulerAngles = new Vector3(tempEularX, camControl.transform.eulerAngles.y, 0);//应用旋转值
            }
        }
    }
}
