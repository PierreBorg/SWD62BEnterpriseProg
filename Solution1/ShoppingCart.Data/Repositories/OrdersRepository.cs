using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Data.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        public void AddOrder(Order order)
        {
            throw new NotImplementedException();
        }

        public void AddOrderDetail(OrderDetail orderDetail)
        {
            throw new NotImplementedException();
        }
    }
}
