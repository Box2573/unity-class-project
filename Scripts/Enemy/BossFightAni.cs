using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BossFightAni : MonoBehaviour
{
    [SerializeField] PlayableDirector playableDirector;
    [SerializeField] GameObject boss;

    GameObject player;

    public bool canMove = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canMove = false;
            boss.SetActive(true);
            playableDirector.Play();
            player = other.gameObject;
            Invoke("finish", (float)playableDirector.duration);
        }
    }

    private void finish()
    {
        canMove = true;
        boss.transform.position = new Vector3(-43.65f, 1.65f, -56.56f);
        this.gameObject.GetComponent<Collider>().isTrigger = false;
    }

}