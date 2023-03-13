using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using JetBrains.Annotations;
using ONITwitchLib.Logger;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ONITwitchCore.Patches;

internal static class DevToolPatches
{
	[HarmonyPatch(typeof(SelectTool), nameof(SelectTool.OnLeftClickDown))]
	private static class SelectTool_OnLeftClickDown_Patch
	{
		[UsedImplicitly]
		private static void Postfix(SelectTool __instance)
		{
			if (TwitchDevTool.Instance != null)
			{
				TwitchDevTool.Instance.SelectedCell(__instance.GetSelectedCell());
			}
		}
	}

	[HarmonyPatch(typeof(DevToolManager), nameof(DevToolManager.UpdateShouldShowTools))]
	private class DevToolKeybindFix
	{
		[UsedImplicitly]
		private static void Postfix(ref bool ___toggleKeyWasDown, ref bool ___showImGui)
		{
			var flag = Input.GetKeyDown(KeyCode.Hash) && Input.GetKey(KeyCode.LeftControl);
			if (!___toggleKeyWasDown & flag)
				___showImGui = !___showImGui;
			___toggleKeyWasDown = flag;
		}
	}

	[HarmonyPatch(typeof(DevToolUI), nameof(DevToolUI.PingHoveredObject))]
	private static class DevToolNoPingCrashFix
	{
		private static readonly MethodInfo privatePingMethod = AccessTools.Method(typeof(DevToolUI), "private_Ping");

		[UsedImplicitly]
		private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> orig, ILGenerator generator)
		{
			var codes = orig.ToList();
			var pingCallIdx = codes.FindLastIndex(ci => ci.Calls(privatePingMethod));
			if (pingCallIdx == -1)
			{
				Log.Warn("Unable to find private_Ping call to fix dev ui ping crash");
				return codes;
			}

			var idx = pingCallIdx - 2;
			codes.Insert(idx++, new CodeInstruction(OpCodes.Dup));
			codes.Insert(idx++, CodeInstruction.Call(typeof(DevToolNoPingCrashFix), "LenHelper"));
			var normalLabel = generator.DefineLabel();
			codes.Insert(idx++, new CodeInstruction(OpCodes.Brtrue, normalLabel));
			codes.Insert(idx++, new CodeInstruction(OpCodes.Pop)); // pop extra list
			// close the dev tool UI
			codes.Insert(idx++, CodeInstruction.Call(typeof(DevTool), "ClosePanel"));
			codes.Insert(idx++, new CodeInstruction(OpCodes.Ret));
			codes.Insert(idx++, new CodeInstruction(OpCodes.Nop).WithLabels(normalLabel));

			return codes;
		}

		[UsedImplicitly]
		private static bool LenHelper(IReadOnlyCollection<RaycastResult> results)
		{
			return results.Count > 0;
		}
	}
}
