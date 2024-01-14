using ONITwitch.Content.Cmps;
using ONITwitch.Toasts;
using ONITwitchLib.Utils;

namespace ONITwitch.DonationAlerts;

public static class MessageBox
{
    public static void Show(string message, string title = "Message")
    {
        MainThreadScheduler.Schedule(
            () =>
            {
                ToastManager.InstantiateToast(
                    title,
                    message
                );
            }
        );
    }
}