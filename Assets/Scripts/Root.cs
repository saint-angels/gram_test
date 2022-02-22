using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Tactics
{
    public class Root : MonoBehaviour
    {
        [SerializeField] private Battle.BattleManager battleManager = null;
        [SerializeField] private View.LevelView levelView = null;

        private static Root _instance;

        void Awake()
        {
            _instance = this;
        }

        void Start()
        {
            levelView.Init(battleManager);

            var selectedUnits = new List<UnitType>() { UnitType.Bard, UnitType.DamageDealer };
            battleManager.StartBattle(selectedUnits);
        }
    }
}
