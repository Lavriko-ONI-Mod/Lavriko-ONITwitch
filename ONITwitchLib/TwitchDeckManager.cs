using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using JetBrains.Annotations;

namespace ONITwitchLib;

public class TwitchDeckManager
{
	private readonly object deckManagerInstance;

	private readonly Action<object> addSingleItemDelegate;
	private readonly Action<object> addListDelegate;
	private readonly Func<object> drawDelegate;

	internal TwitchDeckManager(object inst)
	{
		deckManagerInstance = inst;
		var managerType = deckManagerInstance.GetType();

		var addSingleItemInfo = AccessTools.DeclaredMethod(
			managerType,
			"AddToDeck",
			new[] { EventInterface.EventInfoType }
		);
		addSingleItemDelegate = DelegateUtil.CreateRuntimeTypeActionDelegate(
			addSingleItemInfo,
			deckManagerInstance,
			EventInterface.EventInfoType
		);

		var enumerableType = typeof(IEnumerable<>).MakeGenericType(EventInterface.EventInfoType);
		var addListInfo = AccessTools.DeclaredMethod(managerType, "AddToDeck", new[] { enumerableType });
		addListDelegate = DelegateUtil.CreateRuntimeTypeActionDelegate(
			addListInfo,
			deckManagerInstance,
			enumerableType
		);

		var drawInfo = AccessTools.DeclaredMethod(managerType, "Draw");
		drawDelegate = DelegateUtil.CreateDelegate<Func<object>>(drawInfo, deckManagerInstance);
	}

	public void AddToDeck([NotNull] EventInfo eventInfo)
	{
		addSingleItemDelegate(eventInfo.EventInfoInstance);
	}

	private static readonly Func<IEnumerable<object>, object> CastEventInfo =
		DelegateUtil.CreateRuntimeTypeFuncDelegate(
			AccessTools.Method(
				typeof(Enumerable),
				"Cast",
				new[] { typeof(IEnumerable) },
				new[] { EventInterface.EventInfoType }
			),
			null,
			typeof(IEnumerable),
			typeof(IEnumerable<>).MakeGenericType(EventInterface.EventInfoType)
		);

	public void AddToDeck([NotNull] IEnumerable<EventInfo> eventInfos)
	{
		var instances = eventInfos.Select(info => info.EventInfoInstance);
		var castInstances = CastEventInfo(instances);
		addListDelegate(castInstances);
	}

	[NotNull]
	public EventInfo Draw()
	{
		var result = drawDelegate();
		return new EventInfo(result);
	}
}