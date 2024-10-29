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

        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    { 
        UpdateText();
    }

    private void Start()
    {
        button_Atk_Plus.onClick.AddListener(OnClicked_Atk_Plus);
        button_Hp_Plus.onClick.AddListener(OnClicked_Hp_Plus);
        button_Close.onClick.AddListener(OnClicked_Close);

        Player.player.abilityComponent.LevelChangeEvent += UpdateTextEvent;
    }

    private void OnClicked_Atk_Plus()
    {
        Player.player.abilityComponent.IncreaseAtk();
        UpdateText();
    }

    private void OnClicked_Hp_Plus()
    {
        Player.player.abilityComponent.IncreaseHp();
        UpdateText();
    }
    
    private void OnClicked_Close()
    {
        UIManager.inst.UIController(GameUI.StatUI, false);
    }

    private void UpdateText()
    {
        text_Atk.text = Player.player.abilityComponent.GetStatAtk().ToString();
        text_Hp.text = Player.player.abilityComponent.GetMaxHp().ToString();
        text_StatPoint.text = Player.player.abilityComponent.GetStatPoint().ToString();
    }

    private void UpdateTextEvent(object sender, EventArgs eventArgs)
    {
        if (transform.gameObject.activeSelf)
        {
            UpdateText();
        }       
    }
}
