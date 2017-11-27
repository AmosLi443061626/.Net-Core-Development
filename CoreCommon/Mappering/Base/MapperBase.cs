using AutoMapper;
using CoreCommon.Mappering.Interfaces;
using CoreCommon.Mappering.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CoreCommon.CacheOperation;

namespace CoreCommon.Mappering.Base
{
    public class MapperBase<TModel, TEntity> : IRelationalMapping<TModel, TEntity>
        where TModel : class
        where TEntity : class
    {
        private static MemoryCache<Type, List<PropertyMapping>> _memoryCache = new MemoryCache<Type, List<PropertyMapping>>();

        private IMappingExpression<TModel, TEntity> _toEntityMappingExpression = null;

        public MapperBase()
        {
            InitMapper();
        }

        private void InitMapper()
        {
            Mapper.Initialize(x => _toEntityMappingExpression = x.CreateMap<TModel, TEntity>());
            Mapper.Initialize(x => x.CreateMap<TEntity, TModel>());

            InitMapperByType(typeof(TModel));
            InitMapperByType(typeof(TEntity));
        }

        private void InitMapperByType(Type type)
        {
            var lstPropertyMapping = _memoryCache.GetValue(type, new Lazy<List<PropertyMapping>>(() =>
            {
                var attributeType = typeof(MapperBaseAttribute);
                var properties = type.GetProperties().Where(x => x.IsDefined(attributeType, true));
                return properties.Select(x => new PropertyMapping(x, x.GetCustomAttribute(attributeType) as MapperBaseAttribute)).ToList();
            }));

            lstPropertyMapping.ForEach(p =>
            {
                var mapperAttribute = p.MapperAttribute as IMapperAttribute;
                if (mapperAttribute == null)
                {
                    throw new InvalidCastException("MapperAttribute必须实现IMapperAttribute");
                }

                mapperAttribute.Apply<TModel, TEntity>(_toEntityMappingExpression, p.PropertyInfo.Name);
            });
        }

        public TEntity GetEntity(TModel model)
        {
            var entity = Mapper.Map<TModel, TEntity>(model);

            return entity;
        }

        public TEntity GetEntity(TModel model, TEntity destination)
        {
            var entity = Mapper.Map<TModel, TEntity>(model, destination);

            return entity;
        }

        public TModel GetModel(TEntity entity)
        {
            var dto = Mapper.Map<TEntity, TModel>(entity);

            return dto;
        }

        public List<TEntity> GetEntityList(List<TModel> lstModel)
        {
            var lstEntity = new List<TEntity>();

            lstModel.ForEach(dto =>
            {
                lstEntity.Add(Mapper.Map<TModel, TEntity>(dto));
            });

            return lstEntity;
        }

        public List<TModel> GetModelList(List<TEntity> lstEntity)
        {
            var lstDTO = new List<TModel>();

            lstEntity.ForEach(entity =>
            {
                lstDTO.Add(Mapper.Map<TEntity, TModel>(entity));
            });

            return lstDTO;
        }
    }
}
