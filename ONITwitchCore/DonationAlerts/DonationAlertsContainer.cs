using System;
using System.Net;
using System.Threading.Tasks;
using ONITwitchLib.Logger;

namespace ONITwitch.DonationAlerts;

public class DonationAlertsContainer
{
    public System.DateTime LastCheckedAt { get; private set; }

    public event Action<DonationExportDto> OnDonation;
    public event System.Action OnStopped;
    
    private bool shouldStop = false;

    public async Task Start()
    {
        WebClient webClient = new();
        var failsInARow = 0;
        var lastError = "";
        while (!shouldStop)
        {
            if (failsInARow >= 5)
            {
                MessageBox.Show($"5 раз получена ошибка при получении донатов.\nПроверьте подключение!\n{lastError}");
                failsInARow = 0;
                lastError = "";
            }
            try
            {
                var donation = await webClient.GetJsonAsync<DonationExportDto>("http://localhost:27629/events");
                failsInARow = 0;
                if (donation is not null)
                {
                    OnDonation?.Invoke(donation);
                }

                LastCheckedAt = System.DateTime.Now;
            }
            catch (Exception ex)
            {
                Log.Warn(ex.Message);
                lastError = ex.Message;
                failsInARow++;
            }
        }

        OnStopped?.Invoke();
    }

    public void Disconnect()
    {
        shouldStop = true;
    }
}