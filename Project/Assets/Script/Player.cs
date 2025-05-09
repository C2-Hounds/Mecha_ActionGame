using System.Drawing;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MonoBehaviour
{

    public float speed = 3.0f;

    private Rigidbody rb;
    private Vector3 moveDirection;
    public float  jumpPow = 6;
    private InputAction moveAction;
    public string walkAnime = "Walk_Forward";
    public string idleAnime = "Armature|Idle Pose";
    public string BoostForward = "Boost_Forward";
    public string BoostBackward = "Boost_Backward";

    public string LeftMove = "BoostLeft";


    public string RightMove = "BoostRight";

    public string BoostLeftReturn = "BoostLeftReturn";
    public string BoostRightReturn = "BoostRigutReturn";
    //�E�܂��͍��ދt�����Ɉړ�����Ƃ�
    //���ɕύX
    public string RightUpdateLeft = "RightUpdateLeft";
    public string LeftUpdateRight = "LeftUpdateRight";

    public string Jump = "Jump";

    public string Land = "Land";
    string nowAnime = "";//���ݍĐ����̃A�j����
    string prevAnime = "";//�ЂƂO�ɍĐ������A�j����
    Animator animator;
    AnimatorStateInfo stateInfo;
    float preAction = 0;

    int weightAction = 0;

    bool loopRightFlg = false;
    bool loopLeftFlg = false;

    bool jumpFlg = true;

    int ActionAnime = 0;

    //グラウンド
    public float jumpForce = 5f;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.1f;
    private bool isGrounded;
    void Start()
    {
        //�X���C���L�����ɒǉ����ꂽ�A�j���[�^�[�̎���(SlimeAnime)���擾
        animator = GetComponent<Animator>();
        nowAnime = idleAnime;//�A�j���̏������
        prevAnime = idleAnime;//�A�j���̏������
        moveAction = InputSystem.actions.FindAction("Move");
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        // 地面チェック
        isGrounded = Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, groundCheckDistance + 0.1f, groundLayer);

       
      
        //if (jumpFlg == true)
        {
            // ジャンプ入力（スペースキー）
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            { 
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        
        
          
             
                ActionAnime = 5;
                jumpFlg = false;

            }
        }

        if (jumpFlg)
        {
           ActionAnime = 0;
        }
        // W�L�[�i�O���ړ��j


        // S�L�[�i����ړ��j


        // D�L�[�i�E�ړ��j

        if (Input.GetKey(KeyCode.D))
        {
            if (!loopLeftFlg)
            {
                if (!(weightAction == 2))
                {

                   
                    ActionAnime = 3;

                    weightAction = 1;


                }
                else
                {
                    loopRightFlg = true;
                    weightAction = 1;
                }
            }

        }
        else
        {
            loopLeftFlg = false;
        }

        // A�L�[�i���ړ��j
        if (Input.GetKey(KeyCode.A))
        {
            if (!loopRightFlg)
            {
                if (!(weightAction == 1))
                {


                    ActionAnime = 4;

                    weightAction = 2;

                }
                else
                {
                    loopLeftFlg = true;
                    weightAction = 2;
                }
            }
        }
        else
        {
            loopRightFlg = false;
        }

        if (Input.GetKey(KeyCode.W))
        {
          
            ActionAnime = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
           
            ActionAnime = 2;
        }
        //�A�j���[�V�����Đ�

     

        if (ActionAnime == 0)
        {
            weightAction = 0;
            if (preAction == 3 || preAction == 4)
            {
                if (preAction == 3)
                {
                    nowAnime = BoostRightReturn;//���s�A�j�����w��
                }
                if (preAction == 4)
                {
                    nowAnime = BoostLeftReturn;//���s�A�j�����w��
                }
            }
            else
            {
                nowAnime = idleAnime;//�ҋ@�A�j�����w��

            }
        }
        else
        {

            if (jumpFlg)
            {
                if (ActionAnime == 1)
                {
                    nowAnime = BoostForward;//���s�A�j�����w��

                }
                if (ActionAnime == 2)
                {
                    nowAnime = BoostBackward;//���s�A�j�����w��
                }

                if (preAction == 3 && ActionAnime == 4 || preAction == 4 && ActionAnime == 3)
                {

                    if (ActionAnime == 3)
                    {
                        nowAnime = LeftUpdateRight;// ���s�A�j�����w��
                    }
                    if (ActionAnime == 4)
                    {
                        nowAnime = RightUpdateLeft;//���s�A�j�����w��
                    }
                }
                else
                {
                    if (ActionAnime == 3)
                    {
                        nowAnime = RightMove;//���s�A�j�����w��
                    }
                    if (ActionAnime == 4)
                    {
                        nowAnime = LeftMove;//���s�A�j�����w��
                    }
                }
            }
            else
            {


                if (ActionAnime == 5)
                {
                    nowAnime = Jump;//���s�A�j�����w��
                }
            }
            preAction = ActionAnime;

        }

        

        if (nowAnime != prevAnime)//�A�j�����؂�ւ�����Ƃ�
           {
                prevAnime = nowAnime;   //�A�j���̏�Ԃ��X�V
                animator.Play(nowAnime);//�w�肵���A�j�����Đ�

           }

        float horizontal = Input.GetAxisRaw("Horizontal"); // A, D
        float vertical = Input.GetAxisRaw("Vertical");     // W, S

        moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

    }


    void FixedUpdate()
    {
        // RigidbodyのMovePositionで移動（物理演算に従う）
        Vector3 move = moveDirection * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit"); // ���O��\������
        if (jumpFlg==false)
        {
           
            nowAnime = Land;
            if (nowAnime != prevAnime)//�A�j�����؂�ւ�����Ƃ�
            {
                prevAnime = nowAnime;   //�A�j���̏�Ԃ��X�V
                animator.Play(nowAnime);//�w�肵���A�j�����Đ�
                jumpFlg = true;
            }
           
        }

     
        //if (!animator.IsName("mouseOverEffect"))
        //    animator.Play("mouseOverEffect");


        Debug.Log("Hit"); // ���O��\������
    }
    private void OnTriggerEnter(Collider other)
    {
        Vector3 hitPoint = other.ClosestPoint(transform.position);

        // �v���C���[���i�݂����Ȃ��悤�Ɉʒu��␳
        transform.position = new Vector3(hitPoint.x, transform.position.y, hitPoint.z);

        // �K�v�ɉ����đ��x�[���ɂ���iRigidbody�g���Ă�ꍇ�j
        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.linearVelocity = Vector3.zero;
        }
        Debug.Log(hitPoint); // ���O��\������
    }
    private void OnTriggerStay(Collider other)
    {
       
    }

}

