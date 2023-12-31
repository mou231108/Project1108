using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_kim : MonoBehaviour
{
    public InventoryUI inventoryUI;


    private Animator animator;
    private float aniSpeed = 0; //애니메이션 속도

    //private Player_UI uI;
    private float horizontal;
    private float vertical;
    public int speed = 5;

    public GameObject hitobject;

    public bool moving = false;



    [SerializeField]//인스펙터에서만 참조 가능하게
    private float smoothRotationTime;//target 각도로 회전하는데 걸리는 시간
    [SerializeField]
    private float smoothMoveTime;//target 속도로 바뀌는데 걸리는 시간
    [SerializeField]
    private float moveSpeed;//움직이는 속도
    private float rotationVelocity;//The current velocity, this value is modified by the function every time you call it.
    private float speedVelocity;//The current velocity, this value is modified by the function every time you call it.
    private float currentSpeed;
    private float targetSpeed;



    // Start is called before the first frame update
    void Start()
    {
        //inventoryUI = GetComponent<InventoryUI>();
        animator = GetComponent<Animator>();
        //uI = GetComponent<Player_UI>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        RayCast();
    }

    private void PlayerMove()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        Vector2 input = new Vector2(horizontal, vertical);
        //GetAxisRaw("Horizontal") :오른쪽 방향키누르면 1을 반환, 아무것도 안누르면 0, 왼쪽방향키는 -1 반환
        //GetAxis("Horizontal"):-1과 1 사이의 실수값을 반환
        //Vertical은 위쪽방향키 누를시 1,아무것도 안누르면 0, 아래쪽방향키는 -1 반환

        Vector2 inputDir = input.normalized;
        //벡터 정규화. 만약 input=new Vector2(1,1) 이면 오른쪽위 대각선으로 움직인다.
        //방향을 찾아준다

        if (inputDir != Vector2.zero)//움직임을 멈췄을 때 다시 처음 각도로 돌아가는걸 막기위함
        {
            float rotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, rotation, ref rotationVelocity, smoothRotationTime);
        }
        //각도를 구해주는 코드, 플레이어가 오른쪽 위 대각선으로 움직일시 그 방향을 바라보게 해준다
        //Mathf.Atan2는 라디안을 return하기에 다시 각도로 바꿔주는 Mathf.Rad2Deg를 곱해준다
        //Vector.up은 y axis를 의미한다
        //SmoothDampAngle을 이용해서 부드럽게 플레이어의 각도를 바꿔준다.

        targetSpeed = moveSpeed * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, smoothMoveTime);
        //현재스피드에서 타겟스피드까지 smoothMoveTime 동안 변한다
        transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);

        if (Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0)
        {
            aniSpeed = Mathf.MoveTowards(aniSpeed, 1, Time.deltaTime * 3);
            moving = true;
        }
        else
        {
            aniSpeed = Mathf.MoveTowards(aniSpeed, 0, Time.deltaTime * 3);
            moving = false;
        }
        //animator.SetFloat("Speed", aniSpeed);      


        ///////////////////////////////
        //// 움직이는 방향을 카메라 기준으로 해보기
        ///////////////////////////////
    }

    private void RayCast()
    {
        if (Input.GetMouseButtonDown(0))
        {            RaycastHit hitInfo;
            Debug.DrawRay(transform.position + new Vector3(0, 0.5f, 0), transform.forward * 5.0f, Color.magenta);
            if (Physics.Raycast(transform.position + new Vector3(0, 0.5f, 0), transform.forward, out hitInfo, 2.0f))
            {
                hitobject = hitInfo.collider.gameObject;

                ObjItem_Kim objItem = hitobject.GetComponent<ObjItem_Kim>();
                inventoryUI.FreshSlot(objItem.item);
            }
            else
            {
                hitobject = null;
            }
        }
    }
}
