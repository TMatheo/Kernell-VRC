using System;
using System.Collections.Generic;
using Il2CppSystem.Collections.Generic;
using UnityEngine;

// Token: 0x0200000D RID: 13
public static class HighlightsFXExtensions
{
	// Token: 0x0600002E RID: 46 RVA: 0x0000274C File Offset: 0x0000094C
	public static void AddRenderer(this HighlightsFX fx, Renderer renderer, bool enabled = true)
	{
		bool flag = fx == null || renderer == null;
		if (!flag)
		{
			try
			{
				SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
				bool flag2 = skinnedMeshRenderer != null;
				if (flag2)
				{
					HighlightsFXExtensions.HandleSkinnedMeshRenderer(fx, skinnedMeshRenderer, enabled);
				}
				else
				{
					HighlightsFXExtensions.HandleRegularRenderer(fx, renderer, enabled);
				}
			}
			catch (Exception arg)
			{
				Debug.LogError(string.Format("[HighlightsFXExtensions] Error in AddRenderer: {0}", arg));
			}
		}
	}

	// Token: 0x0600002F RID: 47 RVA: 0x000027CC File Offset: 0x000009CC
	public static void RemoveRenderer(this HighlightsFX fx, Renderer renderer)
	{
		bool flag = fx == null || renderer == null;
		if (!flag)
		{
			try
			{
				SkinnedMeshRenderer skinnedMeshRenderer = renderer as SkinnedMeshRenderer;
				bool flag2 = skinnedMeshRenderer != null;
				if (flag2)
				{
					HighlightsFXExtensions.SkinnedMeshData skinnedMeshData;
					bool flag3 = HighlightsFXExtensions._skinnedMeshData.TryGetValue(skinnedMeshRenderer, out skinnedMeshData);
					if (flag3)
					{
						bool flag4 = skinnedMeshData.TempFilter != null;
						if (flag4)
						{
							HashSet<MeshFilter> field_Protected_HashSet_1_MeshFilter_ = fx.field_Protected_HashSet_1_MeshFilter_0;
							if (field_Protected_HashSet_1_MeshFilter_ != null)
							{
								field_Protected_HashSet_1_MeshFilter_.Remove(skinnedMeshData.TempFilter);
							}
							Object.Destroy(skinnedMeshData.TempFilter);
						}
						HighlightsFXExtensions._skinnedMeshData.Remove(skinnedMeshRenderer);
					}
				}
				else
				{
					MeshFilter component = renderer.GetComponent<MeshFilter>();
					bool flag5 = component != null;
					if (flag5)
					{
						HashSet<MeshFilter> field_Protected_HashSet_1_MeshFilter_2 = fx.field_Protected_HashSet_1_MeshFilter_0;
						if (field_Protected_HashSet_1_MeshFilter_2 != null)
						{
							field_Protected_HashSet_1_MeshFilter_2.Remove(component);
						}
						HighlightsFXExtensions.RestoreOriginalMesh(component);
					}
				}
			}
			catch (Exception arg)
			{
				Debug.LogError(string.Format("[HighlightsFXExtensions] Error in RemoveRenderer: {0}", arg));
			}
		}
	}

	// Token: 0x06000030 RID: 48 RVA: 0x000028CC File Offset: 0x00000ACC
	private static void HandleSkinnedMeshRenderer(HighlightsFX fx, SkinnedMeshRenderer smr, bool enabled)
	{
		bool flag = !enabled;
		if (flag)
		{
			fx.RemoveRenderer(smr);
		}
		else
		{
			HighlightsFXExtensions.SkinnedMeshData skinnedMeshData;
			bool flag2 = HighlightsFXExtensions._skinnedMeshData.TryGetValue(smr, out skinnedMeshData);
			if (flag2)
			{
				bool flag3 = skinnedMeshData.TempFilter != null;
				if (flag3)
				{
					HashSet<MeshFilter> field_Protected_HashSet_1_MeshFilter_ = fx.field_Protected_HashSet_1_MeshFilter_0;
					if (field_Protected_HashSet_1_MeshFilter_ != null)
					{
						field_Protected_HashSet_1_MeshFilter_.Add(skinnedMeshData.TempFilter);
					}
					return;
				}
			}
			GameObject gameObject = new GameObject("HighlightMesh_" + smr.name);
			gameObject.transform.SetParent(smr.transform, false);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
			Mesh mesh = new Mesh();
			smr.BakeMesh(mesh);
			meshFilter.sharedMesh = mesh;
			HighlightsFXExtensions._skinnedMeshData[smr] = new HighlightsFXExtensions.SkinnedMeshData(smr.sharedMesh, meshFilter);
			HashSet<MeshFilter> field_Protected_HashSet_1_MeshFilter_2 = fx.field_Protected_HashSet_1_MeshFilter_0;
			if (field_Protected_HashSet_1_MeshFilter_2 != null)
			{
				field_Protected_HashSet_1_MeshFilter_2.Add(meshFilter);
			}
		}
	}

	// Token: 0x06000031 RID: 49 RVA: 0x000029DC File Offset: 0x00000BDC
	private static void HandleRegularRenderer(HighlightsFX fx, Renderer renderer, bool enabled)
	{
		MeshFilter component = renderer.GetComponent<MeshFilter>();
		bool flag = component == null;
		if (!flag)
		{
			if (enabled)
			{
				bool flag2 = !HighlightsFXExtensions._originalMeshes.ContainsKey(component);
				if (flag2)
				{
					HighlightsFXExtensions._originalMeshes[component] = component.sharedMesh;
				}
				HashSet<MeshFilter> field_Protected_HashSet_1_MeshFilter_ = fx.field_Protected_HashSet_1_MeshFilter_0;
				if (field_Protected_HashSet_1_MeshFilter_ != null)
				{
					field_Protected_HashSet_1_MeshFilter_.Add(component);
				}
			}
			else
			{
				HashSet<MeshFilter> field_Protected_HashSet_1_MeshFilter_2 = fx.field_Protected_HashSet_1_MeshFilter_0;
				if (field_Protected_HashSet_1_MeshFilter_2 != null)
				{
					field_Protected_HashSet_1_MeshFilter_2.Remove(component);
				}
				HighlightsFXExtensions.RestoreOriginalMesh(component);
			}
		}
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00002A5C File Offset: 0x00000C5C
	private static void RestoreOriginalMesh(MeshFilter meshFilter)
	{
		Mesh sharedMesh;
		bool flag = meshFilter != null && HighlightsFXExtensions._originalMeshes.TryGetValue(meshFilter, out sharedMesh);
		if (flag)
		{
			meshFilter.sharedMesh = sharedMesh;
			HighlightsFXExtensions._originalMeshes.Remove(meshFilter);
		}
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00002AA0 File Offset: 0x00000CA0
	public static void ClearAll(this HighlightsFX fx)
	{
		bool flag = fx == null;
		if (!flag)
		{
			try
			{
				foreach (KeyValuePair<MeshFilter, Mesh> keyValuePair in HighlightsFXExtensions._originalMeshes)
				{
					bool flag2 = keyValuePair.Key != null;
					if (flag2)
					{
						keyValuePair.Key.sharedMesh = keyValuePair.Value;
					}
				}
				HighlightsFXExtensions._originalMeshes.Clear();
				foreach (KeyValuePair<SkinnedMeshRenderer, HighlightsFXExtensions.SkinnedMeshData> keyValuePair2 in HighlightsFXExtensions._skinnedMeshData)
				{
					bool flag3 = keyValuePair2.Value.TempFilter != null;
					if (flag3)
					{
						HashSet<MeshFilter> field_Protected_HashSet_1_MeshFilter_ = fx.field_Protected_HashSet_1_MeshFilter_0;
						if (field_Protected_HashSet_1_MeshFilter_ != null)
						{
							field_Protected_HashSet_1_MeshFilter_.Remove(keyValuePair2.Value.TempFilter);
						}
						Object.Destroy(keyValuePair2.Value.TempFilter.gameObject);
					}
				}
				HighlightsFXExtensions._skinnedMeshData.Clear();
				HashSet<MeshFilter> field_Protected_HashSet_1_MeshFilter_2 = fx.field_Protected_HashSet_1_MeshFilter_0;
				if (field_Protected_HashSet_1_MeshFilter_2 != null)
				{
					field_Protected_HashSet_1_MeshFilter_2.Clear();
				}
			}
			catch (Exception arg)
			{
				Debug.LogError(string.Format("[HighlightsFXExtensions] Error in ClearAll: {0}", arg));
			}
		}
	}

	// Token: 0x0400000E RID: 14
	private static readonly Dictionary<SkinnedMeshRenderer, HighlightsFXExtensions.SkinnedMeshData> _skinnedMeshData = new Dictionary<SkinnedMeshRenderer, HighlightsFXExtensions.SkinnedMeshData>();

	// Token: 0x0400000F RID: 15
	private static readonly Dictionary<MeshFilter, Mesh> _originalMeshes = new Dictionary<MeshFilter, Mesh>();

	// Token: 0x020000BE RID: 190
	private class SkinnedMeshData
	{
		// Token: 0x06000A25 RID: 2597 RVA: 0x0004049E File Offset: 0x0003E69E
		public SkinnedMeshData(Mesh originalMesh, MeshFilter tempFilter)
		{
			this.OriginalMesh = originalMesh;
			this.TempFilter = tempFilter;
		}

		// Token: 0x04000532 RID: 1330
		public Mesh OriginalMesh;

		// Token: 0x04000533 RID: 1331
		public readonly MeshFilter TempFilter;
	}
}
