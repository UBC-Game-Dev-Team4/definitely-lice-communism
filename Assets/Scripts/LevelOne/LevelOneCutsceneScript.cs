using System.Collections;
using DefaultNamespace;
using JetBrains.Annotations;
using Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace LevelOne
{
    /// <summary>
    /// Script containing events for the Level one Cutscene
    /// </summary>
    [DisallowMultipleComponent]
    public class LevelOneCutsceneScript : MonoBehaviour
    {
        [Tooltip("Player in scene")]
        public PlayerScript player;
        [Tooltip("Detective in scene")]
        public GameObject detective;
        [Tooltip("Door in scene")]
        public DoorScript door;
        [Tooltip("Next scene to load")]
        public string nextScene = "Scene1";
        [Tooltip("Delay before player opens door")]
        public float playerDelayBeforeOpen = 1;
        [Tooltip("Player movement speed")]
        public float moveSpeed = 5;

        /// <summary>
        /// Makes the detective open the door and disappear
        /// </summary>
        [UsedImplicitly]
        public void DetectiveDisappear()
        {
            Debug.Assert(door != null);
            UnityEvent e = new UnityEvent();
            e.AddListener(() => detective.SetActive(false));
            door.FakeOpen(e);
        }

        /// <summary>
        /// Starts coroutine to make player disappear/move towards door
        /// </summary>
        [UsedImplicitly]
        public void PlayerDisappear()
        {
            StartCoroutine(PlayerMoveCoroutine());
        }

        /// <summary>
        /// Coroutine to forcibly move/animate the player
        /// </summary>
        /// <returns>Coroutine</returns>
        private IEnumerator PlayerMoveCoroutine()
        {
            // this SHOULD be on the player but i'm too damn lazy
            Animator animator = player.Animator;
            animator.SetFloat(PlayerScript.VelocityXParameter,1);
            Rigidbody2D body = player.Body;
            while (body.position.x < door.transform.position.x)
            {
                body.MovePosition(body.position + new Vector2(moveSpeed*Time.deltaTime,0));
                yield return null;
            }
            animator.SetFloat(PlayerScript.VelocityXParameter,0);
            yield return new WaitForSeconds(playerDelayBeforeOpen);
            UnityEvent e = new UnityEvent();
            e.AddListener(() => player.gameObject.SetActive(false));
            door.FakeOpen(e);
        }

        /// <summary>
        /// Loads the next scene
        /// </summary>
        [UsedImplicitly]
        public void LoadNextScene()
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}