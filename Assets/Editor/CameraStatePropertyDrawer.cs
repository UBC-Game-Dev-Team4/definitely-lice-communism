using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Util;

namespace Editor
{
    [CustomPropertyDrawer(typeof(CameraState))]
    public class CameraStatePropertyDrawer : PropertyDrawer
    {
        private int GetNumberOfDrawnProperties(SerializedProperty property)
        {
            int retVal = 5; // label/modeX/ modeY/ offset/ idealWidth
            switch (property.GetCameraMovementMode(nameof(CameraState.movementModeX)))
            {
                case CameraMovementMode.Fixed:
                {
                    retVal++;
                    break;
                }
                case CameraMovementMode.Range:
                {
                    retVal += 2;
                    break;
                }
                case CameraMovementMode.Free:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            switch (property.GetCameraMovementMode(nameof(CameraState.movementModeY)))
            {
                case CameraMovementMode.Fixed:
                {
                    retVal++;
                    break;
                }
                case CameraMovementMode.Range:
                {
                    retVal += 2;
                    break;
                }
                case CameraMovementMode.Free:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return retVal;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
            int yOffset = 1;
            float height = position.height / GetNumberOfDrawnProperties(property);
            EditorGUI.indentLevel++;
            var modeXFieldRect = new Rect(position.x, position.y + yOffset*height, position.width, height);
            yOffset++;
            EditorGUI.PropertyField(modeXFieldRect, property.FindPropertyRelative(nameof(CameraState.movementModeX)));
            switch (property.GetCameraMovementMode(nameof(CameraState.movementModeX)))
            {
                case CameraMovementMode.Fixed:
                {
                    var xFixedFieldRect = new Rect(position.x, position.y + yOffset* height, position.width, height);
                    EditorGUI.PropertyField(xFixedFieldRect,
                        property.FindPropertyRelative(nameof(CameraState.movementFixedX)));
                    yOffset++;
                    break;
                }
                case CameraMovementMode.Range:
                {
                    var xRangeLeftRect = new Rect(position.x, position.y + yOffset*height, position.width, height);
                    var xRangeRightRect = new Rect(position.x, position.y + (yOffset+1)*height, position.width, height);
                    EditorGUI.PropertyField(xRangeLeftRect,
                        property.FindPropertyRelative(nameof(CameraState.movementRangeXLeft)));
                    EditorGUI.PropertyField(xRangeRightRect,
                        property.FindPropertyRelative(nameof(CameraState.movementRangeXRight)));
                    yOffset += 2;
                    break;
                }
                case CameraMovementMode.Free:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            var modeYFieldRect = new Rect(position.x, position.y + yOffset *height, position.width, height);
            EditorGUI.PropertyField(modeYFieldRect, property.FindPropertyRelative(nameof(CameraState.movementModeY)));
            yOffset++;
            switch (property.GetCameraMovementMode(nameof(CameraState.movementModeY)))
            {
                case CameraMovementMode.Fixed:
                {
                    var xFixedFieldRect = new Rect(position.x, position.y + yOffset* height, position.width, height);
                    EditorGUI.PropertyField(xFixedFieldRect,
                        property.FindPropertyRelative(nameof(CameraState.movementFixedY)));
                    yOffset++;
                    break;
                }
                case CameraMovementMode.Range:
                {
                    var xRangeLeftRect = new Rect(position.x, position.y + yOffset*height, position.width, height);
                    var xRangeRightRect = new Rect(position.x, position.y + (yOffset+1)*height, position.width, height);
                    EditorGUI.PropertyField(xRangeLeftRect,
                        property.FindPropertyRelative(nameof(CameraState.movementRangeYLeft)));
                    EditorGUI.PropertyField(xRangeRightRect,
                        property.FindPropertyRelative(nameof(CameraState.movementRangeYRight)));
                    yOffset += 2;
                    break;
                }
                case CameraMovementMode.Free:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var cameraOffsetRect = new Rect(position.x, position.y + yOffset * height, position.width, height);
            EditorGUI.PropertyField(cameraOffsetRect, property.FindPropertyRelative(nameof(CameraState.cameraOffset)));
            yOffset++;
            var idealWidthRect = new Rect(position.x, position.y + yOffset * height, position.width, height);
            EditorGUI.PropertyField(idealWidthRect, property.FindPropertyRelative(nameof(CameraState.cameraIdealWidth)));
            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label)*(GetNumberOfDrawnProperties(property)+1);
        }
    }

    public static class PropertyExtensions
    {
        private static Dictionary<string, CameraMovementMode> nameToMode = new Dictionary<string, CameraMovementMode>();

        static PropertyExtensions()
        {
            Array values = Enum.GetValues(typeof(CameraMovementMode));
            foreach (var value in values)
            {
                nameToMode[Enum.GetName(typeof(CameraMovementMode), value) ?? throw new InvalidOperationException()] = (CameraMovementMode) value;
            }
        }
        public static CameraMovementMode GetCameraMovementMode(this SerializedProperty property, string name)
        {
            SerializedProperty propertyRelative = property.FindPropertyRelative(name);
            return nameToMode[propertyRelative.enumNames[propertyRelative.enumValueIndex]];
        }
    }
}