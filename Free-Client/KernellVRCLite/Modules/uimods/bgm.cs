using System;
using KernelVRC;
using UnityEngine;

namespace KernellVRCLite.modules.uimods
{
	// Token: 0x020000A4 RID: 164
	public class bgm : KernelModuleBase
	{
		// Token: 0x1700018C RID: 396
		// (get) Token: 0x0600086E RID: 2158 RVA: 0x000347BC File Offset: 0x000329BC
		public override ModuleCapabilities Capabilities
		{
			get
			{
				return ModuleCapabilities.Update | ModuleCapabilities.LateUpdate | ModuleCapabilities.GUI | ModuleCapabilities.SceneEvents;
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x0600086F RID: 2159 RVA: 0x000347C3 File Offset: 0x000329C3
		public override UpdateFrequency UpdateFrequency
		{
			get
			{
				return UpdateFrequency.EveryFrame;
			}
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x000347C8 File Offset: 0x000329C8
		public override void OnUpdate()
		{
			bool flag = this.customClip == null;
			if (flag)
			{
				this.customClip = ClassicEmbeddedResourceLoader.LoadEmbeddedAudioClip("KernellVRCLite.assets.loading.wav");
			}
			this.CheckAndCorrectAudioSource("LoadingBackground_TealGradient_Music/LoadingSound");
			this.CheckAndCorrectAudioSource("LoadingPopup/LoadingSound");
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x00034810 File Offset: 0x00032A10
		private void CheckAndCorrectAudioSource(string gameObjectPath)
		{
			GameObject gameObject = GameObject.Find(gameObjectPath);
			AudioSource audioSource = (gameObject != null) ? gameObject.GetComponent<AudioSource>() : null;
			bool flag = audioSource != null && this.customClip != null;
			if (flag)
			{
				bool flag2 = audioSource.clip != this.customClip;
				if (flag2)
				{
					bool isPlaying = audioSource.isPlaying;
					float time = audioSource.time;
					audioSource.Stop();
					audioSource.clip = this.customClip;
					audioSource.volume = 1f;
					bool flag3 = isPlaying;
					if (flag3)
					{
						audioSource.time = time;
						audioSource.Play();
					}
				}
				bool flag4 = audioSource.volume != 1f;
				if (flag4)
				{
					audioSource.volume = 1f;
				}
			}
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x000348D8 File Offset: 0x00032AD8
		public override void OnSceneWasLoaded(int buildIndex, string sceneName)
		{
			bool flag = this.customClip == null;
			if (flag)
			{
				this.customClip = ClassicEmbeddedResourceLoader.LoadEmbeddedAudioClip("KernellVRCLite.assets.loading.wav");
			}
			this.ApplyCustomAudioToSource("LoadingBackground_TealGradient_Music/LoadingSound");
			this.ApplyCustomAudioToSource("LoadingPopup/LoadingSound");
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x00034920 File Offset: 0x00032B20
		private void ApplyCustomAudioToSource(string gameObjectPath)
		{
			GameObject gameObject = GameObject.Find(gameObjectPath);
			AudioSource audioSource = (gameObject != null) ? gameObject.GetComponent<AudioSource>() : null;
			bool flag = audioSource != null && this.customClip != null;
			if (flag)
			{
				audioSource.Stop();
				audioSource.clip = this.customClip;
				audioSource.volume = 1f;
				audioSource.Play();
			}
		}

		// Token: 0x04000412 RID: 1042
		private AudioClip customClip;

		// Token: 0x04000413 RID: 1043
		private const string CUSTOM_AUDIO_PATH = "KernellVRCLite.assets.loading.wav";
	}
}
