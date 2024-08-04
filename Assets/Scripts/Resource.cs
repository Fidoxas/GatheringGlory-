public class Resource
{
    public Type ResourceType { get; private set; }

    public Resource(Type resourceType)
    {
        ResourceType = resourceType;
    }

    public enum Type
    {
        Essence,
        Crystal,
        Elest,
        Rune,
        Core
    }
}