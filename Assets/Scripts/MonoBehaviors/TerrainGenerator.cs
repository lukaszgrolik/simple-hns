using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MonoBehaviors
{
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(TerrainGenerator))]
    public class TerrainGeneratorEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var script = (TerrainGenerator)target;

            if (DrawDefaultInspector())
            {
                // if (script.autoUpdate)
                // {
                //     script.Generate();
                // }
            }

            if (GUILayout.Button("Generate")) script.Generate();

            // if (GUILayout.Button("Clear")) {
            //     script.Clear();
            // }
        }
    }
#endif

    public class TerrainGenerator : MonoBehaviour
    {
        [System.Serializable]
        class TerrainProp
        {
            [SerializeField] private float chance;
            public float Chance { get => chance; }

            [SerializeField] private List<Sprite> sprites;
            public List<Sprite> Sprites { get => sprites; }
        }

        [SerializeField] Vector2Int size = new Vector2Int(10, 10);
        // [SerializeField] private Transform tilesContainer;
        [SerializeField] private Transform propsContainer;
        // [SerializeField] private GameObject tilePrefab;
        [SerializeField] private GameObject propPrefab;

        // [SerializeField] private GameObject grassPrefab;
        // [SerializeField] private GameObject flowerPrefab;
        // [SerializeField] private GameObject mushroomPrefab;
        // [SerializeField] private GameObject bushPrefab;

        [SerializeField] private float propChance = .5f;
        [SerializeField] private List<TerrainProp> props;

        // [SerializeField] private Sprite[] tileSprites;
        // [SerializeField] private Gradient gradient;

        private bool isGenerated = false;
        public bool IsGenerated { get { return isGenerated; } }

        public class TerrainGeneratedEvent : UnityEvent { };
        public TerrainGeneratedEvent terrainGenerated = new TerrainGeneratedEvent();

        // public class TileInfo {
        //     private float noiseValue;
        //     public float NoiseValue { get { return noiseValue; } }

        //     private SpriteRenderer spriteRend;
        //     public SpriteRenderer SpriteRend { get { return spriteRend; } }

        //     public TileInfo(float noiseValue, SpriteRenderer spriteRend) {
        //         this.noiseValue = noiseValue;
        //         this.spriteRend = spriteRend;
        //     }
        // }

        // private Dictionary<GameObject, TileInfo> tiles = new Dictionary<GameObject, TileInfo>();
        // public Dictionary<GameObject, TileInfo> Tiles { get { return tiles; } }

        void Start()
        {
            // generate on play to populate tiles data
            Generate();

            isGenerated = true;
            terrainGenerated.Invoke();
        }

        public void Clear()
        {
            // tiles.Clear();

            // for (int i = tilesContainer.childCount; i > 0; --i) {
            //     DestroyImmediate(tilesContainer.GetChild(0).gameObject);
            // }

            for (int i = propsContainer.childCount; i > 0; --i)
            {
                DestroyImmediate(propsContainer.GetChild(0).gameObject);
            }
        }

        public void Generate()
        {
            Clear();

            var halfSizeX = size.x / 2;
            var halfSizeZ = size.y / 2;

            for (int x = 0; x < size.x; x++)
            {
                for (int z = 0; z < size.y; z++)
                {
                    var pos = transform.position + new Vector3(-halfSizeX + x, 0, -halfSizeZ + z);
                    // var freq = .2f;
                    // var noiseVal = Mathf.PerlinNoise(x * freq, z * freq);

                    // PlaceTile(pos, noiseVal);

                    if (propChance > Random.value)
                    {
                        var randomVal = Random.value;
                        for (int i = 0; i < props.Count; i++)
                        {
                            if (props[i].Chance > randomVal)
                            {
                                var sprite = props[i].Sprites.Random();

                                PlaceProp(sprite, pos);
                                break;
                            }
                        }

                        // if (grassPrefab && Random.value > .33f) PlaceProp(grassPrefab, pos, noiseVal);
                        // else if (flowerPrefab && Random.value > .6f) PlaceProp(flowerPrefab, pos, noiseVal);
                        // else if (mushroomPrefab && Random.value > .7f) PlaceProp(mushroomPrefab, pos, noiseVal);
                        // else if (bushPrefab && Random.value > .8f) PlaceProp(bushPrefab, pos, noiseVal);
                    }
                }
            }
        }

        // void PlaceTile(Vector3 pos, float noiseVal) {
        //     var tile = Instantiate(tilePrefab, tilePrefab.transform.position + pos, tilePrefab.transform.rotation, tilesContainer);

        //     var spriteRend = tile.GetComponent<SpriteRenderer>();

        //     // spriteRend.color = Color.HSVToRGB(Random.value, .5f, .5f);
        //     // spriteRend.color = Color.HSVToRGB(hue, .5f, .5f);
        //     spriteRend.sprite = tileSprites.Random();
        //     spriteRend.color = gradient.Evaluate(noiseVal);
        //     spriteRend.sortingOrder = Random.Range(0, 2);

        //     // var terrainTile = tile.GetComponent<TerrainTile>();
        //     // terrainTile.Setup()

        //     tiles.Add(tile, new TileInfo(
        //         noiseValue: noiseVal,
        //         spriteRend: spriteRend
        //     ));
        // }

        void PlaceProp(Sprite sprite, Vector3 pos)
        {
            var prop = Instantiate(propPrefab, pos, propPrefab.transform.rotation, propsContainer);
            var spriteRend = prop.GetComponent<SpriteRenderer>();

            spriteRend.sprite = sprite;
            spriteRend.flipX = Random.value > .5f;
        }
    }
}