using System;
using System.Collections.Generic;
using KSerialization;
using ONITwitch.Commands;
using ONITwitchLib;

namespace ONITwitch.DonationAlerts;

[SerializationConfig(MemberSerialization.OptIn)]
public class DonationAlertsController : KMonoBehaviour
{
    public static DonationAlertsController Instance;
    
    
    private DonationAlertsConnection connection;

    public bool Connected => !string.IsNullOrEmpty(connection.AccessToken);

    public bool IsConnecting => connection.IsConnecting;

    public DonationAlertsContainer Container => connection.Container;
    
    protected override void OnSpawn()
    {
        base.OnSpawn();

        Instance = this;
        
        connection = new DonationAlertsConnection();
        
        connection.OnDonation += OnDonation;
    }

    private void OnDonation(DonationExportDto data)
    {
        if (Math.Abs(data.AmountInMyCurrency - 111) < 0.001)
        {
            MessageBox.Show("Задоначено 111р. Спавним дубля!");
            GameScheduler.Instance.ScheduleNextFrame(
                "spawn dupe",
                (_) =>
                {
                    new SpawnDupeCommand().Run(null, data.Sender ?? "Донатный дубль");
                }
            );
        }
    }

    public void Connect()
    {
        connection.Start();
    }
}