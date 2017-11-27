using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCommon.Mappering.Interfaces
{
    public interface IRelationalMapping<Model, Entity>
        where Model : class
        where Entity : class
    {
        Entity GetEntity(Model dto);

        Model GetModel(Entity entity);

        List<Entity> GetEntityList(List<Model> lstDTO);

        List<Model> GetModelList(List<Entity> lstEntity);
    }
}
