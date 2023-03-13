using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using KSerialization;
using ONITwitchCore.Settings;
using ONITwitchLib;
using UnityEngine;

namespace ONITwitchCore;

[SerializationConfig(MemberSerialization.OptIn)]
internal class VoteFile : KMonoBehaviour
{
	private const float FileUpdateTime = 1f / 3f;
	private float accum;

	public void Update()
	{
		accum += Time.unscaledDeltaTime;
		while (accum >= FileUpdateTime)
		{
			accum -= FileUpdateTime;

			if (Game.Instance.TryGetComponent<VoteController>(out var voteController))
			{
				string fileText;
				switch (voteController.State)
				{
					case VoteController.VotingState.NotStarted:
					{
						fileText = STRINGS.ONITWITCH.VOTE_INFO_FILE.NOT_STARTED;
						break;
					}
					case VoteController.VotingState.VoteInProgress:
					{
						var sb = new StringBuilder();
						var votes = voteController.CurrentVote.Votes;
						for (var idx = 0; idx < votes.Count; idx++)
						{
							sb.Append($"{idx + 1}: {votes[idx].EventInfo} ({votes[idx].Count})\n");
						}

						fileText = string.Format(
							STRINGS.ONITWITCH.VOTE_INFO_FILE.IN_PROGRESS_FORMAT,
							voteController.VoteTimeRemaining,
							sb
						);
						break;
					}
					case VoteController.VotingState.VoteDelay:
					{
						fileText = string.Format(
							STRINGS.ONITWITCH.VOTE_INFO_FILE.VOTE_OVER_FORMAT,
							voteController.VoteDelayRemaining
						);
						break;
					}
					case VoteController.VotingState.Error:
					{
						fileText = STRINGS.ONITWITCH.VOTE_INFO_FILE.ERROR;
						break;
					}
					default:
						throw new ArgumentOutOfRangeException();
				}

				var filePath = Path.Combine(TwitchModInfo.MainModFolder, GenericModSettings.SettingsData.VotesPath);
				Task.Run(
					() => { File.WriteAllText(filePath, fileText); }
				);
			}
		}
	}

	protected override void OnCleanUp()
	{
		var filePath = Path.Combine(TwitchModInfo.MainModFolder, GenericModSettings.SettingsData.VotesPath);
		File.WriteAllText(filePath, "Voting not yet started");
		base.OnCleanUp();
	}
}
