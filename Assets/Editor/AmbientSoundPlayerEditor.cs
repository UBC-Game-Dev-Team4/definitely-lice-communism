using Sound;
using UnityEditor;

namespace Editor
{
    /// <summary>
    /// Custom Inspector for <see cref="AmbientSoundPlayer"/>
    /// </summary>
    [CustomEditor(typeof(AmbientSoundPlayer)),CanEditMultipleObjects]
    public class AmbientSoundPlayerEditor : UnityEditor.Editor
    {
        private static readonly string[] DontInclude = new string[] {"m_Script"};
        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            // If not editing multiple object, one can just do script = (class_name) target;
            if (targets.Length > 1) DrawPropertiesExcluding(serializedObject, DontInclude);
            else
            {
                AmbientSoundPlayer script = (AmbientSoundPlayer) target;
                if (script.fadeOnStop)
                {
                    DrawPropertiesExcluding(serializedObject,DontInclude);
                }
                else
                {
                    DrawPropertiesExcluding(serializedObject,"m_Script",
                        // hide fade specific stuff
                        nameof(script.musicAudioGroup),nameof(script.musicFadeGroup),nameof(script.musicMixer));
                }
            }
            if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();
        }
    }
}