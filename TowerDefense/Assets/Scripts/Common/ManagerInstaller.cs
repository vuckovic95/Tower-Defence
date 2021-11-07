using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using NaughtyAttributes;

/// <summary>
/// Manager instaler koristim da bih mogao da pristupim skriptama preko drugih skripti(poput singletona). Razlog zasto sam se odlucio za ovo je sto
/// singletonu moze da pristupi svako i pri vecim projektima i vise ljudi na istim, desavalo nam se da jedan drugog overrideuje i pristupi odredjenim komponentama iz klase koja uopste ne treba
/// da ima pristup tome. Radjeno je po ugledu na WEB development i dependency injection
/// </summary>
public class ManagerInstaller : MonoInstaller
{
    [BoxGroup("Installers")]
    [SerializeField]
    private GameObject _gameManager;

    [BoxGroup("Installers")]
    [SerializeField]
    private GameObject _dialogService;

    [BoxGroup("Installers")]
    [SerializeField]
    private GameObject _timeManager;

    [BoxGroup("Installers")]
    [SerializeField]
    private GameObject _sceneManager;

    [BoxGroup("Installers")]
    [SerializeField]
    private GameObject _bonusManager;

    [BoxGroup("Installers")]
    [SerializeField]
    private GameObject _dataManager;

    [BoxGroup("Installers")]
    [SerializeField]
    private GameObject _poolManager;

    public override void InstallBindings()
    {
        //Container.Bind<GameManager>().FromComponentsOn(_gameManager).AsSingle();
        //Container.Bind<DialogService>().FromComponentsOn(_dialogService).AsSingle();
        //Container.Bind<TimeManager>().FromComponentsOn(_timeManager).AsSingle();
        //Container.Bind<SceneSwitcher>().FromComponentsOn(_sceneManager).AsSingle();
        //Container.Bind<BonusManager>().FromComponentsOn(_bonusManager).AsSingle();
        //Container.Bind<DataManager>().FromComponentsOn(_dataManager).AsSingle();
        //Container.Bind<PoolManager>().FromComponentsOn(_poolManager).AsSingle();

        //Container.Bind<KeyHandler>().AsSingle();
    }
}
