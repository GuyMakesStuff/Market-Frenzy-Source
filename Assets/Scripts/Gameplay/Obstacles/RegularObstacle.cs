using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarketFrenzy.Gameplay.Obstacles;

namespace MarketFrenzy.Gameplay.Obstacles
{
    public class RegularObstacle : Obstacle
    {
        protected override void OnCollide()
        {
            HandleCollision(DecreaseLives, IncreaseScore, DecreaseScore, "You Hit An Obstacle!");
        }
    }
}