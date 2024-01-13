using KSerialization;

namespace ONITwitch.DonationAlerts;

[SerializationConfig(MemberSerialization.OptIn)]
public class DonationAlertsController : KMonoBehaviour
{
    public static DonationAlertsController Instance;
    
    
    private DonationAlertsConnection connection;

    public bool Connected => !string.IsNullOrEmpty(connection.AccessToken);

    public bool IsConnecting => connection.IsConnecting;
    
    protected override void OnSpawn()
    {
        base.OnSpawn();

        Instance = this;
        
        connection = new DonationAlertsConnection();
        connection.OnReady += () =>
        {
            
        };
    }

    public void Connect()
    {
        connection.Start();
    }
}