using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIThings : MonoBehaviour
{
    public GameObject conversation;

    bool isOpen = false;

    private void Update()
    {
        if (Vector3.Distance(this.transform.position, GameObject.FindWithTag("Player").transform.position) <= 5 && !isOpen)
        {
            conversation.SetActive(true);
            isOpen = true;
        }
    }
}
