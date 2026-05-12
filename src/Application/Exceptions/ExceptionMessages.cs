// <copyright file="ExceptionMessages.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace AutoriaStore.Application.Exceptions;

public static class ExceptionMessages
{
    public const string LOGINFAILED = "Email or password is incorrect!";
    public const string USERNOTAUTHENTICATED = "User is not authenticated!";

    public const string USERNOTFOUND = "User not found!";
    public const string USERALREADYEXISTS = "User already exists!";

    public const string PRODUCTCATEGORYNOTFOUND = "Product category not found!";
    public const string PRODUCTCATEGORYALREADYEXISTS = "Product category already exists!";

    public const string PRODUCTNOTFOUND = "Product not found!";
    public const string PRODUCTALREADYEXISTS = "Product already exists!";

    public const string PRODUCTMAXIMAGESREACHED = "Product max images reached!";
    public const string PRODUCTIMAGENOTFOUND = "Product image not found!";
    public const string PRODUCTIMAGEFILEISREQUIRED = "Product image file is required.";

    public const string ORDERITEMSREQUIRED = "Order must contain at least one item!";
    public const string ORDERITEMQUANTITYINVALID = "Order item quantity must be greater than zero!";
    public const string ORDERNOTFOUND = "Order not found!";
    public const string ABACATEPAYWEBHOOKUNAUTHORIZED = "AbacatePay webhook is not authorized.";
    public const string IDEMPOTENCYKEYREQUIRED = "Idempotency-Key header is required.";
    public const string IDEMPOTENCYKEYREUSEDWITHDIFFERENTPAYLOAD = "Idempotency key was already used with a different request payload.";
}
