using System;
using System.IO;
using JetBrains.Annotations;

namespace ONITwitchLib;

public static class TwitchModInfo
{
	private const string TwitchTypeName = "ONITwitch.OniTwitchMod, ONITwitch";
	private static Type mainTwitchType;
	
	/// <summary>
	/// True if the Twitch mod has been detected, false otherwise.
	/// Safe to access even if the Twitch mod is not installed or active.
	/// </summary>
	public static bool TwitchIsPresent => MainTwitchModType != null;
	
	/// <summary>
	/// The Type for the main Twitch mod's UserMod2, if it exists. null if it cannot be found.
	/// Safe to access even if the Twitch mod is not installed or active. 
	/// </summary>
	[CanBeNull]
	public static Type MainTwitchModType => mainTwitchType ??= Type.GetType(TwitchTypeName);

	/// <summary>
	/// Only valid if the Twitch mod is active.
	/// </summary>
	public static readonly string MainModFolder =
		TwitchIsPresent ? Directory.GetParent(MainTwitchModType!.Assembly.Location)!.FullName : "";

	public const string CredentialsFileName = "SECRET_credentials.json";
	
	/// <summary>
	/// Only valid if the Twitch mod is active.
	/// </summary>
	public static readonly string CredentialsPath = Path.Combine(MainModFolder, CredentialsFileName);
	
	public const string ConfigName = "config.json";

	/// <summary>
	/// Only valid if the Twitch mod is active.
	/// </summary>
	public static readonly string ConfigPath = Path.Combine(MainModFolder, ConfigName);
}