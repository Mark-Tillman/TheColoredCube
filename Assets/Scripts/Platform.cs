using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public static float fastFallSpeed;
    public static float normFallSpeed;
    public Transform platformPrefab;
    public float platformDistance = 8;
    public float fallRateStep = 0.1f;
    public float maxFallSpeed;

    [ColorUsage(true, true)]
    public Color[] colors;
    public Platform[] partnerPlats;

    public bool correctPlat = false;
    public int colorNum;
    int nextPlat;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.transform.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().grounded = true;

            if(correctPlat)
            {
                other.gameObject.GetComponent<Player>().ScorePoint();
                other.gameObject.GetComponent<Player>().canMove = false;

                //Generate 2 new platforms
                Transform rightPlat = Instantiate(platformPrefab, new Vector3(transform.position.x + platformDistance, transform.position.y, transform.position.z + platformDistance), transform.rotation);
                Transform leftPlat = Instantiate(platformPrefab, new Vector3(transform.position.x - platformDistance, transform.position.y, transform.position.z + platformDistance), transform.rotation);
                Transform centerPlat = Instantiate(platformPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z + platformDistance * 2), transform.rotation);
                centerPlat.gameObject.SetActive(false);

                //Contain both platforms in array
                Platform[] nextPlats = {leftPlat.GetComponent<Platform>(), rightPlat.GetComponent<Platform>(), centerPlat.GetComponent<Platform>()};

                //Generate colors for each platform
                nextPlats[0].GenerateColor();
                nextPlats[1].GenerateColor();
                nextPlats[2].SetColor(nextPlats[0].GetColor());

                //If left and right have the same color, center is next
                if(nextPlats[0].GetColor() == nextPlats[1].GetColor())
                {
                    nextPlat = 0; //THIS ONLY MEANS PLAYER KEEPS COLOR OF LEFT PLATFORM (same as right)
                    nextPlats[2].SetCorrect(true);
                    nextPlats[1].SetCorrect(false);
                    nextPlats[0].SetCorrect(false);
                }
                //Else, randomly pick left or right to be next
                else
                {
                    //Determine which platform is the next correct choice
                    nextPlat = Random.Range(0, 2);
                    if(nextPlat == 0)
                    {
                        nextPlats[0].SetCorrect(true);
                        nextPlats[1].SetCorrect(false);
                        nextPlats[2].SetCorrect(false);
                    }
                    else
                    {
                        nextPlats[0].SetCorrect(false);
                        nextPlats[1].SetCorrect(true);
                        nextPlats[2].SetCorrect(false);
                    }
                }

                //Tell platforms about their partners
                nextPlats[0].SetPartners(ref nextPlats);
                nextPlats[1].SetPartners(ref nextPlats);
                nextPlats[2].SetPartners(ref nextPlats);
                
                //Update player color
                other.gameObject.GetComponent<Player>().SetColor(nextPlats[nextPlat].GetColor());

                //Tell player about the platforms
                other.gameObject.GetComponent<Player>().SetPlatforms(ref nextPlats);
                
                if(other.gameObject.GetComponent<Player>().GetScore() > 1)
                {
                    Destroy(gameObject, 2.0f);
                    gameObject.GetComponent<Animator>().speed = normFallSpeed;
                    gameObject.GetComponent<Animator>().SetBool("fall", true);
                    EnableColliders();
                }
                else
                {
                    fastFallSpeed = 3.5f;
                    normFallSpeed = 0.7f;
                }

            }
            else
            {
                EnableColliders();
                other.gameObject.GetComponent<Player>().gameManager.LoseGame();
            }

            normFallSpeed = Mathf.Min(normFallSpeed + fallRateStep, maxFallSpeed);

        }
        //StartCoroutine(EnableGravity());
    }

    public Color GetColor()
    {
        return colors[colorNum];
    }

    public void SetColor(Color col)
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<MeshRenderer>().material.color = col;
        }
    }

    public void GenerateColor()
    {
        colorNum = Random.Range(0, colors.Length);
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<MeshRenderer>().material.color = colors[colorNum];
        }
        
    }

    public void SetCorrect(bool correct)
    {
        correctPlat = correct;
    }

    public void SetPartners(ref Platform[] partners)
    {
        partnerPlats = partners;
    }

    public void EnableColliders()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;

        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<BoxCollider>().enabled = true;
        }
    }

    public void FallDown()
    {
        Destroy(gameObject, 4.0f);
        gameObject.GetComponent<Animator>().speed = fastFallSpeed;
        gameObject.GetComponent<Animator>().SetBool("fall", true);
    }
}