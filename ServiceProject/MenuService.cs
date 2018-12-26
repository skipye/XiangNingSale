using DalProject;
using ModelProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace ServiceProject
{
    public class MenuService
    {
        private static readonly MenuDal CDal = new MenuDal();
        public List<MenuModel> GetPageList(SMenuModel SModel)
        {
            try { return CDal.GetPageList(SModel); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<SelectListItem> GetParentType(int? pId)
        {
            try { return CDal.GetParentType(pId); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool AddOrUpdate(MenuModel Models)
        {
            try { CDal.AddOrUpdate(Models); return true; }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public MenuModel GetDetailById(int Id)
        {
            try { return CDal.GetDetailById(Id); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool DeleteMore(string ListId)
        {
            try { CDal.DeleteMore(ListId); return true; }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public List<MenuItemModel> GetMenuItemList(string UserMenuList)
        {
            try { return CDal.GetMenuItemList(UserMenuList); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
    }
}
