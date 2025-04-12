using CoreMart.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreMart.BLL.Repository.Interface
{
    public interface IShppingCartRepository
    {
        Task<IEnumerable<ShoppingCart>> GetAllAsync();
        Task<ShoppingCart> GetByIdAsync(int? id);
        Task<IEnumerable<ShoppingCart>> GetByCategoryIdAsync(int? categoryId);
        Task AddAsync(ShoppingCart product);
        Task UpdateAsync(ShoppingCart product);
        Task DeleteAsync(ShoppingCart product);
    }
}
