using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ONITwitch.Content.Cmps;
using ONITwitchLib.Logger;
using ONITwitchLib.Utils;
using STRINGS;

namespace ONITwitch.DonationAlerts;

public class DonationAlertsConnection
{
    public event System.Action OnReady;
    public bool IsConnecting { get; private set; }

    public string AccessToken { get; private set; }

    public void Start()
    {
        Task.Run(
            async () =>
            {
                // loop up to 5 times if the connection gets dropped/disconnected
                const int maxAttempts = 5;
                IsConnecting = true;

                // big try catch so that things get logged always, instead of silently ignored
                try
                {
                    var client = new WebClient();

                    AccessToken = await client.DownloadStringTaskAsync(new Uri("http://localhost:27629/init"));

                    IsConnecting = false;

                    MainThreadScheduler.Schedule(
                        () =>
                        {
                            Log.Warn("An error occurred");
                            DialogUtil.MakeDialog(
                                STRINGS.ONITWITCH.UI.DIALOGS.CONNECTION_ERROR.TITLE,
                                $"Authorized Donation Alerts",
                                UI.CONFIRMDIALOG.OK,
                                null
                            );
                        }
                    );
                }
                catch (WebException we)
                {
                    MainThreadScheduler.Schedule(
                        () =>
                        {
                            Log.Warn("An error occurred");
                            DialogUtil.MakeDialog(
                                STRINGS.ONITWITCH.UI.DIALOGS.CONNECTION_ERROR.TITLE,
                                $"Failed to authorize donation alerts.\nEnsure DonAlertInt.exe is Running",
                                UI.CONFIRMDIALOG.OK,
                                null
                            );
                        }
                    );
                }
                catch (ThreadAbortException)
                {
                    Log.Warn("Thread aborted");
                    throw;
                }
                catch (Exception e)
                {
                    // TODO: maybe UI warn here?
                    Log.Warn("An unexpected exception occurred");
                    Log.Debug(e.GetType());
                    Log.Warn(e.Message);
                    Log.Debug(e.StackTrace);
                    throw;
                }
            }
        );
    }
}