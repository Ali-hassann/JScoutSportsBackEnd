namespace AMNSystemsERP.DL.DB.Base
{
    public class DeletableEntity : Entity, IDeletableEntity
    {
        public bool? IsDeleted { get; set; }
    }
}
