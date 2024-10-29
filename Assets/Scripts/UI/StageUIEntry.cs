using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageUIEntry : UIBase
{
    private TextMeshProUGUI text_Stage;
    private Button button_Stage;
    private Image image_Lock;
    private int openLevel;

    private string stageText = "Stage ";
    private string stageScene = "Stage_";

    enum Images
    {
        Image_Lock
    }

    enum Texts
    {
        Text_Stage
    }

    private void Awake()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));
        Bind<Image>(typeof(Images));

        text_Stage = Get<TextMeshProUGUI>((int)Texts.Text_Stage);
        button_Stage = GetComponent<Button>();
        image_Lock = Get<Image>((int)Images.Image_Lock);
    }

    private void OnEnable()
    {
        LockStage();
    }

    private void Start()
    {
        button_Stage.onClick.AddListener(OnClicked_Stage);
    }

    private void OnClicked_Stage()
    {
        LoadingSceneManager.LoadScene(stageScene);
    }

    public void Init(int stage, int openLevel)
    {
        stageText += stage;
        stageScene += stage; 
        text_Stage.text = stageText;
        this.openLevel = openLevel;

        LockStage();
    }

    private void LockStage()
    {
        if(Player.player.abilityComponent.GetPlayerData().level < openLevel)
        {
            image_Lock.enabled = true;
            button_Stage.interactable = false;
        }
        else
        {
            image_Lock.enabled = false;
            button_Stage.interactable = true;
        }
    }
}
