using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stone : MonoBehaviour
{
    [SerializeField] GameObject icon;
    [SerializeField] TextMeshProUGUI textMesh;

    static int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        icon.SetActive(false);
    }

    public void picked()
    {
        Debug.Log("pick");
        count++;
        textMesh.text = count.ToString();
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            icon.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            icon.SetActive(false);
        }
    }
}
