using System;
using System.Collections.Generic;
using System.Linq;
using Database;
using HarmonyLib;
using JetBrains.Annotations;
using Klei.AI;
using ONITwitch.Content;
using ONITwitch.DonationAlerts;
using ONITwitch.Toasts;
using ONITwitch.Voting;
using ONITwitchLib.Core;
using ONITwitchLib.Logger;
using ONITwitchLib.Utils;
using UnityEngine;
using Attributes = Database.Attributes;
using TwitchSettings = ONITwitch.Settings.Components.TwitchSettings;

namespace ONITwitch.Commands;

internal class SpawnDupeCommand : CommandBase
{
	private static readonly Dictionary<string, SpecialDupeData> SpecialDupes = new()
	{
		{ "asquared31415", new SpecialDupeData("NAILS") },
	};

	public override bool Condition(object data)
	{
		var maxDupes = (double) ((IDictionary<string, object>) data)["MaxDupes"];
		return Components.MinionIdentities.Count < maxDupes;
	}

	public override void Run(object data)
	{
		var param = (Dictionary<string, object>) data;
		Color? color = null;
		string name = null;
		bool isDev = false;
		if (param.TryGetValue("name", out var nameObj))
		{
			name = (string)nameObj;
		}
		if (param.TryGetValue("isDonatingStandart", out _))
		{
			if (ColorUtil.TryParseHexString("fefe2b", out var colorParse))
			{
				color = colorParse;
			}
		}
		if (param.TryGetValue("isDonatingImmortal", out _))
		{
			if (ColorUtil.TryParseHexString("33a713", out var colorParse))
			{
				color = colorParse;
			}
		}
		if (param.TryGetValue("isDonatingVIP", out _))
		{
			if (ColorUtil.TryParseHexString("ff0000", out var colorParse))
			{
				color = colorParse;
			}
		}
		if (param.TryGetValue("isDev", out _))
		{
			color = Color.yellow;
			isDev = true;
		}

		if (isDev && Components.LiveMinionIdentities.Any(x => x.name == "bird_egop"))
		{
			MessageBox.Show("Dev is already present");
			return;
		}

		var config = TwitchSettings.GetConfig();

		if ((VoteController.Instance != null) && (VoteController.Instance.CurrentVote != null))
		{
			var votes = VoteController.Instance.CurrentVote.GetUserVotes().ToList();
			// Only get users that are not disallowed and are not spawned yet.
			var allowedVotes = votes.Where(
					pair =>
					{
						if (config.DisallowedDupeNames.Any(
								disallowed => string.Equals(
									pair.Key.DisplayName,
									disallowed,
									StringComparison.InvariantCultureIgnoreCase
								)
							))
						{
							return false;
						}

						return !Components.LiveMinionIdentities.Items.Any(
							([NotNull] i) =>
							{
								var normalizedName = i.name.ToLowerInvariant();
								return normalizedName.Contains(pair.Key.DisplayName.ToLowerInvariant());
							}
						);
					}
				)
				.ToList();

			if (allowedVotes.Count > 0)
			{
				var (user, _) = allowedVotes.GetRandom();
				name = user.DisplayName;
				color = user.NameColor;
			}
		}

		var liveMinions = Components.LiveMinionIdentities.Items;
		if (liveMinions.Count == 0)
		{
			Log.Warn("No live minions, aborting spawn");
			ToastManager.InstantiateToast(
				STRINGS.ONITWITCH.TOASTS.WARNINGS.EVENT_FAILURE,
				STRINGS.ONITWITCH.TOASTS.WARNINGS.SPAWN_DUPE_FAILURE.BODY
			);
			return;
		}

		var minion = Util.KInstantiate(Assets.GetPrefab(MinionConfig.ID));
		minion.name = Assets.GetPrefab(MinionConfig.ID).name;
		Immigration.Instance.ApplyDefaultPersonalPriorities(minion);
		minion.SetActive(true);

		var identity = minion.GetComponent<MinionIdentity>();

		if (name != null)
		{
			if (SpecialDupes.TryGetValue(name, out var specialDupeData))
			{
				var personalities = Db.Get().Personalities;
				var personality = personalities.TryGet(specialDupeData.PersonalityId) ??
								  personalities.GetRandom(true, false);
				new MinionStartingStats(personality).Apply(minion);
			}
			else
			{
				new MinionStartingStats(false).Apply(minion);
			}

			var finalColor = color ?? ColorUtil.GetRandomTwitchColor();
			identity.SetName(
				config.UseTwitchNameColors
					? $"<color=#{finalColor.ToHexString()}>{name}"
					: name
			);
		}
		else
		{
			new MinionStartingStats(false).Apply(minion);
		}

		identity.GetComponent<MinionResume>().ForceAddSkillPoint();

		if (isDev)
		{
			var effect = Db.Get().effects.TryGet(CustomEffects.NoOxygenConsumeEffect.Id);
			if (minion.TryGetComponent<Effects>(out var effects))
			{
				effects.Add(effect, true);
				MessageBox.Show("Dupe will not consume oxygen");
			}
		}

		var handler = GameUtil.CreateHasTagHandler<MinionIdentity>(GameTags.Dead, (component, data) =>
		{
			try
			{
				Components.MinionIdentities.Remove(component);
				var proxies = Components.MinionAssignablesProxy.Where(x => x == component.assignableProxy.Get())
					.ToList();
				foreach (var proxy in proxies)
				{
					Components.MinionAssignablesProxy.Remove(proxy);
				}

				MessageBox.Show($"{component.name} УМЕР!");
			}
			catch
			{
				MessageBox.Show($"{component.name} УМЕР!\nНе удалось очистить двери(");
			}
		});
		
		GameUtil.SubscribeToTags(identity, handler, true);

		Vector3 pos;
		// First try to find a printing pod, since that should always be in a safe location.
		var pods = Components.Telepads.Items;
		if (pods.Count > 0)
		{
			pos = pods.GetRandom().transform.position;
		}
		else
		{
			Log.Debug("Unable to find any Telepads, using a random dupe's location instead");
			pos = liveMinions.GetRandom().transform.position;
		}

		minion.transform.SetLocalPosition(pos);

		var upgradeFx = new UpgradeFX.Instance(identity, new Vector3(0.0f, 0.0f, -0.1f));
		upgradeFx.StartSM();

		ToastManager.InstantiateToastWithGoTarget(
			STRINGS.ONITWITCH.TOASTS.SPAWN_DUPE.TITLE,
			string.Format(STRINGS.ONITWITCH.TOASTS.SPAWN_DUPE.BODY_FORMAT, identity.name),
			minion
		);

		Log.Info($"Spawned duplicant {identity.name}");
	}

	private static void ApplyMinionPersonality(string key, GameObject minion)
	{
		var pers = Db.Get()
			.Personalities.GetPersonalityFromNameStringKey(key);

		if (pers is null)
		{
			// don't get from Db, we need a copy
			pers = new Personalities().GetRandom(true, false);
			pers.nameStringKey = key;
			Db.Get().Personalities.Add(pers);
		}
		new MinionStartingStats(pers).Apply(minion);
	}

	private record struct SpecialDupeData([NotNull] string PersonalityId);
}
