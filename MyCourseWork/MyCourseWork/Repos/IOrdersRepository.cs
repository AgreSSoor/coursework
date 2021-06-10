using System.Threading.Tasks;
using MyCourseWork.Models;

namespace MyCourseWork.Repos
{
    public interface IOrdersRepository
    {
        void  CreateOrder(Order order);
    }
}