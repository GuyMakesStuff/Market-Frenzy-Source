using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarketFrenzy.Managers;

namespace MarketFrenzy.Gameplay.ConveyorItems
{
    public class Bomb : ConveyorItem
    {
        public override void OnClick()
        {
            EncreaseScore();
            base.OnClick();
        }
        public override void OnEnterLeftOven()
        {
            GameManager.Instance.Damage("You Have Missed A Bomb!");
            base.OnClick();
        }
    }
}
