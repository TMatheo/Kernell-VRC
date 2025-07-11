using System;
using System.Collections;
using System.Text;
using MelonLoader;
using UnityEngine;

namespace KernellVRCLite.modules.uimods
{
	// Token: 0x020000A1 RID: 161
	public class KernellTextGlitch : MonoBehaviour
	{
		// Token: 0x06000850 RID: 2128 RVA: 0x00033CC8 File Offset: 0x00031EC8
		public static KernellTextGlitch GetInstance()
		{
			bool flag = KernellTextGlitch.instance == null;
			if (flag)
			{
				GameObject gameObject = new GameObject("KernellTextGlitchHolder");
				Object.DontDestroyOnLoad(gameObject);
				KernellTextGlitch.instance = gameObject.AddComponent<KernellTextGlitch>();
			}
			return KernellTextGlitch.instance;
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x00033D10 File Offset: 0x00031F10
		public void ApplyGlitchEffect()
		{
			bool flag = this.isApplying;
			if (!flag)
			{
				MelonCoroutines.Start(this.ApplyGlitchEffectDelayed());
			}
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x00033D36 File Offset: 0x00031F36
		private IEnumerator ApplyGlitchEffectDelayed()
		{
			this.isApplying = true;
			int attempts = 0;
			while (attempts < 20)
			{
				GameObject titleObject = null;
				int num;
				try
				{
					titleObject = GameObject.Find("UserInterface/Canvas_QuickMenu(Clone)/CanvasGroup/Container/Window/QMParent/Menu_Dashboard/Header_H1/LeftItemContainer/Text_Title");
				}
				catch (Exception ex)
				{
					Exception e = ex;
					Debug.LogError("[KernellTextGlitch] Exception during GameObject.Find: " + e.Message);
					num = attempts;
					attempts = num + 1;
					new WaitForSeconds(0.1f);
					continue;
				}
				bool flag = titleObject != null;
				if (flag)
				{
					try
					{
						this.targetText = titleObject.GetComponent<TextMeshProUGUIEx>();
					}
					catch (Exception ex)
					{
						Exception e2 = ex;
						Debug.LogError("[KernellTextGlitch] Exception getting component: " + e2.Message);
						num = attempts;
						attempts = num + 1;
						new WaitForSeconds(0.1f);
						continue;
					}
					bool flag2 = this.targetText != null;
					if (flag2)
					{
						try
						{
							this.originalText = ((!string.IsNullOrEmpty(this.targetText.text)) ? this.targetText.text : "VRChat");
							this.targetText.alignment = 514;
							this.StopGlitchEffect();
							this.currentGlitchCoroutine = (Coroutine)MelonCoroutines.Start(this.GlitchTransitionCoroutine());
							this.isApplying = false;
							yield break;
						}
						catch (Exception ex)
						{
							Exception e3 = ex;
							Debug.LogError("[KernellTextGlitch] Exception setting up text: " + e3.Message);
						}
					}
				}
				num = attempts;
				attempts = num + 1;
				yield return new WaitForSeconds(0.1f);
				titleObject = null;
			}
			Debug.LogError(string.Format("[KernellTextGlitch] Could not find or access title object after {0} attempts", 20));
			this.isApplying = false;
			yield break;
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x00033D48 File Offset: 0x00031F48
		public void StopGlitchEffect()
		{
			bool flag = this.currentGlitchCoroutine != null;
			if (flag)
			{
				base.StopCoroutine(this.currentGlitchCoroutine);
				this.currentGlitchCoroutine = null;
			}
			bool flag2 = this.targetText != null && !string.IsNullOrEmpty(this.originalText);
			if (flag2)
			{
				try
				{
					this.targetText.text = this.originalText;
					this.targetText.color = Color.white;
				}
				catch (Exception ex)
				{
					Debug.LogError("[KernellTextGlitch] Error resetting text: " + ex.Message);
				}
			}
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x00033DF8 File Offset: 0x00031FF8
		private IEnumerator GlitchTransitionCoroutine()
		{
			IEnumerator chaosPhase = this.ChaosPhase();
			while (chaosPhase.MoveNext())
			{
				object obj = chaosPhase.Current;
				yield return obj;
			}
			IEnumerator revealPhase = this.RevealPhase();
			while (revealPhase.MoveNext())
			{
				object obj2 = revealPhase.Current;
				yield return obj2;
			}
			IEnumerator finalizePhase = this.FinalizePhase();
			while (finalizePhase.MoveNext())
			{
				object obj3 = finalizePhase.Current;
				yield return obj3;
			}
			this.SetFinalState();
			yield break;
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x00033E07 File Offset: 0x00032007
		private IEnumerator ChaosPhase()
		{
			float duration = 0.8f;
			float elapsed = 0f;
			while (elapsed < duration && this.targetText != null)
			{
				elapsed += Time.deltaTime;
				try
				{
					StringBuilder randomText = new StringBuilder();
					int num;
					for (int i = 0; i < "   KERNELLVRC LITE".Length; i = num + 1)
					{
						randomText.Append(this.GetRandomGlitchChar());
						num = i;
					}
					this.targetText.text = randomText.ToString();
					this.ApplyRandomColorEffect();
					randomText = null;
				}
				catch (Exception ex)
				{
					Exception e = ex;
					Debug.LogError("[KernellTextGlitch] Error in chaos phase: " + e.Message);
					yield break;
				}
				yield return new WaitForSeconds(Random.Range(0.02f, 0.08f));
			}
			yield break;
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x00033E16 File Offset: 0x00032016
		private IEnumerator RevealPhase()
		{
			float duration = 0.5f;
			float elapsed = 0f;
			bool[] revealed = new bool["   KERNELLVRC LITE".Length];
			while (elapsed < duration && this.targetText != null)
			{
				elapsed += Time.deltaTime;
				float progress = elapsed / duration;
				try
				{
					int targetRevealed = Mathf.FloorToInt(progress * (float)"   KERNELLVRC LITE".Length);
					int num;
					for (int i = 0; i < targetRevealed; i = num + 1)
					{
						bool flag = !revealed[i] && Random.Range(0f, 1f) < 0.3f;
						if (flag)
						{
							revealed[i] = true;
						}
						num = i;
					}
					StringBuilder buildingText = new StringBuilder();
					for (int j = 0; j < "   KERNELLVRC LITE".Length; j = num + 1)
					{
						bool flag2 = revealed[j];
						if (flag2)
						{
							buildingText.Append("   KERNELLVRC LITE"[j]);
						}
						else
						{
							buildingText.Append(this.GetRandomGlitchChar());
						}
						num = j;
					}
					this.targetText.text = buildingText.ToString();
					this.targetText.color = Color.Lerp(Color.white, this.glitchColor, progress);
					buildingText = null;
				}
				catch (Exception ex)
				{
					Exception e = ex;
					Debug.LogError("[KernellTextGlitch] Error in reveal phase: " + e.Message);
					yield break;
				}
				yield return new WaitForSeconds(0.03f);
			}
			yield break;
		}

		// Token: 0x06000857 RID: 2135 RVA: 0x00033E25 File Offset: 0x00032025
		private IEnumerator FinalizePhase()
		{
			float duration = 0.2f;
			float elapsed = 0f;
			while (elapsed < duration && this.targetText != null)
			{
				elapsed += Time.deltaTime;
				float progress = elapsed / duration;
				try
				{
					bool flag = Random.Range(0f, 1f) < 0.7f;
					if (flag)
					{
						this.targetText.text = "   KERNELLVRC LITE";
					}
					else
					{
						StringBuilder almostFinal = new StringBuilder("   KERNELLVRC LITE");
						int glitchCount = Random.Range(0, 3);
						int num;
						for (int i = 0; i < glitchCount; i = num + 1)
						{
							int pos = Random.Range(0, almostFinal.Length);
							almostFinal[pos] = this.GetRandomGlitchChar();
							num = i;
						}
						this.targetText.text = almostFinal.ToString();
						almostFinal = null;
					}
					this.targetText.color = Color.Lerp(Color.white, this.glitchColor, progress);
				}
				catch (Exception ex)
				{
					Exception e = ex;
					Debug.LogError("[KernellTextGlitch] Error in finalize phase: " + e.Message);
					yield break;
				}
				yield return new WaitForSeconds(0.05f);
			}
			yield break;
		}

		// Token: 0x06000858 RID: 2136 RVA: 0x00033E34 File Offset: 0x00032034
		private void SetFinalState()
		{
			bool flag = this.targetText != null;
			if (flag)
			{
				try
				{
					this.targetText.text = "   KERNELLVRC LITE";
					this.targetText.color = this.glitchColor;
				}
				catch (Exception ex)
				{
					Debug.LogError("[KernellTextGlitch] Error setting final state: " + ex.Message);
				}
			}
		}

		// Token: 0x06000859 RID: 2137 RVA: 0x00033EAC File Offset: 0x000320AC
		private void ApplyRandomColorEffect()
		{
			bool flag = this.targetText == null;
			if (!flag)
			{
				float num = Random.Range(0f, 1f);
				bool flag2 = num < 0.3f;
				if (flag2)
				{
					this.targetText.color = Color.white;
				}
				else
				{
					bool flag3 = num < 0.5f;
					if (flag3)
					{
						this.targetText.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
					}
					else
					{
						bool flag4 = num < 0.7f;
						if (flag4)
						{
							this.targetText.color = Color.black;
						}
						else
						{
							this.targetText.color = new Color(Mathf.Clamp01(this.glitchColor.r + Random.Range(-0.2f, 0.2f)), Mathf.Clamp01(this.glitchColor.g + Random.Range(-0.2f, 0.2f)), Mathf.Clamp01(this.glitchColor.b + Random.Range(-0.2f, 0.2f)));
						}
					}
				}
			}
		}

		// Token: 0x0600085A RID: 2138 RVA: 0x00033FEC File Offset: 0x000321EC
		private char GetRandomGlitchChar()
		{
			char result;
			try
			{
				result = this.glitchChars[Random.Range(0, this.glitchChars.Length)][0];
			}
			catch
			{
				result = '?';
			}
			return result;
		}

		// Token: 0x0600085B RID: 2139 RVA: 0x00034034 File Offset: 0x00032234
		private void OnDestroy()
		{
			this.StopGlitchEffect();
			base.StopAllCoroutines();
			this.isApplying = false;
		}

		// Token: 0x04000404 RID: 1028
		private const string TARGET_TEXT = "   KERNELLVRC LITE";

		// Token: 0x04000405 RID: 1029
		private const string TITLE_PATH = "UserInterface/Canvas_QuickMenu(Clone)/CanvasGroup/Container/Window/QMParent/Menu_Dashboard/Header_H1/LeftItemContainer/Text_Title";

		// Token: 0x04000406 RID: 1030
		private readonly Color glitchColor = new Color(0.4f, 0f, 0.6845f);

		// Token: 0x04000407 RID: 1031
		private readonly string[] glitchChars = new string[]
		{
			"!",
			"@",
			"#",
			"$",
			"%",
			"^",
			"&",
			"*",
			"?",
			"~",
			"±",
			"§",
			"¿",
			"¡",
			"€",
			"£",
			"¥",
			"₹",
			"◊",
			"†",
			"‡",
			"∞",
			"√",
			"π",
			"Ω",
			"∂",
			"∆",
			"≈",
			"≠",
			"≤",
			"≥",
			"∫",
			"∑",
			"∏",
			"µ",
			"∂",
			"∇",
			"⊗",
			"⊕",
			"⊙",
			"◈",
			"◉",
			"●",
			"▪",
			"▫",
			"■",
			"□",
			"▲",
			"▼",
			"◆",
			"◇",
			"○",
			"●",
			"⬟",
			"⬡"
		};

		// Token: 0x04000408 RID: 1032
		private Coroutine currentGlitchCoroutine;

		// Token: 0x04000409 RID: 1033
		private TextMeshProUGUIEx targetText;

		// Token: 0x0400040A RID: 1034
		private static KernellTextGlitch instance;

		// Token: 0x0400040B RID: 1035
		private string originalText = "";

		// Token: 0x0400040C RID: 1036
		private bool isApplying = false;
	}
}
