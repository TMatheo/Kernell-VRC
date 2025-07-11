using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using KernellClientUI.UI.QuickMenu;
using KernellVRC;
using KernelVRC;
using TMPro;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace KernellVRCLite
{
	// Token: 0x0200007C RID: 124
	internal class Ghost : KernelModuleBase
	{
		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600058D RID: 1421 RVA: 0x00021A47 File Offset: 0x0001FC47
		public override ModuleCapabilities Capabilities
		{
			get
			{
				return ModuleCapabilities.Update | ModuleCapabilities.LateUpdate | ModuleCapabilities.GUI | ModuleCapabilities.NetworkEvents | ModuleCapabilities.UdonEvents | ModuleCapabilities.SceneEvents | ModuleCapabilities.UIInit;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600058E RID: 1422 RVA: 0x00021A4E File Offset: 0x0001FC4E
		public override string ModuleName
		{
			get
			{
				return "Ghost (serialization)";
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x0600058F RID: 1423 RVA: 0x00003304 File Offset: 0x00001504
		public override string Version
		{
			get
			{
				return "2.0.0";
			}
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x00021A58 File Offset: 0x0001FC58
		public override void OnUiManagerInit()
		{
			ReMenuCategory category = MenuSetup._uiManager.QMMenu.GetCategoryPage("Utility").GetCategory("Movement");
			category.AddToggle("Ghost Mode", "Walk around while a clone stands still.", new Action<bool>(this.ToggleGhost));
			category.AddButton("Reset Ghost", "Teleport to ghost clone and reset it.", new Action(this.ResetGhost), null, "#ffffff");
			category.AddButton("Update Ghost Position", "Update clone position to your current location.", new Action(this.UpdateGhostPosition), null, "#ffffff");
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x00021AE8 File Offset: 0x0001FCE8
		public override void OnLateUpdate()
		{
			bool flag = !this.IsUserTyping();
			if (flag)
			{
				string name = SceneManager.GetActiveScene().name;
				bool flag2 = name != "ui" && name != "Application2";
				if (flag2)
				{
					bool keyDown = Input.GetKeyDown(116);
					if (keyDown)
					{
						kernelllogger.Msg("T key pressed, toggling ghost mode from " + this.ghostMode.ToString() + " to " + (!this.ghostMode).ToString());
						this.ToggleGhost(!this.ghostMode);
					}
				}
			}
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x00021B88 File Offset: 0x0001FD88
		private bool IsUserTyping()
		{
			bool flag = EventSystem.current != null && EventSystem.current.currentSelectedGameObject != null;
			if (flag)
			{
				bool flag2 = EventSystem.current.currentSelectedGameObject.GetComponent<InputField>() != null;
				if (flag2)
				{
					return true;
				}
				bool flag3 = EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() != null;
				if (flag3)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x00021BFC File Offset: 0x0001FDFC
		public override bool OnEventSent(byte code, object data, RaiseEventOptions options, SendOptions sendOptions)
		{
			bool flag = this.ghostMode && code == 12;
			return !flag;
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x00021C28 File Offset: 0x0001FE28
		private void ToggleGhost(bool enable)
		{
			kernelllogger.Msg("ToggleGhost called with enable=" + enable.ToString() + ", current state=" + this.ghostMode.ToString());
			bool flag = this.ghostMode == enable;
			if (!flag)
			{
				this.ghostMode = enable;
				VRCPlayer field_Internal_Static_VRCPlayer_ = VRCPlayer.field_Internal_Static_VRCPlayer_0;
				bool flag2 = field_Internal_Static_VRCPlayer_ == null;
				if (flag2)
				{
					kernelllogger.Warning("Local player not found.");
				}
				else
				{
					bool flag3 = enable;
					if (flag3)
					{
						kernelllogger.Msg("Enabling Ghost Mode");
						this.CleanupExistingClones();
						this.origGhostPos = field_Internal_Static_VRCPlayer_.transform.position + new Vector3(0f, 0.1f, 0f);
						this.origGhostRot = field_Internal_Static_VRCPlayer_.transform.rotation;
						kernelllogger.Msg(string.Format("Saved position: {0}, rotation: {1}", this.origGhostPos, this.origGhostRot.eulerAngles));
						Transform transform = field_Internal_Static_VRCPlayer_.transform.Find("ForwardDirection/Avatar");
						bool flag4 = transform == null;
						if (flag4)
						{
							kernelllogger.Warning("Avatar GameObject not found under ForwardDirection.");
						}
						else
						{
							Animator component = transform.GetComponent<Animator>();
							bool flag5 = component == null;
							if (flag5)
							{
								kernelllogger.Warning("Animator not found on avatar object.");
							}
							else
							{
								GameObject gameObject = Object.Instantiate<GameObject>(component.gameObject, this.origGhostPos, this.origGhostRot);
								gameObject.name = "Ghost Clone";
								kernelllogger.Msg(string.Format("Created clone at position: {0}, rotation: {1}", gameObject.transform.position, gameObject.transform.rotation.eulerAngles));
								Animator source = component;
								Animator component2 = gameObject.GetComponent<Animator>();
								bool flag6 = component2 != null;
								if (flag6)
								{
									kernelllogger.Msg("Copying animator parameters and state...");
									this.CopyAnimatorParameters(source, component2);
									component2.Update(0f);
									component2.enabled = false;
									bool isHuman = component2.isHuman;
									if (isHuman)
									{
										Transform boneTransform = component2.GetBoneTransform(10);
										bool flag7 = boneTransform != null;
										if (flag7)
										{
											boneTransform.localScale = Vector3.one;
										}
									}
								}
								this.clonedAvatar.Add(gameObject);
								ToastNotif.Toast("Ghost Enabled", "You are now hidden. A clone is standing still.", null, 5f);
							}
						}
					}
					else
					{
						kernelllogger.Msg("Disabling Ghost Mode");
						this.CleanupExistingClones();
						ToastNotif.Toast("Ghost Disabled", "You are now visible again.", null, 5f);
					}
				}
			}
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x00021EA4 File Offset: 0x000200A4
		private void CleanupExistingClones()
		{
			foreach (GameObject gameObject in this.clonedAvatar)
			{
				bool flag = gameObject != null;
				if (flag)
				{
					kernelllogger.Msg("Destroying clone: " + gameObject.name);
					Object.Destroy(gameObject);
				}
			}
			this.clonedAvatar.Clear();
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x00021F2C File Offset: 0x0002012C
		private void CopyAnimatorParameters(Animator source, Animator destination)
		{
			try
			{
				bool flag = source == null || destination == null;
				if (flag)
				{
					kernelllogger.Warning("Cannot copy parameters: source or destination animator is null");
				}
				else
				{
					Il2CppReferenceArray<AnimatorControllerParameter> parameters = source.parameters;
					foreach (AnimatorControllerParameter animatorControllerParameter in parameters)
					{
						try
						{
							AnimatorControllerParameterType type = animatorControllerParameter.type;
							AnimatorControllerParameterType animatorControllerParameterType = type;
							switch (animatorControllerParameterType)
							{
							case 1:
							{
								float @float = source.GetFloat(animatorControllerParameter.name);
								destination.SetFloat(animatorControllerParameter.name, @float);
								kernelllogger.Msg(string.Format("Copied float parameter: {0} = {1}", animatorControllerParameter.name, @float));
								break;
							}
							case 2:
								break;
							case 3:
							{
								int integer = source.GetInteger(animatorControllerParameter.name);
								destination.SetInteger(animatorControllerParameter.name, integer);
								kernelllogger.Msg(string.Format("Copied int parameter: {0} = {1}", animatorControllerParameter.name, integer));
								break;
							}
							case 4:
							{
								bool @bool = source.GetBool(animatorControllerParameter.name);
								destination.SetBool(animatorControllerParameter.name, @bool);
								kernelllogger.Msg(string.Format("Copied bool parameter: {0} = {1}", animatorControllerParameter.name, @bool));
								break;
							}
							default:
								if (animatorControllerParameterType == 9)
								{
									kernelllogger.Msg("Skipping trigger parameter: " + animatorControllerParameter.name);
								}
								break;
							}
						}
						catch (Exception ex)
						{
							kernelllogger.Warning("Error copying parameter " + animatorControllerParameter.name + ": " + ex.Message);
						}
					}
					int num = 0;
					while (num < source.layerCount && num < destination.layerCount)
					{
						float layerWeight = source.GetLayerWeight(num);
						destination.SetLayerWeight(num, layerWeight);
						kernelllogger.Msg(string.Format("Copied layer {0} weight: {1}", num, layerWeight));
						num++;
					}
					destination.speed = source.speed;
					kernelllogger.Msg(string.Format("Copied animator speed: {0}", source.speed));
					int num2 = 0;
					while (num2 < source.layerCount && num2 < destination.layerCount)
					{
						AnimatorStateInfo currentAnimatorStateInfo = source.GetCurrentAnimatorStateInfo(num2);
						destination.Play(currentAnimatorStateInfo.fullPathHash, num2, currentAnimatorStateInfo.normalizedTime);
						kernelllogger.Msg(string.Format("Copied state for layer {0}: hash={1}, time={2}", num2, currentAnimatorStateInfo.fullPathHash, currentAnimatorStateInfo.normalizedTime));
						num2++;
					}
					destination.Update(0f);
					kernelllogger.Msg("Successfully copied animator state and parameters");
				}
			}
			catch (Exception ex2)
			{
				kernelllogger.Error("Error in CopyAnimatorParameters: " + ex2.Message + "\n" + ex2.StackTrace);
			}
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x00022258 File Offset: 0x00020458
		private void ResetGhost()
		{
			bool flag = !this.ghostMode || this.clonedAvatar.Count == 0 || this.clonedAvatar[0] == null;
			if (flag)
			{
				kernelllogger.Warning("Cannot reset: Ghost mode inactive or clone missing.");
			}
			else
			{
				VRCPlayer field_Internal_Static_VRCPlayer_ = VRCPlayer.field_Internal_Static_VRCPlayer_0;
				bool flag2 = field_Internal_Static_VRCPlayer_ == null;
				if (flag2)
				{
					kernelllogger.Warning("Local player not found.");
				}
				else
				{
					field_Internal_Static_VRCPlayer_.transform.position = this.clonedAvatar[0].transform.position;
					field_Internal_Static_VRCPlayer_.transform.rotation = this.clonedAvatar[0].transform.rotation;
					this.ToggleGhost(false);
					this.ToggleGhost(true);
					ToastNotif.Toast("Ghost Reset", "You have been moved to your ghost clone.", null, 5f);
				}
			}
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x00022330 File Offset: 0x00020530
		private void UpdateGhostPosition()
		{
			bool flag = !this.ghostMode;
			if (flag)
			{
				kernelllogger.Msg("Ghost mode is not active.");
			}
			else
			{
				this.ToggleGhost(false);
				this.ToggleGhost(true);
				ToastNotif.Toast("Ghost Updated", "Ghost clone position updated.", null, 5f);
			}
		}

		// Token: 0x0400027F RID: 639
		private bool ghostMode = false;

		// Token: 0x04000280 RID: 640
		private Vector3 origGhostPos;

		// Token: 0x04000281 RID: 641
		private Quaternion origGhostRot;

		// Token: 0x04000282 RID: 642
		private readonly List<GameObject> clonedAvatar = new List<GameObject>();

		// Token: 0x04000283 RID: 643
		private Coroutine toggleCheckCoroutine;
	}
}
