namespace UserCrud.Application.Helpers;

public static class ObjectStorageHelper
{
    public static string ExtractObjectKey(string imageUrl)
    {
        var uri = new Uri(imageUrl);
        
        return uri.AbsolutePath.TrimStart('/');
    }
}