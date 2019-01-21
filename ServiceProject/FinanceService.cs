using DalProject;
using ModelProject;
using System;

namespace ServiceProject
{
    public class FinanceService
    {
        private static readonly FinanceDal FDal = new FinanceDal();
        public bool AddOrUpdate(FinanceModel Models)
        {
            try { FDal.AddOrUpdate(Models);return true; }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
