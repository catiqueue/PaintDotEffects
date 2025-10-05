using System;
using System.Linq.Expressions;
using System.Reflection;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Binding;

public delegate void Setter<in TModel, in TProperty>(TModel instance, TProperty value);

internal static class Setter {
  public static Setter<TModel, TProperty> Create<TModel, TProperty>(Expression<Func<TModel, TProperty>> selector) 
  where TModel : class 
    => selector.CreateSetter();
}



file static class ExpressionExtensions {
  private static void ValidateSelector<TModel, TProperty>(this Expression<Func<TModel, TProperty>> selector, out PropertyInfo propertyInfo) 
  where TModel: class {
    if (selector.Body is not MemberExpression memberExpression)
      throw new ArgumentException("The expression must be a simple property access.", nameof(selector)); 
    if (memberExpression.Member is not PropertyInfo checkedPropertyInfo) 
      throw new ArgumentException("The member must be a property.", nameof(selector));
    if (!checkedPropertyInfo.CanWrite)
      throw new ArgumentException($"Property '{checkedPropertyInfo.Name}' does not have a public setter.", nameof(selector));
    
    propertyInfo = checkedPropertyInfo;
  }
  
  internal static Setter<TModel, TProperty> CreateSetter<TModel, TProperty>(this Expression<Func<TModel, TProperty>> selector) 
  where TModel: class { 
    ValidateSelector(selector, out var propertyInfo);
    
    var instanceParam = Expression.Parameter(typeof(TModel), "instance");
    var valueParam = Expression.Parameter(typeof(TProperty), "value");
    var propertyAccess = Expression.Property(instanceParam, propertyInfo);
    var assignExpression = Expression.Assign(propertyAccess, valueParam);
    var setterExpression = Expression.Lambda<Action<TModel, TProperty>>(
      assignExpression,
      instanceParam,
      valueParam);
    
    return setterExpression.Compile().Invoke;
  }
}
