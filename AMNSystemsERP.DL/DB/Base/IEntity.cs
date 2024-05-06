namespace AMNSystemsERP.DL.DB.Base
{
    public interface IEntity
    {
        DateTime CreatedDate { get; set; }
        string? CreatedBy { get; set; }
        DateTime? ModifiedDate { get; set; }
        string? ModifiedBy { get; set; }
    }
}
