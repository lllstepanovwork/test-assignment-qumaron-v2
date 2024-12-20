using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
	[ExecuteAlways]
	[RequireComponent(typeof(RectTransform))]
	public class RectTransformResizer : UIBehaviour
	{
		protected override void OnRectTransformDimensionsChange()
		{
			
		}
	}
}
