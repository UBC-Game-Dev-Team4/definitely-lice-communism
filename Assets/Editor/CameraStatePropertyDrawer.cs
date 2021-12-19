using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Util;

namespace Editor
{
    /// <summary>
    /// Custom property drawer for <see cref="CameraState"/>
    /// </summary>
    [CustomPropertyDrawer(typeof(CameraState))]
    public class CameraStatePropertyDrawer : PropertyDrawer
    {
        /// <summary>
        /// Gets the number of drawn properties for this camera state
        /// </summary>
        /// <param name="property">Serialized camera state property</param>
        /// <returns>NUmber of properties</returns>
        /// <exception cref="ArgumentOutOfRangeException">If serialiezd property has invalid enum type</exception>
        private int GetNumberOfDrawnProperties(SerializedProperty property)
        {
            int retVal = 5; // label/modeX/ modeY/ offset/ idealWidth
            if (!EditorGUIUtility.wideMode) retVal++; // vector3 can be rendered on next line
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

        /// <inheritdoc cref="PropertyDrawer.OnGUI"/>
        /// <exception cref="ArgumentOutOfRangeException">If serialized CameraState has invalid enum values</exception>
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
            if (!EditorGUIUtility.wideMode) yOffset++;
            var idealWidthRect = new Rect(position.x, position.y + yOffset * height, position.width, height);
            EditorGUI.PropertyField(idealWidthRect, property.FindPropertyRelative(nameof(CameraState.cameraIdealWidth)));
            EditorGUI.indentLevel--;
            EditorGUI.EndProperty();
        }

        /// <inheritdoc cref="PropertyDrawer.GetPropertyHeight"/>
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label)*(GetNumberOfDrawnProperties(property)+1);
        }
    }

    /// <summary>
    /// Extension methods to serialized properties
    /// </summary>
    public static class PropertyExtensions
    {
        /// <summary>
        /// Dictionary of names to camera modes
        /// </summary>
        private static readonly Dictionary<string, CameraMovementMode> NameToMode = new Dictionary<string, CameraMovementMode>();

        /// <summary>
        /// Static constructor that initializes the <see cref="NameToMode"/> dictionary
        /// </summary>
        /// <exception cref="InvalidOperationException">Should not be thrown, unless somehow Enum.GetValues returns bad enum values</exception>
        static PropertyExtensions()
        {
            Array values = Enum.GetValues(typeof(CameraMovementMode));
            foreach (var value in values)
            {
                NameToMode[Enum.GetName(typeof(CameraMovementMode), value) ?? throw new InvalidOperationException()] = (CameraMovementMode) value;
            }
        }
        /// <summary>
        /// Returns stored <see cref="CameraMovementMode"/> value in a serialized property given the property name
        /// </summary>
        /// <param name="property">SerializedProperty that has a field </param>
        /// <param name="name">Field name</param>
        /// <returns>Stored CameraMovementMode</returns>
        /// <exception cref="ArgumentException">If property/field does not have a valid CameraMovementMode</exception>
        public static CameraMovementMode GetCameraMovementMode(this SerializedProperty property, string name)
        {
            SerializedProperty propertyRelative = property.FindPropertyRelative(name);
            if (NameToMode.TryGetValue(propertyRelative.enumNames[propertyRelative.enumValueIndex],
                out CameraMovementMode result))
                return result;
            throw new ArgumentException("Field " + name + " has invalid CameraMovementMode");
        }
    }
}