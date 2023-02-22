using JetBrains.Annotations;

namespace ONITwitchLib.Utils;

public static class DialogUtil
{
	public static KScreen MakeDialog(
		string title,
		string text,
		string confirmText,
		[CanBeNull] System.Action onConfirm
	)
	{
		var screen = (ConfirmDialogScreen) KScreenManager.Instance.StartScreen(
			ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject,
			Global.Instance.globalCanvas
		);
		screen.PopupConfirmDialog(
			text,
			onConfirm,
			null,
			null,
			null,
			title,
			confirmText
		);
		return screen;
	}

	public static KScreen MakeDialog(
		string title,
		string text,
		string confirmText,
		[CanBeNull] System.Action onConfirm,
		string cancelText,
		[CanBeNull] System.Action onCancel,
		string thirdText = null,
		[CanBeNull] System.Action thirdAction = null
	)
	{
		var screen = (ConfirmDialogScreen) KScreenManager.Instance.StartScreen(
			ScreenPrefabs.Instance.ConfirmDialogScreen.gameObject,
			Global.Instance.globalCanvas
		);
		screen.PopupConfirmDialog(
			text,
			onConfirm,
			onCancel,
			thirdText,
			thirdAction,
			title,
			confirmText,
			cancelText
		);
		return screen;
	}
}
