using System.IO;
using HarmonyLib;
using ImGuiNET;
using JetBrains.Annotations;
using ONITwitch.DonationAlerts;
using ONITwitchLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ONITwitch.Patches;

public class DonationAlertsPatches
{
    [HarmonyPatch(typeof(MainMenu), "OnSpawn")]
    // ReSharper disable once InconsistentNaming
    private static class MainMenu_OnSpawn_Patch
    {
        [UsedImplicitly]
        private static void Postfix()
        {
            // This code sets up the vote controller
            // Since the vote controller is static, only run this once, before it's set up
            if (DonationAlertsController.Instance == null)
            {
                // Run this even if an error was set, because the credentials were set to an anonymous login
                var donationAlertsController = new GameObject("DonationAlertsController");
                donationAlertsController.AddComponent<DonationAlertsController>();
                Object.DontDestroyOnLoad(donationAlertsController);
            }
        }
    }
}