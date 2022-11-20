using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using KSerialization;
using ONITwitchLib;
using UnityEngine;

namespace ONITwitchCore;

[SerializationConfig(MemberSerialization.OptIn)]
public class VoteFile : KMonoBehaviour, ISim33ms
{
	private int count;

	public void Sim33ms(float dt)
	{
		// only work every 10th update for 330ms
		count += 1;
		if (count % 10 == 0)
		{
			count %= 10;

			var filePath = Path.Combine(TwitchModInfo.MainModFolder, MainConfig.Instance.Config.VotesPath);
			if (Game.Instance.TryGetComponent<VoteController>(out var voteController))
			{
				string fileText;
				switch (voteController.State)
				{
					case VoteController.VotingState.NotStarted:
					{
						fileText = "Voting not yet started";
						break;
					}
					case VoteController.VotingState.VoteInProgress:
					{
						var sb = new StringBuilder();
						sb.Append(
							$"{MainConfig.Instance.Config.VoteHeader} ({Mathf.RoundToInt(voteController.VoteTimeRemaining)}s)\n"
						);
						var votes = voteController.CurrentVote.Votes;
						for (var idx = 0; idx < votes.Count; idx++)
						{
							sb.Append($"{idx + 1}: {votes[idx].EventInfo} ({votes[idx].Count})\n");
						}

						fileText = sb.ToString();
						break;
					}
					case VoteController.VotingState.VoteDelay:
					{
						fileText =
							$"Vote Over ({voteController.VoteDelayRemaining / Constants.SECONDS_PER_CYCLE:F1} cycles to next vote)";
						break;
					}
					default:
						throw new ArgumentOutOfRangeException();
				}

				Task.Run(
					() => { File.WriteAllText(filePath, fileText); }
				);
			}
		}
	}
}