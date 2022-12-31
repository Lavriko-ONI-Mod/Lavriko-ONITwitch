using ONITwitchLib.Utils;
using ToastManager = ONITwitchCore.Toasts.ToastManager;

namespace ONITwitchCore.Commands;

public class SpawnPrefabCommand : CommandBase
{
	public override bool Condition(object data)
	{
		var prefabId = (string) data;
		return Assets.TryGetPrefab(prefabId) != null;
	}

	public override void Run(object data)
	{
		var prefabId = (string) data;
		var prefab = Assets.GetPrefab(prefabId);
		var position = GridUtil.NearestEmptyCell(PosUtil.RandomCellNearMouse());
		var sceneLayer = Grid.SceneLayer.Front;
		if (prefab.TryGetComponent<KBatchedAnimController>(out var kbac))
		{
			sceneLayer = kbac.sceneLayer;
		}

		var go = GameUtil.KInstantiate(
			prefab,
			Grid.CellToPosCBC(position, sceneLayer),
			sceneLayer
		);
		if (go != null)
		{
			go.SetActive(true);
			ToastManager.InstantiateToastWithGoTarget(
				"Spawning Object",
				$"A new {Util.StripTextFormatting(prefab.GetProperName())} has been created",
				go
			);
		}
		else
		{
			Debug.LogWarning($"[Twitch Integration] Unable to spawn prefab {prefabId}");
		}
	}
}
