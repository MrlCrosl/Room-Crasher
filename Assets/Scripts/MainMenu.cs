using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : UIGroup
{
   
    [SerializeField] private Button _startButton;
    [SerializeField] private TextMeshProUGUI _tapToPlayText;
    [SerializeField] private SessionController _sessionController;
    [SerializeField] private GameObject _joystick;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        _startButton.onClick.AddListener(StartSession);
        _tapToPlayText.DOFade(0, 1f).SetLoops(-1, LoopType.Yoyo);
    }



    private void StartSession()
    {
        _tapToPlayText.DOKill();
        _joystick.gameObject.SetActive(true);
        _sessionController.StartSession();
        Hide();
    }

    public override void Reset()
    {
        
    }
}
