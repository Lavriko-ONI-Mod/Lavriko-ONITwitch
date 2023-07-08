// Side screen code and general design mostly taken from Aki

using System.Collections;
using KSerialization;
using ONITwitchLib;
using ONITwitchLib.Logger;
using UnityEngine;

namespace ONITwitch.Cmps;

internal class SurpriseBox : KMonoBehaviour, ISidescreenButtonControl
{
	public string SidescreenButtonText => STRINGS.ONITWITCH.UI.SURPRISE_BOX_SIDE_SCREEN.NAME;

	public string SidescreenButtonTooltip => STRINGS.ONITWITCH.UI.SURPRISE_BOX_SIDE_SCREEN.TOOLTIP;

	public int HorizontalGroupID()
	{
		return -1;
	}

	public int ButtonSideScreenSortOrder() => 0;

	[Serialize] private bool started;

	protected override void OnSpawn()
	{
		base.OnSpawn();
		if (started)
		{
			StartCoroutine(SpawnGifts());
		}
	}

	public void OnSidescreenButtonPressed()
	{
		StartCoroutine(SpawnGifts());
		started = true;
		if ((CameraController.Instance != null) && (CameraController.Instance.followTarget == transform))
		{
			CameraController.Instance.ClearFollowTarget();
		}
	}

	internal static bool PrefabIsValid(KPrefabID prefab)
	{
		// never allow a force disabled object
		if (prefab.HasTag(ExtraTags.OniTwitchSurpriseBoxForceDisabled))
		{
			return false;
		}

		// explicitly enabled objects
		if (prefab.HasTag(ExtraTags.OniTwitchSurpriseBoxForceEnabled))
		{
			return true;
		}

		// must be a pickupable
		if (prefab.GetComponent<Pickupable>() == null)
		{
			return false;
		}

		// dont spawn the compostable copies of things
		return !prefab.TryGetComponent(out Compostable compostable) || !compostable.isMarkedForCompost;
	}

	private IEnumerator SpawnGifts()
	{
		GetComponent<KBatchedAnimController>().Play("open");
		var spawnCount = Random.Range(5, 11);
		for (var idx = 0; idx < spawnCount; idx++)
		{
			KPrefabID randPrefab;
			do
			{
				randPrefab = Assets.Prefabs.GetRandom();
			} while (!PrefabIsValid(randPrefab));

			SpawnPrefab(randPrefab, transform.position);

			yield return new WaitForSeconds(Random.Range(0.75f, 2.0f));
		}

		Destroy(gameObject);
	}

	internal static void SpawnPrefab(KPrefabID prefabID, Vector3 position)
	{
		Log.Debug($"Surprise box spawning a {prefabID.name} at {position}");
		var go = Util.KInstantiate(prefabID.gameObject, position);
		go.SetActive(true);

		if (go.TryGetComponent(out ElementChunk _) && go.TryGetComponent(out PrimaryElement primaryElement))
		{
			primaryElement.Mass = 50f;
		}

		// make it fly a little bit
		var velocity = Random.Range(1, 3) * Random.insideUnitCircle.normalized;
		velocity.y = Mathf.Abs(velocity.y);
		// whether to restore the faller after 
		var hadFaller = false;
		if (GameComps.Fallers.Has(go))
		{
			hadFaller = true;
			GameComps.Fallers.Remove(go);
		}

		GameComps.Fallers.Add(go, velocity);

		GameScheduler.Instance.Schedule(
			"TwitchRemoveSurpriseBoxFaller",
			15f,
			_ =>
			{
				if (go != null)
				{
					// only clear fallers for things that didnt have it before
					if (!hadFaller && GameComps.Fallers.Has(go))
					{
						GameComps.Fallers.Remove(go);
					}

					// trigger cell changes
					go.transform.SetPosition(go.transform.position);
				}
			}
		);
	}

	public void SetButtonTextOverride(ButtonMenuTextOverride textOverride)
	{
	}

	public bool SidescreenButtonInteractable() => !started;

	public bool SidescreenEnabled() => !started;
}
