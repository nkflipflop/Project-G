using System.Text;
using UnityEngine;

namespace Utilities
{
    public static partial class Extensions
    {
        private static StringBuilder stringBuilder = new();
        public static string ConcatenateString(params object[] texts)
        {
            stringBuilder.Clear();

            for (int i = 0; i < texts.Length; i++)
            {
                stringBuilder.Append(texts[i]);
            }

            return stringBuilder.ToString();
        }
		
        public static string SetRichTextSize(this string text, int size)
        {
            return ConcatenateString("<size=", size, ">", text, "</size>");
        }
	
        public static string SetRichTextColor(this string text, Color color)
        {
            return ConcatenateString("<color=#", ColorUtility.ToHtmlStringRGB(color), ">", text, "</color>");
        }
    }
}