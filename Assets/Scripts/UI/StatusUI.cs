using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class StatusUI : UIBase
{
    private Button button_Atk_Plus;
    private Button button_Hp_Plus;
    private Button button_Close;

    private TextMeshProUGUI text_Atk;
    private TextMeshProUGUI text_Hp;
    private TextMeshProUGUI text_StatPoint;

    private AbilityComponent abilityComponent;

    enum Buttons
    {
        Button_Atk_Plus,
        Button_Hp_Plus,
        Button_Close
    }

    enum Texts
    {
        Text_Atk,
        Text_Hp,
        Text_StatPoint
    }

    private void Awake()
    {
        Bind<Button>(typeof(Buttons));
        Bind<TextMeshProUGUI>(typeof(Texts));

        button_Atk_Plus = Get<Button>((int)Buttons.Button_Atk_Plus);
        button_Hp_Plus = Get<Button>((int)Buttons.Button_Hp_Plus);
        button_Close = Get<Button>((int)Buttons.Button_Close);

        text_Atk = Get<TextMeshProUGUI>((int)Texts.Text_Atk);
        text_Hp = Get<TextMeshProUGUI>((int)Texts.Text_Hp);
        text_StatPoint = Get<TextMeshProUGUI>((int)Texts.Text_StatPoint);
    }

    private void OnEnable()
    {
        if(abilityComponent == null) 
        {
            abilityComponent = Player.player.abilityComponent;
        }

        UpdateText();
    }

    private void Start()
    {
        button_Atk_Plus.onClick.AddListener(OnClicked_Atk_Plus);
        button_Hp_Plus.onClick.AddListener(OnClicked_Hp_Plus);
        button_Close.onClick.AddListener(OnClicked_Close);

        abilityComponent.LevelChangeEvent += UpdateTextEvent;
    }

    private void OnClicked_Atk_Plus()
    {
        abilityComponent.IncreaseAtk();
        UpdateText();
    }

    private void OnClicked_Hp_Plus()
    {
        abilityComponent.IncreaseHp();
        UpdateText();
    }
    
    private void OnClicked_Close()
    {
        UIManager.inst.UIController(GameUI.StatUI, false);
    }

    private void UpdateText()
    {
        text_Atk.text = abilityComponent.GetAtk().ToString();
        text_Hp.text = abilityComponent.GetHp().ToString();
        text_StatPoint.text = abilityComponent.GetStatPoint().ToString();
    }

    private void UpdateTextEvent(object sender, EventArgs eventArgs)
    {
        if (transform.gameObject.activeSelf)
        {
            UpdateText();
        }       
    }
}
