using DalProject;
using ModelProject;
using System;
using System.Collections.Generic;

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
        public bool AddOrUpdateWX(FinanceModel Models)
        {
            try { FDal.AddOrUpdateWX(Models); return true; }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<FinanceFRLogsModel> GetFinanceFRLogs(int HTId)
        {
            try { return FDal.GetFinanceFRLogs(HTId); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<FinanceFRLogsModel> GetWXFinanceFRLogs(int HTId)
        {
            try { return FDal.GetWXFinanceFRLogs(HTId); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
