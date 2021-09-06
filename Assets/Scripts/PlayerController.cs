using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [Header("Bools")]
    public bool isLevelStart;
    [Tooltip("Level'ýn Baþarýlý Bitiþ Durumu")]
    public bool isLevelDone;
    public bool isLevelFail;
    public bool isMagnetTriggered;
    public bool isEndGameTriggered;
    



    [Space(15)]
    public int score;
    public Rigidbody RB;
    GameController GC;
    UIController UI;
    public int multiplierValue;
    public int finalScore;
 
    public Vector3 targetPosition;


    float firstPosX;
    float mouseFirstPosX;

    [Header("Ground Limits")]
    public float groundLimitXB;
    public float groundLimitXL;
    public float groundLimitZB;
    public float groundLimitZL;

    [Header("Speed Set")]
    public float speedForward;
    public float smoothSpeed;
     
    float speedSide;

    [Space(15)]
    public float screenDivideCount;
    public float delayTime;
    public float fallTime;
    float fallCounter;
    bool isFalling;
    public List<GameObject> myCubes;
    public GameObject Magnet;
    public int cubeCount;
    public int meltingFrameCount;
    public GameObject Trail;
    

    GameObject tempCube;
    Vector3 tempPos;
    public GameObject CubePrefab;

    public Transform cubeContainer;

    [Header("Side Way Components")]
    public Vector3 wpTargetPosition; // waypoint target position.
    //public NavMeshAgent myAgent;
    public int waypointIndex = 0;
    public List<Transform> wayPoints;
    public int listIndex;
    public float speed;
    public Vector3 direction;
    public bool isWaypointPart;
    Quaternion firstRotation;

    [Header("Turning")]
    public bool isTurned;
    public bool isTurning;
    Quaternion rotatedTarget;
    public float smooth;





    string TagObstacle;
    string TagFinish;
    string TagCollectable;
    string TagCube;
    string TagWall;
    string TagFallTrigger;
    string TagBoosterStart;
    string TagMagnetTrigger;
    string TagLava;
    string TagFlag;
    string TagMultiplierBox;
    string TagWayPoint;
    string TagTurnTrigger;
    string TagEndGameTrigger;
   

    int LayerIndexMeltedCube;
   



    WaitForSeconds delaySec;
    public static PlayerController instance;

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
        StartMethods();
    }
    #region StartMethods

    void StartMethods()
    {
        GC = GameController.instance;
        UI = UIController.instance;
        //GetLevel();
        GetRB();
        GetTags();
        SetSideSpeed();
        SetCubesOpening();
        delaySec = new WaitForSeconds(delayTime);
        
    }
    void GetLevel()
    {
        //level = GC.level;
    }
    void GetRB()
    {
        RB = gameObject.GetComponent<Rigidbody>();
    }
    void GetTags()
    {
        TagObstacle = GC.TagObstacle;
        TagFinish = GC.TagFinish;
        TagCollectable = GC.TagCollectable;
        TagCube = GC.TagCube;
        TagWall = GC.TagWall;
        TagFallTrigger = GC.TagFallTrigger;
        TagBoosterStart = GC.TagBoosterStart;
        TagMagnetTrigger = GC.TagMagnetTrigger;
        TagLava = GC.TagLava;
        TagFlag = GC.TagFlag;
        TagMultiplierBox = GC.TagMultiplierBox;
        TagWayPoint = GC.TagWayPoint;
        TagTurnTrigger = GC.TagTurnTrigger;
        TagEndGameTrigger = GC.TagEndGameTrigger;
       
    }

    void SetSideSpeed()
    {

        speedSide = Screen.width / screenDivideCount;


    }
    void SetCubesOpening()
    {
        for(int i = 0; i < cubeContainer.childCount-2; i++)
        {
            myCubes.Add(cubeContainer.GetChild(i).gameObject);
            cubeCount++;
        }
    }

    #endregion

    #region TapToStartActions


    public void TapToStartActions()
    {

    }


    #endregion

    // Update is called once per frame
    void Update()
    {

        if (isLevelStart && !isLevelDone && !isLevelFail)
        {
            if (!isWaypointPart)
            {
                targetPosition = transform.position;
                //targetPosition.z += speedForward;
                targetPosition = transform.position + transform.forward * speedForward;



                if (Input.GetMouseButtonDown(0))
                {
                    mouseFirstPosX = Input.mousePosition.x;
                    firstPosX = transform.position.x;

                }
                else if (Input.GetMouseButton(0))
                {
                    /*
                    if (Input.mousePosition.x != mouseFirstPosX)
                    {
                        targetPosition.x = firstPosX + (Input.mousePosition.x - mouseFirstPosX) / speedSide;
                        targetPosition.x = targetPosition.x > groundLimitXB ? groundLimitXB : targetPosition.x;
                        targetPosition.x = targetPosition.x < groundLimitXL ? groundLimitXL : targetPosition.x;

                    }
                    */
                    if (Input.mousePosition.x != mouseFirstPosX)
                    {
                        targetPosition = targetPosition + transform.right * (Input.mousePosition.x - mouseFirstPosX) / speedSide;
                       // targetPosition.x = firstPosX + (Input.mousePosition.x - mouseFirstPosX) / speedSide;
                       // targetPosition.x = targetPosition.x > groundLimitXB ? groundLimitXB : targetPosition.x;
                       // targetPosition.x = targetPosition.x < groundLimitXL ? groundLimitXL : targetPosition.x;

                    }
                   

                  
                    if (isTurned)
                    {
                        targetPosition.z = targetPosition.z > groundLimitZB ? groundLimitZB : targetPosition.z;
                        targetPosition.z = targetPosition.z < groundLimitZL ? groundLimitZL : targetPosition.z;
                    }
                    else
                    {
                        targetPosition.x = targetPosition.x > groundLimitXB ? groundLimitXB : targetPosition.x;
                        targetPosition.x = targetPosition.x < groundLimitXL ? groundLimitXL : targetPosition.x;
                        
                       // isTurned = true; 

                    }
                }


                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);


                ActionFall();




                if (isMagnetTriggered)
                {
                    cubeCount = cubeContainer.childCount - 2;
                    tempPos = Magnet.transform.localPosition;
                    tempPos.y = cubeCount * -1 + 1;
                    Magnet.transform.localPosition = tempPos;
                }

                if (isLevelStart)
                {
                    cubeCount = cubeContainer.childCount - 2;
                    tempPos = Trail.transform.localPosition;
                    tempPos.y = cubeCount * -1 + 1;
                    Trail.transform.localPosition = tempPos;
                }

            }
            
            else
            {
                WaypointAction();
                cubeCount = cubeContainer.childCount - 2;
                tempPos = Trail.transform.localPosition;
                tempPos.y = cubeCount * -1 + 1;
                Trail.transform.localPosition = tempPos;
            }

            //döndürme iþlemi update içinde timea baðlý þekilde kullanýcaksýn.

            if (isTurning)
            {

                var rotationVector = transform.rotation.eulerAngles;
                rotationVector.y = 90;
                rotatedTarget = Quaternion.Euler(rotationVector);

                transform.rotation = Quaternion.Slerp(transform.rotation, rotatedTarget , Time.deltaTime * smooth);



            }



        }
    }

    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag(TagCollectable))
        {

            Destroy(other.gameObject);
            
            score++;
            
            UI.UpdateScoreText(score.ToString());

        }
        else if (other.CompareTag(TagFinish) && !isLevelDone)
        {
            isLevelDone = true;

            GetFinalScore();
            UI.UpdateScoreText(score.ToString());

            GC.LevelComplete();
          
        }
        else if (other.CompareTag(TagCube))
        {

            AddCube(other.gameObject);

        }

        else if (other.CompareTag(TagFallTrigger))
        {


            StartCoroutine(DelayedFall());


        }
        else if (other.CompareTag(TagBoosterStart))
        {

            StartCoroutine(BoostTrigger());

        }
        else if (other.CompareTag(TagMagnetTrigger))
        {

            StartCoroutine(MagnetBoolControl());

            Destroy(other.gameObject);

        }

        else if (other.CompareTag(TagFlag))
        {

            FlagAction(other.gameObject);

        }

        /*
        else if (other.CompareTag(TagLava))
        {
            //DropCube(myCubes[myCubes.Count - 1]);
            Debug.Log("Lava Trigger");
            //LavaTrigger();
            tempPos = transform.position;
            tempPos.y -= 1;
            transform.position = tempPos;

        }
        */

        else if (other.CompareTag(TagWayPoint))
        {
            other.gameObject.GetComponent<Collider>().enabled = false;
            
            
            if (!isWaypointPart)
            {
                firstRotation = transform.rotation;
                wayPoints.Clear();
                for (int i = 0; i < other.gameObject.transform.parent.childCount; i++)
                {
                    wayPoints.Add(other.gameObject.transform.parent.GetChild(i).transform);

                }
               
                isWaypointPart = true;

                targetPosition = wayPoints[1].position;
                targetPosition.y = transform.position.y;
                waypointIndex++;
                
           
            }

            else
            {
                waypointIndex++;

                if (waypointIndex >= wayPoints.Count)
                {
                    isWaypointPart = false;
                    transform.rotation = firstRotation;
                }
                else
                {
                    targetPosition = wayPoints[waypointIndex].position;
                    targetPosition.y = transform.position.y;
                }
            }
        }

       
       
        else if (other.CompareTag(TagTurnTrigger))
        {
            isTurned = true;
            isTurning = true;

            /*
            var rotationVector = transform.rotation.eulerAngles;
            rotationVector.y = 90;
            transform.rotation = Quaternion.Euler(rotationVector);
            */

            GC.TurnCamera();



        }

        else if (other.CompareTag(TagEndGameTrigger))
        {
            
            isEndGameTriggered = true;


        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagLava))
        {
            Debug.Log("Lavadana Çýktý");
            MeltingAction(other.gameObject);
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.collider.CompareTag(TagObstacle))
        {
            DropCube(collision.contacts[0].thisCollider.transform.gameObject);
         
            StartCoroutine(DelayedFall());
           
        }

        else if (collision.collider.CompareTag(TagWall))
        {
          
            DropCube(collision.contacts[0].thisCollider.transform.gameObject);

        }

        else if (collision.collider.CompareTag(TagMultiplierBox))
        {
            collision.collider.transform.tag = "Untagged";
            
            DropCube(collision.contacts[0].thisCollider.transform.gameObject);

            if (cubeCount > 0)
            {
                multiplierValue++;
            }
                
            if(multiplierValue > 12)
            {
                multiplierValue = 20;
            }           
        }
    }

    void AddCube(GameObject go)
    {

        Destroy(go);
        tempPos.x = transform.position.x;
        tempPos.y = 0;
        tempPos.z = transform.position.z;
        tempCube = Instantiate(CubePrefab, tempPos, Quaternion.identity);
        tempCube.transform.SetParent(cubeContainer);
        transform.position += Vector3.up;
        myCubes.Add(tempCube);
        cubeCount++;
    }

    void DropCube(GameObject go)
    {
        go.transform.SetParent(null);
        myCubes.Remove(go);
        go.AddComponent<Rigidbody>();
        cubeCount--;
        CheckFail();
        
    }

  
    void DropCubeWall(int index)
    {
        for (int i = 0; i < index; i++)
        {

            GameObject go = myCubes[myCubes.Count - 1];

            go.transform.SetParent(null);
            myCubes.Remove(go);
            go.AddComponent<Rigidbody>();
            cubeCount--;
            CheckFail();


        }
    }

    IEnumerator BoostTrigger()
    {
       
        
        speedForward +=1;
        yield return new WaitForSeconds(3f);

        speedForward -= 1;

    }

    
    IEnumerator MagnetBoolControl()
    {
        Magnet.SetActive(true);
        isMagnetTriggered = true;

        yield return new WaitForSeconds(3f);

        isMagnetTriggered = false;
        Magnet.SetActive(false);

    }


    void MeltingAction(GameObject trigger)
    {
        trigger.transform.tag = "Untagged";
        tempCube = myCubes[myCubes.Count - 1];
        myCubes.Remove(tempCube);
        cubeCount--;
        Destroy(tempCube);
        tempPos = transform.position;
        tempPos.y -= 1;
        transform.position = tempPos;
        CheckFail();
    }


    IEnumerator DelayedFall()
    {
        if (!isFalling)
        {
            isFalling = true;
            yield return delaySec;
            fallCounter = fallTime;
        }
               
    }

    void FlagAction(GameObject go)
    {
        go.GetComponent<Animator>().enabled = true;
    }

    void GetFinalScore()
    {
        finalScore = multiplierValue * score;
        score += finalScore;
    }
    
    /*
    void TrailAction(GameObject go)
    {
        tempPos = Trail.transform.localPosition;
        tempPos.y = cubeCount * -1 + 1;
        Trail.transform.localPosition = tempPos;
    }
    */

    void ActionFall()
    {

        if (fallCounter > 0)
        {
            fallCounter -= Time.deltaTime;

           
            Vector3 tempPos2;
         
            for (int i = 0; i < myCubes.Count; i++)
            {
                tempPos.y = i * -1;
                tempPos.x = myCubes[i].transform.localPosition.x;
                tempPos.z = myCubes[i].transform.localPosition.z;
                tempPos2 = myCubes[i].transform.localPosition;
                tempPos2 = (myCubes[i].transform.localPosition - tempPos) * (fallTime - fallCounter);
                tempPos2 = myCubes[i].transform.localPosition - tempPos2;
                myCubes[i].transform.localPosition = tempPos2;
            }
            tempPos.x = transform.position.x;
            tempPos.z = transform.position.z;
            tempPos.y = cubeCount;

           

            tempPos2 = (transform.position - tempPos) * (fallTime - fallCounter);
            tempPos2 = transform.position - tempPos2;

            transform.position = tempPos2;

           
            if (fallCounter < 0)
            {
                isFalling = false;
            }
        }


    }

    
    void WaypointAction()
    {                 
        targetPosition.y = transform.position.y;
        direction = (targetPosition - transform.position).normalized; // normalized birim vektöre çeviriyor.
        transform.position = transform.position + direction * Time.deltaTime * speed;
        transform.LookAt(targetPosition);
    }  
    
        #region End Game Actions

        void CheckFail()
    {
        if (isEndGameTriggered)    
        {
            
            if (cubeCount <= 0)
            {
                GC.LevelComplete();
            }

        }

        else if (cubeCount <= 0)

        {
            GC.LevelFail();

        }

    }

    public void ActionLevelFail()
    {
        RB.isKinematic = false;
        RB.constraints = RigidbodyConstraints.None;

    }

    public void ActionLevelDone()
    {
        //anim.SetBool("Dance", true);
    }


    #endregion
}
