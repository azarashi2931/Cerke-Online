﻿using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using static UnityEngine.UI.Dropdown;
using Azarashi.CerkeOnline.Application.Language;
using static Azarashi.CerkeOnline.Domain.Entities.Terminologies;

namespace Azarashi.CerkeOnline.Presentation.View.UI
{
    public class GameRuleSelectionView : MonoBehaviour
    {
        public IObservable<int> OnDropDownChanged => dropdown.OnValueChangedAsObservable().TakeUntilDestroy(this);

        [SerializeField] Dropdown dropdown = default;
        
        void Start()
        {
            if (dropdown == null) throw new NullReferenceException();

            List<OptionData> options = Enum.GetNames(typeof(RulesetName))
                .Select(name => "Ruleset" + name)
                .Select(keyString => (TranslatableKeys)Enum.Parse(typeof(TranslatableKeys), keyString))
                .Select(LanguageManager.Instance.Translator.Translate)
                .Select(name => new OptionData(name)).ToList();
            dropdown.options = options;
        }

    }
}