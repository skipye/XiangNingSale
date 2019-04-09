using DalProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ServiceProject
{
    public class ChinaService
    {
        private static readonly ChinaDal CDal = new ChinaDal();
        public List<SelectListItem> GetPDropdownlist(int? pId)
        {
            try { return CDal.GetPDropdownlist(pId); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<SelectListItem> GetCDropdownlist(int? pId, int? Id)
        {
            try { return CDal.GetCDropdownlist(pId, Id); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string GetCoption(int? pId)
        {
            try { return CDal.GetCoption(pId); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<SelectListItem> GetADropdownlist(int? pId, int? Id)
        {
            try { return CDal.GetADropdownlist(pId, Id); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string GetAoption(int? pId)
        {
            try { return CDal.GetAoption(pId); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
