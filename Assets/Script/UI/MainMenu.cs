using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YARG.Input;
using YARG.Settings;

namespace YARG.UI {
	public partial class MainMenu : MonoBehaviour {
		public static bool showSongSelect = false;

		public static MainMenu Instance {
			get;
			private set;
		}

		private enum ButtonIndex {
			QUICKPLAY,
			ADD_EDIT_PLAYERS,
			SETTINGS,
			CREDITS,
			EXIT
		}

		[SerializeField]
		private Canvas mainMenu;
		[SerializeField]
		private Canvas songSelect;
		[SerializeField]
		private Canvas difficultySelect;
		[SerializeField]
		private Canvas postSong;
		[SerializeField]
		private Canvas editPlayers;
		[SerializeField]
		private Canvas addPlayer;
		[SerializeField]
		private Canvas credits;

		[SerializeField]
		private TextMeshProUGUI versionText;

		[SerializeField]
		private GameObject updateObject;

		[Space]
		[SerializeField]
		private Button[] menuButtons;

		private bool isUpdateShown;

		private void Start() {
			Instance = this;

			versionText.text = Constants.VERSION_TAG.ToString();

			if (showSongSelect) {
				ShowSongSelect();
			}
		}

		private void OnEnable() {
			// Bind input events
			Navigator.Instance.NavigationEvent += NavigationEvent;

			var quickplayText = menuButtons[(int) ButtonIndex.QUICKPLAY].GetComponentInChildren<TextMeshProUGUI>();
			if (PlayerManager.players.Count > 0) {
				quickplayText.text = "QUICKPLAY";
			} else {
				quickplayText.text = "<color=#808080>QUICKPLAY<size=50%> (Add a player!)</size></color>";
			}
		}

		private void OnDisable() {
			// Save player prefs
			PlayerPrefs.Save();

			// Unbind input events
			Navigator.Instance.NavigationEvent -= NavigationEvent;
		}

		private void Update() {
			if (!isUpdateShown && UpdateChecker.Instance.IsOutOfDate) {
				isUpdateShown = true;

				updateObject.gameObject.gameObject.SetActive(true);
			}
		}

		private void NavigationEvent(NavigationContext ctx) {
			if (ctx.Action == MenuAction.Confirm) {
				ShowSongSelect();
			}
		}

		private void HideAll() {
			MainMenuBackground.Instance.cursorMoves = false;

			editPlayers.gameObject.SetActive(false);
			addPlayer.gameObject.SetActive(false);
			mainMenu.gameObject.SetActive(false);
			songSelect.gameObject.SetActive(false);
			difficultySelect.gameObject.SetActive(false);
			postSong.gameObject.SetActive(false);
			credits.gameObject.SetActive(false);
		}

		public void ShowMainMenu() {
			HideAll();

			MainMenuBackground.Instance.cursorMoves = true;

			mainMenu.gameObject.SetActive(true);
		}

		public void ShowEditPlayers() {
			HideAll();
			editPlayers.gameObject.SetActive(true);
		}

		public void ShowAddPlayer() {
			HideAll();
			addPlayer.gameObject.SetActive(true);
		}

		public void ShowSongSelect() {
			if (PlayerManager.players.Count > 0) {
				HideAll();
				songSelect.gameObject.SetActive(true);
			}
		}

		public void ShowPreSong() {
			HideAll();
			GameManager.AudioManager.StopPreviewAudio();
			difficultySelect.gameObject.SetActive(true);
		}

		public void ShowPostSong() {
			HideAll();
			postSong.gameObject.SetActive(true);
			showSongSelect = false;
		}

		public void ShowCredits() {
			HideAll();
			credits.gameObject.SetActive(true);
		}

		public void ShowSettingsMenu() {
			GameManager.Instance.SettingsMenu.gameObject.SetActive(true);
		}

		public void ShowCalibrationScene() {
			if (PlayerManager.players.Count > 0) {
				GameManager.Instance.LoadScene(SceneIndex.CALIBRATION);
			}
		}

		public void AbortSongLoad() {
			SettingsManager.DeleteSettings();

			Quit();
		}

		public void OpenLatestRelease() {
			Application.OpenURL("https://github.com/EliteAsian123/YARG/releases/latest");
		}

		public void Quit() {
#if UNITY_EDITOR

			UnityEditor.EditorApplication.isPlaying = false;

#else

			Application.Quit();

#endif
		}
	}
}