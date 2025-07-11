using System;
using System.IO;
using System.Reflection;
using Il2CppSystem.Collections.Generic;
using KernelVRC;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Core.Styles;

namespace KernellVRCLite.modules.uimods
{
	// Token: 0x020000A3 RID: 163
	public class backgroundchange : KernelModuleBase
	{
		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06000864 RID: 2148 RVA: 0x00034416 File Offset: 0x00032616
		public override string ModuleName
		{
			get
			{
				return "Background Change";
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000865 RID: 2149 RVA: 0x0002EF62 File Offset: 0x0002D162
		public override string Version
		{
			get
			{
				return "1.0.0";
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000866 RID: 2150 RVA: 0x0003441D File Offset: 0x0003261D
		public override ModuleCapabilities Capabilities
		{
			get
			{
				return ModuleCapabilities.Update | ModuleCapabilities.MenuEvents | ModuleCapabilities.UIInit;
			}
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000867 RID: 2151 RVA: 0x00003312 File Offset: 0x00001512
		public override UpdateFrequency UpdateFrequency
		{
			get
			{
				return UpdateFrequency.Every2Frames;
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000868 RID: 2152 RVA: 0x00003315 File Offset: 0x00001515
		public override ModulePriority Priority
		{
			get
			{
				return ModulePriority.Normal;
			}
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x00034424 File Offset: 0x00032624
		public override void OnMenuOpened()
		{
			this.bg = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)");
			Sprite sprite = this.EditBackgroundSprite("Background");
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x00034450 File Offset: 0x00032650
		public Sprite EditBackgroundSprite(string spriteName)
		{
			bool flag = this.bg == null;
			Sprite result;
			if (flag)
			{
				Debug.LogError("Canvas_QuickMenu not found. Call OnMenuOpened first.");
				result = null;
			}
			else
			{
				StyleEngine component = this.bg.GetComponent<StyleEngine>();
				bool flag2 = component == null;
				if (flag2)
				{
					Debug.LogError("StyleEngine component not found on Canvas_QuickMenu.");
					result = null;
				}
				else
				{
					Dictionary<string, Sprite> field_Private_Dictionary_2_String_Sprite_ = component.field_Private_Dictionary_2_String_Sprite_0;
					bool flag3 = field_Private_Dictionary_2_String_Sprite_ == null;
					if (flag3)
					{
						Debug.LogError("Sprite dictionary is null.");
						result = null;
					}
					else
					{
						string text = spriteName.StartsWith("kernell_") ? spriteName : ("kernell_" + spriteName);
						Image image = backgroundchange.LoadEmbeddedImage("KernellVRCLite.KernellVRCLite.assets.Mainbg.png");
						bool flag4 = image == null || image.sprite == null;
						if (flag4)
						{
							kernelllogger.Error("Failed to load embedded image for sprite '" + text + "'");
							result = null;
						}
						else
						{
							Sprite sprite = image.sprite;
							bool flag5 = field_Private_Dictionary_2_String_Sprite_.ContainsKey(text);
							if (flag5)
							{
								field_Private_Dictionary_2_String_Sprite_[text] = sprite;
								kernelllogger.Info("Updated existing sprite '" + text + "'");
							}
							else
							{
								field_Private_Dictionary_2_String_Sprite_.Add(text, sprite);
								kernelllogger.Info("Added new sprite '" + text + "' to dictionary");
							}
							result = sprite;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x000345AC File Offset: 0x000327AC
		public static Sprite LoadEmbeddedSprite(string resourcePath)
		{
			try
			{
				Assembly executingAssembly = Assembly.GetExecutingAssembly();
				using (Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(resourcePath))
				{
					bool flag = manifestResourceStream == null;
					if (flag)
					{
						kernelllogger.Error("Could not find embedded resource: " + resourcePath);
						return null;
					}
					byte[] array = new byte[manifestResourceStream.Length];
					manifestResourceStream.Read(array, 0, array.Length);
					Texture2D texture2D = new Texture2D(2, 2);
					bool flag2 = ImageConversion.LoadImage(texture2D, array);
					if (flag2)
					{
						return Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("Error loading embedded sprite: " + ex.Message);
			}
			return null;
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x000346AC File Offset: 0x000328AC
		public static Image LoadEmbeddedImage(string resourcePath)
		{
			try
			{
				Assembly executingAssembly = Assembly.GetExecutingAssembly();
				using (Stream manifestResourceStream = executingAssembly.GetManifestResourceStream(resourcePath))
				{
					bool flag = manifestResourceStream == null;
					if (flag)
					{
						kernelllogger.Error("Could not find embedded resource: " + resourcePath);
						return null;
					}
					byte[] array = new byte[manifestResourceStream.Length];
					manifestResourceStream.Read(array, 0, array.Length);
					Texture2D texture2D = new Texture2D(2, 2);
					bool flag2 = ImageConversion.LoadImage(texture2D, array);
					if (flag2)
					{
						return new Image
						{
							sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f))
						};
					}
				}
			}
			catch (Exception ex)
			{
				kernelllogger.Error("Error loading embedded sprite: " + ex.Message);
			}
			return null;
		}

		// Token: 0x04000411 RID: 1041
		private GameObject bg;
	}
}
