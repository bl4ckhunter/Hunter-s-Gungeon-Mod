using UnityEngine;

namespace Mod
{
	public class KillCount : MonoBehaviour
	{
		private AIActor aiactor;
		private GameObject RockSlidePrefab;

		private void Start()
		{
			aiactor = base.gameObject.GetComponent<AIActor>();
			aiactor.healthHaver.OnPreDeath += Spawnnextphase;
		}

		private void Spawnnextphase(Vector2 obj)
		{
			RoRItems.kills += 1;
		}

		

	}

}
