using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour {
	public string wave_name;

	/// <summary>
	/// The entities that will spawn when this wave is started.
	/// </summary>
	[HideInInspector] public List<Entity> entities;

	/// <summary>
	/// The stage this wave belongs in.
	/// </summary>
	private StageController stage;

	private void Start() {
		stage = GetComponentInParent<StageController>();
	}

	public void AddToWave(Entity entity) {
		if (entities.Contains(entity)) { return; }
		entities.Add(entity);
	}

	public void RemoveFromWave(Entity entity) {
		if (entities.Contains(entity) == false) { return; }
		entities.Remove(entity);

		if (entities.Count <= 0) { stage.StartNextWave(); }
	}
}
