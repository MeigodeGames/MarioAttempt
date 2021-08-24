using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    public Transform m_Player;
    public Transform m_RespawnPoint;
    public LifeUI m_LifeUI;

    private void OnTriggerEnter(Collider other) {
        if (other.tag.Contains("Player"))
        //if (other.CompareTag("Player"))
        {
            m_Player.transform.position = m_RespawnPoint.transform.position;
            m_LifeUI.LifeDown();
            Physics.SyncTransforms();
        }

    }
}
