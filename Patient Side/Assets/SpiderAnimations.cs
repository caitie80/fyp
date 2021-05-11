using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAnimations : MonoBehaviour
{
    Animator anim;
    private GameObject[] hands;
    private float timeToChangeDirection;

    int distanceHash = Animator.StringToHash("Distance");

    int isWalkingHash = Animator.StringToHash("IsWalking");
    private Vector3 newPosition;
    private Vector3 newDirection;
    private Quaternion lookRotation;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        hands = GameObject.FindGameObjectsWithTag("Hand");
        Debug.Log("HAND: " + hands[0]);
        Debug.Log("THIS: " + this);

        newPosition = transform.position;

        ChangeDirection();
    }

    // Update is called once per frame
    void Update()
    {
        float space1 = (hands[0].transform.position - this.transform.position).sqrMagnitude;
        float space2 = (hands[1].transform.position - this.transform.position).sqrMagnitude;

        if(space1 < space2)
        {
            anim.SetFloat(distanceHash, space1);
        }
        else
        {
            anim.SetFloat(distanceHash, space2);            
        }

        timeToChangeDirection -= Time.deltaTime;

        if (timeToChangeDirection <= 0)
        {
            ChangeDirection();
        }
        else if(timeToChangeDirection <=1.5f)
        {
            transform.position = Vector3.Lerp(transform.position, newPosition, 0.01f);
        }
        else
        {
            newDirection = (newPosition - transform.position).normalized;

            lookRotation = Quaternion.LookRotation(newDirection);

            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 1f);
        }


    }

    private void ChangeDirection()
    {
        //Quaternion quat = Quaternion.Euler(transform.rotation.x, Random.Range(0f, 360f), transform.rotation.z);
        //transform.rotation = quat;
        newPosition = new Vector3(Random.Range(0.55f, 1.08f), 0.5f, Random.Range(0.25f, -0.92f));

        timeToChangeDirection = 4f;        
    }
}
