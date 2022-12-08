using KSerialization;
using ONITwitchLib;
using UnityEngine;

namespace ONITwitchCore.Cmps.PocketDimension;

[SerializationConfig(MemberSerialization.OptIn)]
public class PocketDimensionExteriorPortal : KMonoBehaviour
{
	[Serialize] [SerializeField] public int CreatedWorldIdx = ClusterManager.INVALID_WORLD_IDX;

	[Serialize] public Ref<PocketDimensionInteriorPortal> InteriorPortal;

	protected override void OnSpawn()
	{
		base.OnSpawn();

		gameObject.GetComponent<KBatchedAnimController>().Play("idle", KAnim.PlayMode.Loop);

		var interior = InteriorPortal.Get();
		if (interior != null)
		{
			var innerTeleporter = interior.GetComponent<NavTeleporter>();
			var selfNav = GetComponent<NavTeleporter>();
			selfNav.TwoWayTarget(innerTeleporter);

			GameScheduler.Instance.Schedule(
				"update pocket dim pathfinding",
				0.1f,
				_ =>
				{
					var building = gameObject.GetComponent<Building>();
					foreach (var cell in building.PlacementCells)
					{
						foreach (var c in GridUtil.GetNeighborsInBounds(cell))
						{
							Pathfinding.Instance.AddDirtyNavGridCell(c);
						}
					}
				}
			);
		}
		else
		{
			Debug.LogWarning("[Twitch Integration] No return portal for pocket dimension, removing");
			Destroy();
		}
	}

	public void Destroy()
	{
		Object.Destroy(GetComponent<NavTeleporter>());

		var kbac = gameObject.GetComponent<KBatchedAnimController>();
		kbac.Play("disappear");
		kbac.onAnimComplete += _ => { Object.Destroy(gameObject); };
	}
}