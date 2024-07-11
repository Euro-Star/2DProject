using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager inst { get { return instance; } }

    //* ������ ���� �κ� *//
    [SerializeField]
    private GameObject statUIPrefab;

    //* ������Ʈ �� ���� ���� �κ� *//
    private GameObject statUI;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if (statUI == null)
        {
            statUI = Instantiate<GameObject>(statUIPrefab);
            statUI.SetActive(false);
        }
    }

    public void UIController(GameUI _enum, bool bOpen)
    {
        switch(_enum) 
        {
            case GameUI.StatUI:
                {
                    statUI.SetActive(bOpen);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}