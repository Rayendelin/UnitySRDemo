using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerControl03 PlayerControl03 = other.GetComponent<PlayerControl03>();
        if (PlayerControl03 != null)
        {
            PlayerControl03.SetRespawnPosition(transform.position);
        }
    }
}
