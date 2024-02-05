using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using ONITwitch.EventLib;


namespace ONITwitch.DonationAlerts;

public static class AmountExtensions
{
    public static bool IsCloseTo(this float amount, int destintation)
    {
        return Math.Abs(amount - destintation) < 0.001;
    }
    
    
    public static IEnumerable<(string Id, EventInfo info)> Flatten(this List<(string Namespace, List<(string GroupName, List<EventInfo> Events)> GroupedEvents)> entries)
    {
        var names = new HashSet<string>();
        foreach (var (eventNamespace, groups) in entries)
        {
            foreach (var (groupName, events) in groups)
            {
                foreach (var eventInfo in events)
                {
                    yield return (eventInfo.Id, eventInfo);
                }
            }
        }
    }
}