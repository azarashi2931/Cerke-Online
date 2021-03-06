﻿using System;
using UnityEngine;
using UniRx;
using static Azarashi.CerkeOnline.Domain.Entities.Terminologies;

namespace Azarashi.CerkeOnline.Application
{
    [CreateAssetMenu(menuName = "ScriptableObject/PreGameSettings")]
    public class PreGameSettings : ScriptableObject
    {
        public IObservable<Unit> OnStartButtonClicked => onStartButtonClicked;
        readonly Subject<Unit> onStartButtonClicked = new Subject<Unit>();

        public RulesetName rulesetName;
        public FirstOrSecond firstOrSecond;
        public Encampment encampment;
        public bool isZeroDistanceMovementPermitted;

        public void OnStartButton() => onStartButtonClicked.OnNext(Unit.Default);
    }
}