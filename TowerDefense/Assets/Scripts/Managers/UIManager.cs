using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using NaughtyAttributes;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [BoxGroup("Panels")]
    [SerializeField]
    private GameObject _menuPanel;
    [BoxGroup("Panels")]
    [SerializeField]
    private GameObject _playPanel;
    [BoxGroup("Panels")]
    [SerializeField]
    private GameObject _endPanel;

    [BoxGroup("Text Fields")]
    [SerializeField]
    private TextMeshProUGUI _scoreTxt;
    [BoxGroup("Text Fields")]
    [SerializeField]
    private TextMeshProUGUI _goldTxt;

    [BoxGroup("Buttons")]
    [SerializeField]
    private Button _playBtn;
    [BoxGroup("Buttons")]
    [SerializeField]
    private Button _tryAgainBtn;
    [BoxGroup("Buttons")]
    [SerializeField]
    private Button _toMenuBtn;
    [BoxGroup("Buttons")]
    [SerializeField]
    private Button _newTurretBtn;


    private List<GameObject> _uiPanels;

    private void Start()
    {
        PopulatePanels();
        SubscribeToActions();
    }

    private void SubscribeToActions()
    {
        Actions.UpdateScore += UpdateScore;
        Actions.UpdateGold += UpdateGold;
        Actions.EndGameAction += () => SwitchPanel("End");

        _toMenuBtn.onClick.AddListener(ToMenuClicked);
        _playBtn.onClick.AddListener(PlayGameClicked);
        _tryAgainBtn.onClick.AddListener(PlayGameClicked);
    }

    private void SwitchPanel(string panel)
    {
        TurnOffAllPanels();

        switch (panel)
        {
            case "Menu":
                _menuPanel.SetActive(true);
                return;
            case "Play":
                _playPanel.SetActive(true);
                return;
            case "End":
                _endPanel.SetActive(true);
                return;
        }
    }

    private void PopulatePanels()
    {
        foreach (GameObject panel in this.transform)
        {
            _uiPanels.Add(panel);
        }
    }

    private void TurnOffAllPanels()
    {
        foreach (GameObject panel in _uiPanels)
        {
            panel.SetActive(false);
        }
    }

    private void UpdateScore(int score)
    {
        _scoreTxt.text = "Score : " + score.ToString();
    }

    private void UpdateGold(int gold)
    {
        _goldTxt.text = "Gold : " + gold.ToString();
    }

    private void PlayGameClicked()
    {
        SwitchPanel("Play");
    }

    private void ToMenuClicked()
    {
        SwitchPanel("Menu");
    }
}
