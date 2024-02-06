using System;
using System.Collections.Generic;
using System.Linq;
using KSerialization;
using ONITwitch.Commands;
using ONITwitch.Content;
using ONITwitch.Content.EntityConfigs;
using ONITwitch.DevTools.Panels;
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
        connection.OnStopped += OnStopped;
    }

    private void OnStopped()
    {
        connection.OnStopped -= OnStopped;
        connection.OnDonation -= OnDonation;
        connection = new DonationAlertsConnection();
        connection.OnDonation += OnDonation;
        connection.OnStopped += OnStopped;
    }

    private void OnDonation(DonationExportDto data)
    {
        CommandBase command = null;
        object eventData = null;

        if (data.AmountInMyCurrency.IsCloseTo(150))
        {
            command = new SpawnDupeCommand();
            eventData = new Dictionary<string, object>()
            {
                ["name"] = data.Sender ?? "Донатный дубль",
                ["isDonatingStandart"] = true
            };
        }
        else if (data.AmountInMyCurrency.IsCloseTo(500))
        {
            command = new SpawnDupeCommand();
            eventData = new Dictionary<string, object>()
            {
                ["name"] = data.Sender ?? "Донатный дубль",
                ["isDonatingImmortal"] = true
            };
        }
        else if (data.AmountInMyCurrency.IsCloseTo(1000))
        {
            command = new SpawnDupeCommand();
            eventData = new Dictionary<string, object>()
            {
                ["name"] = data.Sender ?? "Донатный дубль",
                ["isDonatingVIP"] = true
            };
        }
        else if (data.AmountInMyCurrency.IsCloseTo(95))
        {
            command = new BansheeWailCommand();
            eventData = null;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(96))
        {
            command = new EclipseCommand();
            eventData = 3d * Constants.SECONDS_PER_CYCLE;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(97))
        {
            command = new FartCommand();
            eventData = 25.0d;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(1001))
        {
            command = new GlobalWarmingCommand();
            eventData = null;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(98))
        {
            command = new PeeCommand();
            eventData = null;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(1002))
        {
            command = new IceAgeCommand();
            eventData = null;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(666))
        {
            command = new KillDupeCommand();
            eventData = null;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(249))
        {
            command = new MorphCommand();
            eventData = null;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(499))
        {
            command = new PartyTimeCommand();
            eventData = 300.0d;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(1003))
        {
            command = new PoisonDupesCommand();
            eventData = null;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(99))
        {
            command = new PoopsplosionCommand();
            eventData = null;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(101))
        {
            command = new ResearchTechCommand();
            eventData = null;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(94))
        {
            command = new SkillCommand();
            eventData = 0.33d;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(498))
        {
            command = new SleepyDupesCommand();
            eventData = null;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(102))
        {
            command = new SpawnPrefabCommand();
            eventData = AtmoSuitConfig.ID;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(103))
        {
            command = new SpawnPrefabCommand();
            eventData = GassyMooCometConfig.ID;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(104))
        {
            command = new SpawnPrefabCommand();
            eventData = GlitterPuftConfig.Id;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(251))
        {
            command = new PocketDimensionCommand();
            eventData = null;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(105))
        {
            command = new SpawnPrefabCommand();
            eventData = CrabConfig.ID;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(106))
        {
            command = new SnazzySuitCommand();
            eventData = null;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(107))
        {
            command = new SpawnPrefabCommand();
            eventData = GeneShufflerRechargeConfig.ID;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(108))
        {
            command = new SpiceFoodCommand();
            eventData = null;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(149))
        {
            command = new GeyserModificationCommand();
            eventData = null;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(501))
        {
            command = new ReduceOxygenCommand();
            eventData = 0.20d;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(151))
        {
            command = new UninsulateCommand();
            eventData = null;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(152))
        {
            command = new EffectCommand();
            eventData = CustomEffects.AthleticsDownEffect.Id;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(109))
        {
            command = new EffectCommand();
            eventData = CustomEffects.AthleticsUpEffect.Id;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(153))
        {
            command = new EffectCommand();
            eventData = CustomEffects.ConstructionDownEffect.Id;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(110))
        {
            command = new EffectCommand();
            eventData = CustomEffects.ConstructionUpEffect.Id;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(154))
        {
            command = new EffectCommand();
            eventData = CustomEffects.ExcavationDownEffect.Id;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(111))
        {
            command = new EffectCommand();
            eventData = CustomEffects.ExcavationDownEffect.Id;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(155))
        {
            command = new EffectCommand();
            eventData = CustomEffects.StrengthDownEffect.Id;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(112))
        {
            command = new EffectCommand();
            eventData = CustomEffects.StrengthUpEffect.Id;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(113))
        {
            command = new FillBedroomCommand();
            eventData = SimHashes.SlimeMold.ToString();
        }
        else if (data.AmountInMyCurrency.IsCloseTo(114))
        {
            command = new FillBedroomCommand();
            eventData = SimHashes.Snow.ToString();
        }
        else if (data.AmountInMyCurrency.IsCloseTo(502))
        {
            command = new ElementFloodCommand();
            eventData = SimHashes.Ethanol.ToString();
        }
        else if (data.AmountInMyCurrency.IsCloseTo(1004))
        {
            command = new ElementFloodCommand();
            eventData = SimHashes.MoltenGold.ToString();
        }
        else if (data.AmountInMyCurrency.IsCloseTo(1005))
        {
            command = new ElementFloodCommand();
            eventData = SimHashes.Magma.ToString();
        }
        else if (data.AmountInMyCurrency.IsCloseTo(1006))
        {
            command = new ElementFloodCommand();
            eventData = SimHashes.NuclearWaste.ToString();
        }
        else if (data.AmountInMyCurrency.IsCloseTo(749))
        {
            command = new ElementFloodCommand();
            eventData = SimHashes.CrudeOil.ToString();
        }
        else if (data.AmountInMyCurrency.IsCloseTo(503))
        {
            command = new ElementFloodCommand();
            eventData = SimHashes.DirtyWater.ToString();
        }
        else if (data.AmountInMyCurrency.IsCloseTo(504))
        {
            command = new ElementFloodCommand();
            eventData = SimHashes.Water.ToString();
        }
        else if (data.AmountInMyCurrency.IsCloseTo(115))
        {
            command = new RainPrefabCommand();
            eventData = new Dictionary<string, object>
            {
                ["PrefabId"] = BeeConfig.ID,
                ["Count"] = 10.0d
            };
        }
        else if (data.AmountInMyCurrency.IsCloseTo(116))
        {
            command = new RainPrefabCommand();
            eventData = new Dictionary<string, object>
            {
                ["PrefabId"] = SimHashes.Diamond.ToString(),
                ["Count"] = 100.0d
            };
        }
        else if (data.AmountInMyCurrency.IsCloseTo(117))
        {
            command = new RainPrefabCommand();
            eventData = new Dictionary<string, object>
            {
                ["PrefabId"] = SimHashes.Gold.ToString(),
                ["Count"] = 50.0d
            };
        }
        else if (data.AmountInMyCurrency.IsCloseTo(118))
        {
            command = new RainPrefabCommand();
            eventData = new Dictionary<string, object>
            {
                ["PrefabId"] = GlomConfig.ID,
                ["Count"] = 10.0d
            };
        }
        else if (data.AmountInMyCurrency.IsCloseTo(119))
        {
            command = new RainPrefabCommand();
            eventData = new Dictionary<string, object>
            {
                ["PrefabId"] = PacuConfig.ID,
                ["Count"] = 10.0d
            };
        }
        else if (data.AmountInMyCurrency.IsCloseTo(120))
        {
            command = new RainPrefabCommand();
            eventData = new Dictionary<string, object>
            {
                ["PrefabId"] = OilFloaterConfig.ID,
                ["Count"] = 10.0d
            };
        }
        else if (data.AmountInMyCurrency.IsCloseTo(49))
        {
            command = new SpawnElementPoolCommand();
            eventData = new Dictionary<string, float>
            {
                [SimHashes.Algae.ToString()] = 1400,
                [SimHashes.OxyRock.ToString()] = 1400,
                [SimHashes.SlimeMold.ToString()] = 1400,
                [SimHashes.IgneousRock.ToString()] = 1400,
                [SimHashes.Rust.ToString()] = 1400,
                [SimHashes.Sand.ToString()] = 1400,
                [SimHashes.Ice.ToString()] = 1400,
                [SimHashes.Carbon.ToString()] = 1400,
                [SimHashes.Dirt.ToString()] = 1400,
                [SimHashes.Salt.ToString()] = 1400,
                [SimHashes.Sucrose.ToString()] = 1400,
            };
        }
        else if (data.AmountInMyCurrency.IsCloseTo(505))
        {
            command = new SpawnElementPoolCommand();
            eventData = new Dictionary<string, float>
            {
                // All of these are one tile surrounded by insulation
                [SimHashes.Magma.ToString()] = 4000,
                [SimHashes.MoltenIron.ToString()] = 4000,
                // really bad SHC
                [SimHashes.MoltenTungsten.ToString()] = 5000,
                // Aluminium and Rock Gas have decent SHC (~1), really hot
                [SimHashes.AluminumGas.ToString()] = 2000,
                [SimHashes.RockGas.ToString()] = 2000,

                // huge SHC, but only 850K. Condenses at 710K. There's a mechanism to make sure
                // that nothing spawns less than 300 degrees below its melting point if it's above 200C though.
                // So the final temp will be 1010K and really high SHC.
                [SimHashes.SuperCoolantGas.ToString()] = 1000,
            };
        }
        else if (data.AmountInMyCurrency.IsCloseTo(252))
        {
            command = new SpawnElementPoolCommand();
            eventData = new Dictionary<string, float>
            {
                [SimHashes.Diamond.ToString()] = 1000,
                [SimHashes.Ceramic.ToString()] = 2000,
                [SimHashes.Fossil.ToString()] = 1400,
                [SimHashes.Graphite.ToString()] = 1000,
                [SimHashes.EnrichedUranium.ToString()] = 2000,
                [SimHashes.Niobium.ToString()] = 500,
                [SimHashes.Tungsten.ToString()] = 1200,
                // (20/2)*5 = 50 = 5 tiles in a cooling loop
                [SimHashes.SuperCoolant.ToString()] = 20,
                // 5 pipes worth
                [SimHashes.SuperInsulator.ToString()] = 800,
                [SimHashes.Resin.ToString()] = 500,
            };
        }
        else if (data.AmountInMyCurrency.IsCloseTo(121))
        {
            command = new SpawnElementPoolCommand();
            eventData = new Dictionary<string, float>
            {
                // CO2, polluted O2, and O2 excluded, they're too boring
                [SimHashes.ChlorineGas.ToString()] = 10,
                [SimHashes.Hydrogen.ToString()] = 10,
                [SimHashes.Methane.ToString()] = 20,
                [SimHashes.SourGas.ToString()] = 10,
                [SimHashes.Steam.ToString()] = 10,
                [SimHashes.EthanolGas.ToString()] = 10,
            };
        }
        else if (data.AmountInMyCurrency.IsCloseTo(253))
        {
            command = new SpawnElementPoolCommand();
            eventData = new Dictionary<string, float>
            {
                // several waters are in common instead, they're boring
                // 100 tiles of gas once these heat up
                [SimHashes.Chlorine.ToString()] = 20,
                [SimHashes.LiquidHydrogen.ToString()] = 20,
                [SimHashes.LiquidOxygen.ToString()] = 20,


                // only one tile spawns of these
                [SimHashes.LiquidPhosphorus.ToString()] = 1500,
                [SimHashes.MoltenGlass.ToString()] = 2500,
                [SimHashes.MoltenSucrose.ToString()] = 1500,

                // Liquid sulfur is just barely under 200 degrees, but it doesn't have a ton of heat
                [SimHashes.LiquidSulfur.ToString()] = 500,

                [SimHashes.Naphtha.ToString()] = 500,
                [SimHashes.Petroleum.ToString()] = 500,
                [SimHashes.Ethanol.ToString()] = 500,
                [SimHashes.CrudeOil.ToString()] = 500,
            };
        }
        else if (data.AmountInMyCurrency.IsCloseTo(122))
        {
            command = new SpawnElementPoolCommand();
            eventData = new Dictionary<string, float>
            {
                [SimHashes.Cuprite.ToString()] = 1500,
                [SimHashes.FoolsGold.ToString()] = 1500,
                [SimHashes.IronOre.ToString()] = 1500,
                [SimHashes.Electrum.ToString()] = 1500,
                [SimHashes.Cobaltite.ToString()] = 1500,
                [SimHashes.GoldAmalgam.ToString()] = 1500,
                [SimHashes.AluminumOre.ToString()] = 1500,
            };
        }
        else if (data.AmountInMyCurrency.IsCloseTo(254))
        {
            command = new StressCommand();
            eventData = +0.75d;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(123))
        {
            command = new StressCommand();
            eventData = -0.75d;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(156))
        {
            command = new SurpriseBoxCommand();
            eventData = null;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(157))
        {
            command = new SurpriseCommand();
            eventData = null;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(506))
        {
            command = new TileTempCommand();
            eventData = -40.0d;
        }
        else if (data.AmountInMyCurrency.IsCloseTo(507))
        {
            command = new TileTempCommand();
            eventData = +40.0d;
        }

        if (command is not null)
        {
            GameScheduler.Instance.ScheduleNextFrame(
                "donation-alerts",
                (_) => { command.Run(eventData); }
            );
        }
        else
        {
            MessageBox.Show("Получен донат, но сумма не \nсоответствует какому-либо событию(");
        }
    }

    public void Connect()
    {
        connection.Start();
    }
    public void Disconnect()
    {
        connection?.Disconnect();
    }
}