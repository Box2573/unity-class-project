using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerSingle//GameManagerSingle為一個Class(類別)
{//寫在此{內的}都是GameManagerSingle這個類別的方法、屬性
    private GameObject gameObject;//場景中唯一存在實體
    private static GameManagerSingle m_Instance;//m_Instance為GameManagerSingle類別
    //就像人類為一個類別，王大明是一個人類類別

    public static GameManagerSingle Instance//透過Instance來存取m_Instance(也透過此來取用)
    {
        get
        {
            if (m_Instance == null)//假如m_Instance不存在
            {
                m_Instance = new GameManagerSingle();//就生成一個m_Instance
                m_Instance.gameObject = new GameObject("GameManager");//並且生成一個新的GameObject叫GameManager
                                                                      //gameobject為GameManagerSingle的屬性可直接調用
                m_Instance.gameObject.AddComponent<InputController>();//在此物件上先增InputController腳本
            }
            return m_Instance;//把m_Instance放進Instance
        }
    }

    private InputController m_Inputcontroller;

    public InputController InputController
    {
        get
        {
            if (m_Inputcontroller == null)
            {
                m_Inputcontroller = gameObject.GetComponent<InputController>();
            }
            return m_Inputcontroller;
        }
    }
}
