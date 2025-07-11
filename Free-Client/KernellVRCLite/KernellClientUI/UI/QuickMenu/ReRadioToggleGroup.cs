using System;
using System.Collections.Generic;
using KernellClientUI.VRChat;
using TMPro;
using UnityEngine;

namespace KernellClientUI.UI.QuickMenu
{
	// Token: 0x02000053 RID: 83
	public class ReRadioToggleGroup : UiElement
	{
		// Token: 0x060003A1 RID: 929 RVA: 0x00013E30 File Offset: 0x00012030
		public ReRadioToggleGroup(string name, Transform parent, Action<object> onSelectionChanged, string color = "#ffffff") : base(QMMenuPrefabs.ContainerPrefab, parent, "RadioGroup_" + name, true)
		{
			try
			{
				this._onSelectionChanged = onSelectionChanged;
				this._color = color;
				this._groupHeader = new ReMenuDesc(name, base.RectTransform, color, 16f, 513);
				this._groupHeader.SetBold(true);
			}
			catch (Exception ex)
			{
				Debug.LogError("[ReRadioToggleGroup] Error in constructor: " + ex.Message + "\n" + ex.StackTrace);
				try
				{
					GameObject gameObject = new GameObject("GroupHeader");
					gameObject.transform.SetParent(base.RectTransform, false);
					TextMeshProUGUI textMeshProUGUI = gameObject.AddComponent<TextMeshProUGUI>();
					textMeshProUGUI.text = name;
					textMeshProUGUI.fontSize = 16f;
					textMeshProUGUI.color = Color.white;
					textMeshProUGUI.alignment = 513;
					textMeshProUGUI.fontStyle = 1;
				}
				catch
				{
				}
			}
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x00013F4C File Offset: 0x0001214C
		public ReRadioToggle AddOption(string text, object value, bool isDefault = false)
		{
			ReRadioToggle result;
			try
			{
				ReRadioToggle reRadioToggle = new ReRadioToggle(base.RectTransform, text, text, value, isDefault);
				reRadioToggle.ToggleStateUpdated = (Action<ReRadioToggle, bool>)Delegate.Combine(reRadioToggle.ToggleStateUpdated, new Action<ReRadioToggle, bool>(this.OnToggleSelect));
				this._radioToggles.Add(reRadioToggle);
				if (isDefault)
				{
					Action<object> onSelectionChanged = this._onSelectionChanged;
					if (onSelectionChanged != null)
					{
						onSelectionChanged(value);
					}
				}
				result = reRadioToggle;
			}
			catch (Exception ex)
			{
				Debug.LogError("[ReRadioToggleGroup] Error adding option '" + text + "': " + ex.Message);
				result = null;
			}
			return result;
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x00013FF0 File Offset: 0x000121F0
		public void SelectOption(object value)
		{
			try
			{
				bool flag = false;
				foreach (ReRadioToggle reRadioToggle in this._radioToggles)
				{
					bool flag2 = reRadioToggle != null;
					if (flag2)
					{
						bool flag3 = reRadioToggle.ToggleData != null && reRadioToggle.ToggleData.Equals(value);
						reRadioToggle.SetToggle(flag3);
						bool flag4 = flag3;
						if (flag4)
						{
							flag = true;
							Action<object> onSelectionChanged = this._onSelectionChanged;
							if (onSelectionChanged != null)
							{
								onSelectionChanged(value);
							}
						}
					}
				}
				bool flag5 = !flag && this._radioToggles.Count > 0 && this._radioToggles[0] != null;
				if (flag5)
				{
					this._radioToggles[0].SetToggle(true);
					Action<object> onSelectionChanged2 = this._onSelectionChanged;
					if (onSelectionChanged2 != null)
					{
						onSelectionChanged2(this._radioToggles[0].ToggleData);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[ReRadioToggleGroup] Error selecting option: " + ex.Message);
			}
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x00014124 File Offset: 0x00012324
		private void OnToggleSelect(ReRadioToggle toggle, bool state)
		{
			try
			{
				bool flag = !state;
				if (!flag)
				{
					foreach (ReRadioToggle reRadioToggle in this._radioToggles)
					{
						bool flag2 = reRadioToggle != null && reRadioToggle != toggle;
						if (flag2)
						{
							reRadioToggle.SetToggle(false);
						}
					}
					bool flag3 = toggle != null && toggle.ToggleData != null;
					if (flag3)
					{
						Action<object> onSelectionChanged = this._onSelectionChanged;
						if (onSelectionChanged != null)
						{
							onSelectionChanged(toggle.ToggleData);
						}
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[ReRadioToggleGroup] Error in toggle selection: " + ex.Message);
			}
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00014200 File Offset: 0x00012400
		public void ClearOptions()
		{
			try
			{
				foreach (ReRadioToggle reRadioToggle in this._radioToggles)
				{
					bool flag = ((reRadioToggle != null) ? reRadioToggle.GameObject : null) != null;
					if (flag)
					{
						Object.Destroy(reRadioToggle.GameObject);
					}
				}
				this._radioToggles.Clear();
			}
			catch (Exception ex)
			{
				Debug.LogError("[ReRadioToggleGroup] Error clearing options: " + ex.Message);
			}
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x000142B4 File Offset: 0x000124B4
		public void SetHeaderText(string text)
		{
			try
			{
				bool flag = this._groupHeader != null;
				if (flag)
				{
					this._groupHeader.Text = text;
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[ReRadioToggleGroup] Error setting header text: " + ex.Message);
			}
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00014314 File Offset: 0x00012514
		public object GetSelectedValue()
		{
			try
			{
				foreach (ReRadioToggle reRadioToggle in this._radioToggles)
				{
					bool flag = reRadioToggle != null && reRadioToggle.IsOn;
					if (flag)
					{
						return reRadioToggle.ToggleData;
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError("[ReRadioToggleGroup] Error getting selected value: " + ex.Message);
			}
			return null;
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060003A8 RID: 936 RVA: 0x000143B8 File Offset: 0x000125B8
		public int OptionCount
		{
			get
			{
				return this._radioToggles.Count;
			}
		}

		// Token: 0x0400017D RID: 381
		private readonly List<ReRadioToggle> _radioToggles = new List<ReRadioToggle>();

		// Token: 0x0400017E RID: 382
		private readonly Action<object> _onSelectionChanged;

		// Token: 0x0400017F RID: 383
		private readonly string _color;

		// Token: 0x04000180 RID: 384
		private ReMenuDesc _groupHeader;
	}
}
