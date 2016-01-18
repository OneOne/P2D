using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class TileManager : MonoBehaviour {

	//public TextAsset Map;
	public Texture2D Map;
	public GameObject TileBlock;
	public double TileSize;

	// Use this for initialization
	void Start () {
		LoadMap (Map);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private List<GameObject> _tiles = new List<GameObject>();

	void LoadMap(Texture2D iMap)
	{

		//Texture2D tex = new Texture2D(0, 0);
		//tex.LoadImage(iMap.bytes);

		for (int i = 0; i < iMap.width; i++)
		{
			for (int j = 0; j < iMap.height; j++)
			{
				Color pixel = iMap.GetPixel(i,j);

				if (pixel == Color.black)
				{
					GameObject tile = (GameObject)Instantiate (TileBlock, new Vector3 ((float)(i * TileSize), (float)(j * TileSize), this.transform.position.z), new Quaternion ());
					tile.transform.parent = this.transform;
					_tiles.Add(tile);

				}
			}
		} 
	}
}


