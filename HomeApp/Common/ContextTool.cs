
namespace HomeApp.Common;
public class ContextTool
{
    public static void UpdateRelations<T>(object entityExisting, object entityEditing)
    {
        var existingCastToList = (List<T>)entityExisting;
        var editingCastToList = (List<T>)entityEditing;


        foreach (var entity in editingCastToList)
        {
            long id = GetIdFromEntity(entity);
            if (!existingCastToList.Any(e => GetIdFromEntity(e) == id))
                existingCastToList.Add(entity);
        }

        foreach (var entity in existingCastToList.ToList())
        {
            long id = GetIdFromEntity(entity);
            if (!editingCastToList.Any(e => GetIdFromEntity(e) == id))
                existingCastToList.Remove(entity);
        }
    }


    public static long GetIdFromEntity(object @object)
    {
        if (@object == null) return 0;
        var properties = @object.GetType().GetProperties();

        foreach (var property in properties)
        {
            if (property.Name.ToLower() == "id")
            {
                object value = property.GetValue(@object);
                if (value == null)
                    continue;
                long id = (long)value;
                return id;
            }
        }
        return 0;
    }
}