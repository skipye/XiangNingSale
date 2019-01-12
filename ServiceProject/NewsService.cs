using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ModelProject;
using DalProject;
using System.Web.Mvc;
using MvcPager.WebControls.Mvc;

namespace ServiceProject
{
    public class NewsService
    {
        private static readonly NewsDal IDal = new NewsDal();
        public PagedList<NewsModel> GetWebPageList(SNewsModel SModel, int Type,int PageIndex,int PageSize)
        {
            try { return IDal.GetWebPageList(SModel, Type, PageIndex, PageSize); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CRMItem> GetWebTypeList(int TypeId)
        {
            try { return IDal.GetWebTypeList(TypeId); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<CRMItem> GetWebArealist()
        {
            try { return IDal.GetWebArealist(); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<NewsModel> GetPageList(SNewsModel SModel)
        {
            try { return IDal.GetPageList(SModel); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public NewsItmeModel GetNewsType(int TypeId)
        {
            try { return IDal.GetNewsType(TypeId); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool AddOrUpdate(NewsModel Models)
        {
            try {IDal.AddOrUpdate(Models); return true; }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public NewsModel GetDetailById(int Id)
        {
            try { return IDal.GetDetailById(Id); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool DeleteMore(string ListId)
        {
            try { IDal.DeleteMore(ListId); return true; }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool Checked(string ListId, int CheckedId, int UserId)
        {
            try { IDal.CheckedMore(ListId, CheckedId, UserId); return true; }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<SelectListItem> GetNewTypeDrolist(int? pId)
        {
            try { return IDal.GetNewTypeDrolist(pId); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<SelectListItem> GetNewAreaDrolist(int? pId)
        {
            try { return IDal.GetNewAreaDrolist(pId); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string GetNewsImgs(int Id)
        {
            try { return IDal.GetNewsImgs(Id); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<GalleryModel> GetFileInfoList(int Id)
        {
            try { return IDal.GetFileInfoList(Id); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
