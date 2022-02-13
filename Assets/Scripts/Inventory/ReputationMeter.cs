using System.Runtime.CompilerServices;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace ItemInventory
{
    /// <summary>
    /// Class to draw the reputation meter
    /// </summary>
    [DisallowMultipleComponent,RequireComponent(typeof(Image))]
    public class ReputationMeter : MonoBehaviour
    {
        [Tooltip("Murderer reputation color")]
        public Color32 murderColor = Color.red;
        [Tooltip("Detective reputation color")]
        public Color32 detectiveColor = Color.blue;
        [Tooltip("Border color")]
        public Color32 borderColor = Color.black;
        [Min(0), Tooltip("Border width")]
        public ushort borderWidth = 10;
        [Min(0), Tooltip("Texture width")]
        public ushort textureWidth = 200;
        [Min(0), Tooltip("Texture height")]
        public ushort textureHeight = 50;
        private Image _image;
        private Texture2D _texture;
        
        private void Awake()
        {
            _image = GetComponent<Image>();
            _texture = new Texture2D(textureWidth, textureHeight);
            
            var infoStore = NextStageInfoStorer.Instance;
            if (infoStore == null) return;
            var nullableInfo = infoStore.information;
            if (nullableInfo == null)
            {
                Debug.LogError("Null PreviousStageInfoSetterScript");
            }
            else
            {
                infoStore.information.RespectChange += OnRespectChange;
            }
            RecreateBar();
        }

        /// <summary>
        /// Event called on Respect Change
        /// </summary>
        private void OnRespectChange(PreviousStageInformation ignored)
        {
            RecreateBar();
        }

        /// <summary>
        /// Recreate the bar
        /// </summary>
        public void RecreateBar()
        {
            _image.material.mainTexture = null;
            Texture2D newTexture = new Texture2D(textureWidth, textureHeight)
            {
                filterMode = FilterMode.Point,
                wrapMode = TextureWrapMode.Clamp
            };
            if (_texture != null)
            {
                Texture2D oldTex = _texture;
                // pray for no memory leaks
                DestroyImmediate(oldTex);
                _texture = null;
            }

            Color32[] array = newTexture.GetPixels32();
            
            
            DrawBorder(array);
            DrawMurderSection(array);
            DrawDetectiveSection(array);
            newTexture.SetPixels32(array);
            newTexture.Apply();
            _image.material.mainTexture = newTexture;
            _image.SetMaterialDirty();
        }

        /// <summary>
        /// Draw the border section of the bar
        /// </summary>
        /// <param name="array">Color32 array to draw border on</param>
        private void DrawBorder(Color32[] array)
        {
            for (int i = 0; i < textureWidth; i++)
            {
                for (int j = 0; j < borderWidth; j++)
                {
                    DrawPixel(array,i,j,borderColor);
                    DrawPixel(array,i,textureHeight-j-1,borderColor);
                }
            }

            for (int i = 0; i < textureHeight; i++)
            {
                for (int j = 0; j < borderWidth; j++)
                {
                    DrawPixel(array, j, i, borderColor);
                    DrawPixel(array, textureWidth-j-1, i, borderColor);
                }
            }
        }

        /// <summary>
        /// Draw murder color of the bar
        /// </summary>
        /// <param name="array">Color32 array to draw on</param>
        private void DrawMurderSection(Color32[] array)
        {
            int respect;
            var infoStore = NextStageInfoStorer.Instance;
            if (infoStore == null) return;
            var nullableInfo = infoStore.information;
            if (nullableInfo == null)
            {
                Debug.LogError("Null PreviousStageInfoSetterScript");
                return;
            }

            respect = nullableInfo.murdererRespect;
            int width = Mathf.FloorToInt((textureWidth-(borderWidth*2)) * respect / (float) PreviousStageInformation.TotalRespect);
            int height = textureHeight - 2 * borderWidth;
            for (int x = borderWidth; x < borderWidth + width; x++)
            {
                for (int y = borderWidth; y < borderWidth + height; y++)
                {
                    DrawPixel(array,x,y,murderColor);
                }
            }
        }
        
        /// <summary>
        /// Draw detective color of the bar
        /// </summary>
        /// <param name="array">Color32 array to draw on</param>
        private void DrawDetectiveSection(Color32[] array)
        {
            int respect;
            var infoStore = NextStageInfoStorer.Instance;
            if (infoStore == null) return;
            var nullableInfo = infoStore.information;
            if (nullableInfo == null)
            {
                Debug.LogError("Null PreviousStageInfoSetterScript");
                return;
            }

            respect = nullableInfo.detectiveRespect;
            int width = Mathf.FloorToInt((textureWidth-(borderWidth*2)) * respect / (float) PreviousStageInformation.TotalRespect);
            int height = textureHeight - 2 * borderWidth;
            for (int x = textureWidth- borderWidth-1; x >= textureWidth-borderWidth-width-1; x--)
            {
                for (int y = borderWidth; y < borderWidth + height; y++)
                {
                    DrawPixel(array,x,y,detectiveColor);
                }
            }
        }
        
        /// <summary>
        /// Draw an individual pixel on the array with given x/y and color
        /// </summary>
        /// <param name="array">Color32 array to draw on</param>
        /// <param name="x">X position (0-indexed)</param>
        /// <param name="y">Y position (0-indexed)</param>
        /// <param name="color">Color32 to draw</param>
        /// <remarks>
        /// Aggressive inlining since it's a small method
        /// </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void DrawPixel(Color32[] array, int x, int y, Color32 color)
        {
            array[x + (y * textureWidth)] = color;
        }
    }
}