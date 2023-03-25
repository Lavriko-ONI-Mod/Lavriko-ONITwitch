using System.Linq;
using ONITwitch.Cmps;

namespace ONITwitch.Content;

internal static class ComponentsExt
{
	public static readonly Components.Cmps<FloorTileExt> FloorTiles = new();
	public static readonly Components.Cmps<ToiletsExt> Toilets = new();
	public static readonly Components.Cmps<InsulatedTileExt> InsulatedTiles = new();

	public static void CollectFloorTiles()
	{
		foreach (var floor in Assets.BuildingDefs.Where(def => def.BuildingComplete.HasTag(GameTags.FloorTiles)))
		{
			floor.BuildingComplete.AddOrGet<FloorTileExt>();
		}
	}
}
