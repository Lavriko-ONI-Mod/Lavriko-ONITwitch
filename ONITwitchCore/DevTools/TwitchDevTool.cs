using System;
using System.Collections.Generic;
using System.Linq;
using ImGuiNET;
using JetBrains.Annotations;
using Newtonsoft.Json;
using ONITwitch.DevTools.Panels;
using ONITwitch.DonationAlerts;
using ONITwitchLib;
using ONITwitchLib.Utils;
using PeterHan.PLib.Core;
using PeterHan.PLib.UI;
using STRINGS;
using UnityEngine;
using DataManager = ONITwitch.EventLib.DataManager;
using EventInfo = ONITwitch.EventLib.EventInfo;

namespace ONITwitch.DevTools;

internal class TwitchDevTool : DevTool
{
	// Panels for the dev tools.
	[NotNull] private readonly CameraPath cameraPath;
	[NotNull] private readonly CameraPathPanel cameraPathPanel;
	[NotNull] private readonly DebugInfoPanel debugInfoPanel;

	// Handles creating debug markers. In the future this will use pooled objects.
	[NotNull] private readonly DebugMarkers debugMarkers;
	[NotNull] private readonly EventsPanel eventsPanel;

	// The primary style used by the dev tools.
	[NotNull] private readonly ImGuiStyle mainStyle;
	[NotNull] private readonly WeightsPanel weightsPanel;

	// true if the camera path is currently being edited.
	internal bool EditingCamPath;

	public TwitchDevTool()
	{
		Instance = this;

		debugMarkers = new DebugMarkers();
		cameraPath = new CameraPath();
		cameraPathPanel = new CameraPathPanel(debugMarkers, cameraPath);
		debugInfoPanel = new DebugInfoPanel(debugMarkers);
		eventsPanel = new EventsPanel();
		weightsPanel = new WeightsPanel();

		mainStyle = new ImGuiStyle();
		mainStyle.AddStyle(ImGuiStyleVar.FrameRounding, 4);

		mainStyle.AddColor(ImGuiCol.WindowBg, new Color(0.06f, 0.06f, 0.06f, 0.75f));
		mainStyle.AddColor(ImGuiCol.FrameBg, new Color(0.18f, 0.00f, 0.42f, 1.00f));
		mainStyle.AddColor(ImGuiCol.FrameBgActive, new Color(0.30f, 0.000f, 0.84f, 1.00f));
		mainStyle.AddColor(ImGuiCol.FrameBgHovered, new Color(0.30f, 0.00f, 0.84f, 1.00f));
		mainStyle.AddColor(ImGuiCol.CheckMark, new Color(0.57f, 0.27f, 1.00f, 1.00f));
		mainStyle.AddColor(ImGuiCol.SliderGrab, new Color(0.60f, 0.33f, 1.00f, 1.00f));
		mainStyle.AddColor(ImGuiCol.SliderGrabActive, new Color(0.71f, 0.40f, 1.00f, 1.00f));
		mainStyle.AddColor(ImGuiCol.Button, new Color(0.57f, 0.27f, 1.00f, 1.00f));
		mainStyle.AddColor(ImGuiCol.ButtonHovered, new Color(0.64f, 0.34f, 1.00f, 1.00f));
		mainStyle.AddColor(ImGuiCol.ButtonActive, new Color(0.71f, 0.40f, 1.00f, 1.00f));
		mainStyle.AddColor(ImGuiCol.Header, new Color(0.57f, 0.27f, 1.00f, 1.00f));
		mainStyle.AddColor(ImGuiCol.HeaderHovered, new Color(0.64f, 0.34f, 1.00f, 1.00f));
		mainStyle.AddColor(
			ImGuiCol.HeaderActive,
			new Color(
				0.71f,
				0.40f,
				1.00f,
				1.00f
			)
		);
	}

	internal static TwitchDevTool Instance { get; private set; }

	public void SelectedCell(int cell)
	{
		// Add a new cell to the camera positions
		// Call only once, the camera positions store the cell
		if (EditingCamPath)
		{
			cameraPath.AddCameraCell(cell);
		}
	}

	protected override void RenderTo(DevPanel panel)
	{
		// ==========================================================
		// WARNING: game may not be active unless explicitly checked!
		// ==========================================================

		// Clear the markers at the start of each frame.
		debugMarkers.Clear();

		TwitchImGui.WithStyle(
			mainStyle,
			() =>
			{
				if (ImGui.CollapsingHeader("Event Weights"))
				{
					weightsPanel.DrawPanel();
				}

				ImGui.Separator();
				
				if (ImGui.CollapsingHeader("Donation Alerts", ImGuiTreeNodeFlags.DefaultOpen))
				{
					ImGui.Text("Status: ");
					ImGui.SameLine();
					if (DonationAlertsController.Instance.Connected)
					{
						ImGui.Text("Connected");
					}
					else
					{
						ImGui.Text("Disonnected");
					}
					
					if (ImGui.Button("Reconnect to donation alerts"))
					{
						DonationAlertsController.Instance.Connect();
					}

					if (DonationAlertsController.Instance.IsConnecting)
					{
						ImGui.ProgressBar((float)ImGui.GetTime() * -0.2f, new Vector2(), "Authorizing");
					}

					if (ImGui.Button("Start PLib UI"))
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

						var entries = EventsPanel.GenerateEventEntries(null);

						var content = new PPanel()
						{
							Alignment = TextAnchor.MiddleLeft
						};
						pDialog.Body.AddChild(new PScrollPane() {Child = content, ScrollVertical = true, AlwaysShowVertical = true, FlexSize = new Vector2(200f, 0)});
						var dataInst = DataManager.Instance;
						foreach (var (eventNamespace, groups) in entries)
						{
							foreach (var (groupName, events) in groups)
							{
								content.AddChild(
									new PLabel()
									{
										Text = groupName,
										TextAlignment = TextAnchor.MiddleCenter
									}
								);
								foreach (var eventInfo in events)
								{
									var eventName = eventInfo.FriendlyName ?? "";
									content.AddChild(
										new PButton()
										{
											Text = eventName,
											TextAlignment = TextAnchor.MiddleLeft,
											OnClick = source =>
											{
												var data = dataInst.GetDataForEvent(eventInfo);
												GameScheduler.Instance.Schedule(
													"dev trigger event",
													0,
													_ => { eventInfo.Trigger(data); }
												);
											}
										}
									);
								}
							}
						}

						var obj = pDialog.Build();

						if (obj.TryGetComponent(out KScreen dialog))
						{
							dialog.Activate();
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

				// Early out if the game isn't active, we can't do most things.
				if (Game.Instance == null)
				{
					ImGui.TextColored(Color.red, "Game not yet active");
					return;
				}

				// ==========================================================
				// Game is active at this point
				// ==========================================================

				if (ImGui.CollapsingHeader("Debug Info", ImGuiTreeNodeFlags.DefaultOpen))
				{
					debugInfoPanel.DrawPanel();
				}

				ImGui.Separator();

				if (ImGui.CollapsingHeader("Camera Path"))
				{
					cameraPathPanel.DrawPanel();
				}

				ImGui.Separator();

				if (ImGui.CollapsingHeader("Events", ImGuiTreeNodeFlags.DefaultOpen))
				{
					eventsPanel.DrawPanel();
				}
			}
		);
	}
}
