namespace AMNSystemsERP.DL.DB.Base
{
    public interface IDeletableEntity : IEntity
    {
        public bool? IsDeleted { get; set; }
    }
}
