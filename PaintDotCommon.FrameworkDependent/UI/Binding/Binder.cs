using System;
using System.Linq.Expressions;
using catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Binding;

public delegate void Binder<in TSettings>(TSettings settings, PropertyCollection properties) ;

internal static class Binder {
  public static Binder<TSettings> CreateDirect<TSettings, TTarget>(
    ValueNodeBase<TTarget> node,
    Expression<Func<TSettings, TTarget>> selector) 
    where TSettings: class 
  {
    var setter = Setter.Create(selector);
    return (settings, properties) => setter(settings, properties.GetPropertyValue<TTarget>(node.Name));
  }

  public static Binder<TSettings> CreateComplex<TSettings, TFirst, TSecond, TTarget>(
    ValueNodeBase<TFirst> first,
    ValueNodeBase<TSecond> second,
    Expression<Func<TSettings, TTarget>> selector,
    Func<TFirst, TSecond, TTarget> producer)
    where TSettings: class
  {
    var setter = Setter.Create(selector);
    return (settings, properties) => setter(settings, 
      producer(
        properties.GetPropertyValue<TFirst>(first.Name), 
        properties.GetPropertyValue<TSecond>(second.Name)));
  }
  
  public static Binder<TSettings> CreateComplex<TSettings, TFirst, TSecond, TThird, TTarget>(
    ValueNodeBase<TFirst> first,
    ValueNodeBase<TSecond> second,
    ValueNodeBase<TThird> third,
    Expression<Func<TSettings, TTarget>> selector,
    Func<TFirst, TSecond, TThird, TTarget> producer)
    where TSettings: class
  {
    var setter = Setter.Create(selector);
    return (settings, properties) => setter(settings, 
      producer(
        properties.GetPropertyValue<TFirst>(first.Name), 
        properties.GetPropertyValue<TSecond>(second.Name),
        properties.GetPropertyValue<TThird>(third.Name)));
  }
  
  public static Binder<TSettings> CreateComplex<TSettings, TFirst, TSecond, TThird, TFourth, TTarget>(
    ValueNodeBase<TFirst> first,
    ValueNodeBase<TSecond> second,
    ValueNodeBase<TThird> third,
    ValueNodeBase<TFourth> fourth,
    Expression<Func<TSettings, TTarget>> selector,
    Func<TFirst, TSecond, TThird, TFourth, TTarget> producer)
    where TSettings: class
  {
    var setter = Setter.Create(selector);
    return (settings, properties) => setter(settings, 
      producer(
        properties.GetPropertyValue<TFirst>(first.Name), 
        properties.GetPropertyValue<TSecond>(second.Name),
        properties.GetPropertyValue<TThird>(third.Name),
        properties.GetPropertyValue<TFourth>(fourth.Name)));
  }
  
  public static Binder<TSettings> CreateComplex<TSettings, TFirst, TSecond, TThird, TFourth, TFifth, TTarget>(
    ValueNodeBase<TFirst> first,
    ValueNodeBase<TSecond> second,
    ValueNodeBase<TThird> third,
    ValueNodeBase<TFourth> fourth,
    ValueNodeBase<TFifth> fifth,
    Expression<Func<TSettings, TTarget>> selector,
    Func<TFirst, TSecond, TThird, TFourth, TFifth, TTarget> producer)
    where TSettings: class
  {
    var setter = Setter.Create(selector);
    return (settings, properties) => setter(settings, 
      producer(
        properties.GetPropertyValue<TFirst>(first.Name), 
        properties.GetPropertyValue<TSecond>(second.Name),
        properties.GetPropertyValue<TThird>(third.Name),
        properties.GetPropertyValue<TFourth>(fourth.Name),
        properties.GetPropertyValue<TFifth>(fifth.Name)));
  }
  
  public static Binder<TSettings> CreateComplex<TSettings, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TTarget>(
    ValueNodeBase<TFirst> first,
    ValueNodeBase<TSecond> second,
    ValueNodeBase<TThird> third,
    ValueNodeBase<TFourth> fourth,
    ValueNodeBase<TFifth> fifth,
    ValueNodeBase<TSixth> sixth,
    Expression<Func<TSettings, TTarget>> selector,
    Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TTarget> producer)
    where TSettings: class
  {
    var setter = Setter.Create(selector);
    return (settings, properties) => setter(settings, 
      producer(
        properties.GetPropertyValue<TFirst>(first.Name), 
        properties.GetPropertyValue<TSecond>(second.Name),
        properties.GetPropertyValue<TThird>(third.Name),
        properties.GetPropertyValue<TFourth>(fourth.Name),
        properties.GetPropertyValue<TFifth>(fifth.Name),
        properties.GetPropertyValue<TSixth>(sixth.Name)));
  }
}