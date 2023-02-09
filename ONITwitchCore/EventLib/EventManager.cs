﻿using System.Collections.Generic;
using JetBrains.Annotations;

namespace EventLib;

public class EventManager
{
	private static EventManager instance;

	private readonly Dictionary<string, EventInfo> registeredEvents = new();

	public static EventManager Instance
	{
		get
		{
			instance ??= new EventManager();
			return instance;
		}
	}

	internal void RegisterEvent([NotNull] EventInfo eventInfo)
	{
		registeredEvents[eventInfo.Id] = eventInfo;
	}

	/// <summary>
	/// Gets an <see cref="EventInfo"/> for the specified ID, if the ID is registered.
	/// </summary>
	/// <param name="eventNamespace">The namespace for the ID</param>
	/// <param name="id">The ID to look for</param>
	/// <returns>An <see cref="EventInfo"/> representing the event, if the ID is found, or <c>null</c> otherwise.</returns>
	[CanBeNull]
	public EventInfo GetEventByID([NotNull] string eventNamespace, [NotNull] string id)
	{
		return registeredEvents.TryGetValue($"{eventNamespace}.{id}", out var eventInfo) ? eventInfo : null;
	}
}