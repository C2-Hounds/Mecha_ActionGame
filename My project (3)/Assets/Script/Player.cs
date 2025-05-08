using System.Net;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Player : MonoBehaviour
{

    public float speed = 3.0f;

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
    //右または左彼逆方向に移動するとき
    //左に変更
    public string RightUpdateLeft = "RightUpdateLeft";
    public string LeftUpdateRight = "LeftUpdateRight";

    public string Jump = "Jump";

    public string Land = "Land";
    string nowAnime = "";//現在再生中のアニメ名
    string prevAnime = "";//ひとつ前に再生したアニメ名
    Animator animator;
    AnimatorStateInfo stateInfo;
    float preAction = 0;

    int weightAction = 0;

    bool loopRightFlg = false;
    bool loopLeftFlg = false;

    bool jumpFlg = true;

    int ActionAnime = 0;

    void Start()
    {
        //スライムキャラに追加されたアニメーターの実態(SlimeAnime)を取得
        animator = GetComponent<Animator>();
        nowAnime = idleAnime;//アニメの初期状態
        prevAnime = idleAnime;//アニメの初期状態
        moveAction = InputSystem.actions.FindAction("Move");
    }
    private void Update()
    {
        if (jumpFlg == true)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                transform.position += transform.up * jumpPow;
                ActionAnime = 5;
                jumpFlg = false;

            }
        }

        if (jumpFlg)
        {
           ActionAnime = 0;
        }
        // Wキー（前方移動）


        // Sキー（後方移動）


        // Dキー（右移動）

        if (Input.GetKey(KeyCode.D))
        {
            if (!loopLeftFlg)
            {
                if (!(weightAction == 2))
                {

                    transform.position += speed * transform.right * Time.deltaTime;
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

        // Aキー（左移動）
        if (Input.GetKey(KeyCode.A))
        {
            if (!loopRightFlg)
            {
                if (!(weightAction == 1))
                {

                    transform.position -= speed * transform.right * Time.deltaTime;

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
            transform.position += speed * transform.forward * Time.deltaTime;
            ActionAnime = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position -= speed * transform.forward * Time.deltaTime;
            ActionAnime = 2;
        }
        //アニメーション再生

     

        if (ActionAnime == 0)
        {
            weightAction = 0;
            if (preAction == 3 || preAction == 4)
            {
                if (preAction == 3)
                {
                    nowAnime = BoostRightReturn;//歩行アニメを指定
                }
                if (preAction == 4)
                {
                    nowAnime = BoostLeftReturn;//歩行アニメを指定
                }
            }
            else
            {
                nowAnime = idleAnime;//待機アニメを指定

            }
        }
        else
        {

            if (jumpFlg)
            {
                if (ActionAnime == 1)
                {
                    nowAnime = BoostForward;//歩行アニメを指定

                }
                if (ActionAnime == 2)
                {
                    nowAnime = BoostBackward;//歩行アニメを指定
                }

                if (preAction == 3 && ActionAnime == 4 || preAction == 4 && ActionAnime == 3)
                {

                    if (ActionAnime == 3)
                    {
                        nowAnime = LeftUpdateRight;// 歩行アニメを指定
                    }
                    if (ActionAnime == 4)
                    {
                        nowAnime = RightUpdateLeft;//歩行アニメを指定
                    }
                }
                else
                {
                    if (ActionAnime == 3)
                    {
                        nowAnime = RightMove;//歩行アニメを指定
                    }
                    if (ActionAnime == 4)
                    {
                        nowAnime = LeftMove;//歩行アニメを指定
                    }
                }
            }
            else
            {


                if (ActionAnime == 5)
                {
                    nowAnime = Jump;//歩行アニメを指定
                }
            }
            preAction = ActionAnime;

        }

        

        if (nowAnime != prevAnime)//アニメが切り替わったとき
           {
                prevAnime = nowAnime;   //アニメの状態を更新
                animator.Play(nowAnime);//指定したアニメを再生

           }
        
    }

   

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit"); // ログを表示する
        if (jumpFlg==false)
        {
           
            nowAnime = Land;
            if (nowAnime != prevAnime)//アニメが切り替わったとき
            {
                prevAnime = nowAnime;   //アニメの状態を更新
                animator.Play(nowAnime);//指定したアニメを再生
                jumpFlg = true;
            }
           
        }

     
        //if (!animator.IsName("mouseOverEffect"))
        //    animator.Play("mouseOverEffect");


        Debug.Log("Hit"); // ログを表示する
    }
 

}
