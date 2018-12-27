using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DalProject;
using ModelProject;
using System.Web.Mvc;

namespace ServiceProject
{
    public class INVService
    {
        private static readonly INVDal LDal = new INVDal();
        public List<LabelsModel> GetLabelsList(SLabelsModel SModel)
        {
            try { return LDal.GetLabelsList(SModel); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<SemiModel> GetSemiList(SSemiModel SModel)
        {
            try { return LDal.GetSemiList(SModel); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<SelectListItem> GetXLDrolist(int? pId)
        {
            try { return LDal.GetXLDrolist(pId); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<SelectListItem> GetAreaDrolist(int? pId)
        {
            try { return LDal.GetAreaDrolist(pId); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<SelectListItem> GetCKDrolist(int? pId, int? CKTypeI)
        {
            try { return LDal.GetCKDrolist(pId, CKTypeI); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<SelectListItem> GetWoodDrolist(int? pId)
        {
            try { return LDal.GetWoodDrolist(pId); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<SelectListItem> GetSHDrolist(int? pId)
        {
            try { return LDal.GetSHDrolist(pId); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
