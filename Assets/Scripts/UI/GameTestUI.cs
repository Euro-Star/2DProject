using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameTestUI : UIBase
{
    private Button button_Confirm;

    enum Buttons
    {
        Button_Confirm
    }

    private void Awake()
    {
        Bind<Button>(typeof(Buttons));

        button_Confirm = Get<Button>((int)Buttons.Button_Confirm);

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        button_Confirm.onClick.AddListener(OnClicked_Confirm);
    }

    private void OnClicked_Confirm()
    {
        Application.Quit();
    }
}
