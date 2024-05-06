namespace AMNSystemsERP.CL.Enums
{
    public enum FileType
    {
        Images
        , Pdf
        , Excel
    }

    public enum FolderType
    {
        Profile
        , Logo
        , QRCode
        , Voucher
        , Documents
        , FingerPrint
    }

    public enum InventoryDocumentType
    {
        Purchase = 1,
        PurchaseReturn = 2,
        Sale = 3,
        SaleReturn = 4,
        Issuance = 5,
        IssuanceReturn = 6
    }
}
