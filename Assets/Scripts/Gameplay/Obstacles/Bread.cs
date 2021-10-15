using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MarketFrenzy.Gameplay.Obstacles
{
    public class Bread : Obstacle
    {
        protected override void OnCollide()
        {
            base.OnCollide();
            HandleCollision(IncreaseScore, DecreaseScore, DecreaseLives, "You Didnt Catch The Bread Loaf!");
        }
    }
}