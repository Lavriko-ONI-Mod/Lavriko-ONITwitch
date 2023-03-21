using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using JetBrains.Annotations;
using UnityEngine;

namespace ONITwitchCore.Patches;

internal static class PauseMenuPatches
{
	private static readonly KButtonMenu.ButtonInfo TwitchButtonInfo = new(
		STRINGS.ONITWITCH.UI.PAUSE_MENU.START_VOTES,
		Action.NumActions,
		OnTwitchButtonPressed
	);

	private static ColorStyleSetting twitchButtonStyle;

	private static void OnTwitchButtonPressed()
	{
		TwitchButtonInfo.isEnabled = false;
		PauseScreen.Instance.RefreshButtons();

		var controller = Game.Instance.gameObject.AddOrGet<VoteController>();
		GameScheduler.Instance.ScheduleNextFrame(
			"TwitchStartVotes",
			_ =>
			{
				var started = controller.StartVote();
				TwitchButtonInfo.isEnabled = !started;
				PauseScreen.Instance.RefreshButtons();
			}
		);
	}

	[HarmonyPatch(typeof(PauseScreen), "OnPrefabInit")]
	// ReSharper disable once InconsistentNaming
	private static class PauseScreen_OnPrefabInit_Patch
	{
		[UsedImplicitly]
		// ReSharper disable once InconsistentNaming
		// buttons is an array cast to IList in the PauseScreen
		// need to copy to a List and resize and reassign
		private static void Postfix(ref IList<KButtonMenu.ButtonInfo> ___buttons)
		{
			var buttons = ___buttons.ToList();
			TwitchButtonInfo.isEnabled = true;
			buttons.Insert(4, TwitchButtonInfo);
			___buttons = buttons;
		}
	}

	private static readonly Color DisabledColor = new Color32(0x6A, 0x69, 0x66, 0xFF);
	private static readonly Color InactiveTwitchColor = new Color32(0x91, 0x46, 0xFF, 0xFF);
	private static readonly Color HoverTwitchColor = new Color32(0xA2, 0x56, 0xFF, 0xFF);
	private static readonly Color PressedTwitchColor = new Color32(0xB5, 0x67, 0xFF, 0xFF);

	[HarmonyPatch(typeof(KButtonMenu), nameof(KButtonMenu.RefreshButtons))]
	// ReSharper disable once InconsistentNaming
	private static class PauseScreen_RefreshButtons_Patch
	{
		[UsedImplicitly]
		// ReSharper disable once InconsistentNaming
		private static void Postfix(KButtonMenu __instance)
		{
			if (__instance is PauseScreen && (TwitchButtonInfo.uibutton != null))
			{
				if ((twitchButtonStyle == null) || (TwitchButtonInfo.uibutton.bgImage.colorStyleSetting == null) ||
					(TwitchButtonInfo.uibutton.bgImage.colorStyleSetting != twitchButtonStyle))
				{
					twitchButtonStyle = ScriptableObject.CreateInstance<ColorStyleSetting>();
					twitchButtonStyle.disabledColor = DisabledColor;
					twitchButtonStyle.inactiveColor = InactiveTwitchColor;
					twitchButtonStyle.hoverColor = HoverTwitchColor;
					twitchButtonStyle.activeColor = PressedTwitchColor;

					TwitchButtonInfo.uibutton.bgImage.colorStyleSetting = twitchButtonStyle;
				}
			}
		}
	}
}
