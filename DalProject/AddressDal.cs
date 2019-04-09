using DataBase;
using ModelProject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DalProject
{
    public class AddressDal
    {
        //获取订单默认地址或者第一个地址
        public AddressModel GetTop1Address(Guid MemberId)
        {
            AddressModel models = new AddressModel();
            using (var db = new XiangNingSaleEntities())
            {
                var list = (from p in db.Address_Info.Where(k => k.MemberId == MemberId && k.State == true)
                            orderby p.CreateTime descending
                            select new AddressModel
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Telphone = p.Telphone,
                                Province = p.Province,
                                City = p.City,
                                Region = p.Region,
                                IsTop = p.IsTop,
                                addressNo = p.addressNo,
                            }).ToList();
                models = list.FirstOrDefault();
                foreach (var item in list)
                {
                    if (item.IsTop == true)
                    {
                        models = item;
                    }
                }
                return models;
            }
        }
        public void AddOrUpdateAddress(AddressModel models, out int AId)
        {
            using (var db = new XiangNingSaleEntities())
            {
                if (models.Id > 0)
                {
                    var tables = db.Address_Info.Where(k => k.Id == models.Id).FirstOrDefault();
                    tables.Name = models.Name;
                    tables.Telphone = models.Telphone;
                    tables.Province = models.Province;
                    tables.City = models.City;
                    tables.Region = models.Region;
                    tables.ProvinceId = models.ProvinceId;
                    tables.CityId = models.CityId;
                    tables.RegionId = models.RegionId;
                    tables.MemberId = models.MemberId;
                    tables.addressNo = models.addressNo;
                    AId = models.Id;
                    db.SaveChanges();
                }
                else
                {
                    var table = db.Address_Info.Where(k => k.MemberId == models.MemberId && k.State == true).ToList();
                    foreach (var item in table)
                    {
                        item.IsTop = false;
                    }
                    db.SaveChanges();
                    var tables = new Address_Info();
                    tables.Name = models.Name;
                    tables.Telphone = models.Telphone;
                    tables.Province = models.Province;
                    tables.City = models.City;
                    tables.Region = models.Region;
                    tables.ProvinceId = models.ProvinceId;
                    tables.CityId = models.CityId;
                    tables.RegionId = models.RegionId;
                    tables.MemberId = models.MemberId;
                    tables.addressNo = models.addressNo;
                    tables.IsTop = true;
                    tables.CreateTime = DateTime.Now;
                    tables.State = true;
                    db.Address_Info.Add(tables);
                    db.SaveChanges();
                    AId = tables.Id;
                }

            }
        }
        
        //获取地址列表
        public List<AddressModel> GetAddressList(Guid MemberId)
        {
            using (var db = new XiangNingSaleEntities())
            {
                List<AddressModel> Models = new List<AddressModel>();
                var list = (from p in db.Address_Info.Where(k => k.MemberId == MemberId && k.State == true)
                            orderby p.CreateTime descending
                            select new AddressModel
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Province = p.Province,
                                City = p.City,
                                Region = p.Region,
                                ProvinceId = p.ProvinceId,
                                CityId = p.CityId,
                                RegionId = p.RegionId,
                                addressNo = p.addressNo,
                                Telphone = p.Telphone,
                                IsTop = p.IsTop
                            }).ToList();
                Models = list;
                var ZTtables = (from p in db.Address_Info.Where(k => k.Id == 40)
                                orderby p.CreateTime descending
                                select new AddressModel
                                {
                                    Id = p.Id,
                                    Name = p.Name,
                                    Province = p.Province,
                                    City = p.City,
                                    Region = p.Region,
                                    ProvinceId = p.ProvinceId,
                                    CityId = p.CityId,
                                    RegionId = p.RegionId,
                                    addressNo = p.addressNo,
                                    Telphone = p.Telphone,
                                    IsTop = p.IsTop
                                }).SingleOrDefault();
                Models.Add(ZTtables);
                return Models;
            }
        }
        public AddressModel GetAddressDetailById(int Id)
        {
            using (var db = new XiangNingSaleEntities())
            {
                var list = (from p in db.Address_Info.Where(k => k.Id == Id && k.State == true)
                            orderby p.CreateTime descending
                            select new AddressModel
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Province = p.Province,
                                City = p.City,
                                Region = p.Region,
                                ProvinceId = p.ProvinceId,
                                CityId = p.CityId,
                                RegionId = p.RegionId,
                                addressNo = p.addressNo,
                                Telphone = p.Telphone,
                                IsTop = p.IsTop,
                            }).FirstOrDefault();
                return list;
            }
        }
        //设置默认地址
        public void SetIsTop(int Id, Guid MemberId)
        {
            using (var db = new XiangNingSaleEntities())
            {
                var table = db.Address_Info.Where(k => k.MemberId == MemberId && k.State == true).ToList();
                foreach (var item in table)
                {
                    item.IsTop = false;
                    if (item.Id == Id)
                    {
                        item.IsTop = true;
                    }
                }
                db.SaveChanges();
            }
        }
        
    }
}
