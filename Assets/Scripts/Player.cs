using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameManager gameManager;
    public float dropHeight;
    public float speed;
    public float deathHeight;
    private Platform leftPlatform;
    private Platform rightPlatform;
    private Platform centerPlatform;
    private Platform nextPlatform;
    private Transform currentPlatform;
    private Vector3 targetPosition;
    private Vector3 defaultRotation;
    public bool canMove = false;
    public bool atPosition = true;
    public bool grounded = false;
    bool leftPressed = false;
    bool rightPressed = false;
    public float keyCheckTime = 0.5f;
    public bool inputReceived = false;
    bool touching = false;
    Vector2 touchStart;
    Vector2 touchEnd;
    Vector2 touchDirection;

    // Start is called before the first frame update
    void Start()
    {
        targetPosition = transform.position;
        defaultRotation = new Vector3(0, 45, 0);
    }

    // Update is called once per frame
    void Update()
    {  
        //Fist check that game is active
        if(gameManager.gameActive)
        {
            //Lost the game is the player is below the deathHeight
            if(transform.position.y < deathHeight)
                gameManager.LoseGame();

            
            //Get touch controls
            if (Input.touchCount > 0 && grounded)
            {
                Touch touch = Input.GetTouch(0);

                // Handle finger movements based on TouchPhase
                switch (touch.phase)
                {
                    //When a touch has first been detected, change the message and record the starting position
                    case TouchPhase.Began:
                        // Record initial touch position.
                        touchStart = touch.position;
                        Debug.Log("STARTED TOUCH");
                        break;

                    //Determine if the touch is a moving touch
                    case TouchPhase.Moved:
                        // Determine direction by comparing the current touch position with the initial one
                        touchDirection = touch.position - touchStart;
                        Debug.Log("MOVING");
                        break;

                    case TouchPhase.Ended:
                        // Report that the touch has ended when it ends
                        Debug.Log("STOP TOUCH");
                        if(Vector2.Dot(touchDirection.normalized, Vector2.up) > 0.9)
                        {
                            Debug.Log("moved up");
                            MoveForward();
                        }
                        else if(touchDirection.x < 0)
                        {
                            Debug.Log("moved left");
                            MoveLeft();
                        }
                        else if(touchDirection.x > 0)
                        {
                            Debug.Log("moved right");
                            MoveRight();
                        }
                        touchDirection = Vector2.zero;
                        break;
                }
            }
            

            //Get keyboard controls
            if(Input.GetKeyDown(KeyCode.A))
                OnLeft();
            else if(Input.GetKeyDown(KeyCode.D))
                OnRight();
            else if(Input.GetKeyDown(KeyCode.W))
            {
                OnLeft();
                OnRight();
            }

            //Check if input has been received and the player is grounded
            if((leftPressed || rightPressed) && !inputReceived && grounded)
            {
                inputReceived = true;
                StartCoroutine(GetMovement());
            }

            //Move the player towards the next position and rotation
            if(canMove && (transform.position.x != targetPosition.x || transform.position.z != targetPosition.z))
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);
                transform.eulerAngles = defaultRotation;
                gameObject.GetComponent<Rigidbody>().freezeRotation = true;
            }
            /*
            if((transform.rotation.x != defaultRotation.x || transform.rotation.z != defaultRotation.z || transform.rotation.y != defaultRotation.y))
            {
                Vector3 newDirection = Vector3.RotateTowards(transform.rotation.eulerAngles, defaultRotation.eulerAngles, (Time.deltaTime * speed), 0.0f);
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
            */
            //Stop moving the player when they are at the next position
            if(Vector3.Distance(transform.position, targetPosition) < 0.01f)
            {
                
                gameObject.GetComponent<Rigidbody>().useGravity = true;
                gameObject.GetComponent<Rigidbody>().freezeRotation = false;
                //gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                canMove = false;
            }
        }
        else
        {
            gameObject.GetComponent<Rigidbody>().useGravity = true;
        }
            
    }
    public IEnumerator GetMovement()
    {
        //Wait for keyCheckTime to allow the player to input left and right if they want to move forward
        yield return new WaitForSeconds(keyCheckTime);
        if(leftPressed && rightPressed)
        {
            MoveForward();
            leftPressed = false;
            rightPressed = false;
        }
        else if (leftPressed)
        {
            MoveLeft();
            leftPressed = false;
        }

        else if (rightPressed)
        {
            MoveRight();
            rightPressed = false;
        }
        inputReceived = false;
    }

    public void MoveLeft()
    {
        targetPosition = new Vector3(leftPlatform.transform.position.x, leftPlatform.transform.position.y + dropHeight, leftPlatform.transform.position.z);
        canMove = true;
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        grounded = false;
        DropPlatforms();
    }

    public void MoveRight()
    {
        targetPosition = new Vector3(rightPlatform.transform.position.x, rightPlatform.transform.position.y + dropHeight, rightPlatform.transform.position.z);
        canMove = true;
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        grounded = false;
        DropPlatforms();
    }

    public void MoveForward()
    {
        centerPlatform.gameObject.SetActive(true);
        targetPosition = new Vector3(centerPlatform.transform.position.x, centerPlatform.transform.position.y + dropHeight, centerPlatform.transform.position.z);
        canMove = true;
        gameObject.GetComponent<Rigidbody>().useGravity = false;
        grounded = false;
        DropPlatforms();
    }

    public void DropPlatforms()
    {
        if(!leftPlatform.correctPlat)
            leftPlatform.FallDown();
        if(!rightPlatform.correctPlat)
            rightPlatform.FallDown();
        if(!centerPlatform.correctPlat)
            centerPlatform.FallDown();
    }

    public void SetColor(Color newColor)
    {
        transform.GetComponent<MeshRenderer>().material.color = newColor;
    }

    public void SetPlatforms(ref Platform[] nextPlats)
    {
        leftPlatform = nextPlats[0];
        rightPlatform = nextPlats[1];
        centerPlatform = nextPlats[2];
    }

    public void OnLeft()
    {
        if(gameManager.gameActive)
            leftPressed = true;
    }

    public void OnRight()
    {
        if(gameManager.gameActive)
            rightPressed = true;
    }

    public void ScorePoint()
    {
        gameManager.ScorePoint(1);
    }

    public float GetScore()
    {
        return gameManager.GetScore();
    }
}
