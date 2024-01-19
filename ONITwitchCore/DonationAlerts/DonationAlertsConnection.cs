using System;
using System.Net;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace ONITwitch.DonationAlerts;

public class DonationAlertsConnection
{
    public event System.Action OnReady;

    private DonationAlertsContainer _container;

    [CanBeNull]
    public DonationAlertsContainer Container => _container;
    
    public bool IsConnecting { get; private set; }

    public string AccessToken { get; private set; }

    public event Action<DonationExportDto> OnDonation;

    public void Start()
    {
        if (!string.IsNullOrEmpty(AccessToken))
        {
            MessageBox.Show("Already connected");
            return;
        }
        Task.Run(
                async () =>
                {
                    // loop up to 5 times if the connection gets dropped/disconnected
                    const int maxAttempts = 5;
                    IsConnecting = true;

                    MessageBox.Show("Connecting to DonationAlerts");

                    // big try catch so that things get logged always, instead of silently ignored
                    try
                    {
                        WebClient webClient = new();
                        var response = await webClient.DownloadStringTaskAsync("http://localhost:27629/init");
                        
                        AccessToken = response;
                        
                        MessageBox.Show("DonationAlerts got access_token");

                        IsConnecting = false;
                        try
                        {
                            _container = new DonationAlertsContainer();
                            _container.OnDonation += OnDonation;
                            Task.Run(async () => await _container.Start());
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"DonationAlerts unestablished {ex}");
                            throw;
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"DonationAlerts failed.\n{ex}");
                        throw;
                    }
                }
            );
    }
}