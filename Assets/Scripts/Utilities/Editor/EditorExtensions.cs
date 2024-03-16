using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.U2D;

namespace Utilities.Editor
{
    public static class EditorExtensions
    {
        #region Atlas Check
        
        [MenuItem("Assets/Sprite/Check Sprite Is Packed?", false)]
        public static void IsSpritePacked()
        {
            Log.ClearLogs();

            Sprite sprite = null;
            if (Selection.activeObject is Texture2D)
            {
                Texture2D texture = Selection.activeObject as Texture2D;
                string spriteSheet = AssetDatabase.GetAssetPath(texture);
                Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheet).OfType<Sprite>().ToArray();

                if (sprites.Length > 0)
                {
                    sprite = sprites[0];
                }
            }
            else if (Selection.activeObject is Sprite)
            {
                sprite = Selection.activeObject as Sprite;
            }
            else
            {
                Log.Error("Wrong Type. Choose a sprite to use this feature.");
                return;
            }

            string[] atlasesGUID = AssetDatabase.FindAssets("t:spriteatlas");

            if (sprite != null)
            {
                foreach (string atlasGUID in atlasesGUID)
                {
                    SpriteAtlas atlas = AssetDatabase.LoadAssetAtPath<SpriteAtlas>(AssetDatabase.GUIDToAssetPath(atlasGUID));

                    if (atlas.CanBindTo(sprite))
                    {
                        Selection.activeObject = atlas;
                        EditorGUIUtility.PingObject(atlas);
                        return;
                    }
                }

                Log.Error("The sprite could not be found in any atlases.");
            }
        }

        [MenuItem("Assets/Sprite/Check Sprite Is Packed?", true)]
        public static bool IsSpriteOrTexture2D()
        {
            return Selection.activeObject is Texture2D || Selection.activeObject is Sprite;
        }
        
        #endregion

        [MenuItem("Assets/Copy GUID", false)]
        public static void CopyGUID()
        {
            if (Selection.activeObject != null)
            {
                AssetDatabase.TryGetGUIDAndLocalFileIdentifier(Selection.activeObject, out string guid, out long localID);
                
                if (GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl) is TextEditor textEditor)
                {
                    textEditor.text = guid;
                    textEditor.SelectAll();
                    textEditor.Copy();
                    
                    Log.Debug((Selection.activeObject.name, "- GUID:", guid), color: Color.green);
                }
            }
        }
        
        [MenuItem("Assets/Copy GUID", true)]
        public static bool IsThereAnySelectedObject()
        {
            return Selection.activeObject != null;
        }
        
        [MenuItem("Shortcuts/Discard Changes In Current Scene %F1")]
        public static void ReloadSceneWithoutSaving()
        {
            EditorSceneManager.OpenScene(EditorApplication.currentScene);
        }
    }
}