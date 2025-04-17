using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
    [SerializeField]
    Animator m_ani;

    Animator m_Animator;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void PlayStandAnimation()
    //{
    //    m_ani.Play("Idel");
    //}
    public void PlayRunAnimation()
    {
        m_Animator.SetTrigger("Walk");
    }
    public Vector3 Move(Vector3 forward, float speed)
    {
        Vector3 curPos = transform.position;
        Vector3 offsetPos = new Vector3(forward.x * speed, forward.y * speed, forward.z * speed);
        Vector3 newPos = curPos + offsetPos;
        transform.position = newPos;
        transform.forward = forward;

        return offsetPos;
    }
}
