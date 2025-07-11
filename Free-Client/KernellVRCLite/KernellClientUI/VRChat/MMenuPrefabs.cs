using System;
using MelonLoader;
using UnityEngine;

namespace KernellClientUI.VRChat
{
	// Token: 0x0200002A RID: 42
	public static class MMenuPrefabs
	{
		// Token: 0x1700005F RID: 95
		// (get) Token: 0x060001BC RID: 444 RVA: 0x0000A030 File Offset: 0x00008230
		public static Transform MainMenuRoot
		{
			get
			{
				bool flag = MMenuPrefabs._mainMenuRoot == null;
				if (flag)
				{
					GameObject gameObject = GameObject.Find("UserInterface/Canvas_MainMenu(Clone)/Container/MMParent");
					MMenuPrefabs._mainMenuRoot = ((gameObject != null) ? gameObject.transform : null);
					bool flag2 = MMenuPrefabs._mainMenuRoot == null;
					if (flag2)
					{
						MelonLogger.Msg("Didnt find UserInterface/Canvas_MainMenu(Clone)/Container/MMParent using alternative approach");
						GameObject gameObject2 = GameObject.Find("MMParent");
						MMenuPrefabs._mainMenuRoot = ((gameObject2 != null) ? gameObject2.transform : null);
					}
					bool flag3 = MMenuPrefabs._mainMenuRoot == null;
					if (flag3)
					{
						foreach (Transform transform in Object.FindObjectsOfType<Transform>())
						{
							bool flag4 = transform.name.Contains("MMParent") && !transform.name.Contains("QM");
							if (flag4)
							{
								MMenuPrefabs._mainMenuRoot = transform;
								MelonLogger.Msg("[KernellClient] Found MMParent at: " + MMenuPrefabs.GetFullPath(transform));
								break;
							}
						}
					}
					bool flag5 = MMenuPrefabs._mainMenuRoot == null;
					if (flag5)
					{
						MelonLogger.Error("[KernellClient] Failed to find MMParent in hierarchy!");
					}
				}
				return MMenuPrefabs._mainMenuRoot;
			}
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000A170 File Offset: 0x00008370
		private static string GetFullPath(Transform transform)
		{
			string text = transform.name;
			Transform parent = transform.parent;
			while (parent != null)
			{
				text = parent.name + "/" + text;
				parent = parent.parent;
			}
			return text;
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000A1BC File Offset: 0x000083BC
		private static GameObject FindRecursively(Transform parent, string name)
		{
			Transform transform = parent.Find(name);
			bool flag = transform != null;
			GameObject result;
			if (flag)
			{
				result = transform.gameObject;
			}
			else
			{
				bool flag2 = parent.name == name;
				if (flag2)
				{
					result = parent.gameObject;
				}
				else
				{
					for (int i = 0; i < parent.childCount; i++)
					{
						Transform child = parent.GetChild(i);
						bool flag3 = child.name.Contains("QM") || child.name.Contains("QuickMenu");
						if (!flag3)
						{
							GameObject gameObject = MMenuPrefabs.FindRecursively(child, name);
							bool flag4 = gameObject != null;
							if (flag4)
							{
								MelonLogger.Msg("[KernellClient] Found " + name + " at: " + MMenuPrefabs.GetFullPath(gameObject.transform));
								return gameObject;
							}
						}
					}
					result = null;
				}
			}
			return result;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000A2A8 File Offset: 0x000084A8
		private static GameObject FindComponent(string name, string alternativeName = null)
		{
			bool flag = MMenuPrefabs.MainMenuRoot == null;
			GameObject result;
			if (flag)
			{
				MelonLogger.Error("[KernellClient] Cannot find component " + name + ": MainMenuRoot is null");
				result = null;
			}
			else
			{
				GameObject gameObject = MMenuPrefabs.FindRecursively(MMenuPrefabs.MainMenuRoot, name);
				bool flag2 = gameObject == null && !string.IsNullOrEmpty(alternativeName);
				if (flag2)
				{
					gameObject = MMenuPrefabs.FindRecursively(MMenuPrefabs.MainMenuRoot, alternativeName);
				}
				bool flag3 = gameObject == null;
				if (flag3)
				{
					MelonLogger.Warning("[KernellClient] Failed to find component: " + name);
				}
				result = gameObject;
			}
			return result;
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x0000A338 File Offset: 0x00008538
		public static GameObject MMHeaderButtonPrefab
		{
			get
			{
				bool flag = MMenuPrefabs._MMHeaderButtonPrefab == null;
				if (flag)
				{
					MMenuPrefabs._MMHeaderButtonPrefab = MMenuPrefabs.FindComponent("Button_ToggleNavPanel", "HeaderButton");
				}
				return MMenuPrefabs._MMHeaderButtonPrefab;
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x0000A374 File Offset: 0x00008574
		public static GameObject MMDropdownPrefab
		{
			get
			{
				bool flag = MMenuPrefabs._MMDropdownPrefab == null;
				if (flag)
				{
					MMenuPrefabs._MMDropdownPrefab = MMenuPrefabs.FindComponent("Field_MM_SortBy", "DropdownField");
				}
				return MMenuPrefabs._MMDropdownPrefab;
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x0000A3B0 File Offset: 0x000085B0
		public static GameObject MMTogglePrefab
		{
			get
			{
				bool flag = MMenuPrefabs._MMTogglePrefab == null;
				if (flag)
				{
					MMenuPrefabs._MMTogglePrefab = MMenuPrefabs.FindComponent("AlwaysShowVisualAide", "Toggle_MM");
				}
				return MMenuPrefabs._MMTogglePrefab;
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x0000A3EC File Offset: 0x000085EC
		public static GameObject MMSeparatorPrefab
		{
			get
			{
				bool flag = MMenuPrefabs._MMSeparatorPrefab == null;
				if (flag)
				{
					MMenuPrefabs._MMSeparatorPrefab = MMenuPrefabs.FindComponent("Separator", "Separator_MM");
				}
				return MMenuPrefabs._MMSeparatorPrefab;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x0000A428 File Offset: 0x00008628
		public static GameObject MMTabButtonPrefab
		{
			get
			{
				bool flag = MMenuPrefabs._MMTabButtonPrefab == null;
				if (flag)
				{
					MMenuPrefabs._MMTabButtonPrefab = MMenuPrefabs.FindComponent("Menu_Settings", "TabButton");
				}
				return MMenuPrefabs._MMTabButtonPrefab;
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x0000A464 File Offset: 0x00008664
		public static GameObject MMCategoryButtonPrefab
		{
			get
			{
				bool flag = MMenuPrefabs._MMCategoryButtonPrefab == null;
				if (flag)
				{
					MMenuPrefabs._MMCategoryButtonPrefab = MMenuPrefabs.FindComponent("Cell_MM_Audio & Voice", "CategoryButton");
				}
				return MMenuPrefabs._MMCategoryButtonPrefab;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x0000A4A0 File Offset: 0x000086A0
		public static GameObject MMCategoryContainerPrefab
		{
			get
			{
				bool flag = MMenuPrefabs._MMCategoryContainerPrefab == null;
				if (flag)
				{
					MMenuPrefabs._MMCategoryContainerPrefab = MMenuPrefabs.FindComponent("AudioAndVoice", "CategoryContainer");
				}
				return MMenuPrefabs._MMCategoryContainerPrefab;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060001C7 RID: 455 RVA: 0x0000A4DC File Offset: 0x000086DC
		public static GameObject MMSelectorPrefab
		{
			get
			{
				bool flag = MMenuPrefabs._MMSelectorPrefab == null;
				if (flag)
				{
					MMenuPrefabs._MMSelectorPrefab = MMenuPrefabs.FindComponent("ShapeFollowsCameraRotation", "Selector_MM");
				}
				return MMenuPrefabs._MMSelectorPrefab;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x0000A518 File Offset: 0x00008718
		public static GameObject MMSideBarHeaderButtonPrefab
		{
			get
			{
				bool flag = MMenuPrefabs._MMSideBarHeaderButtonPrefab == null;
				if (flag)
				{
					MMenuPrefabs._MMSideBarHeaderButtonPrefab = MMenuPrefabs.FindComponent("Button_Logout", "SidebarHeaderButton");
				}
				return MMenuPrefabs._MMSideBarHeaderButtonPrefab;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060001C9 RID: 457 RVA: 0x0000A554 File Offset: 0x00008754
		public static GameObject MMSliderPrefab
		{
			get
			{
				bool flag = MMenuPrefabs._MMSliderPrefab == null;
				if (flag)
				{
					MMenuPrefabs._MMSliderPrefab = MMenuPrefabs.FindComponent("FalloffForwardShift", "Slider_MM");
				}
				return MMenuPrefabs._MMSliderPrefab;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060001CA RID: 458 RVA: 0x0000A590 File Offset: 0x00008790
		public static GameObject MMCategorySectionPrefab
		{
			get
			{
				bool flag = MMenuPrefabs._MMCategorySectionPrefab == null;
				if (flag)
				{
					MMenuPrefabs._MMCategorySectionPrefab = MMenuPrefabs.FindComponent("EarmuffMode", "CategorySection");
				}
				return MMenuPrefabs._MMCategorySectionPrefab;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060001CB RID: 459 RVA: 0x0000A5CC File Offset: 0x000087CC
		public static GameObject MMCategorySectionBackGroundPrefab
		{
			get
			{
				bool flag = MMenuPrefabs._MMCategorySectionGroundPrefab == null;
				if (flag)
				{
					MMenuPrefabs._MMCategorySectionGroundPrefab = MMenuPrefabs.FindComponent("Background_Info", "SectionBackground");
				}
				return MMenuPrefabs._MMCategorySectionGroundPrefab;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060001CC RID: 460 RVA: 0x0000A608 File Offset: 0x00008808
		public static GameObject MMUserDetailButton
		{
			get
			{
				bool flag = MMenuPrefabs._MMUserDetailButton == null;
				if (flag)
				{
					MMenuPrefabs._MMUserDetailButton = MMenuPrefabs.FindComponent("JoinBtn", "UserDetailButton");
				}
				return MMenuPrefabs._MMUserDetailButton;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060001CD RID: 461 RVA: 0x0000A644 File Offset: 0x00008844
		public static GameObject MMAvatarButton
		{
			get
			{
				bool flag = MMenuPrefabs._MMAvatarButton == null;
				if (flag)
				{
					MMenuPrefabs._MMAvatarButton = MMenuPrefabs.FindComponent("ViewAvatarDetails", "AvatarButton");
				}
				return MMenuPrefabs._MMAvatarButton;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060001CE RID: 462 RVA: 0x0000A680 File Offset: 0x00008880
		public static GameObject MMSeparatorprefab
		{
			get
			{
				return MMenuPrefabs.MMSeparatorPrefab;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001CF RID: 463 RVA: 0x0000A698 File Offset: 0x00008898
		public static GameObject MMLabelPrefab
		{
			get
			{
				bool flag = MMenuPrefabs._MMLabelPrefab == null;
				if (flag)
				{
					MMenuPrefabs._MMLabelPrefab = MMenuPrefabs.FindComponent("AlwaysShowVisualAide", "Label_MM");
				}
				return MMenuPrefabs._MMLabelPrefab;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x0000A6D4 File Offset: 0x000088D4
		public static GameObject MMLabelTextPrefab
		{
			get
			{
				bool flag = MMenuPrefabs._MMLabelTextPrefab == null;
				if (flag)
				{
					MMenuPrefabs._MMLabelTextPrefab = MMenuPrefabs.FindComponent("Title", "LabelText");
				}
				return MMenuPrefabs._MMLabelTextPrefab;
			}
		}

		// Token: 0x040000A3 RID: 163
		private static GameObject _MMHeaderButtonPrefab;

		// Token: 0x040000A4 RID: 164
		private static GameObject _MMDropdownPrefab;

		// Token: 0x040000A5 RID: 165
		private static GameObject _MMTogglePrefab;

		// Token: 0x040000A6 RID: 166
		private static GameObject _MMSeparatorPrefab;

		// Token: 0x040000A7 RID: 167
		private static GameObject _MMTabButtonPrefab;

		// Token: 0x040000A8 RID: 168
		private static GameObject _MMCategoryButtonPrefab;

		// Token: 0x040000A9 RID: 169
		private static GameObject _MMCategoryContainerPrefab;

		// Token: 0x040000AA RID: 170
		private static GameObject _MMSelectorPrefab;

		// Token: 0x040000AB RID: 171
		private static GameObject _MMSideBarHeaderButtonPrefab;

		// Token: 0x040000AC RID: 172
		private static GameObject _MMSliderPrefab;

		// Token: 0x040000AD RID: 173
		private static GameObject _MMCategorySectionPrefab;

		// Token: 0x040000AE RID: 174
		private static GameObject _MMCategorySectionGroundPrefab;

		// Token: 0x040000AF RID: 175
		private static GameObject _MMUserDetailButton;

		// Token: 0x040000B0 RID: 176
		private static GameObject _MMAvatarButton;

		// Token: 0x040000B1 RID: 177
		private static GameObject _MMLabelPrefab;

		// Token: 0x040000B2 RID: 178
		private static GameObject _MMLabelTextPrefab;

		// Token: 0x040000B3 RID: 179
		private static Transform _mainMenuRoot;
	}
}
