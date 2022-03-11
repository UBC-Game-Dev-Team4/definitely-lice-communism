﻿using DefaultNamespace;
using UnityEngine;

namespace LevelOne
{
    /// <summary>
    /// Script to detect player and make the cat move away
    /// </summary>
    [DisallowMultipleComponent,RequireComponent(typeof(Collider2D))]
    public class CatPlayerDetectionScript : MonoBehaviour
    {
        [Tooltip("Cat to make move away from player")]
        public CatAIScript cat;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (CatFoodItem.IsPlaced) return;
            if (cat.shouldRunAway)
            {
                cat.directionIsLeft = other.transform.position.x > cat.transform.position.x;
                cat.SetMode(AIMode.SpecificDirection);
            }
            else
            {
                cat.SetMode(AIMode.Stationary);
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (CatFoodItem.IsPlaced) return;
            if (cat.shouldRunAway)
                cat.directionIsLeft = other.transform.position.x > cat.transform.position.x;
            else
            {
                cat.FacePlayer(other.transform);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (CatFoodItem.IsPlaced) return;
            cat.SetMode(AIMode.WanderLocked);
        }
    }
}