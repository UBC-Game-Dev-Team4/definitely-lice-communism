using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

namespace Util
{
    public class AreaScript : MonoBehaviour
    {
        public CameraState cameraStateOnEnter = CameraState.CreateFreeXYCameraState(new Vector3(0, 0, -10));

        private DoorScript[] _doors;
        private AreaScript[] _adjacentAreas;
        
        private void Awake()
        {
            _doors = transform.GetComponentsInChildren<DoorScript>();
            _adjacentAreas = _doors.Select(door => door.areaToTeleportTo).Where(door => door != null).ToArray();
        }
    }
}