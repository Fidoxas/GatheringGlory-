using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaGenerator : MonoBehaviour
{
    [SerializeField] private List<List<Tile.Type>> _arenaTiles;  
    [SerializeField] StageV[] workValues;
    [SerializeField] private Stage[] stages;
    [SerializeField] private GameObject terrainPrefab;

    public int stageRows = 3;
    public int spacing = 10;
    [SerializeField]float seed;

    [ContextMenu("Generate Arena")]
    public void GenerateArenaFromEditor()
    {
        if (workValues == null)
        {
            workValues = new StageV[stageRows * stageRows];
            for (int i = 0; i < workValues.Length; i++)
            {
                workValues[i] = new StageV();
            }
        }

        seed = Random.Range(0.1f, seed);
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
        if (workValues == null)
        {
            workValues = new StageV[stageRows * stageRows];
            for (int i = 0; i < workValues.Length; i++)
            {
                workValues[i] = new StageV();
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
        CreateStages();
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
        if (workValues == null || index >= workValues.Length || workValues[index] == null)
        {
            Debug.LogError($"Invalid workValues at index {index}. Ensure workValues is properly initialized and has sufficient elements.");
            return;
        }

        Stage s;

        if (index == 4)
        {
            s = stage.AddComponent<SpecialStage>();
            s.material = workValues[index].material != null ? workValues[index].material : new Material(Shader.Find($"Standard"));
            s.numberP = Player.PlayersNum.None;
            s.type = Stage.StageType.Special;
        }
        else
        {
            s = stage.AddComponent<Stage>();
            s.pName = workValues[index].pName;
            s.material = workValues[index].material != null ? workValues[index].material : new Material(Shader.Find($"Standard"));

            if (index < (int)Stage.StageType.Special)
            {
                s.numberP = (Player.PlayersNum)index;
                s.type = (Stage.StageType)index;
            }
            else
            {
                s.numberP = (Player.PlayersNum)(index - 1);
                s.type = (Stage.StageType)index;
            }
        }

        // Assign common properties
        s.gameObject = stage;
        stages[index] = s;
    }

    private void CreateTerrain()
    {
        foreach (Stage stage in stages)
        {
            var terrainObj = Instantiate(terrainPrefab, stage.gameObject.transform);
            var ter = terrainObj.GetComponent<Terrain>();
            ter.stage = stage;
            ter._terrainObj = terrainObj;
            // Terrain ter = Terrain.CreateTerrainObj(stage).GetComponent<Terrain>();
            StartCoroutine(ter.CreateTerrain(stageRows, spacing, seed));
        }
    }
}