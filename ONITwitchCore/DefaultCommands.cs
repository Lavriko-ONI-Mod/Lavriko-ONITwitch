﻿using System.Collections.Generic;
using JetBrains.Annotations;
using ONITwitch;
using ONITwitchCore.Commands;
using ONITwitchCore.Config;
using ONITwitchCore.Content;
using ONITwitchCore.Content.Entities;
using ONITwitchLib;
using DataManager = EventLib.DataManager;
using EventGroup = EventLib.EventGroup;
using EventInfo = EventLib.EventInfo;
using EventManager = EventLib.EventManager;

namespace ONITwitchCore;

public static class DefaultCommands
{
	public static void SetupCommands()
	{
		Debug.Log("[Twitch Integration] Setting up default commands");
		RegisterCommand(
			new CommandInfo(
				"SpawnDupe",
				"Spawn Dupe",
				new SpawnDupeCommand(),
				new Dictionary<string, object> { { "MaxDupes", 30.0d } },
				Danger.Small,
				50
			)
		);
		RegisterCommand(
			new CommandInfo(
				"ElementGroupCommon",
				"Spawn Common Element",
				new SpawnElementPoolCommand(),
				new List<string>
				{
					SimHashes.Water.ToString(),
					SimHashes.Algae.ToString(),
					SimHashes.OxyRock.ToString(),
					SimHashes.SlimeMold.ToString(),
					SimHashes.Obsidian.ToString(),
					SimHashes.IgneousRock.ToString(),
					SimHashes.SandStone.ToString(),
					SimHashes.Rust.ToString(),
					SimHashes.Sand.ToString(),
					SimHashes.Snow.ToString(),
					SimHashes.Ice.ToString(),
					SimHashes.Carbon.ToString(),
					SimHashes.DirtyWater.ToString(),
					SimHashes.DirtyIce.ToString(),
					SimHashes.Brine.ToString(),
					SimHashes.BrineIce.ToString(),
					SimHashes.Dirt.ToString(),
					SimHashes.MaficRock.ToString(),
					SimHashes.Salt.ToString(),
					SimHashes.Sucrose.ToString(),
					SimHashes.Katairite.ToString(),
				},
				Danger.Small,
				100,
				"core.spawn_element"
			)
		);
		RegisterCommand(
			new CommandInfo(
				"ElementGroupExotic",
				"Spawn Exotic Element",
				new SpawnElementPoolCommand(),
				new List<string>
				{
					SimHashes.Diamond.ToString(),
					SimHashes.Ceramic.ToString(),
					SimHashes.Fossil.ToString(),
					SimHashes.Graphite.ToString(),
					SimHashes.EnrichedUranium.ToString(),
					SimHashes.Niobium.ToString(),
					SimHashes.Tungsten.ToString(),
					SimHashes.SuperCoolant.ToString(),
					SimHashes.SuperInsulator.ToString(),
					SimHashes.Resin.ToString(),
				},
				Danger.Small,
				10,
				"core.spawn_element"
			)
		);
		RegisterCommand(
			new CommandInfo(
				"ElementGroupMetal",
				"Spawn Metal",
				new SpawnElementPoolCommand(),
				new List<string>
				{
					SimHashes.Cuprite.ToString(),
					SimHashes.FoolsGold.ToString(),
					SimHashes.IronOre.ToString(),
					SimHashes.Electrum.ToString(),
					SimHashes.Cobaltite.ToString(),
					SimHashes.GoldAmalgam.ToString(),
					SimHashes.AluminumOre.ToString(),
				},
				Danger.Small,
				50,
				"core.spawn_element"
			)
		);
		RegisterCommand(
			new CommandInfo(
				"ElementGroupGas",
				"Spawn Gas",
				new SpawnElementPoolCommand(),
				new List<string>
				{
					// CO2, polluted O2, and O2 excluded because they are in common
					SimHashes.ChlorineGas.ToString(),
					SimHashes.Hydrogen.ToString(),
					SimHashes.Methane.ToString(),
					SimHashes.SourGas.ToString(),
					SimHashes.Steam.ToString(),
					SimHashes.EthanolGas.ToString(),
				},
				Danger.Small,
				50,
				"core.spawn_element"
			)
		);
		RegisterCommand(
			new CommandInfo(
				"ElementGroupLiquid",
				"Spawn Liquid",
				new SpawnElementPoolCommand(),
				new List<string>
				{
					// Brine, salt water, water, polluted water in common instead
					SimHashes.Chlorine.ToString(),
					SimHashes.CrudeOil.ToString(),
					SimHashes.LiquidCarbonDioxide.ToString(),
					SimHashes.LiquidHydrogen.ToString(),
					SimHashes.LiquidMethane.ToString(),
					SimHashes.LiquidOxygen.ToString(),
					SimHashes.LiquidPhosphorus.ToString(),
					SimHashes.LiquidSulfur.ToString(),
					SimHashes.MoltenGlass.ToString(),
					SimHashes.Naphtha.ToString(),
					SimHashes.Petroleum.ToString(),
					SimHashes.Ethanol.ToString(),
					SimHashes.MoltenSucrose.ToString(),
				},
				Danger.Small,
				50,
				"core.spawn_element"
			)
		);
		RegisterCommand(
			new CommandInfo(
				"ElementGroupDeadly",
				"Spawn Deadly Element",
				new SpawnElementPoolCommand(),
				new List<string>
				{
					SimHashes.Magma.ToString(),
					SimHashes.MoltenIron.ToString(),
					SimHashes.MoltenTungsten.ToString(),
					SimHashes.AluminumGas.ToString(),
					SimHashes.RockGas.ToString(),
					SimHashes.SuperCoolantGas.ToString(),
				},
				Danger.Small,
				10,
				"core.spawn_element"
			)
		);
		RegisterCommand(
			new CommandInfo(
				"AttributeAthleticsUp",
				"Athletics Up",
				new EffectCommand(),
				CustomEffects.AthleticsUpEffect.Id,
				Danger.None,
				20,
				"core.attribute_mod"
			)
		);
		RegisterCommand(
			new CommandInfo(
				"AttributeAthleticsDown",
				"Athletics Down",
				new EffectCommand(),
				CustomEffects.AthleticsDownEffect.Id,
				Danger.None,
				20,
				"core.attribute_mod"
			)
		);
		RegisterCommand(
			new CommandInfo(
				"AttributeConstructionUp",
				"Construction Up",
				new EffectCommand(),
				CustomEffects.ConstructionUpEffect.Id,
				Danger.None,
				20,
				"core.attribute_mod"
			)
		);
		RegisterCommand(
			new CommandInfo(
				"AttributeConstructionDown",
				"Construction Down",
				new EffectCommand(),
				CustomEffects.ConstructionDownEffect.Id,
				Danger.None,
				20,
				"core.attribute_mod"
			)
		);
		RegisterCommand(
			new CommandInfo(
				"AttributeExcavationUp",
				"Excavation Up",
				new EffectCommand(),
				CustomEffects.ExcavationUpEffect.Id,
				Danger.None,
				20,
				"core.attribute_mod"
			)
		);
		RegisterCommand(
			new CommandInfo(
				"AttributeExcavationDown",
				"Excavation Down",
				new EffectCommand(),
				CustomEffects.ExcavationDownEffect.Id,
				Danger.None,
				20,
				"core.attribute_mod"
			)
		);
		RegisterCommand(
			new CommandInfo(
				"AttributeStrengthUp",
				"Strength Up",
				new EffectCommand(),
				CustomEffects.StrengthUpEffect.Id,
				Danger.None,
				20,
				"core.attribute_mod"
			)
		);
		RegisterCommand(
			new CommandInfo(
				"AttributeStrengthDown",
				"Strength Down",
				new EffectCommand(),
				CustomEffects.StrengthDownEffect.Id,
				Danger.None,
				20,
				"core.attribute_mod"
			)
		);
		RegisterCommand(
			new CommandInfo("BansheeWail", "Banshee Wail", new BansheeWailCommand(), null, Danger.Small, 10)
		);
		RegisterCommand(
			new CommandInfo(
				"SnowBedrooms",
				"Snowy Bedrooms",
				new FillBedroomCommand(),
				SimHashes.Snow.ToString(),
				Danger.Small,
				20,
				"core.bedroom_element"
			)
		);
		RegisterCommand(
			new CommandInfo(
				"SlimeBedrooms",
				"Slimy Bedrooms",
				new FillBedroomCommand(),
				SimHashes.SlimeMold.ToString(),
				Danger.Small,
				20,
				"core.bedroom_element"
			)
		);
		RegisterCommand(
			new CommandInfo(
				"FloodWater",
				"Water Flood",
				new ElementFloodCommand(),
				SimHashes.Water.ToString(),
				Danger.Medium,
				30,
				"core.flood"
			)
		);
		RegisterCommand(
			new CommandInfo(
				"FloodPollutedWater",
				"Polluted Water Flood",
				new ElementFloodCommand(),
				SimHashes.DirtyWater.ToString(),
				Danger.Medium,
				30,
				"core.flood"
			)
		);
		RegisterCommand(
			new CommandInfo(
				"FloodEthanol",
				"Ethanol Flood",
				new ElementFloodCommand(),
				SimHashes.Ethanol.ToString(),
				Danger.Medium,
				10,
				"core.flood"
			)
		);
		RegisterCommand(
			new CommandInfo(
				"FloodOil",
				"Oil Flood",
				new ElementFloodCommand(),
				SimHashes.CrudeOil.ToString(),
				Danger.Medium,
				10,
				"core.flood"
			)
		);
		RegisterCommand(
			new CommandInfo(
				"FloodLava",
				"Lava Flood",
				new ElementFloodCommand(),
				SimHashes.Magma.ToString(),
				Danger.Deadly,
				2,
				"core.flood"
			)
		);
		RegisterCommand(
			new CommandInfo(
				"FloodGold",
				"Gold Rain",
				new ElementFloodCommand(),
				SimHashes.MoltenGold.ToString(),
				Danger.Deadly,
				2,
				"core.flood"
			)
		);
		RegisterCommand(
			new CommandInfo(
				"FloodNuclearWaste",
				"Nuclear Waste Disposal Accident",
				new ElementFloodCommand(),
				SimHashes.NuclearWaste.ToString(),
				Danger.High,
				2,
				"core.flood"
			)
		);
		RegisterCommand(
			new CommandInfo("IceAge", "Ice Age", new IceAgeCommand(), null, Danger.Extreme, 1)
		);
		RegisterCommand(
			new CommandInfo("GlobalWarming", "Global Warming", new GlobalWarmingCommand(), null, Danger.High, 1)
		);
		RegisterCommand(new CommandInfo("Pee", "Gold Rain", new PeeCommand(), null, Danger.Small, 30));
		RegisterCommand(
			new CommandInfo("Kill", "Kill Duplicant", new KillDupeCommand(), null, Danger.Deadly, 1)
		);
		RegisterCommand(
			new CommandInfo(
				"TileTempDown",
				"Icy Floors",
				new TileTempCommand(),
				-40.0d,
				Danger.Medium,
				10,
				"core.tile_temp"
			)
		);
		RegisterCommand(
			new CommandInfo(
				"TileTempUp",
				"Floor is Lava",
				new TileTempCommand(),
				+40.0d,
				Danger.Medium,
				10,
				"core.tile_temp"
			)
		);
		RegisterCommand(
			new CommandInfo("PartyTime", "Party Time", new PartyTimeCommand(), 300.0d, Danger.None, 30)
		);
		RegisterCommand(
			new CommandInfo("PoisonDupes", "Poison Dupes", new PoisonDupesCommand(), null, Danger.High, 20)
		);
		RegisterCommand(
			new CommandInfo("Poopsplosion", "Poopsplosion", new PoopsplosionCommand(), null, Danger.Small, 50)
		);
		RegisterCommand(
			new CommandInfo(
				"RainPrefabGold",
				"Gold Rain",
				new RainPrefabCommand(),
				new Dictionary<string, object> { { "PrefabId", SimHashes.Gold.ToString() }, { "Count", 50.0d } },
				Danger.None,
				30,
				"core.rain_prefab"
			)
		);
		RegisterCommand(
			new CommandInfo(
				"RainPrefabMorb",
				OniTwitchMod.ModIntegration.IsModPresentAndActive("AmogusMorb") ? "Amorbus" : "Morbin' Time",
				new RainPrefabCommand(),
				new Dictionary<string, object> { { "PrefabId", GlomConfig.ID }, { "Count", 10.0d } },
				Danger.Small,
				30,
				"core.rain_prefab"
			)
		);
		RegisterCommand(
			new CommandInfo(
				"RainPrefabDiamond",
				"Diamond Rain",
				new RainPrefabCommand(),
				new Dictionary<string, object> { { "PrefabId", SimHashes.Diamond.ToString() }, { "Count", 25.0d } },
				Danger.None,
				10,
				"core.rain_prefab"
			)
		);
		RegisterCommand(
			new CommandInfo(
				"RainPrefabSlickster",
				"Slickster Rain",
				new RainPrefabCommand(),
				new Dictionary<string, object> { { "PrefabId", OilFloaterConfig.ID }, { "Count", 10.0d } },
				Danger.Small,
				20,
				"core.rain_prefab"
			)
		);
		RegisterCommand(
			new CommandInfo(
				"RainPrefabPacu",
				OniTwitchMod.ModIntegration.IsModPresentAndActive("WeebPacu") ? "PacUwU Rain 🐠👉👈" : "Pacu Rain",
				new RainPrefabCommand(),
				new Dictionary<string, object> { { "PrefabId", PacuConfig.ID }, { "Count", 10.0d } },
				Danger.Small,
				20,
				"core.rain_prefab"
			)
		);
		RegisterCommand(
			new CommandInfo(
				"RainPrefabBee",
				"Bee Infestation",
				new RainPrefabCommand(),
				new Dictionary<string, object> { { "PrefabId", BeeConfig.ID }, { "Count", 10.0d } },
				Danger.Medium,
				10,
				"core.rain_prefab"
			)
		);
		RegisterCommand(
			new CommandInfo(
				"ReduceOxygen",
				"Un-include Oxygen",
				new ReduceOxygenCommand(),
				0.20d,
				Danger.High,
				10
			)
		);
		RegisterCommand(
			new CommandInfo("SkillPoints", "Skilling Spree", new SkillCommand(), 0.33d, Danger.None, 20)
		);
		RegisterCommand(
			new CommandInfo(
				"SleepyDupes",
				"Sleepy Dupes",
				new SleepyDupesCommand(),
				null,
				Danger.Medium,
				20
			)
		);
		RegisterCommand(
			new CommandInfo(
				"SnazzySuit",
				"Spawn Snazzy Suit",
				new SnazzySuitCommand(),
				null,
				Danger.None,
				20
			)
		);
		RegisterCommand(
			new CommandInfo(
				"SpawnGlitterPuft",
				"Spawn Glitter Puft",
				new SpawnPrefabCommand(),
				GlitterPuftConfig.Id,
				Danger.None,
				20
			)
		);
		RegisterCommand(
			new CommandInfo(
				"SpawnVacillatorCharge",
				"Spawn Vacillator Charge",
				new SpawnPrefabCommand(),
				GeneShufflerRechargeConfig.ID,
				Danger.None,
				5
			)
		);
		RegisterCommand(
			new CommandInfo(
				"SpawnAtmoSuit",
				"Spawn Atmo Suit",
				new SpawnPrefabCommand(),
				AtmoSuitConfig.ID,
				Danger.None,
				5
			)
		);
		RegisterCommand(
			new CommandInfo(
				"SpawnCrab",
				"Spawn Pokeshell",
				new SpawnPrefabCommand(),
				CrabConfig.ID,
				Danger.None,
				10
			)
		);
		RegisterCommand(
			new CommandInfo(
				"SpawnMooComet",
				"Spawn Gassy Moo Comet",
				new SpawnPrefabCommand(),
				GassyMooCometConfig.ID,
				Danger.None,
				10
			)
		);
		RegisterCommand(
			new CommandInfo(
				"StressAdd",
				"Add Stress",
				new StressCommand(),
				+0.75d,
				Danger.Medium,
				10,
				"core.stress"
			)
		);
		RegisterCommand(
			new CommandInfo(
				"StressRemove",
				"Remove Stress",
				new StressCommand(),
				-0.75d,
				Danger.None,
				20,
				"core.stress"
			)
		);
		RegisterCommand(
			new CommandInfo(
				"Surprise",
				"Surprise!!!",
				new SurpriseCommand(),
				null,
				// will only choose a command that is within the expected danger range
				// but this command should ignore danger settings
				null,
				80,
				"core.surprise"
			)
		);
		RegisterCommand(
			new CommandInfo(
				"Uninsulate",
				"Uninsulate Tiles",
				new UninsulateCommand(),
				null,
				Danger.Extreme,
				5
			)
		);
		RegisterCommand(
			new CommandInfo(
				"ResearchTech",
				"Research Technology",
				new ResearchTechCommand(),
				null,
				Danger.None,
				10
			)
		);

		RegisterCommand(
			new CommandInfo(
				"Eclipse",
				"Eclipse",
				new EclipseCommand(),
				(double) (3 * Constants.SECONDS_PER_CYCLE),
				Danger.Small,
				10
			)
		);

		RegisterCommand(
			new CommandInfo(
				"PocketDimension",
				"Spawn Pocket Dimension",
				new PocketDimensionCommand(),
				null,
				Danger.None,
				25
			)
		);

		RegisterCommand(
			new CommandInfo(
				"SurpriseBox",
				"Surprise Box",
				new SurpriseBoxCommand(),
				null,
				Danger.None,
				20,
				"core.surprise"
			)
		);

		RegisterCommand(
			new CommandInfo(
				"GeyserModification",
				"Tune Geyser",
				new GeyserModificationCommand(),
				null,
				Danger.Medium,
				10
			)
		);

		RegisterCommand(
			new CommandInfo(
				"MorphCritters",
				"Morph Critters",
				new MorphCommand(),
				null,
				Danger.Small,
				5
			)
		);

		RegisterCommand(
			new CommandInfo(
				"Fart",
				"Fart",
				new FartCommand(),
				25.0d,
				Danger.Small,
				30
			)
		);

		// update user configs
		UserCommandConfigManager.Instance.Reload();
	}

	private record struct CommandInfo(
		[NotNull] string Id,
		[NotNull] string Name,
		[NotNull] CommandBase Command,
		[CanBeNull] object Data,
		[CanBeNull] Danger? Danger,
		int Weight,
		[CanBeNull] string Group = null
	);

	private static void RegisterCommand(CommandInfo info)
	{
		var deckInst = TwitchDeckManager.Instance;
		EventInfo eventInfo;
		if (info.Group != null)
		{
			var group = deckInst.GetGroup(info.Group);
			if (group == null)
			{
				group = new EventGroup(info.Group);
				deckInst.AddGroup(group);
			}

			eventInfo = group.AddEvent(info.Id, info.Weight, info.Name);
		}
		else
		{
			(eventInfo, var group) = EventGroup.DefaultSingleEventGroup(info.Id, info.Weight, info.Name);
			deckInst.AddGroup(group);
		}

		eventInfo.AddListener(info.Command.Run);
		DataManager.Instance.SetDataForEvent(eventInfo, info.Data);
		eventInfo.AddCondition(info.Command.Condition);
		eventInfo.Danger = info.Danger;
	}

	public static void ReloadData(Dictionary<string, Dictionary<string, CommandConfig>> userConfig)
	{
		Debug.Log("[Twitch Integration] Reloading user command config");
		var eventInst = EventManager.Instance;
		var dataInst = DataManager.Instance;
		var deckInst = TwitchDeckManager.Instance;
		foreach (var (namespaceId, namespaceInfo) in userConfig)
		{
			foreach (var (id, config) in namespaceInfo)
			{
				var eventId = eventInst.GetEventByID(namespaceId, id);
				if (eventId != null)
				{
					eventId.FriendlyName = config.FriendlyName ?? eventId.Id;
					dataInst.SetDataForEvent(eventId, config.Data);

					var group = eventId.Group;
					if (group.Name == config.GroupName)
					{
						group.SetWeight(eventId, config.Weight);
					}
					else
					{
						group.RemoveEvent(eventId);

						if (config.GroupName != null)
						{
							var newGroup = deckInst.GetGroup(config.GroupName);
							if (newGroup == null)
							{
								newGroup = new EventGroup(config.GroupName);
								deckInst.AddGroup(newGroup);
							}

							eventId.MoveToGroup(newGroup, config.Weight);
						}
						else
						{
							var newGroup = new EventGroup(
								EventGroup.GetItemDefaultGroupName(eventId.EventNamespace, eventId.EventId)
							);
							deckInst.AddGroup(newGroup);
							eventId.MoveToGroup(newGroup, config.Weight);
						}
					}
				}
			}
		}
	}
}
