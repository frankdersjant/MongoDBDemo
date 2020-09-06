using Models;

namespace DAL
{
    public class BikeRepository : EntityBaseRepository<Bike>, IBikeRepository
    {

        public BikeRepository() : base()
        {
        }
    }
}
