using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager inst { get { return instance; } }

    //* 프리펩 선언 부분 *//
    [SerializeField] private GameObject statUIPrefab;
    [SerializeField] private GameObject skillUIPrefab;
    [SerializeField] private GameObject stageUIPrefab;
    [SerializeField] private GameObject deathUIPrefab;
    [SerializeField] private GameObject gameTestUIPrefab;

    //* 오브젝트 및 변수 선언 부분 *//
    private GameObject statUI;
    private GameObject skillUI;
    private GameObject stageUI;
    private GameObject deathUI;
    private GameObject gameTestUI;

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

        if(skillUI == null)
        {
            skillUI = Instantiate<GameObject>(skillUIPrefab);
            skillUI.SetActive(false);
        }

        if (stageUI == null)
        {
            stageUI = Instantiate<GameObject>(stageUIPrefab);
            stageUI.SetActive(false);
        }

        if (deathUI == null)
        {
            deathUI = Instantiate<GameObject>(deathUIPrefab);
            deathUI.SetActive(false);
        }

        if (gameTestUI == null)
        {
            gameTestUI = Instantiate<GameObject>(gameTestUIPrefab);
            gameTestUI.SetActive(false);
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
                    skillUI.SetActive(bOpen);
                    break;
                }
            case GameUI.StageUI:
                {
                    stageUI.SetActive(bOpen);
                    break;
                }
            case GameUI.DeathUI:
                {
                    deathUI.SetActive(bOpen);
                    break;
                }
            case GameUI.GameTestUI:
                {
                    gameTestUI.SetActive(bOpen);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}
