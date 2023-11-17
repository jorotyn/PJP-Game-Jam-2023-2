using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class MenuSystem : MonoBehaviour
{
	private enum State { Game, Options }

	private const KeyCode _toggleKey = KeyCode.Escape;

	[SerializeField] private GameObject _gameGroup;
	[SerializeField] private GameObject _optionsGroup;
	[SerializeField] private Slider _masterVolumeSlider;
	[SerializeField] private Slider _musicVolumeSlider;

	private State _state = State.Game;

	private void Start()
	{
		_masterVolumeSlider.value = AudioManager.Instance.MasterVolume;
		_musicVolumeSlider.value = AudioManager.Instance.MusicVolume;
		_gameGroup.SetActive(true);
		_optionsGroup.SetActive(false);
		NextState();
	}

	private void NextState()
	{
		switch (_state)
		{
			case State.Game:
				StartCoroutine(GameState());
				break;
			case State.Options:
				StartCoroutine(OptionsState());
				break;
		}
	}

	private IEnumerator GameState()
	{
		_gameGroup.SetActive(true);
		while(_state == State.Game)
		{
			yield return null;
			if (Input.GetKeyDown(_toggleKey))
				_state = State.Options;
		}
		_gameGroup.SetActive(false);
		NextState();
	}

	private IEnumerator OptionsState()
	{
		_optionsGroup.SetActive(true);
		while (_state == State.Options)
		{
			yield return null;
			if (Input.GetKeyDown(_toggleKey))
				_state = State.Game;
		}
		_optionsGroup.SetActive(false);
		NextState();
	}
}
