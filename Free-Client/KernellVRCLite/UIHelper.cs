using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using KernelVRC;
using UnityEngine;
using UnityEngine.UI;

namespace KernellVRCLite
{
	// Token: 0x0200007B RID: 123
	public static class UIHelper
	{
		// Token: 0x06000583 RID: 1411 RVA: 0x000216DC File Offset: 0x0001F8DC
		public static GameObject FindGameObjectByFullPath(string fullPath, bool useCache = true)
		{
			bool flag = string.IsNullOrEmpty(fullPath);
			GameObject result;
			if (flag)
			{
				result = null;
			}
			else
			{
				GameObject gameObject;
				bool flag2 = useCache && UIHelper._gameObjectCache.TryGetValue(fullPath, out gameObject);
				if (flag2)
				{
					bool flag3 = gameObject != null;
					if (flag3)
					{
						return gameObject;
					}
					UIHelper._gameObjectCache.Remove(fullPath);
				}
				string[] array = fullPath.Split(new char[]
				{
					'/'
				});
				GameObject gameObject2 = GameObject.Find(array[0]);
				bool flag4 = gameObject2 == null;
				if (flag4)
				{
					result = null;
				}
				else
				{
					for (int i = 1; i < array.Length; i++)
					{
						Transform transform = gameObject2.transform.Find(array[i]);
						bool flag5 = transform == null;
						if (flag5)
						{
							return null;
						}
						gameObject2 = transform.gameObject;
					}
					if (useCache)
					{
						UIHelper._gameObjectCache[fullPath] = gameObject2;
					}
					result = gameObject2;
				}
			}
			return result;
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x000217C8 File Offset: 0x0001F9C8
		public static bool SetGameObjectActive(string fullPath, bool isActive)
		{
			GameObject gameObject = UIHelper.FindGameObjectByFullPath(fullPath, true);
			bool flag = gameObject == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				gameObject.SetActive(isActive);
				result = true;
			}
			return result;
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x000217FA File Offset: 0x0001F9FA
		public static bool HideGameObject(string fullPath)
		{
			return UIHelper.SetGameObjectActive(fullPath, false);
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x00021803 File Offset: 0x0001FA03
		public static bool ShowGameObject(string fullPath)
		{
			return UIHelper.SetGameObjectActive(fullPath, true);
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0002180C File Offset: 0x0001FA0C
		public static bool ChangeButtonIcon(string buttonPath, string iconResourcePath)
		{
			GameObject gameObject = UIHelper.FindGameObjectByFullPath(buttonPath, true);
			return gameObject != null && UIHelper.ChangeButtonIcon(gameObject, iconResourcePath);
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0002183C File Offset: 0x0001FA3C
		public static bool ChangeButtonIcon(GameObject buttonObj, string iconResourcePath)
		{
			bool flag = buttonObj == null;
			bool result;
			if (flag)
			{
				Debug.LogWarning("Button GameObject is null.");
				result = false;
			}
			else
			{
				Image image = buttonObj.GetComponent<Image>();
				bool flag2 = image == null;
				if (flag2)
				{
					Transform transform = buttonObj.transform.Find("Icon");
					image = ((transform != null) ? transform.GetComponent<Image>() : null);
				}
				bool flag3 = image == null;
				if (flag3)
				{
					Debug.LogWarning("Icon Image component not found for button " + buttonObj.name);
					result = false;
				}
				else
				{
					try
					{
						image.sprite = UIHelper.LoadEmbeddedSprite(iconResourcePath);
						result = true;
					}
					catch (Exception ex)
					{
						Debug.LogError("Failed to load sprite from " + iconResourcePath + ": " + ex.Message);
						result = false;
					}
				}
			}
			return result;
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0002191C File Offset: 0x0001FB1C
		public static void ClearCache()
		{
			UIHelper._gameObjectCache.Clear();
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0002192C File Offset: 0x0001FB2C
		private static Sprite LoadEmbeddedSprite(string resourcePath)
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

		// Token: 0x0600058B RID: 1419 RVA: 0x00021A2C File Offset: 0x0001FC2C
		public static void RemoveFromCache(string fullPath)
		{
			UIHelper._gameObjectCache.Remove(fullPath);
		}

		// Token: 0x0400027E RID: 638
		private static readonly Dictionary<string, GameObject> _gameObjectCache = new Dictionary<string, GameObject>();
	}
}
