using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	private static AudioManager instance_;
	public static AudioManager Instance {
		get {
			if (!instance_) {
				instance_ = (AudioManager) FindObjectOfType(typeof(AudioManager));
			}
			return instance_;
		}
	}

	private AudioSource audio_;

	void Awake() {
		audio_ = (AudioSource)this.GetComponent<AudioSource>();
	}

	public void PlaySFX(AudioClip clip) {
		audio_.PlayOneShot(clip);
	}

	public void PlayVoice(AudioClip clip) {

	}
}
