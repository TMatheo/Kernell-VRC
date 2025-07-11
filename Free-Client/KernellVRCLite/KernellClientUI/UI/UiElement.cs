using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace KernellClientUI.UI
{
	// Token: 0x02000035 RID: 53
	public class UiElement
	{
		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600022B RID: 555 RVA: 0x0000C0BB File Offset: 0x0000A2BB
		public string Name { get; }

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600022C RID: 556 RVA: 0x0000C0C3 File Offset: 0x0000A2C3
		// (set) Token: 0x0600022D RID: 557 RVA: 0x0000C0CB File Offset: 0x0000A2CB
		public GameObject GameObject { get; set; }

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600022E RID: 558 RVA: 0x0000C0D4 File Offset: 0x0000A2D4
		public RectTransform RectTransform { get; }

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x0600022F RID: 559 RVA: 0x0000C0DC File Offset: 0x0000A2DC
		// (set) Token: 0x06000230 RID: 560 RVA: 0x0000C0F9 File Offset: 0x0000A2F9
		public Vector3 Position
		{
			get
			{
				return this.RectTransform.localPosition;
			}
			set
			{
				this.RectTransform.localPosition = value;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000231 RID: 561 RVA: 0x0000C10C File Offset: 0x0000A30C
		// (set) Token: 0x06000232 RID: 562 RVA: 0x0000C129 File Offset: 0x0000A329
		public bool Active
		{
			get
			{
				return this.GameObject.activeSelf;
			}
			set
			{
				this.GameObject.SetActive(value);
			}
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000C13C File Offset: 0x0000A33C
		public UiElement(Transform transform)
		{
			this.RectTransform = transform.GetComponent<RectTransform>();
			bool flag = this.RectTransform == null;
			if (flag)
			{
				throw new ArgumentException("Transform has to be a RectTransform.", "transform");
			}
			this.GameObject = transform.gameObject;
			this.Name = this.GameObject.name;
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000C19C File Offset: 0x0000A39C
		public UiElement(GameObject original, Transform parent, Vector3 pos, string name, bool defaultState = true) : this(original, parent, name, defaultState)
		{
			this.GameObject.transform.localPosition = pos;
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000C1C0 File Offset: 0x0000A3C0
		public UiElement(GameObject original, Transform parent, string name, bool defaultState = true)
		{
			this.GameObject = Object.Instantiate<GameObject>(original, parent);
			this.GameObject.name = UiElement.GetCleanName(name);
			this.Name = this.GameObject.name;
			this.GameObject.SetActive(defaultState);
			this.RectTransform = this.GameObject.GetComponent<RectTransform>();
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000C225 File Offset: 0x0000A425
		public virtual void Destroy()
		{
			Object.Destroy(this.GameObject);
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000C234 File Offset: 0x0000A434
		public static string GetCleanName(string name)
		{
			return Regex.Replace(Regex.Replace(name, "<.*?>", string.Empty), "[^0-9a-zA-Z_]+", string.Empty);
		}
	}
}
