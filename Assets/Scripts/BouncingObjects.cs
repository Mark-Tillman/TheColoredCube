using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingObjects : MonoBehaviour
{
    public float maxHeight;
    public float minHeight;
    public float maxSpeed;
    public float minSpeed;

    public float minR;
    public float maxR;
    public float minG;
    public float maxG;
    public float minB;
    public float maxB;


    public bool debugGenColor = false;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<BounceObject>().SetParams(Random.Range(minHeight, maxHeight) + transform.position.y, Random.Range(minSpeed, maxSpeed));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(debugGenColor)
        {
            debugGenColor = false;
            GenerateColors();
        }
    }

    void GenerateColors()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<BounceObject>().SetColor(new Color(Random.Range(minR, maxR), Random.Range(minG, maxG), Random.Range(minB, maxB)));
        }
    }
}
