using Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Services.BikeServices
{
    public interface IBikeService
    {
        IEnumerable<Bike> GetAllBikes();
        long CountThemAll();

        Bike GetSingle(string id);

        Bike GetSingleWherePredicate(Expression<Func<Bike, bool>> predicate);

        void AddBike(Bike bike);

        void UpdateBike(Bike bike);

        void RemoveBike(Bike bike);

    }
}
