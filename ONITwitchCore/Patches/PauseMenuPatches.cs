using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using JetBrains.Annotations;
using ONITwitch.DevTools;
using ONITwitch.Voting;
using UnityEngine;

namespace ONITwitch.Patches;

internal static class PauseMenuPatches {
    private static readonly KButtonMenu.ButtonInfo TwitchButtonInfo = new(
        STRINGS.ONITWITCH.UI.PAUSE_MENU.START_VOTES,
        Action.NumActions,
        OnTwitchButtonPressed
    );
	
	private static readonly KButtonMenu.ButtonInfo LavrikoButtonInfo = new(
		"Lavriko UI",
		Action.NumActions,
		OnLavrikoButtonPressed
	);

    private static ColorStyleSetting twitchButtonStyle;
	private static ColorStyleSetting lavrikoButtonStyle;

    private static readonly Color DisabledColor = new Color32(0x6A, 0x69, 0x66, 0xFF);
    private static readonly Color InactiveTwitchColor = new Color32(0x91, 0x46, 0xFF, 0xFF);
    private static readonly Color HoverTwitchColor = new Color32(0xA2, 0x56, 0xFF, 0xFF);
    private static readonly Color PressedTwitchColor = new Color32(0xB5, 0x67, 0xFF, 0xFF);

    private static readonly Color InactiveLavrikoColor = new Color32(0x92, 0xC6, 0xD7, 0xFF);
	private static readonly Color HoverLavrikoColor = new Color32(0xA2, 0xD6, 0xE7, 0xFF);
	private static readonly Color PressedLavrikoColor = new Color32(0xB2, 0xE6, 0xF7, 0xFF);

	private static void OnTwitchButtonPressed(){
        TwitchButtonInfo.isEnabled = false;
        PauseScreen.Instance.RefreshButtons();

        GameScheduler.Instance.ScheduleNextFrame(
            "TwitchStartVotes",
            static _ => {
                var started = VoteController.Instance.StartVote();
                TwitchButtonInfo.isEnabled = !started;
                PauseScreen.Instance.RefreshButtons();
            }
        );
    }

    private static void OnLavrikoButtonPressed()
	{
		PauseScreen.Instance.RefreshButtons();
		PauseScreen.Instance.Show(false);

		LavrikoPanel.Instance.Spawn();
	}

	[HarmonyPatch(typeof(PauseScreen), "ConfigureButtonInfos")]
    // ReSharper disable once InconsistentNaming
    private static class PauseScreen_OnPrefabInit_Patch {
        [UsedImplicitly]
        // ReSharper disable once InconsistentNaming
        // buttons is an array cast to IList in the PauseScreen
        // need to copy to a List and resize and reassign
        private static void Postfix(PauseScreen __instance,ref IList<KButtonMenu.ButtonInfo> ___buttons)
		{
			var buttons = ___buttons.ToList();
			TwitchButtonInfo.isEnabled = true;
			buttons.Insert(4, TwitchButtonInfo);
			buttons.Insert(5, LavrikoButtonInfo);
            __instance.SetButtons(buttons);
        }
    }

    [HarmonyPatch(typeof(KButtonMenu), nameof(KButtonMenu.RefreshButtons))]
    // ReSharper disable once InconsistentNaming
    private static class PauseScreen_RefreshButtons_Patch {
        
		[UsedImplicitly]
		// ReSharper disable once InconsistentNaming
		private static void Postfix(KButtonMenu __instance)
		{
			if (__instance is PauseScreen)
			{
				if (TwitchButtonInfo.uibutton != null)
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
				
				if (LavrikoButtonInfo.uibutton != null)
				{
					if ((lavrikoButtonStyle == null) || (LavrikoButtonInfo.uibutton.bgImage.colorStyleSetting == null) ||
					    (LavrikoButtonInfo.uibutton.bgImage.colorStyleSetting != lavrikoButtonStyle))
					{
						lavrikoButtonStyle = ScriptableObject.CreateInstance<ColorStyleSetting>();
						lavrikoButtonStyle.disabledColor = DisabledColor;
						lavrikoButtonStyle.inactiveColor = InactiveLavrikoColor;
						lavrikoButtonStyle.hoverColor = HoverLavrikoColor;
						lavrikoButtonStyle.activeColor = PressedLavrikoColor;

						var texts = LavrikoButtonInfo.uibutton.GetComponentsInChildren<LocText>();
						foreach (LocText locText in texts)
						{
							locText.color = new Color(0.426f, 0.224f, 0.157f);
						}

						LavrikoButtonInfo.uibutton.bgImage.colorStyleSetting = lavrikoButtonStyle;
					}
				}
			}
		}
	}
}
