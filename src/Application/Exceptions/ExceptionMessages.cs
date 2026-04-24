namespace AutoriaStore.Application.Exceptions;

public static class ExceptionMessages
{
    public const string LOGIN_FAILED = "Email or password is incorrect!";
    public const string USER_NOT_AUTHENTICATED = "User is not authenticated!";
    
    public const string USER_NOT_FOUND = "User not found!";
    public const string USER_ALREADY_EXISTS = "User already exists!";
    
    public const string PRODUCT_CATEGORY_NOT_FOUND = "Product category not found!";
    public const string PRODUCT_CATEGORY_ALREADY_EXISTS = "Product category already exists!";
    
    public const string PRODUCT_NOT_FOUND = "Product not found!";
    public const string PRODUCT_ALREADY_EXISTS = "Product already exists!";
    
    public const string PRODUCT_MAX_IMAGES_REACHED = "Product max images reached!";
    public const string PRODUCT_IMAGE_NOT_FOUND = "Product image not found!";
    public const string PRODUCT_IMAGE_FILE_IS_REQUIRED = "Product image file is required.";

    public const string ORDER_ITEMS_REQUIRED = "Order must contain at least one item!";
    public const string ORDER_ITEM_QUANTITY_INVALID = "Order item quantity must be greater than zero!";
    public const string IDEMPOTENCY_KEY_REQUIRED = "Idempotency-Key header is required.";
    public const string IDEMPOTENCY_KEY_REUSED_WITH_DIFFERENT_PAYLOAD = "Idempotency key was already used with a different request payload.";
}
