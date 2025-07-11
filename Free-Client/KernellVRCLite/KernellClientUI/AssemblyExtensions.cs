using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KernellClientUI
{
	// Token: 0x02000023 RID: 35
	internal static class AssemblyExtensions
	{
		// Token: 0x06000181 RID: 385 RVA: 0x00008D0C File Offset: 0x00006F0C
		public static IEnumerable<Type> TryGetTypes(Assembly asm)
		{
			IEnumerable<Type> result;
			try
			{
				result = asm.GetTypes();
			}
			catch (ReflectionTypeLoadException ex)
			{
				try
				{
					result = asm.GetExportedTypes();
				}
				catch
				{
					result = Enumerable.Where<Type>(ex.Types, (Type t) => t != null);
				}
			}
			catch
			{
				result = Enumerable.Empty<Type>();
			}
			return result;
		}
	}
}
