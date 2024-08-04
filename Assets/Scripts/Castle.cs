using Unity.Netcode;
using UnityEngine;

public class Castle : NetworkBehaviour
{
    private Player _owner;
    [SerializeField] private GameObject adeptPrefab;

    private OwnerShip _ownerShipComponent;


    private void Awake()
    {
        PlayerSelection playerSelection = FindObjectOfType<PlayerSelection>();
        if (playerSelection != null)
        {
            playerSelection.OnPlayerAssigned += HandlePlayerAssigned;
        }
    }

    private void HandlePlayerAssigned(int playernumber)
    {
        RespawnAdeptServerRpc();
    }

    private void Start()
    {
        _ownerShipComponent = GetComponent<OwnerShip>();

        _owner = _ownerShipComponent.player;
        _owner.castle = gameObject;
    }


    [ServerRpc(RequireOwnership = false)]
    private void RespawnAdeptServerRpc()
    {
        RespawnAdeptClientRpc();
    }

    [ClientRpc(RequireOwnership = false)]
    private void RespawnAdeptClientRpc()
    {
        SpawnAdept();
    }

    void SpawnAdept()
    {
        Vector3 spawnPos = CalculateSpawnPosition();

        GameObject adept = Instantiate(adeptPrefab, spawnPos, Quaternion.identity);

        SetUnitOwner(adept, _owner);
        _owner.Adept = adept;

        var networkObject = adept.GetComponent<NetworkObject>();
        if (networkObject != null)
        {
            // Spawn the Adept on the network
            networkObject.Spawn();
        }
        else
        {
            Debug.LogError("Adept prefab is missing a NetworkObject component!");
        }

        adept.transform.SetParent(_owner.transform);
    }

    private Vector3 CalculateSpawnPosition()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.x += (spawnPos.x < 30) ? 1 : -1;
        spawnPos.z += (spawnPos.z < 30) ? 1 : -1;
        spawnPos.y += 1;
        return spawnPos;
    }

    private void SetUnitOwner(GameObject unit, Player owner)
    {
        var unitId = unit.GetComponent<ObjSelector>();
        if (unitId != null)
        {
            unitId.player = owner;
        }

        var unitOwner = unit.GetComponent<OwnerShip>();
        if (unitOwner != null)
        {
            unitOwner.player = owner;
        }
    }

    public void CreateUnit(GameObject unitPrefab)
    {
        Vector3 spawnPos = CalculateSpawnPosition();
        GameObject unit = Instantiate(unitPrefab, spawnPos, Quaternion.identity);
        SetUnitOwner(unit, _owner);
    }
}