﻿using UnityEngine;
using UnityEngine.UI;


public class TankHealth : MonoBehaviour
{
	public float m_StartingHealth = 100f;			   // The amount of health each tank starts with.
	public Slider m_Slider;							 // The slider to represent how much health the tank currently has.
	public Image m_FillImage;						   // The image component of the slider.
	public Color m_FullHealthColor = Color.green;	   // The color the health bar will be when on full health.
	public Color m_ZeroHealthColor = Color.red;		 // The color the health bar will be when on no health.
	public GameObject m_ExplosionPrefab;                // A prefab that will be instantiated in Awake, then used whenever the tank dies.

	private AudioSource m_ExplosionAudio;			   // The audio source to play when the tank explodes.
	private ParticleSystem m_ExplosionParticles;		// The particle system the will play when the tank is destroyed.
	[HideInInspector] public float m_CurrentHealth;					  // How much health the tank currently has.
	private bool m_Dead;                                // Has the tank been reduced beyond zero health yet?
	private int damageModifier = 1;


	private void Awake ()
	{
		// Instantiate the explosion prefab and get a reference to the particle system on it.
		m_ExplosionParticles = Instantiate (m_ExplosionPrefab).GetComponent<ParticleSystem> ();

		// Get a reference to the audio source on the instantiated prefab.
		m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource> ();

		// Disable the prefab so it can be activated when it's required.
		m_ExplosionParticles.gameObject.SetActive (false);
	}


	private void OnEnable()
	{
		// When the tank is enabled, reset the tank's health and whether or not it's dead.
		m_CurrentHealth = m_StartingHealth;
		m_Dead = false;

		// Update the health slider's value and color.
		SetHealthUI();
	}

	//!
	private void Update() {
		if (gameObject.name == "Tank 0" && Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.Alpha1)) {
			if (damageModifier == 1) {
				damageModifier *= 2;
				Debug.Log("Damage cheat enabled. 1");
			} else {
				damageModifier = 1;
				Debug.Log("Damage cheat disabled. 1");
			}
		} else if (gameObject.name == "Tank 1" && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha1)) {
			if (damageModifier == 1) {
				damageModifier *= 2;
				Debug.Log("Damage cheat enabled. 2");
			} else {
				damageModifier = 1;
				Debug.Log("Damage cheat disabled. 2");
			}
		} else if (gameObject.name == "Tank 1" && Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.Alpha2)) {
			if (damageModifier == 1) {
				damageModifier = 0;
				Debug.Log("GodMode cheat enabled. 1");
			} else {
				damageModifier = 1;
				Debug.Log("GodMode cheat disabled. 1");
			}
		} else if (gameObject.name == "Tank 0" && Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Alpha2)) {
			if (damageModifier == 1) {
				damageModifier = 0;
				Debug.Log("GodMode cheat enabled. 1");
			} else {
				damageModifier = 1;
				Debug.Log("GodMode cheat disabled. 1");
			}
		}
	}
	//^

	public void TakeDamage (float amount = 0)
	{

		//!
		if (amount == 0)
		{
			amount = Random.Range(5, 50);
		}
		//^

		// Reduce current health by the amount of damage done.
		m_CurrentHealth -= amount * damageModifier;

		// Change the UI elements appropriately.
		SetHealthUI ();

		// If the current health is at or below zero and it has not yet been registered, call OnDeath.
		if (m_CurrentHealth <= 0f && !m_Dead)
		{
			OnDeath ();
		}
	}

	//!
	public void FullyHeal()  //OBS
	{
		m_CurrentHealth = m_StartingHealth;
		SetHealthUI();
	}

	public void Heal(int amount = 0)
	{
		if (amount == 0)
		{
			amount = Random.Range(10, 80);
		}

		m_CurrentHealth += amount;
		m_CurrentHealth = m_CurrentHealth > m_StartingHealth ? m_StartingHealth : m_CurrentHealth;  //Lerp, unopt

		SetHealthUI();
	}
	//^


	private void SetHealthUI ()
	{
		// Set the slider's value appropriately.
		m_Slider.value = m_CurrentHealth;

		// Interpolate the color of the bar between the choosen colours based on the current percentage of the starting health.
		m_FillImage.color = Color.Lerp (m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
	}


	private void OnDeath ()
	{
		// Set the flag so that this function is only called once.
		m_Dead = true;

		// Move the instantiated explosion prefab to the tank's position and turn it on.
		m_ExplosionParticles.transform.position = transform.position;
		m_ExplosionParticles.gameObject.SetActive (true);

		// Play the particle system of the tank exploding.
		m_ExplosionParticles.Play ();

		// Play the tank explosion sound effect.
		m_ExplosionAudio.Play();

		// Turn the tank off.
		gameObject.SetActive (false);
	}
}
