using System;
using System.Linq.Expressions;

namespace Core.Interfaces;

public interface ISpecification<T>
{
    Expression<Func<T, bool>> Criteria { get; }//retourner le critere
    Expression<Func<T, object>>? Orderby { get; }
    Expression<Func<T, object>>? OrderbyDescending { get; }
    
    bool isDistinct { get;  }
}


public interface ISpecification<T, TResult>:ISpecification<T>// ISpecification<T, TResult> prend en paramètre le type T et retorne le type TResult
{
    Expression<Func<T, TResult>>? Select { get; }
}
