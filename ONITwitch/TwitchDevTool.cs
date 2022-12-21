using System.Collections.Generic;
using EventLib;
using ImGuiNET;
using ONITwitchCore.Toasts;

namespace ONITwitch;

public class TwitchDevTool : DevTool
{
	protected override void RenderTo(DevPanel panel)
	{
		// WARNING: game may not be active unless explicitly checked!

		if (ImGui.Button("Test Toast"))
		{
			ToastManager.InstantiateToast(
				"Dev Testing Toast",
				"This is a testing toast created by the Twitch Integration dev tools.\n<color=#FF00FF>this is color</color> <i>this is italic</i> <b>this is bold</b>\n<gradient=\"Red to Green - Vertical\">testing gradient</gradient>"
			);
		}

		// Everything below this needs the game to be active
		if (Game.Instance == null)
		{
			ImGui.Text("Game not yet active");
			return;
		}

		ImGui.Separator();
		ImGui.Text("Trigger Events");
		ImGui.Indent();
		var eventInst = EventManager.Instance;
		var dataInst = DataManager.Instance;

		var namespacedEvents = new SortedDictionary<string, List<EventInfo>>();
		var eventKeys = eventInst.GetAllRegisteredEvents();
		foreach (var eventInfo in eventKeys)
		{
			if (!namespacedEvents.ContainsKey(eventInfo.Namespace))
			{
				namespacedEvents.Add(eventInfo.Namespace, new List<EventInfo> { eventInfo });
			}
			else
			{
				namespacedEvents[eventInfo.Namespace].Add(eventInfo);
			}
		}

		// sort events within their respective category
		foreach (var (_, events) in namespacedEvents)
		{
			events.Sort();
		}

		foreach (var (eventNamespace, eventInfos) in namespacedEvents)
		{
			var mod = Global.Instance.modManager.mods.Find(mod => mod.staticID == eventNamespace);
			var headerName = mod != null ? mod.title : eventNamespace;
			if (ImGui.CollapsingHeader(headerName))
			{
				ImGui.Indent();

				foreach (var eventInfo in eventInfos)
				{
					if (ImGui.Button($"{eventInfo} ({eventInfo.EventId})"))
					{
						Debug.Log($"[Twitch Integration] Dev Tool triggering Event {eventInfo} (id {eventInfo.Id})");
						var data = dataInst.GetDataForEvent(eventInfo);
						eventInst.TriggerEvent(eventInfo, data);
					}
				}

				ImGui.Unindent();
			}
		}
	}
}
