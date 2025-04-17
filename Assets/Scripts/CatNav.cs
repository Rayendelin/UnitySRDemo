using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatNav : MonoBehaviour
{
    public NavMeshAgent Nav;
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.Nav.SetDestination(this.target.position);

        Nav.SetDestination(target.position);
 
        if (Nav.velocity.sqrMagnitude == 0)
            transform.GetComponent<Animator>().SetBool("IsRun", false);
        else
            transform.GetComponent<Animator>().SetBool("IsRun", true);
    }
}
