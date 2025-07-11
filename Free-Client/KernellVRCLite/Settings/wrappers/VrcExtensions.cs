using System;
using System.Collections.Generic;
using UnityEngine;
using VRC.Networking;

namespace KernellVRCLite.Settings.wrappers
{
	// Token: 0x020000A6 RID: 166
	public static class VrcExtensions
	{
		// Token: 0x0600087E RID: 2174 RVA: 0x00034A60 File Offset: 0x00032C60
		internal static void ToggleCharacterController(bool toggle)
		{
			bool flag = VrcExtensions._cachedLocalPlayer == null || VrcExtensions._cachedLocalPlayer != VRCPlayer.field_Internal_Static_VRCPlayer_0;
			if (flag)
			{
				VrcExtensions._cachedLocalPlayer = VRCPlayer.field_Internal_Static_VRCPlayer_0;
				VRCPlayer cachedLocalPlayer = VrcExtensions._cachedLocalPlayer;
				VrcExtensions._cachedCharacterController = ((cachedLocalPlayer != null) ? cachedLocalPlayer.gameObject.GetComponent<CharacterController>() : null);
			}
			bool flag2 = VrcExtensions._cachedCharacterController != null;
			if (flag2)
			{
				VrcExtensions._cachedCharacterController.enabled = toggle;
			}
		}

		// Token: 0x0600087F RID: 2175 RVA: 0x00034AD4 File Offset: 0x00032CD4
		internal static void ToggleNetworkSerializer(bool value)
		{
			bool flag = VrcExtensions._cachedLocalPlayer == null || VrcExtensions._cachedLocalPlayer != VRCPlayer.field_Internal_Static_VRCPlayer_0;
			if (flag)
			{
				VrcExtensions._cachedLocalPlayer = VRCPlayer.field_Internal_Static_VRCPlayer_0;
				VRCPlayer cachedLocalPlayer = VrcExtensions._cachedLocalPlayer;
				VrcExtensions._cachedNetworkSerializer = ((cachedLocalPlayer != null) ? cachedLocalPlayer.gameObject.GetComponent<FlatBufferNetworkSerializer>() : null);
			}
			bool flag2 = VrcExtensions._cachedNetworkSerializer != null;
			if (flag2)
			{
				VrcExtensions._cachedNetworkSerializer.enabled = value;
			}
		}

		// Token: 0x06000880 RID: 2176 RVA: 0x00034B48 File Offset: 0x00032D48
		public static void AddRenderer(this HighlightsFXStandalone fx, MeshFilter meshFilter)
		{
			bool flag = fx != null && meshFilter != null;
			if (flag)
			{
				fx.field_Protected_HashSet_1_MeshFilter_0.Add(meshFilter);
			}
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x00034B7C File Offset: 0x00032D7C
		public static void RemoveRenderer(this HighlightsFXStandalone fx, MeshFilter meshFilter)
		{
			bool flag = fx != null && meshFilter != null;
			if (flag)
			{
				fx.field_Protected_HashSet_1_MeshFilter_0.Remove(meshFilter);
			}
		}

		// Token: 0x06000882 RID: 2178 RVA: 0x00034BB0 File Offset: 0x00032DB0
		public static void Render(this HighlightsFX fx, Renderer renderer, bool enabled)
		{
			bool flag = fx == null || renderer == null;
			if (!flag)
			{
				MeshFilter cachedMeshFilter = VrcExtensions.GetCachedMeshFilter(renderer);
				bool flag2 = cachedMeshFilter == null;
				if (!flag2)
				{
					if (enabled)
					{
						fx.field_Protected_HashSet_1_MeshFilter_0.Add(cachedMeshFilter);
					}
					else
					{
						fx.field_Protected_HashSet_1_MeshFilter_0.Remove(cachedMeshFilter);
					}
				}
			}
		}

		// Token: 0x06000883 RID: 2179 RVA: 0x00034C0C File Offset: 0x00032E0C
		public static void Render(this HighlightsFX fx, MeshFilter meshFilter, bool enabled)
		{
			bool flag = fx == null || meshFilter == null;
			if (!flag)
			{
				if (enabled)
				{
					fx.field_Protected_HashSet_1_MeshFilter_0.Add(meshFilter);
				}
				else
				{
					fx.field_Protected_HashSet_1_MeshFilter_0.Remove(meshFilter);
				}
			}
		}

		// Token: 0x06000884 RID: 2180 RVA: 0x00034C54 File Offset: 0x00032E54
		public static void Render(this HighlightsFXStandalone fx, Renderer renderer, bool enabled)
		{
			bool flag = fx == null || renderer == null;
			if (!flag)
			{
				MeshFilter cachedMeshFilter = VrcExtensions.GetCachedMeshFilter(renderer);
				bool flag2 = cachedMeshFilter == null;
				if (!flag2)
				{
					if (enabled)
					{
						fx.field_Protected_HashSet_1_MeshFilter_0.Add(cachedMeshFilter);
					}
					else
					{
						fx.field_Protected_HashSet_1_MeshFilter_0.Remove(cachedMeshFilter);
					}
				}
			}
		}

		// Token: 0x06000885 RID: 2181 RVA: 0x00034CB4 File Offset: 0x00032EB4
		public static void Render(this HighlightsFXStandalone fx, MeshFilter meshFilter, bool enabled)
		{
			bool flag = fx == null || meshFilter == null;
			if (!flag)
			{
				if (enabled)
				{
					fx.field_Protected_HashSet_1_MeshFilter_0.Add(meshFilter);
				}
				else
				{
					fx.field_Protected_HashSet_1_MeshFilter_0.Remove(meshFilter);
				}
			}
		}

		// Token: 0x06000886 RID: 2182 RVA: 0x00034D00 File Offset: 0x00032F00
		private static MeshFilter GetCachedMeshFilter(Renderer renderer)
		{
			MeshFilter component;
			bool flag = !VrcExtensions._meshFilterCache.TryGetValue(renderer, out component);
			if (flag)
			{
				component = renderer.GetComponent<MeshFilter>();
				bool flag2 = component != null;
				if (flag2)
				{
					VrcExtensions._meshFilterCache[renderer] = component;
					bool flag3 = ++VrcExtensions._cacheCleanupCounter >= 100;
					if (flag3)
					{
						VrcExtensions.CleanupCache();
						VrcExtensions._cacheCleanupCounter = 0;
					}
				}
			}
			return component;
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x00034D74 File Offset: 0x00032F74
		private static void CleanupCache()
		{
			List<Renderer> list = new List<Renderer>();
			foreach (KeyValuePair<Renderer, MeshFilter> keyValuePair in VrcExtensions._meshFilterCache)
			{
				bool flag = keyValuePair.Key == null || keyValuePair.Value == null;
				if (flag)
				{
					list.Add(keyValuePair.Key);
				}
			}
			foreach (Renderer key in list)
			{
				VrcExtensions._meshFilterCache.Remove(key);
			}
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x00034E44 File Offset: 0x00033044
		public static void Toast(string content, string type = "standard")
		{
			ToastNotif.Toast(content, "", null, 5f);
		}

		// Token: 0x04000418 RID: 1048
		private static CharacterController _cachedCharacterController;

		// Token: 0x04000419 RID: 1049
		private static FlatBufferNetworkSerializer _cachedNetworkSerializer;

		// Token: 0x0400041A RID: 1050
		private static VRCPlayer _cachedLocalPlayer;

		// Token: 0x0400041B RID: 1051
		private static readonly Dictionary<Renderer, MeshFilter> _meshFilterCache = new Dictionary<Renderer, MeshFilter>(100);

		// Token: 0x0400041C RID: 1052
		private static int _cacheCleanupCounter = 0;

		// Token: 0x0400041D RID: 1053
		private const int CACHE_CLEANUP_INTERVAL = 100;
	}
}
