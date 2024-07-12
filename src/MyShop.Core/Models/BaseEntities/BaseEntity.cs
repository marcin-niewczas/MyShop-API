using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models.BaseEntities;
public abstract class BaseEntity : IEntity
{
    public Guid Id { get; init; } = Guid.NewGuid();
}

public interface IEntity : IModel
{
    public Guid Id { get; }
}

public interface IModel { }
