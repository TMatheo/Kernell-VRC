using System;
using System.Collections.Generic;
using Il2CppSystem;
using Il2CppSystem.Collections;
using KernellClientUI.VRChat;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace KernellClientUI.UI.QuickMenu
{
	// Token: 0x02000042 RID: 66
	public class ReMenuButtonContainer : UiElement
	{
		// Token: 0x060002B7 RID: 695 RVA: 0x0000E3CF File Offset: 0x0000C5CF
		public ReMenuButtonContainer(string name, Transform parent = null) : base(QMMenuPrefabs.MenuCategoryContainerPrefab, parent ?? QMMenuPrefabs.MenuCategoryContainerPrefab.transform.parent, "Buttons_" + name, true)
		{
			this.InitializeContainer();
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000E405 File Offset: 0x0000C605
		public ReMenuButtonContainer(Transform transform) : base(transform)
		{
			GameObject gameObject = base.GameObject;
			this._gridLayoutGroup = ((gameObject != null) ? gameObject.GetComponent<GridLayoutGroup>() : null);
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000E428 File Offset: 0x0000C628
		private void InitializeContainer()
		{
			bool flag = base.RectTransform == null;
			if (!flag)
			{
				try
				{
					this.ClearChildren();
					this.SetupGridLayout();
				}
				catch (Exception arg)
				{
					MelonLogger.Error(string.Format("Failed to initialize ReMenuButtonContainer: {0}", arg));
				}
			}
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000E484 File Offset: 0x0000C684
		private void ClearChildren()
		{
			bool flag = base.RectTransform == null;
			if (!flag)
			{
				List<Transform> list = new List<Transform>();
				IEnumerator enumerator = base.RectTransform.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Object @object = enumerator.Current;
						bool flag2 = @object != null;
						if (flag2)
						{
							Transform transform = @object.Cast<Transform>();
							bool flag3 = transform != null;
							if (flag3)
							{
								list.Add(transform);
							}
						}
					}
				}
				finally
				{
					IDisposable disposable = enumerator as IDisposable;
					bool flag4 = disposable != null;
					if (flag4)
					{
						disposable.Dispose();
					}
				}
				foreach (Transform transform2 in list)
				{
					bool flag5 = transform2 != null && transform2.gameObject != null;
					if (flag5)
					{
						Object.Destroy(transform2.gameObject);
					}
				}
			}
		}

		// Token: 0x060002BB RID: 699 RVA: 0x0000E59C File Offset: 0x0000C79C
		private void SetupGridLayout()
		{
			this._gridLayoutGroup = base.GameObject.GetComponent<GridLayoutGroup>();
			bool flag = this._gridLayoutGroup == null;
			if (flag)
			{
				MelonLogger.Warning("GridLayoutGroup component not found on ReMenuButtonContainer");
			}
			else
			{
				this._gridLayoutGroup.childAlignment = 0;
				this._gridLayoutGroup.padding.top = 8;
				this._gridLayoutGroup.padding.left = 64;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060002BC RID: 700 RVA: 0x0000E60B File Offset: 0x0000C80B
		public GridLayoutGroup GridLayout
		{
			get
			{
				return this._gridLayoutGroup;
			}
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000E614 File Offset: 0x0000C814
		public void SetPadding(int top, int left, int right = 0, int bottom = 0)
		{
			bool flag = this._gridLayoutGroup == null;
			if (!flag)
			{
				this._gridLayoutGroup.padding.top = top;
				this._gridLayoutGroup.padding.left = left;
				this._gridLayoutGroup.padding.right = right;
				this._gridLayoutGroup.padding.bottom = bottom;
			}
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000E680 File Offset: 0x0000C880
		public void SetChildAlignment(TextAnchor alignment)
		{
			bool flag = this._gridLayoutGroup != null;
			if (flag)
			{
				this._gridLayoutGroup.childAlignment = alignment;
			}
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000E6B0 File Offset: 0x0000C8B0
		public void SetCellSize(float width, float height)
		{
			bool flag = this._gridLayoutGroup != null;
			if (flag)
			{
				this._gridLayoutGroup.cellSize = new Vector2(width, height);
			}
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000E6E4 File Offset: 0x0000C8E4
		public void SetSpacing(float x, float y)
		{
			bool flag = this._gridLayoutGroup != null;
			if (flag)
			{
				this._gridLayoutGroup.spacing = new Vector2(x, y);
			}
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x0000E718 File Offset: 0x0000C918
		public void SetConstraint(GridLayoutGroup.Constraint constraint, int constraintCount)
		{
			bool flag = this._gridLayoutGroup != null;
			if (flag)
			{
				this._gridLayoutGroup.constraint = constraint;
				this._gridLayoutGroup.constraintCount = constraintCount;
			}
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000E754 File Offset: 0x0000C954
		public void AddButton(ReMenuButton button)
		{
			bool flag = ((button != null) ? button.GameObject : null) != null && base.RectTransform != null;
			if (flag)
			{
				button.GameObject.transform.SetParent(base.RectTransform, false);
			}
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000E7A3 File Offset: 0x0000C9A3
		public void ClearButtons()
		{
			this.ClearChildren();
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x0000E7AD File Offset: 0x0000C9AD
		public int ButtonCount
		{
			get
			{
				RectTransform rectTransform = base.RectTransform;
				return (rectTransform != null) ? rectTransform.childCount : 0;
			}
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000E7C4 File Offset: 0x0000C9C4
		public void RefreshLayout()
		{
			bool flag = base.RectTransform != null;
			if (flag)
			{
				LayoutRebuilder.ForceRebuildLayoutImmediate(base.RectTransform);
			}
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000E7F0 File Offset: 0x0000C9F0
		public void ApplyLayoutPreset(ButtonLayoutPreset preset)
		{
			bool flag = this._gridLayoutGroup == null;
			if (!flag)
			{
				switch (preset)
				{
				case ButtonLayoutPreset.Compact:
					this.SetCellSize(80f, 60f);
					this.SetSpacing(5f, 5f);
					this.SetPadding(5, 5, 0, 0);
					break;
				case ButtonLayoutPreset.Standard:
					this.SetCellSize(120f, 80f);
					this.SetSpacing(8f, 8f);
					this.SetPadding(8, 64, 0, 0);
					break;
				case ButtonLayoutPreset.Large:
					this.SetCellSize(160f, 100f);
					this.SetSpacing(10f, 10f);
					this.SetPadding(10, 10, 0, 0);
					break;
				case ButtonLayoutPreset.Wide:
					this.SetCellSize(200f, 60f);
					this.SetSpacing(5f, 8f);
					this.SetPadding(8, 16, 0, 0);
					break;
				}
			}
		}

		// Token: 0x04000128 RID: 296
		private GridLayoutGroup _gridLayoutGroup;

		// Token: 0x04000129 RID: 297
		private const int DEFAULT_PADDING_TOP = 8;

		// Token: 0x0400012A RID: 298
		private const int DEFAULT_PADDING_LEFT = 64;

		// Token: 0x0400012B RID: 299
		private const TextAnchor DEFAULT_ALIGNMENT = 0;
	}
}
