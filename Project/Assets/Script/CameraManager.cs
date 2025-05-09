using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float targetDistanceX = 0f;
    public float targetDistanceY = 3f;
    public float targetDistanceZ = -3f;
    public float smoothSpeed = 5f;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    //void LateUpdate()
    //{
    //    if (player != null)
    //    {
    //        Vector3 targetPos = player.transform.position + new Vector3(targetDistanceX, targetDistanceY, targetDistanceZ);
    //        transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.deltaTime);
    //    }
    //}
    void FixedUpdate()
    {
        if (player != null)
        {
            Vector3 targetPos = player.transform.position + new Vector3(targetDistanceX, targetDistanceY, targetDistanceZ);
            transform.position = Vector3.Lerp(transform.position, targetPos, smoothSpeed * Time.fixedDeltaTime);
        }
    }
}
