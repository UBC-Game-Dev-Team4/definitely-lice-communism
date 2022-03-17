using UnityEngine;

namespace LevelOne.Cat
{
    /// <summary>
    /// Script to be attached to the created cat food object to lure the cat over
    /// </summary>
    public class CatFoodScript : MonoBehaviour
    {
        /// <summary>
        /// On trigger enter, if the cat is the cause, tell it it has reached its destination
        /// </summary>
        /// <param name="other">Other collider, potentially a cat</param>
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