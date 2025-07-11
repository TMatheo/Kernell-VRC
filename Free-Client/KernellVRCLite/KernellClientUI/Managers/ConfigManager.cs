using System;
using System.Collections.Generic;
using System.Reflection;

namespace KernellClientUI.Managers
{
	// Token: 0x0200006D RID: 109
	public class ConfigManager
	{
		// Token: 0x06000492 RID: 1170 RVA: 0x0001A428 File Offset: 0x00018628
		public ConfigManager(string categoryName)
		{
			bool flag = ConfigManager.Instances.ContainsValue(categoryName) || ConfigManager.Instances.ContainsKey(Assembly.GetCallingAssembly().Location);
			if (flag)
			{
				throw new Exception("ConfigManager already exists.");
			}
			ConfigManager.Instances.Add(Assembly.GetCallingAssembly().Location, categoryName);
		}

		// Token: 0x040001E3 RID: 483
		public static readonly Dictionary<string, string> Instances = new Dictionary<string, string>();
	}
}
