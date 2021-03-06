﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using Azarashi.CerkeOnline.Application;
using static Azarashi.CerkeOnline.Domain.Entities.Terminologies;
using Azarashi.CerkeOnline.Presentation.View.UI;
using Azarashi.CerkeOnline.Data.DataStructure;

//本当はusingしたくない
using UnityEngine.UI;

namespace Azarashi.CerkeOnline.Presentation.Presenter.UI
{
    public class PreGameSettingsPresenter : MonoBehaviour
    {
        [SerializeField] PreGameSettings preGameSettings = default;

        [SerializeField] GameRuleSelectionView gameRuleSelectionView = default;
        [SerializeField] FirstOrSecondSelectionView firstOrSecondSelectionView = default;
        [SerializeField] EncampmentSelectionView encampmentSelectionView = default;

        //Viewコンポーネントをはさむべき？（描画のための特別な処理はないので迷う）
        [SerializeField] Toggle ZeroDistanceMovementPermissionToggle = default;
        [SerializeField] Button startButton = default;

        void Start()
        {
            Bind();
        }

        void Bind()
        {
            gameRuleSelectionView.OnDropDownChanged.TakeUntilDestroy(this).Subscribe(value => preGameSettings.rulesetName = (RulesetName)value);
            firstOrSecondSelectionView.OnDropDownChanged.TakeUntilDestroy(this).Subscribe(value => preGameSettings.firstOrSecond = (FirstOrSecond)value);
            encampmentSelectionView.OnDropDownChanged.TakeUntilDestroy(this).Subscribe(value => preGameSettings.encampment = (Encampment)value);
            
            ZeroDistanceMovementPermissionToggle.OnValueChangedAsObservable().TakeUntilDestroy(this).Subscribe(value => preGameSettings.isZeroDistanceMovementPermitted = value);
            startButton.OnClickAsObservable().TakeUntilDestroy(this).Subscribe(_ => 
            {
                preGameSettings.OnStartButton();
                SceneManager.UnloadSceneAsync(SceneName.MainSceneUI.PreGameSettings);
            });
        }
    }
}