using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
     Animator animator;
    Vector2 playerInputVec;
    bool isRunning;
    bool isJumping;
    public float rotateSpeed = 700;

    Transform playerTransform;
    Transform cameraTransform;

    Vector3 playerMovement;

    float currentSpeed;//当前实际运动速度
    float targetSpeed;//想要达到的速度
    public float walkSpeed=2f;
    public float runSpeed = 5f;

    
    void Start()
    {
        animator =this. GetComponent<Animator>();
        playerTransform= this. transform;//缓存transform，直接从内存地址获取playerTransform
        cameraTransform=Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        RotatePlayer();
        MovePlayer();
    }

    public void GetPlayerMoveInput(InputAction.CallbackContext ctx)
    {
        playerInputVec = ctx.ReadValue<Vector2>();
    }

    public void GetPlayerRunInput(InputAction.CallbackContext ctx)
    {
        isRunning = ctx.ReadValue<float>() > 0 ? true : false;

    }

    public void GetPlayerJumpInput(InputAction.CallbackContext ctx)
    {
        isJumping = ctx.ReadValue<float>() > 0;
    }

    void RotatePlayer()
    {
        if ( playerInputVec.Equals(Vector2.zero))
        {
            return;
        }
        float x = playerInputVec.x;
        float z = playerInputVec.y;

        Vector3 targetDirection = new Vector3(x, 0, z);
        float y = cameraTransform.rotation.eulerAngles.y;
        targetDirection = Quaternion.Euler(0, y, 0) * targetDirection;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        playerTransform.rotation = Quaternion.RotateTowards(playerTransform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        //playerMovement.x= playerInputVec.x;
        //playerMovement.z= playerInputVec.y;

        //print(playerMovement);
        //Quaternion targetRotation = Quaternion.LookRotation(playerMovement, Vector3.up);
        //playerTransform.rotation = Quaternion.RotateTowards(playerTransform.rotation, targetRotation,rotateSpeed*Time.deltaTime);
    }

    void MovePlayer()
    {
        if (playerInputVec.Equals(Vector2.zero))
        {
            animator.SetFloat("Speed", 0);
            return;
        }
        targetSpeed =isRunning ? runSpeed : walkSpeed;
        playerMovement = Vector3.forward * currentSpeed * Time.deltaTime;

        this.transform.Translate(playerMovement, Space.Self);
        currentSpeed = Mathf.Lerp(targetSpeed, currentSpeed, 0.5f);//线性插值，让currentSpeed逐渐靠近targetSpeed
        animator.SetFloat("Speed", currentSpeed);

        if (isJumping)
        {

        }
    } 
   


}
