using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using KernellClientUI.Managers;
using MelonLoader;

namespace KernellClientUI
{
	// Token: 0x02000026 RID: 38
	public class ConfigValue<T>
	{
		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000184 RID: 388 RVA: 0x00008DA5 File Offset: 0x00006FA5
		// (set) Token: 0x06000185 RID: 389 RVA: 0x00008DB2 File Offset: 0x00006FB2
		public T Value
		{
			get
			{
				return this._entry.Value;
			}
			set
			{
				this._entry.Value = value;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000186 RID: 390 RVA: 0x00008DC1 File Offset: 0x00006FC1
		public T DefaultValue
		{
			get
			{
				return this._entry.DefaultValue;
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000187 RID: 391 RVA: 0x00008DD0 File Offset: 0x00006FD0
		// (remove) Token: 0x06000188 RID: 392 RVA: 0x00008E08 File Offset: 0x00007008
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action OnValueChanged;

		// Token: 0x06000189 RID: 393 RVA: 0x00008E40 File Offset: 0x00007040
		[Obsolete("Obsolete")]
		public ConfigValue(string name, T defaultValue, string displayName = null, string description = null, bool isHidden = false, string filePath = null)
		{
			bool flag = !ConfigManager.Instances.ContainsKey(Assembly.GetCallingAssembly().Location);
			if (flag)
			{
				throw new Exception("ConfigManager was not found. Please create it first.");
			}
			MelonPreferences_Category melonPreferences_Category = MelonPreferences.CreateCategory(ConfigManager.Instances[Assembly.GetCallingAssembly().Location]);
			bool flag2 = filePath != null;
			if (flag2)
			{
				melonPreferences_Category.SetFilePath(filePath);
			}
			string text = string.Concat<char>(Enumerable.Where<char>(name, (char c) => char.IsLetter(c) || char.IsNumber(c)));
			this._entry = (melonPreferences_Category.GetEntry<T>(text) ?? melonPreferences_Category.CreateEntry<T>(text, defaultValue, displayName, description, isHidden, false, null, null));
			this._entry.OnValueChangedUntyped += delegate()
			{
				Action onValueChanged = this.OnValueChanged;
				if (onValueChanged != null)
				{
					onValueChanged();
				}
			};
		}

		// Token: 0x0600018A RID: 394 RVA: 0x00008F10 File Offset: 0x00007110
		public static implicit operator T(ConfigValue<T> conf)
		{
			return conf._entry.Value;
		}

		// Token: 0x0600018B RID: 395 RVA: 0x00008F2D File Offset: 0x0000712D
		public void SetValue(T value)
		{
			this._entry.Value = value;
			MelonPreferences.Save();
		}

		// Token: 0x0600018C RID: 396 RVA: 0x00008F44 File Offset: 0x00007144
		public override string ToString()
		{
			T value = this._entry.Value;
			return value.ToString();
		}

		// Token: 0x04000085 RID: 133
		private readonly MelonPreferences_Entry<T> _entry;
	}
}
