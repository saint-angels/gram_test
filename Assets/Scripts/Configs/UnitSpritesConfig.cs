using System.Collections;
using System.Collections.Generic;
using Tactics.SharedData;
using UnityEngine;

namespace Tactics.Configs
{
    [CreateAssetMenu(fileName = "UnitSpritesConfig", menuName = "Config/UnitSpritesConfig")]
    public class UnitSpritesConfig : ScriptableObject
    {
        [System.Serializable]
        public struct UnitSprite
        {
            public UnitType unitType;
            public Sprite sprite;
        }

        [SerializeField] public UnitSprite[] unitSprites = null;

        public Sprite GetSpriteForUnit(UnitType unitType)
        {
            foreach (var unitSprite in unitSprites)
            {
                if (unitSprite.unitType == unitType)
                {
                    return unitSprite.sprite;
                }
            }
            Debug.LogError("Can't find sprite for unit type " + unitType);
            return null;
        }
    }
}
