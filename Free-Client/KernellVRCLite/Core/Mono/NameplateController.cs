using System;
using KernelVRC;

namespace KernellVRCLite.Core.Mono
{
	// Token: 0x02000088 RID: 136
	internal class NameplateController
	{
		// Token: 0x060006C8 RID: 1736 RVA: 0x0002A49C File Offset: 0x0002869C
		public NameplateController(NameplateState state, NameplateRenderer renderer)
		{
			this._state = state;
			this._renderer = renderer;
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x0002A4C4 File Offset: 0x000286C4
		public bool Initialize()
		{
			bool flag = CustomNameplate.IsGlobalShutdown() || this._isDisposed;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this._isInitialized = this._renderer.Initialize();
				result = this._isInitialized;
			}
			return result;
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x0002A508 File Offset: 0x00028708
		public void UpdateNameplate()
		{
			bool flag = !this._isInitialized || this._isDisposed || CustomNameplate.IsGlobalShutdown();
			if (!flag)
			{
				try
				{
					this._state.TrackNetworkUpdates();
					bool flag2 = !CustomNameplate.IsGlobalShutdown();
					if (flag2)
					{
						this._renderer.UpdateDisplay();
					}
				}
				catch (Exception ex)
				{
					bool flag3 = !CustomNameplate.IsGlobalShutdown();
					if (flag3)
					{
						kernelllogger.Error("[NameplateController] Update error: " + ex.Message);
					}
				}
			}
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x0002A598 File Offset: 0x00028798
		public void Dispose()
		{
			bool isDisposed = this._isDisposed;
			if (!isDisposed)
			{
				this._isDisposed = true;
				NameplateRenderer renderer = this._renderer;
				if (renderer != null)
				{
					renderer.Dispose();
				}
				this._isInitialized = false;
			}
		}

		// Token: 0x0400033F RID: 831
		private readonly NameplateState _state;

		// Token: 0x04000340 RID: 832
		private readonly NameplateRenderer _renderer;

		// Token: 0x04000341 RID: 833
		private bool _isInitialized = false;

		// Token: 0x04000342 RID: 834
		private bool _isDisposed = false;
	}
}
