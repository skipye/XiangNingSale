using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataBase;
using ModelProject;
using System.Web.Mvc;

namespace DalProject
{
    public class NewsDal
    {
        public List<NewsModel> GetPageList(SNewsModel SModel)
        { 
            using(var db=new XNArticleEntities())
            {
                var List = (from p in db.A_News.Where(k => k.State == true)
                            where !string.IsNullOrEmpty(SModel.Name) ? p.Name.Contains(SModel.Name) : true
                            where SModel.TypeId!=null && SModel.TypeId>0 ? p.TypeId==SModel.TypeId : true
                            orderby p.CreateTime descending
                            select new NewsModel
                            {
                                Id = p.Id,
                                Name = p.Name,
                                TypeId = p.TypeId,
                                TypeName = p.A_NewsType.Name,
                                Remarks = p.Remarks,
                                CreateTime = p.CreateTime
                            }).ToList();
                return List;
            }
        }
        public List<NewsModel> GetList(int TypeId, int PageIndex, int PageSize)
        {
            using (var db = new XNArticleEntities())
            {
                var List = (from p in db.A_News.Where(k => k.State == true)
                            where TypeId != null && TypeId > 0 ? p.TypeId == TypeId : true
                            orderby p.CreateTime descending
                            select new NewsModel
                            {
                                Id = p.Id,
                                Name = p.Name,
                                TypeId = p.TypeId,
                                TypeName = p.A_NewsType.Name,
                                Remarks = p.Remarks,
                                ConvertImg=p.ConvertPic,
                                CreateTime = p.CreateTime,
                                KeyWord=p.KeyWord
                            }).Skip(PageIndex * PageSize).Take(PageSize).ToList();
                return List;
            }
        }
        public NewsItmeModel GetNewsType(int TypeId)
        {
            using (var db = new XNArticleEntities())
            {
                var tables = (from p in db.A_NewsType.Where(k => k.State == true)
                            where TypeId != null && TypeId > 0 ? p.Id == TypeId : true
                            orderby p.CreateTime descending
                              select new NewsItmeModel
                            {
                                Id = p.Id,
                                Name = p.Name,
                            }).FirstOrDefault();
                return tables;
            }
        }
        //新增和修改仓库设置
        public void AddOrUpdate(NewsModel Models)
        {
            using (var db = new XNArticleEntities())
            {
                if (Models.Id != null && Models.Id >0)
                {
                    string StrSqlImg = string.Format(@"delete A_News_File  where NewsId='{0}'", Models.Id);
                    db.Database.ExecuteSqlCommand(StrSqlImg);//先删除图片后面在添加

                    var table = db.A_News.Where(k => k.Id == Models.Id).SingleOrDefault();
                    table.Name = Models.Name;
                    table.TypeId = Models.TypeId;
                    table.Remarks = Models.Remarks;
                    table.KeyWord = Models.KeyWord;
                    table.StrContent = Models.StrContent;
                    table.ConvertPic = Models.ConvertImg;
                    db.SaveChanges();
                }
                else
                {
                    A_News table = new A_News();
                    table.Name = Models.Name;
                    table.TypeId = Models.TypeId;
                    table.Remarks = Models.Remarks;
                    table.KeyWord = Models.KeyWord;
                    table.StrContent = Models.StrContent;
                    table.ConvertPic = Models.ConvertImg;
                    table.IsShow = false;
                    table.IsTop = false;
                    table.UpTime = DateTime.Now;
                    table.CreateTime = DateTime.Now;
                    table.HitTimes = 10;
                    table.State = true;
                    db.A_News.Add(table);
                    db.SaveChanges();
                    Models.Id = table.Id;
                }
                if (Models.GalleryItems != null && !string.IsNullOrEmpty(Models.GalleryItems))
                {
                    Models.GalleryItems = Models.GalleryItems.Remove(Models.GalleryItems.Length - 1);
                    string[] galleryList = Models.GalleryItems.Split(';');
                    int i = 1;
                    foreach (var item in galleryList)
                    {
                        A_News_File table = new A_News_File();
                        table.Id = Guid.NewGuid();
                        table.Name = "新闻图片";
                        table.NewsId = Models.Id;
                        table.CreateTime = DateTime.Now;
                        table.Sequence = i;
                        table.PublishUrl = item.Replace("Thumbnails", "Published");
                        table.ThumbnailsUrl = item;
                        table.State = true;
                        db.A_News_File.Add(table);
                        i++;
                    }
                    db.SaveChanges();
                }
            }
        }
        //根据文章Id获取内容
        public NewsModel GetDetailById(int Id)
        {
            using (var db = new XNArticleEntities())
            {
                var tables = (from p in db.A_News.Where(k => k.Id == Id)
                              select new NewsModel
                              {
                                  Id = p.Id,
                                  Name = p.Name,
                                  TypeId = p.TypeId,
                                  TypeName = p.A_NewsType.Name,
                                  Remarks = p.Remarks,
                                  ConvertImg = p.ConvertPic,
                                  CreateTime = p.CreateTime,
                                  KeyWord = p.KeyWord,
                                  StrContent=p.StrContent
                              }).SingleOrDefault();
                tables.GalleryItems = GetNewsImgs(Id);
                return tables;
            }
        }
        public void DeleteOne(int Id)
        {
            using (var db = new XNArticleEntities())
            {
                var tables = db.A_News.Where(k => k.Id == Id).SingleOrDefault();
                tables.State = true;
                db.SaveChanges();
            }
        }
        public void DeleteMore(string ListId)
        {
            using (var db = new XNArticleEntities())
            {
                string[] ArrId = ListId.Split('$');
                foreach (var item in ArrId)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        int Id = Convert.ToInt32(item);
                        var tables = db.A_News.Where(k => k.Id == Id).SingleOrDefault();
                        tables.State = true;
                    }
                }
                db.SaveChanges();
            }
        }
        public List<SelectListItem> GetNewTypeDrolist(int? pId)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "请选择新闻类别", Value = "" });
            using (var db = new XNArticleEntities())
            {
                List<A_NewsType> model = db.A_NewsType.Where(b => b.State == true && b.ParentId==0 ).OrderBy(k => k.Id).ToList();
                foreach (var item in model)
                {
                    items.Add(new SelectListItem() { Text = "╋" + item.Name, Value = item.Id.ToString(), Selected = pId.HasValue && item.Id.Equals(pId) });
                    List<A_NewsType> childrenmodel = db.A_NewsType.Where(b => b.State == true && b.ParentId == item.Id).OrderBy(k => k.Id).ToList();
                    if (childrenmodel != null && childrenmodel.Any())
                    {
                        foreach (var Citem in childrenmodel)
                        {
                            items.Add(new SelectListItem() { Text = "----└" + Citem.Name, Value = Citem.Id.ToString(), Selected = pId.HasValue && Citem.Id.Equals(pId) });
                        }
                    }
                }
            }
            return items;
        }
        //获取图片列表信息
        public string GetNewsImgs(int Id)
        {
            string str = string.Empty;
            using (var db = new XNArticleEntities())
            {
                var list = db.A_News_File.Where(k => k.NewsId == Id).ToList();
                if (list != null && list.Any())
                {
                    foreach (var item in list)
                    {
                        str += item.ThumbnailsUrl.Replace('\\', '/') + ";";
                    }
                }
                return str;
            }
        }
        //根据Id获取所有的图片列表
        public List<GalleryModel> GetFileInfoList(int Id)
        {
            List<GalleryModel> Models = new List<GalleryModel>();
            using (var db = new XNArticleEntities())
            {
                Models = db.A_News_File.Where(k => k.NewsId == Id).OrderBy(k => k.Sequence).Select(k => new GalleryModel
                {
                    Id = k.Id,
                    ProductId = k.NewsId,
                    Name = k.Name,
                    ThumbnailsUrl = k.ThumbnailsUrl,
                    PublishUrl = k.PublishUrl
                }).ToList();
                return Models;
            }
        }
    }
}
