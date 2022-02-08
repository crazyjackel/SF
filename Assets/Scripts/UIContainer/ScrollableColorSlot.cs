using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


namespace UI.Scrollable
{
	[UnityEngine.Scripting.Preserve]
	public class ScrollableColorSlot : VisualElement
	{
		[UnityEngine.Scripting.Preserve]
		public new class UxmlFactory : UxmlFactory<ScrollableColorSlot, UxmlTraits> { }

		[UnityEngine.Scripting.Preserve]
		public new class UxmlTraits : VisualElement.UxmlTraits
		{
			/*
			readonly UxmlColorAttributeDescription negativeXColor = new UxmlColorAttributeDescription() { name = "negative-x-color", defaultValue = Color.red };
			readonly UxmlColorAttributeDescription positiveXColor = new UxmlColorAttributeDescription() { name = "positive-x-color", defaultValue = Color.red };
			readonly UxmlColorAttributeDescription negativeYColor = new UxmlColorAttributeDescription() { name = "negative-y-color", defaultValue = Color.red };
			readonly UxmlColorAttributeDescription positiveYColor = new UxmlColorAttributeDescription() { name = "positive-y-color", defaultValue = Color.red };
			*/
			public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
			{
				get { yield break; }
			}

			public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
			{
				base.Init(ve, bag, cc);
				var element = ve as ScrollableColorSlot;
				if (element != null)
				{
					/*
					element.NegativeXColor = negativeXColor.GetValueFromBag(bag, cc);
					element.NegativeYColor = negativeYColor.GetValueFromBag(bag, cc);
					element.PositiveXColor = positiveXColor.GetValueFromBag(bag, cc);
					element.PositiveYColor = positiveYColor.GetValueFromBag(bag, cc);
					*/
				}
			}
		}

		public Color32 NegativeXColor { get; private set; } = Color.green;
		public Color32 NegativeYColor { get; private set; } = Color.blue;
		public Color32 PositiveXColor { get; private set; } = Color.red;
		public Color32 PositiveYColor { get; private set; } = Color.cyan;

		public ScrollableColorSlot()
		{
			//this.AddManipulator(new TextureDragger());
			RegisterCallback<AttachToPanelEvent>(OnAttachToPanelEvent);
		}

		void OnAttachToPanelEvent(AttachToPanelEvent evt)
		{
			GenerateTexture();
		}

		void GenerateTexture()
		{
			var sub_imageheight = 1;
			var sub_imagewidth = 1;
			var texture = new Texture2D(sub_imageheight * 3, sub_imagewidth * 3, TextureFormat.RGBA32, false);
			texture.filterMode = FilterMode.Point;
			texture.wrapMode = TextureWrapMode.Clamp;

			Color32 defaultColor = this.style.backgroundColor.value;
			var data = texture.GetRawTextureData<Color32>();

			for(int i = 0; i < data.Length; i++)
            {
				data[i] = defaultColor;
            }

			data[7] = PositiveYColor; //top Middle
			data[1] = NegativeYColor; //bottom Middle
			data[3] = NegativeXColor;
			data[5] = PositiveXColor;
			texture.Apply();


			TextureScaler.scale(texture, 300, 300, FilterMode.Point);

			style.backgroundImage = new StyleBackground(texture);
			style.backgroundColor = new StyleColor(Color.white);
		}
	}
}