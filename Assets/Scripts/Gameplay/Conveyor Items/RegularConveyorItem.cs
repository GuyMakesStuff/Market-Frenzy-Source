using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MarketFrenzy.Gameplay.ConveyorItems
{
    public class RegularConveyorItem : ConveyorItem
    {
        public override void OnClick()
        {
            DecreaseScore();
            base.OnClick();
        }
    }
}