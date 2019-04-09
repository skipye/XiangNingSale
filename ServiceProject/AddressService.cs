using DalProject;
using ModelProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceProject
{
    public class AddressService
    {
        private static readonly AddressDal MDal = new AddressDal();
        //获取订单默认地址或者第一个地址
        public AddressModel GetTop1Address(Guid MemberId)
        {
            try { return MDal.GetTop1Address(MemberId); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool AddOrUpdateAddress(AddressModel models, out int AId)
        {
            try { MDal.AddOrUpdateAddress(models, out AId); return true; }
            catch (Exception)
            {
                AId = 0;
                return false;
            }
        }
        //获取地址列表
        public List<AddressModel> GetAddressList(Guid MemberId)
        {
            try { return MDal.GetAddressList(MemberId); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public AddressModel GetAddressDetailById(int Id)
        {
            try { return MDal.GetAddressDetailById(Id); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool SetIsTop(int Id, Guid MemberId)
        {
            try { MDal.SetIsTop(Id, MemberId); return true; }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
