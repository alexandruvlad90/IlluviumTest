using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{
	/// <summary>
	/// struct to keep track of the tiles X and Z coordinates
	/// </summary>
	[System.Serializable]
	public struct HexCoordinates
	{
		[SerializeField]
		private int x, z;

		public int X
		{
			get
			{
				return x;
			}
		}

		public int Z
		{
			get
			{
				return z;
			}
		}

		public int Y
		{
			get
			{
				return -X - Z;
			}
		}

		public HexCoordinates(int x, int z)
		{
			this.x = x;
			this.z = z;
		}
	}

	private MeshRenderer thisMesh; //reference to this objects mesh renderer

	public HexCoordinates coordinates;

	void OnEnable()
    {
        thisMesh = GetComponent<MeshRenderer>();
    }

    public void SetColor(Color newColor)
    {
        thisMesh.materials[0].color = newColor;
    }

}
