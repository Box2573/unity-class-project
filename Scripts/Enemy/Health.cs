using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Health : MonoBehaviour
{
    [SerializeField] GameObject hp;
    [SerializeField] float health;
    [SerializeField] float currentHealth;

    [SerializeField] Image image;
    [SerializeField] TextMeshPro textMeshPro;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
        hp.SetActive(false);
        Move.ReGen += reGen;
        Move.onAttack += damaged;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (currentHealth == health)
            {
                hp.SetActive(true);
            }

            currentHealth -= GameObject.FindWithTag("Player").GetComponent<PlayerAttack>().damage;
            changeHp();
        }
    }

    private void damaged(Animator animator)
    {
        if (currentHealth == health)
        {
            hp.SetActive(true);
        }

        if ((currentHealth - GameObject.FindWithTag("Player").GetComponent<PlayerAttack>().damage) <= 0)
        {
            if (animator != null)
            {
                //Dead
                hp.SetActive(false);
                animator.SetTrigger("Dead");
            }
        }
        else
        {
            currentHealth -= GameObject.FindWithTag("Player").GetComponent<PlayerAttack>().damage;
            changeHp();
        }
    }

    private void changeHp()
    {
        image.fillAmount = Mathf.Lerp(image.fillAmount, currentHealth / health, 1);
    }

    private void reGen()
    {
        currentHealth = health;
        hp.SetActive(false);
    }
}
