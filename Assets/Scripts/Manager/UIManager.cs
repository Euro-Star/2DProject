using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager inst { get { return instance; } }

    //* 프리펩 선언 부분 *//
    [SerializeField]
    private GameObject statUIPrefab;

    [SerializeField]
    private GameObject SkillUIPrefab;

    //* 오브젝트 및 변수 선언 부분 *//
    private GameObject statUI;
    private GameObject SkillUI;

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

        if(SkillUI == null)
        {
            SkillUI = Instantiate<GameObject>(SkillUIPrefab);
            SkillUI.SetActive(false);
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
            case GameUI.SkillUI:
                {
                    SkillUI.SetActive(bOpen);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}
