using Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DAL
{
    public class EntityBaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, IEntityBase, new()
    {
        private IMongoDatabase mongoDataBase;
        private IMongoCollection<TEntity> _collEntities;
        private string _databaseName;

        public EntityBaseRepository() : base()
        {
            //FOR DEMO PURPOSES ONLY
            string connectionString = "mongodb://localhost:27017/Automotive";
            var client = new MongoClient(connectionString);
            _databaseName = MongoUrl.Create(connectionString).DatabaseName;
            mongoDataBase = client.GetDatabase(_databaseName);
            _collEntities = mongoDataBase.GetCollection<TEntity>(_databaseName);
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _collEntities.Find(_ => true).ToList();
        }

        public virtual long Count()
        {
            return _collEntities.CountDocuments(new BsonDocument());
        }

        public TEntity GetSingle(string id)
        {
            return _collEntities.Find<TEntity>(x => x.Id == id).FirstOrDefault();
        }

        public TEntity GetSingleItemPredicate(Expression<Func<TEntity, bool>> predicate)
        {
            return _collEntities.AsQueryable<TEntity>()
                                .Where(predicate.Compile())
                                .FirstOrDefault();
        }


        public virtual IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            return _collEntities.AsQueryable<TEntity>()
                                .Where(predicate.Compile())
                                .ToList();
        }

        public virtual void Add(TEntity entity)
        {
            _collEntities.InsertOne(entity);
        }

        public virtual void Update(TEntity entity)
        {
            var EntityIdFilter = Builders<TEntity>.Filter.Eq(x => x.Id, entity.Id);
            try
            {

                var result = _collEntities.ReplaceOne(EntityIdFilter, entity);
            }
            catch (MongoException ex)
            {
                string message = ex.Message;
            }
        }

        public virtual void Delete(TEntity entity)
        {
            _collEntities.DeleteOne<TEntity>(x => x.Id == entity.Id);
        }

        public void DeleteWhere(Expression<Func<TEntity, bool>> predicate)
        {
            foreach (TEntity entity in _collEntities.AsQueryable<TEntity>().Where(predicate).ToList())
            {
                _collEntities.DeleteMany((Builders<TEntity>.Filter.Eq("_id", entity.Id)));
            }
        }
    }
}
