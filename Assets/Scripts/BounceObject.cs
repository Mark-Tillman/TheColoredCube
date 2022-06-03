using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceObject : MonoBehaviour
{
    float startHeight;
    float bounceHeight;
    float bounceSpeed;

    bool up = true;

    // Start is called before the first frame update
    void Start()
    {
        startHeight = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(up && transform.position.y < bounceHeight)
        {
            transform.position += (new Vector3(0, bounceSpeed * Time.deltaTime, 0));
        }
        else if(up && transform.position.y >= bounceHeight)
        {
            up =false;
        }

        if(!up && transform.position.y > startHeight)
        {
            transform.position += (new Vector3(0, -bounceSpeed * Time.deltaTime, 0));
        }
        else if(!up && transform.position.y <= startHeight)
        {
            up = true;
        }
    }

    public void SetParams(float height, float speed)
    {
        bounceHeight = height;
        bounceSpeed = speed;
        
    }

    public void SetColor(Color newColor)
    {
        GetComponent<MeshRenderer>().material.color = newColor;
    }
}
