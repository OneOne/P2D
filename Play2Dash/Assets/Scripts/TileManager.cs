using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;

[ExecuteInEditMode]
public class TileManager : MonoBehaviour {

	//public TextAsset Map;
	public Texture2D Map;
	public Texture2D GroundTileset;
	//_public GameObject TileBlock;
	public double TileSize = 30;
	public bool DoGenerate = false;



	private Sprite[] Tileset;
	private List<GameObject> _tiles = new List<GameObject>();	

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (DoGenerate) {
			string spritePath = AssetDatabase.GetAssetPath(GroundTileset);
			Tileset = AssetDatabase.LoadAllAssetsAtPath (spritePath).OfType<Sprite>().ToArray();
			if(Tileset.Length >0)
				LoadMap (Map);
			DoGenerate = false;
		}
	}



	void LoadMap(Texture2D iMap)
	{
		// clean
		foreach (GameObject g in _tiles) {
			DestroyImmediate (g);
		}
		_tiles.Clear();

		//Texture2D tex = new Texture2D(0, 0);
		//tex.LoadImage(iMap.bytes);

		for (int i = 0; i < iMap.width; i++)
		{
			for (int j = 0; j < iMap.height; j++)
			{
				Color pixel = iMap.GetPixel(i,j);

				if (pixel.a != 0)
				{
					//GameObject tile = (GameObject)Instantiate (GameObject, new Vector3 ((float)(i * TileSize), (float)(j * TileSize), this.transform.position.z), new Quaternion ());
					GameObject tile = new GameObject();
					tile.name = "Tile " + i + " " + j;
					tile.transform.position = new Vector3 ((float)(i * TileSize), (float)(j * TileSize), this.transform.position.z);
					tile.transform.parent = this.transform;

					SpriteRenderer sr = tile.AddComponent<SpriteRenderer> ();
					sr.sprite = Tileset [1];

					BoxCollider2D bc =tile.AddComponent<BoxCollider2D> ();

					_tiles.Add(tile);

				}
			}
		} 
	}
}


