// <copyright file="ObjectStorageHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Application.Helpers;

public static class ObjectStorageHelper
{
    public static string ExtractObjectKey(string imageUrl)
    {
        var uri = new Uri(imageUrl);

        return uri.AbsolutePath.TrimStart('/');
    }
}