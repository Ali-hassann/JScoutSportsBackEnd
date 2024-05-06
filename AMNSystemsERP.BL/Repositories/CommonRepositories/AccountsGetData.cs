using AMNSystemsERP.CL.Models.AccountModels.ChartOfAccounts;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace AMNSystemsERP.BL.Repositories.CommonRepositories
{
    public class AccountsGetData
    {
        //private string _connectionString { get; set; }
        DL.DB.ERPContext _context;

        private readonly IMapper _mapper;
        public AccountsGetData(DL.DB.ERPContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;

            //if (string.IsNullOrEmpty(connectionString))
            //{
            //    throw new Exception("Connection String Not Defined in SpBase Class");
            //}
            //_connectionString = connectionString;
        }

        public   HeadCategoriesRequest GetHeadCategoryByIdAsync(long Id)
        {
            try
            {
                var data = new HeadCategoriesRequest();
                var res1 = _context.HeadCategories.Include("MainHead").SingleOrDefault(c => c.HeadCategoriesId == Id);
              
                // var res1 = _context.Database.GetService<HeadCategoriesRequest>("");
                
                if (res1 == null)
                {

                   data = _mapper.Map<HeadCategoriesRequest>(res1);
                }
                return data;

            }
            catch (Exception)
            {
                throw;
            }
        }

    }



}
