using UnityEngine;

public class ResourceTile : Tile
{
    public new Player.Numbers player;
    //public new Material mat;
    //public new Type type = Type.Resource;
    public Resource resource;
    
    
    
   

    // public static Material AssignResourceMat(Resource.ResourceType resourceType)
    // {
    //     if (resourceType == Resource.ResourceType.Copper)
    //     {
    //         return ArenaGenerator.CopperRes;
    //     }
    //     else if (resourceType == Resource.ResourceType.Iron)
    //     {
    //         return ArenaGenerator.IronRes;
    //     }
    //     else if (resourceType == Resource.ResourceType.Gold)
    //     {
    //         return ArenaGenerator.GoldRes;
    //     }
    //     else
    //     {
    //         return new Material(Shader.Find($"Standard"));
    //     }
    // }
}