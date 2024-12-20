using UnityEngine;
using UnityEngine.Serialization;

namespace OleksiiStepanov.Utils
{
	[ExecuteInEditMode]
	public class RectTransformExtension : MonoBehaviour
	{
		
		/// <summary>
		/// The relativeRectTransform is the object you want this rect to be relative to
		/// </summary>
		[SerializeField] private RectTransform relativeRectTransform;
		
		/// <summary>
		/// The objectRect is the rect of the object this script is attached to
		/// </summary>
		private RectTransform objectRect;
		
		/// <summary>
		/// The offsetTop is the value of the relativeRectTransform height / offsetTop = relativeRectTransform.rect.height
		/// </summary>
		private float offsetTop;
		
		/// <summary>
		/// Change top value of the rect relative to the height of the rect transform placed in the inspector only once on awake
		/// </summary>
		private void Awake()
		{
			objectRect = GetComponent<RectTransform>();
			offsetTop = relativeRectTransform.rect.height;
			objectRect.offsetMax = new Vector2(0, -offsetTop);
		}

		
		/// <summary>
		/// Change top value of the rect relative to the height of the rect transform placed in the inspector once every time
		/// you have done a change in the inspector but only in unity editor (does not apply on devices)
		/// </summary>
	#if UNITY_EDITOR
		public void Update()
		{
			if (offsetTop == relativeRectTransform.rect.height)
				return;
			
			offsetTop = relativeRectTransform.rect.height;
			objectRect.offsetMax = new Vector2(0, -offsetTop);
		}
	#endif

	}
}
