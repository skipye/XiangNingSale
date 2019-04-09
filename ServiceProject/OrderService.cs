using DalProject;
using ModelProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceProject
{
    public class OrderService
    {
        private static readonly OrderDal ODal = new OrderDal();
        public bool AddOrder(OrderModel models)
        {
            try { ODal.AddOrder(models); return true; }
            catch (Exception)
            {
                return false;
            }
        }
        
        /// <summary>
        /// 获取所有订单列表
        /// </summary>
        /// <param name="KeyWord"></param>
        /// <param name="MemberId"></param>
        /// <param name="IsTimeOut"></param>
        /// <returns></returns>
        public List<OrderModel> GetOrderList(string KeyWord, Guid MemberId, bool? IsTimeOut, int PageSize, int PageIndex, bool? PayState)
        {
            try { return ODal.GetOrderList(KeyWord, MemberId, IsTimeOut, PageSize, PageIndex, PayState); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 根据OrderId获取详情
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        public OrderModel GetOrderDetail(string OrderId)
        {
            try { return ODal.GetOrderDetail(OrderId); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="Id"></param>
        public bool DelOrderById(int Id)
        {
            try { ODal.DelOrderById(Id); return true; }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// 修改订单状态
        /// </summary>
        /// <param name="OrderNumber"></param>
        public bool UpdateOrder(string OrderNumber)
        {
            try { ODal.UpdateOrder(OrderNumber); return true; }
            catch (Exception)
            {
                return false;
            }
        }
        
        public List<OrderModel> GetReturnOrderList(Guid MemberId, int PageSize, int PageIndex)
        {
            try { return ODal.GetReturnOrderList(MemberId, PageSize, PageIndex); }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
    }
}
