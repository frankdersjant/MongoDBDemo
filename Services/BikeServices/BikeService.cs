using DAL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Services.BikeServices
{
    public class BikeService : IBikeService
    {
        private readonly IBikeRepository _bikes;

        public BikeService(IBikeRepository bikesrepo)
        {
            _bikes = bikesrepo;
        }

        public IEnumerable<Bike> GetAllBikes()
        {
            return _bikes.GetAll();
        }

        public long CountThemAll()
        {
            return _bikes.Count();
        }

        public Bike GetSingle(string id)
        {
            return _bikes.GetSingle(id);
        }

        public Bike GetSingleWherePredicate(Expression<Func<Bike, bool>> predicate)
        {
            return _bikes.GetSingleItemPredicate(predicate);
        }

        public void AddBike(Bike bike)
        {
            _bikes.Add(bike);
        }

        public void UpdateBike(Bike bike)
        {
            _bikes.Update(bike);
        }

        public void RemoveBike(Bike bike)
        {
            _bikes.Delete(bike);
        }
    }
}
