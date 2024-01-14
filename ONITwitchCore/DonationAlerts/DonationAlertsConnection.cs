using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ONITwitch.DonationAlerts;

public class DonationAlertsConnection
{
    public event System.Action OnReady;
    public bool IsConnecting { get; private set; }

    public string AccessToken { get; private set; }

    public string SocketToken { get; private set; }

    private readonly HttpClient _httpClient = new();

    public void Start()
    {
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
                        var response = await _httpClient.GetAsync("http://localhost:27629/init");

                        response.EnsureSuccessStatusCode();

                        AccessToken = await response.Content.ReadAsStringAsync();
                        
                        MessageBox.Show("DonationAlerts got access_token");

                        IsConnecting = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"DonationAlerts failed.\n{ex}");
                        throw;
                    }
                }
            )
            .ContinueWith(
                async (_) =>
                {
                    MessageBox.Show("Subscribing to donation alerts");
                    try
                    {
                        var response = await _httpClient.GetAsync("http://localhost:27629/socket-token");

                        response.EnsureSuccessStatusCode();
                        
                        MessageBox.Show("DonationAlerts got access_token");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"DonationAlerts unestablished {ex}");
                        throw;
                    }
                }
            );
    }
}