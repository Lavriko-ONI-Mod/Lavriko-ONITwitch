// using UnityEngine;
//
// namespace ONITwitch.Commands;
//
// public class ReviveDupeCommand : CommandBase
// {
//     public struct TargetIdentity
//     {
//         public MinionIdentity identity;
//
//         public readonly GameObject Revive()
//         {
//             if (identity != null)
//             {
//                 var go = new GameObject("temporary carrier");
//                 var storage = go.AddComponent<MinionStorage>();
//                 var pos = identity.transform.position;
//                 storage.SerializeMinion(identity.gameObject);
//                 var result = storage.DeserializeMinion(storage.GetStoredMinionInfo()[0].id, pos);
//
//                 Object.Destroy(go);
//
//                 return result;
//             }
//
//             return null;
//         }
//     }
//     
//     public override void Run(object data)
//     {
//         var targets = ListPool<TargetIdentity, ReviveDupeCommand>.Allocate();
//
//         foreach (var minion in Components.MinionIdentities.Items)
//         {
//             if (minion.HasTag(GameTags.Dead))
//             {
//                 targets.Add(new TargetIdentity()
//                 {
//                     identity = minion
//                 });
//             }
//         }
//
//         foreach (var grave in Components.Graves.Items)
//         {
//             if (grave.HasDupe())
//             {
//                 targets.Add(new TargetIdentity());
//             }
//         }
//
//         if (targets.Count > 0)
//         {
//             var target = targets.GetRandom();
//
//             var minionName = target.identity == null
//                 ? target.storage.GetName()
//                 : target.identity.GetProperName();
//
//             this.target = target;
//
//             SetName(string.Format("Revive {0}", minionName));
//         }
//         else
//         {
//             target = default;
//             SetName("Revive (not available)");
//         }
//
//         targets.Recycle();
//     }
// }