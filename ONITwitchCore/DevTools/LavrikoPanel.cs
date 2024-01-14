using System.Collections.Generic;
using ONITwitch.DevTools.Panels;
using ONITwitch.EventLib;
using ONITwitchLib.Utils;
using PeterHan.PLib.Core;
using PeterHan.PLib.UI;
using STRINGS;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ONITwitch.DevTools;

public static class Extensions
{
    public static HashSet<string> Flatten(this List<(string Namespace, List<(string GroupName, List<EventInfo> Events)> GroupedEvents)> entries)
    {
        var names = new HashSet<string>();
        foreach (var (eventNamespace, groups) in entries)
        {
            foreach (var (groupName, events) in groups)
            {
                foreach (var eventInfo in events)
                {
                    var eventName = eventInfo.FriendlyName ?? "";
                    names.Add(eventName);
                }
            }
        }

        return names;
    }
}

public class LavrikoPanel
{
    private static LavrikoPanel _instance;
    public static LavrikoPanel Instance => _instance ??= new LavrikoPanel();
    
    private List<(string Namespace, List<(string GroupName, List<EventInfo> Events)> GroupedEvents)> _currentEntries;

    private readonly List<IUIComponent> _children = new List<IUIComponent>();
    private string _currentFilter = "";

    private KScreen _currentDialog;

    private Dictionary<string, GameObject> _builtButtons = new();

    public void Spawn()
    {
        var pDialog = new PDialog("PLib Test")
            {
                Title = "Lavriko Twitch Integration",
                Size = new Vector2(600.0f, 200.0f),
                MaxSize = new Vector2(600f, 800f),
                SortKey = 150.0f,
                DialogBackColor = PUITuning.Colors.OptionsBackground,
                DialogClosed = option => { },
                RoundToNearestEven = true,
            }
            .AddButton(
                "ok",
                "Ok",
                "Ok",
                PUITuning.Colors.ButtonPinkStyle
            )
            .AddButton(
                PDialog.DIALOG_KEY_CLOSE,
                PLibStrings.TOOLTIP_CANCEL,
                PLibStrings.TOOLTIP_CANCEL,
                PUITuning.Colors.ButtonBlueStyle
            );

        var content = new PPanel()
        {
            Alignment = TextAnchor.MiddleLeft,
            FlexSize = new Vector2(1, 1)
        };

        pDialog.Body.AddChild(new PScrollPane() {Child = content, ScrollVertical = true, AlwaysShowVertical = true, FlexSize = new Vector2(1, 1)});

        content.AddChild(
            new PTextField()
            {
                FlexSize = new Vector2(1, 0),
                Text = _currentFilter,
                OnValueChanged = (_, text) =>
                {
                    _currentFilter = text;
                    var foundEntries = EventsPanel.GenerateEventEntries(text).Flatten();
                    foreach (var key in _builtButtons.Keys)
                    {
                        _builtButtons[key]
                            .SetActive(foundEntries.Contains(key));
                    }
                }
            }
        );
        
        var dataInst = DataManager.Instance;
        
        var entries = EventsPanel.GenerateEventEntries(null);

        var buttons = new Dictionary<string, PButton>();
        
        foreach (var (eventNamespace, groups) in entries)
        {
            foreach (var (groupName, events) in groups)
            {
                var label = new PLabel()
                {
                    Text = groupName,
                    TextAlignment = TextAnchor.MiddleCenter,
                    FlexSize = new Vector2(1, 0)
                };

                content.AddChild(
                    label
                );
                _children.Add(label);
                foreach (var eventInfo in events)
                {
                    var eventName = eventInfo.FriendlyName ?? "";
                    var pButton = new PButton()
                    {
                        Text = eventName,
                        TextAlignment = TextAnchor.MiddleLeft,
                        FlexSize = new Vector2(1, 0),
                        OnClick = _ =>
                        {
                            var data = dataInst.GetDataForEvent(eventInfo);
                            GameScheduler.Instance.Schedule(
                                "dev trigger event",
                                0,
                                _ => { eventInfo.Trigger(data); }
                            );
                        }
                    };
                    content.AddChild(
                        pButton
                    );
                    buttons.Add(eventName, pButton);
                }
            }
        }

        var obj = pDialog.Build();

        if (obj.TryGetComponent(out KScreen dialog))
        {
            _currentDialog = dialog;
            dialog.Activate();
            
            _builtButtons.Clear();
            foreach (var key in buttons.Keys)
            {
                _builtButtons[key] = buttons[key]
                    .BuiltObject;
            }

            dialog.ConsumeMouseScroll = true;
        }
        else
        {
            DialogUtil.MakeDialog(
                STRINGS.ONITWITCH.UI.DIALOGS.CONNECTION_ERROR.TITLE,
                $"Failed to instantiate PLib UI",
                UI.CONFIRMDIALOG.OK,
                null
            );
        }
    }
}