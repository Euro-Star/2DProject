using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SignInUI : UIBase
{
    private Button button_SignIn;

    enum Buttons
    {
        Button_SignIn
    }

    private void Awake()
    {
        Bind<Button>(typeof(Buttons));

        button_SignIn = Get<Button>((int)Buttons.Button_SignIn);
    }

    private void Start()
    {
        button_SignIn.onClick.AddListener(OnClicked_SignIn);
    }
    
    private void OnClicked_SignIn()
    {
        SignInManager.inst.SignInWithGoogle();
    }
}
