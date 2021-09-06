using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Bools")]
    public bool isLevelStart;
    [Tooltip("Level'ýn Baþarýlý Bitiþ Durumu")]
    public bool isLevelDone;
    public bool isLevelFail;
    public bool isTurning;
   
    [Header("Settings")]
    public Vector3 offSet;
    public Vector3 upOffset;

    Quaternion rotatedTarget;
    public float smooth;

    Transform Player;
    Transform target;

    public float smoothTime;


    Vector3 targetPosition;

    public float rotateSpeed;



    private Vector3 velocity = Vector3.zero;

    public static CameraController instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Player = PlayerController.instance.transform;
      
        target = Player;
    }

    // Update is called once per frame
    void Update()
    {
        //camera dönüþünü de burda ayarlýcaksýn if ile 
    }

    private void LateUpdate()
    {
        if (isLevelStart && !isLevelDone && !isLevelFail)
        {
            targetPosition = target.transform.position + offSet;
            //targetPosition.x = 0;

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
            //transform.position = Vector3.Lerp(transform.position, targetPosition, smoothTime);

            if (isTurning)
            {

                var rotationVector = transform.rotation.eulerAngles;
                rotationVector.y = 60;
                rotatedTarget = Quaternion.Euler(rotationVector);

                transform.rotation = Quaternion.Slerp(transform.rotation, rotatedTarget, Time.deltaTime * smooth);

                if (offSet.z != -15 && offSet.x != -15 )
                {
                    for (int i = 0; i < 5; i++)
                    {

                        offSet.z--;

                        offSet.x = offSet.x - 3;

                    }
                }


            }

        }
        else if (isLevelDone)
        {
            targetPosition = target.transform.position + offSet;
            transform.position = new Vector3(transform.position.x, Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime).y, transform.position.z);

            Vector3 targetRotation = target.position - transform.position + upOffset;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetRotation), Time.deltaTime * 3);

            transform.RotateAround(target.position, Vector3.up, rotateSpeed * Time.deltaTime);
        }
    
    
    }

    public void StartTurning()
    {
        isTurning = true;


    }
}
