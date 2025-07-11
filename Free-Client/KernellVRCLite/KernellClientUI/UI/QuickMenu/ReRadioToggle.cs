using System;
using System.Linq;
using KernellClientUI.VRChat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRC.UI.Core.Styles;

namespace KernellClientUI.UI.QuickMenu
{
	// Token: 0x02000052 RID: 82
	public class ReRadioToggle : UiElement
	{
		// Token: 0x170000BE RID: 190
		// (get) Token: 0x0600039A RID: 922 RVA: 0x00012EC4 File Offset: 0x000110C4
		private static GameObject TogglePrefab
		{
			get
			{
				bool flag = ReRadioToggle._togglePrefab == null;
				if (flag)
				{
					try
					{
						Transform[] array = Resources.FindObjectsOfTypeAll<Transform>();
						Transform transform = Enumerable.FirstOrDefault<Transform>(array, (Transform t) => t.name.Contains("Cell_QM_AudioDeviceOption") && t.GetComponentInChildren<Toggle>(true) != null);
						bool flag2 = transform != null;
						if (flag2)
						{
							ReRadioToggle._togglePrefab = transform.gameObject;
							Debug.Log("[ReRadioToggle] Found audio device toggle prefab: " + transform.name);
						}
						else
						{
							Transform transform2 = MenuEx.QMInstance.transform.Find("CanvasGroup/Container/Window/QMParent/Menu_ChangeAudioInputDevice/ScrollRect/Viewport/VerticalLayoutGroup");
							bool flag3 = transform2 != null && transform2.childCount > 0;
							if (flag3)
							{
								for (int i = 0; i < transform2.childCount; i++)
								{
									Transform child = transform2.GetChild(i);
									bool flag4 = child.name.Contains("Cell_QM_AudioDeviceOption");
									if (flag4)
									{
										ReRadioToggle._togglePrefab = child.gameObject;
										Debug.Log("[ReRadioToggle] Found audio device toggle prefab via menu: " + child.name);
										break;
									}
								}
							}
							bool flag5 = ReRadioToggle._togglePrefab == null;
							if (flag5)
							{
								Debug.LogError("[ReRadioToggle] Failed to find toggle prefab, creating fallback");
								ReRadioToggle._togglePrefab = ReRadioToggle.CreateBasicToggle();
							}
						}
					}
					catch (Exception ex)
					{
						Debug.LogError("[ReRadioToggle] Error finding prefab: " + ex.Message);
						ReRadioToggle._togglePrefab = ReRadioToggle.CreateBasicToggle();
					}
				}
				return ReRadioToggle._togglePrefab;
			}
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00013078 File Offset: 0x00011278
		private static GameObject CreateBasicToggle()
		{
			GameObject gameObject = new GameObject("RadioToggleFallback");
			RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
			rectTransform.sizeDelta = new Vector2(500f, 70f);
			LayoutElement layoutElement = gameObject.AddComponent<LayoutElement>();
			layoutElement.minHeight = 70f;
			layoutElement.preferredHeight = 70f;
			Button button = gameObject.AddComponent<Button>();
			button.transition = 1;
			Toggle toggle = gameObject.AddComponent<Toggle>();
			toggle.isOn = false;
			toggle.transition = 0;
			GameObject gameObject2 = new GameObject("Background");
			gameObject2.transform.SetParent(gameObject.transform, false);
			RectTransform rectTransform2 = gameObject2.AddComponent<RectTransform>();
			rectTransform2.anchorMin = Vector2.zero;
			rectTransform2.anchorMax = Vector2.one;
			rectTransform2.offsetMin = Vector2.zero;
			rectTransform2.offsetMax = Vector2.zero;
			Image image = gameObject2.AddComponent<Image>();
			image.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
			GameObject gameObject3 = new GameObject("Text");
			gameObject3.transform.SetParent(gameObject.transform, false);
			RectTransform rectTransform3 = gameObject3.AddComponent<RectTransform>();
			rectTransform3.anchorMin = new Vector2(0f, 0f);
			rectTransform3.anchorMax = new Vector2(1f, 1f);
			rectTransform3.offsetMin = new Vector2(20f, 0f);
			rectTransform3.offsetMax = new Vector2(-70f, 0f);
			TMP_Text tmp_Text = gameObject3.AddComponent<TextMeshProUGUI>();
			tmp_Text.fontSize = 20f;
			tmp_Text.alignment = 513;
			tmp_Text.verticalAlignment = 512;
			tmp_Text.color = Color.white;
			GameObject gameObject4 = new GameObject("Checkmark");
			gameObject4.transform.SetParent(gameObject.transform, false);
			RectTransform rectTransform4 = gameObject4.AddComponent<RectTransform>();
			rectTransform4.anchorMin = new Vector2(1f, 0.5f);
			rectTransform4.anchorMax = new Vector2(1f, 0.5f);
			rectTransform4.pivot = new Vector2(1f, 0.5f);
			rectTransform4.sizeDelta = new Vector2(50f, 50f);
			rectTransform4.anchoredPosition = new Vector2(-20f, 0f);
			Image image2 = gameObject4.AddComponent<Image>();
			image2.color = Color.green;
			toggle.graphic = image2;
			return gameObject;
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00013304 File Offset: 0x00011504
		public ReRadioToggle(Transform parent, string name, string text, object obj, bool defaultState = false) : base(ReRadioToggle.TogglePrefab, parent, "ReRadioToggle_" + UiElement.GetCleanName(name), true)
		{
			try
			{
				RectTransform component = base.GameObject.GetComponent<RectTransform>();
				bool flag = component != null;
				if (flag)
				{
					component.sizeDelta = new Vector2(600f, 70f);
				}
				LayoutElement layoutElement = base.GameObject.GetComponent<LayoutElement>();
				bool flag2 = layoutElement == null;
				if (flag2)
				{
					layoutElement = base.GameObject.AddComponent<LayoutElement>();
				}
				layoutElement.minHeight = 70f;
				layoutElement.preferredHeight = 70f;
				string[] array = new string[]
				{
					"MonoBehaviourPublicObVoVoUITeStUnique",
					"MonoBehaviourPublicSt_cReGa_c_eBo_fObAcUnique",
					"MonoBehaviourPublic_c_spSt_c_LiBoUITeStUnique",
					"MonoBehaviour2PublicOb_s_lOb_rInObInUnique",
					"DataContext",
					"VRCButtanHandle"
				};
				foreach (string componentName in array)
				{
					this.SafeDestroyComponent<MonoBehaviour>(componentName);
				}
				this._button = base.GameObject.GetComponent<Button>();
				bool flag3 = this._button == null;
				if (flag3)
				{
					this._button = base.GameObject.AddComponent<Button>();
					this._button.transition = 1;
					ColorBlock colors = this._button.colors;
					colors.normalColor = new Color(1f, 1f, 1f, 0f);
					colors.highlightedColor = new Color(1f, 1f, 1f, 0.1f);
					colors.pressedColor = new Color(1f, 1f, 1f, 0.2f);
					colors.selectedColor = new Color(1f, 1f, 1f, 0.3f);
					this._button.colors = colors;
				}
				this._toggle = base.GameObject.GetComponentInChildren<Toggle>(true);
				bool flag4 = this._toggle == null;
				if (flag4)
				{
					this._toggle = base.GameObject.AddComponent<Toggle>();
				}
				this._checkmark = this._toggle.graphic;
				bool flag5 = this._checkmark == null;
				if (flag5)
				{
					Transform transform = base.GameObject.transform.Find("ButtonElement_CheckBox/Checkmark");
					bool flag6 = transform != null;
					if (flag6)
					{
						this._checkmark = transform.GetComponent<Graphic>();
					}
					bool flag7 = this._checkmark == null;
					if (flag7)
					{
						GameObject gameObject = new GameObject("Checkmark");
						gameObject.transform.SetParent(base.GameObject.transform, false);
						RectTransform rectTransform = gameObject.AddComponent<RectTransform>();
						rectTransform.anchorMin = new Vector2(1f, 0.5f);
						rectTransform.anchorMax = new Vector2(1f, 0.5f);
						rectTransform.pivot = new Vector2(1f, 0.5f);
						rectTransform.sizeDelta = new Vector2(50f, 50f);
						rectTransform.anchoredPosition = new Vector2(-20f, 0f);
						Image image = gameObject.AddComponent<Image>();
						image.color = Color.green;
						this._checkmark = image;
						this._toggle.graphic = image;
					}
				}
				this._text = base.GameObject.GetComponentInChildren<TMP_Text>(true);
				bool flag8 = this._text == null;
				if (flag8)
				{
					Transform transform2 = base.GameObject.transform.Find("Text_ToggleName");
					bool flag9 = transform2 != null;
					if (flag9)
					{
						this._text = transform2.GetComponent<TMP_Text>();
					}
					bool flag10 = this._text == null;
					if (flag10)
					{
						GameObject gameObject2 = new GameObject("Text_ToggleName");
						gameObject2.transform.SetParent(base.GameObject.transform, false);
						RectTransform rectTransform2 = gameObject2.AddComponent<RectTransform>();
						rectTransform2.anchorMin = new Vector2(0f, 0f);
						rectTransform2.anchorMax = new Vector2(1f, 1f);
						rectTransform2.offsetMin = new Vector2(20f, 0f);
						rectTransform2.offsetMax = new Vector2(-70f, 0f);
						this._text = gameObject2.AddComponent<TextMeshProUGUI>();
						this._text.fontSize = 20f;
						this._text.alignment = 513;
						this._text.verticalAlignment = 512;
						this._text.color = Color.white;
					}
				}
				this._style = base.GameObject.GetComponent<StyleElement>();
				bool flag11 = this._text != null;
				if (flag11)
				{
					this._text.text = text;
					this._text.richText = true;
				}
				this.ToggleData = obj;
				this.SetToggle(defaultState);
				bool flag12 = this._button != null;
				if (flag12)
				{
					this._button.onClick.RemoveAllListeners();
					this._button.onClick.AddListener(new Action(this.ToggleOn));
				}
				Debug.Log("[ReRadioToggle] Successfully created toggle for: " + text);
			}
			catch (Exception ex)
			{
				Debug.LogError("[ReRadioToggle] Error in constructor: " + ex.Message + "\n" + ex.StackTrace);
				try
				{
					for (int j = base.GameObject.transform.childCount - 1; j >= 0; j--)
					{
						Object.Destroy(base.GameObject.transform.GetChild(j).gameObject);
					}
					GameObject gameObject3 = new GameObject("Background");
					gameObject3.transform.SetParent(base.GameObject.transform, false);
					RectTransform rectTransform3 = gameObject3.AddComponent<RectTransform>();
					rectTransform3.anchorMin = Vector2.zero;
					rectTransform3.anchorMax = Vector2.one;
					rectTransform3.offsetMin = Vector2.zero;
					rectTransform3.offsetMax = Vector2.zero;
					Image image2 = gameObject3.AddComponent<Image>();
					image2.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
					this._button = base.GameObject.AddComponent<Button>();
					this._toggle = base.GameObject.AddComponent<Toggle>();
					GameObject gameObject4 = new GameObject("Text_ToggleName");
					gameObject4.transform.SetParent(base.GameObject.transform, false);
					RectTransform rectTransform4 = gameObject4.AddComponent<RectTransform>();
					rectTransform4.anchorMin = new Vector2(0f, 0f);
					rectTransform4.anchorMax = new Vector2(1f, 1f);
					rectTransform4.offsetMin = new Vector2(20f, 0f);
					rectTransform4.offsetMax = new Vector2(-70f, 0f);
					this._text = gameObject4.AddComponent<TextMeshProUGUI>();
					this._text.fontSize = 20f;
					this._text.alignment = 513;
					this._text.verticalAlignment = 512;
					this._text.color = Color.white;
					this._text.text = text;
					GameObject gameObject5 = new GameObject("Checkmark");
					gameObject5.transform.SetParent(base.GameObject.transform, false);
					RectTransform rectTransform5 = gameObject5.AddComponent<RectTransform>();
					rectTransform5.anchorMin = new Vector2(1f, 0.5f);
					rectTransform5.anchorMax = new Vector2(1f, 0.5f);
					rectTransform5.pivot = new Vector2(1f, 0.5f);
					rectTransform5.sizeDelta = new Vector2(50f, 50f);
					rectTransform5.anchoredPosition = new Vector2(-20f, 0f);
					Image image3 = gameObject5.AddComponent<Image>();
					image3.color = Color.green;
					this._checkmark = image3;
					this._toggle.graphic = image3;
					this.ToggleData = obj;
					this.SetToggle(defaultState);
					this._button.onClick.AddListener(new Action(this.ToggleOn));
					Debug.Log("[ReRadioToggle] Created fallback toggle for: " + text);
				}
				catch (Exception ex2)
				{
					Debug.LogError("[ReRadioToggle] Failed to create fallback: " + ex2.Message);
				}
			}
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00013BDC File Offset: 0x00011DDC
		private T GetOrAddComponent<T>() where T : Component
		{
			T t = base.GameObject.GetComponent<T>();
			bool flag = t == null;
			if (flag)
			{
				t = base.GameObject.AddComponent<T>();
			}
			return t;
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00013C1C File Offset: 0x00011E1C
		private void SafeDestroyComponent<T>(string componentName = null) where T : Component
		{
			try
			{
				bool flag = !string.IsNullOrEmpty(componentName);
				if (flag)
				{
					Component[] array = base.GameObject.GetComponents<Component>();
					foreach (Component component in array)
					{
						bool flag2 = component != null && component.GetType().Name.Contains(componentName);
						if (flag2)
						{
							Object.DestroyImmediate(component);
						}
					}
				}
				T component2 = base.GameObject.GetComponent<T>();
				bool flag3 = component2 != null;
				if (flag3)
				{
					Object.DestroyImmediate(component2);
				}
			}
			catch (Exception ex)
			{
				Debug.LogWarning("[ReRadioToggle] Failed to destroy component " + (componentName ?? typeof(T).Name) + ": " + ex.Message);
			}
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00013D14 File Offset: 0x00011F14
		public void SetToggle(bool state)
		{
			this.IsOn = state;
			bool flag = this._checkmark != null && this._checkmark.gameObject != null;
			if (flag)
			{
				this._checkmark.gameObject.SetActive(this.IsOn);
			}
			bool flag2 = this._toggle != null;
			if (flag2)
			{
				this._toggle.isOn = this.IsOn;
			}
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00013D8C File Offset: 0x00011F8C
		private void ToggleOn()
		{
			bool flag = !this.IsOn;
			if (flag)
			{
				this.IsOn = true;
				bool flag2 = this._checkmark != null && this._checkmark.gameObject != null;
				if (flag2)
				{
					this._checkmark.gameObject.SetActive(this.IsOn);
				}
				bool flag3 = this._toggle != null;
				if (flag3)
				{
					this._toggle.isOn = this.IsOn;
				}
				Action<ReRadioToggle, bool> toggleStateUpdated = this.ToggleStateUpdated;
				if (toggleStateUpdated != null)
				{
					toggleStateUpdated(this, this.IsOn);
				}
			}
		}

		// Token: 0x04000174 RID: 372
		private static GameObject _togglePrefab;

		// Token: 0x04000175 RID: 373
		public bool IsOn;

		// Token: 0x04000176 RID: 374
		public Action<ReRadioToggle, bool> ToggleStateUpdated;

		// Token: 0x04000177 RID: 375
		public object ToggleData;

		// Token: 0x04000178 RID: 376
		private Button _button;

		// Token: 0x04000179 RID: 377
		private Toggle _toggle;

		// Token: 0x0400017A RID: 378
		private Graphic _checkmark;

		// Token: 0x0400017B RID: 379
		private TMP_Text _text;

		// Token: 0x0400017C RID: 380
		private StyleElement _style;
	}
}
