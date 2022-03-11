using System;
using ItemInventory;
using Player;
using UnityEngine;

namespace LevelOne
{
    [CreateAssetMenu(fileName = "Cat Food Item", menuName = "Scene One/Cat Food Item")]
    public class CatFoodItem : Item
    {
        public delegate void OnPlaceDelegate();

        public static event OnPlaceDelegate OnPlaceEvent;

        public static bool IsPlaced
        {
            get;
            private set;
        }

        public static Transform PlacedLocation
        {
            get;
            private set;
        }
        public GameObject catFoodPrefab;
        public override void Use()
        {
            IsPlaced = true;
            GameObject go = Instantiate(catFoodPrefab, PlayerScript.Instance.transform.position, Quaternion.identity);
            PlacedLocation = go != null ? go.transform : null;
            Inventory.Instance.Remove(this);
            OnPlaceEvent?.Invoke();
        }

        private void OnDestroy()
        {
            if (OnPlaceEvent == null) return;
            foreach (OnPlaceDelegate d in OnPlaceEvent.GetInvocationList())
            {
                OnPlaceEvent -= d;
            }
        }
    }
}