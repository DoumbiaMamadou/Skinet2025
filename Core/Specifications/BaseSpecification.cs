using System;
using System.Linq.Expressions;
using System.Reflection;
using Core.Interfaces;

namespace Core.Specifications;

public class BaseSpecification<T>(Expression<Func<T, bool>> criteria) : ISpecification<T>//implimentation de l'interface
{

    protected BaseSpecification() : this(null!)//constructeur par defaut
    {

    }

    public Expression<Func<T, bool>>? Criteria => criteria;//retourner le critere

    public Expression<Func<T, object>>? Orderby { get; private set; } = null;

    public Expression<Func<T, object>>? OrderbyDescending { get; private set; } = null;

    public bool isDistinct { get; private set; }

    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        Orderby = orderByExpression;
    }
    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
    {
        OrderbyDescending = orderByDescExpression;
    }

    protected void ApplyDistinct()
    {
        isDistinct = true;
    }
}

public class BaseSpecification<T, TResult> (Expression<Func<T, bool>> criteria)
: BaseSpecification<T>(criteria), ISpecification<T, TResult>
{
    protected BaseSpecification() : this(null!)//constructeur par defaut
    {

    }

    public Expression<Func<T, TResult>>? Select { get; private set; }

    protected void AddSelect(Expression<Func<T, TResult>> selectExpression)
    {
        Select = selectExpression;
    }
}   
