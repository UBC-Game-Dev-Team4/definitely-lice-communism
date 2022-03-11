using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelOne
{
    public class CatFoodScript : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            CatAIScript script = other.gameObject.GetComponent<CatAIScript>();
            if (script != null)
            {
                script.OnReachCatFood();
            }
        }
    }
}