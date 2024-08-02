using System.Collections.Generic;
using UnityEngine;

public class ArenaGenerator : MonoBehaviour
{
    [SerializeField] private List<List<Tile.Type>> _arenaTiles;  
    [SerializeField] private Stage[] stages;
    [SerializeField] private GameObject terrainPrefab;
    [SerializeField]float _seed;
    
    public int stageRows = 3;
    public int spacing = 10;
    private float seed;
        
    
    [ContextMenu("Generate Arena")]
    public void GenerateArenaFromEditor()
    {
        seed = Random.Range(0.1f, _seed);
        ClearArena();
        GenerateArena();
    }

    
    [ContextMenu("Generate Arena 2")]
    public void GenerateArenaFromEditor2()
    {
        var arenaSize = stageRows * spacing;
        _arenaTiles = new List<List<Tile.Type>>();
        for (int i = 0; i < arenaSize; i++)
        {
            var row = new List<Tile.Type>();
            _arenaTiles.Add(row);
            for (int j = 0; j < arenaSize; j++)
            {
                row.Add(Tile.Type.Neutral);
            }
        }
      
        seed = Random.Range(0.1f, seed);
        ClearArena();
        GenerateArena();
    }

    private void ClearArena()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    void GenerateArena()
    {
        Prepare();
        // CreateStages();
        CreateTerrain();
    }

    private void Prepare()
    {
        if (stages == null || stages.Length != stageRows * stageRows)
        {
            stages = new Stage[stageRows * stageRows];
        }

    }

    private void CreateStages()
    {
        for (int i = 0; i < stageRows; i++)
        {
            for (int j = 0; j < stageRows; j++)
            {
                int index = i * stageRows + j;
                if (stages[index] == null)
                {
                    GameObject stageObject = new GameObject("stage_" + (index + 1).ToString());
                    stageObject.transform.SetParent(this.transform);
                    stageObject.transform.localPosition = new Vector3(j * spacing, 0, i * -spacing);
                    stageObject.transform.localRotation = Quaternion.identity;
                    SetStage(stageObject, index);
                }
            }
        }
    }

    private void SetStage(GameObject stage, int index)
    {
        Stage s;

        if (index == 4)
        {
            s = stage.AddComponent<SpecialStage>();
            s.numberP = Player.Numbers.None;
            s.type = Stage.Type.Special;
        }
        else
        {
            s = stage.AddComponent<Stage>();

            if (index < (int)Stage.Type.Special)
            {
                s.numberP = (Player.Numbers)index;
                s.type = (Stage.Type)index;
            }
            else
            {
                s.numberP = (Player.Numbers)(index - 1);
                s.type = (Stage.Type)index;
            }
        }

        s.gameObject = stage;
        stages[index] = s;
    }

    private void CreateTerrain()
    {
         var terrainObj = Instantiate(terrainPrefab, gameObject.transform);
         var ter = terrainObj.GetComponent<Terrain>();
         // Terrain ter = Terrain.CreateTerrainObj(stage).GetComponent<Terrain>();
         StartCoroutine(ter.CreateTerrain(stageRows, spacing, seed)); // Correctly start the coroutine
    }
}