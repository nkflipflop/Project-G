using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Utilities
{
	public static class Extensions
	{
		#region String
		
		private static StringBuilder stringBuilder = new StringBuilder();
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
		
		#endregion
		
		#region List
		
		private static List<int> hashCodeList;
		public static bool SortBy<T>(this List<T> list, Comparison<T> comparison)
		{
			hashCodeList ??= new List<int>();
			foreach (var item in list)
			{
				hashCodeList.Add(item.GetHashCode());
			}
			list.Sort(comparison);
			for (var i = 0; i < list.Count; i++)
			{
				if (hashCodeList[i] != list[i].GetHashCode())
				{
					return true;
				}
			}
			return false;
		}
		
		#endregion
		
		#region Rect Transform
		
		private static Vector3[] corners = new Vector3[4];
		public static Vector2 GetBottomPosition(this RectTransform rectTransform)
		{
			rectTransform.GetWorldCorners(corners);
			return (corners[0] + corners[3]) / 2f;
		}
	
		public static Vector2 GetTopPosition(this RectTransform rectTransform)
		{
			rectTransform.GetWorldCorners(corners);
			return (corners[1] + corners[2]) / 2f;
		}
	
		public static Vector2 GetLeftPosition(this RectTransform rectTransform)
		{
			rectTransform.GetWorldCorners(corners);
			return (corners[0] + corners[3]) / 2f;
		}
	
		public static Vector2 GetRightPosition(this RectTransform rectTransform)
		{
			rectTransform.GetWorldCorners(corners);
			return (corners[1] + corners[2]) / 2f;
		}
		
		public static float GetWorldWidth(this RectTransform rectTransform)
		{
			rectTransform.GetWorldCorners(corners);
			return Mathf.Abs((corners[2] - corners[1]).x);
		}
	
		public static float GetWorldHeight(this RectTransform rectTransform)
		{
			rectTransform.GetWorldCorners(corners);
			return Mathf.Abs((corners[3] - corners[1]).y);
		}
		
		// <summary> Set pivot without changing the position of the element </summary>
		public static void SetPivot(this RectTransform rectTransform, Vector2 pivot)
		{
			Vector3 deltaPosition = rectTransform.pivot - pivot;            // get change in pivot
			deltaPosition.Scale(rectTransform.rect.size);                   // apply sizing
			deltaPosition.Scale(rectTransform.localScale);                  // apply scaling
			deltaPosition = rectTransform.localRotation * deltaPosition;    // apply rotation

			rectTransform.pivot = pivot;                                    // change the pivot
			rectTransform.localPosition -= deltaPosition;                   // reverse the position change
		}
		
		public static void SetLocalPositionX(this RectTransform rectTransform, float x)
		{
			Vector3 position = rectTransform.localPosition;
			position.x = x;
			rectTransform.localPosition = position;
		}

		public static void SetLocalPositionY(this RectTransform rectTransform, float y)
		{
			Vector3 position = rectTransform.localPosition;
			position.y = y;
			rectTransform.localPosition = position;
		}

		public static void SetLocalPositionZ(this RectTransform rectTransform, float z)
		{
			Vector3 position = rectTransform.localPosition;
			position.z = z;
			rectTransform.localPosition = position;
		}

		public static void SetAnchoredPositionX(this RectTransform rect, float x)
		{
			Vector2 position = rect.anchoredPosition;
			position.x = x;
			rect.anchoredPosition = position;
		}
	
		public static void SetAnchoredPositionY(this RectTransform rect, float y)
		{
			Vector2 position = rect.anchoredPosition;
			position.y = y;
			rect.anchoredPosition = position;
		}
		
		#endregion
		
		#region Transform
		
		public static void SetPositionX(this Transform transform, float x)
		{
			Vector3 position = transform.position;
			position.x = x;
			transform.position = position;
		}

		public static void SetPositionY(this Transform transform, float y)
		{
			Vector3 position = transform.position;
			position.y = y;
			transform.position = position;
		}

		public static void SetPositionZ(this Transform transform, float z)
		{
			Vector3 position = transform.position;
			position.z = z;
			transform.position = position;
		}
		
		public static async UniTask Punch(this Transform transform)
		{
			transform.DOKill();
			transform.localScale = Vector3.one;
			await transform.DOScale(1.1f, 0.1f).SetLoops(2, LoopType.Yoyo).ToUniTask();
		}
		
		#endregion
		
		#region Scroll Rect
		
		public static void FocusOnItem(this ScrollRect scrollView, RectTransform item)
		{
			scrollView.normalizedPosition = scrollView.CalculateFocusedScrollPosition(item);
		}

		public static Vector2 CalculateFocusedScrollPosition(this ScrollRect scrollView, RectTransform item)
		{
			Vector2 itemCenterPoint =
				scrollView.content.InverseTransformPoint(item.transform.TransformPoint(item.rect.center));

			Vector2 contentSizeOffset = scrollView.content.rect.size;
			contentSizeOffset.Scale(scrollView.content.pivot);

			return scrollView.CalculateFocusedScrollPosition(itemCenterPoint + contentSizeOffset);
		}

		public static Vector2 CalculateFocusedScrollPosition(this ScrollRect scrollView, Vector2 focusPoint)
		{
			Vector2 contentSize = scrollView.content.rect.size;
			Vector2 viewportSize = ((RectTransform)scrollView.content.parent).rect.size;
			Vector2 contentScale = scrollView.content.localScale;

			contentSize.Scale(contentScale);
			focusPoint.Scale(contentScale);

			Vector2 scrollPosition = scrollView.normalizedPosition;
			if (scrollView.horizontal && contentSize.x > viewportSize.x)
				scrollPosition.x =
					Mathf.Clamp01((focusPoint.x - viewportSize.x * 0.5f) / (contentSize.x - viewportSize.x));
			if (scrollView.vertical && contentSize.y > viewportSize.y)
				scrollPosition.y =
					Mathf.Clamp01((focusPoint.y - viewportSize.y * 0.5f) / (contentSize.y - viewportSize.y));

			return scrollPosition;
		}
		
		#endregion
		
		#region Hash Set
		
		public static T GetFirst<T>(this HashSet<T> hashSet)
		{
			foreach (var item in hashSet)
			{
				return item;
			}
			return default;
		}
		
		#endregion
		
		#region UniTask
		
		public static async UniTask PeriodicAsync(Func<UniTask> action, float interval, float initialDelay = 0f,
			CancellationToken cancellationToken = default)
		{
			await UniTask.Delay(TimeSpan.FromSeconds(initialDelay), cancellationToken: cancellationToken);
			while (true)
			{
				UniTask delayTask = UniTask.Delay(TimeSpan.FromSeconds(interval), cancellationToken: cancellationToken);
				await action();
				await delayTask;
			}
		}
		
		#endregion
	}
}