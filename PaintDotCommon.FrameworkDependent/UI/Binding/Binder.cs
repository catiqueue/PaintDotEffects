using System;
using System.Linq.Expressions;
using catiqueue.PaintDotNet.Plugins.Common.UI.Nodes;
using PaintDotNet.IndirectUI;
using PaintDotNet.PropertySystem;

namespace catiqueue.PaintDotNet.Plugins.Common.UI.Binding;

public delegate void Binder<in TSettings>(TSettings settings, PropertyCollection properties) ;

internal static class Binder {
  public static Binder<TSettings> CreateDirect<TSettings, TTarget>(
    ValueNodeBase<TTarget> node,
    Setter<TSettings, TTarget> setter) 
    where TSettings: class 
    => (settings, properties) => setter(settings, properties.GetPropertyValue<TTarget>(node.Name));

  // If this keeps happening, I'll start to accept UiNodeBase directly.
  // For now, having a separate function will suffice
  public static Binder<TSettings> CreateForTabNumber<TSettings>(
    TabsetNode tabset,
    Setter<TSettings, int> setter) 
    => (settings, properties) 
      => setter(settings, properties.GetPropertyValue<TabContainerState>(tabset.Name).SelectedTabIndex + 1);

  public static Binder<TSettings> CreateMutating<TSettings, TValue, TTarget>(
    ValueNodeBase<TValue> node,
    Setter<TSettings, TTarget> setter,
    Func<TValue, TTarget> mutator) 
    where TSettings: class 
    => (settings, properties) => setter(settings, mutator(properties.GetPropertyValue<TValue>(node.Name)));

  public static Binder<TSettings> CreateComplex<TSettings, TFirst, TSecond, TTarget>(
    ValueNodeBase<TFirst> first,
    ValueNodeBase<TSecond> second,
    Setter<TSettings, TTarget> setter,
    Func<TFirst, TSecond, TTarget> producer)
    where TSettings: class 
    => (settings, properties) => setter(settings, producer(
      properties.GetPropertyValue<TFirst>(first.Name), 
      properties.GetPropertyValue<TSecond>(second.Name)));

  public static Binder<TSettings> CreateComplex<TSettings, TFirst, TSecond, TThird, TTarget>(
    ValueNodeBase<TFirst> first,
    ValueNodeBase<TSecond> second,
    ValueNodeBase<TThird> third,
    Setter<TSettings, TTarget> setter,
    Func<TFirst, TSecond, TThird, TTarget> producer)
    where TSettings: class 
    => (settings, properties) => setter(settings, producer(
      properties.GetPropertyValue<TFirst>(first.Name), 
      properties.GetPropertyValue<TSecond>(second.Name),
      properties.GetPropertyValue<TThird>(third.Name)));

  public static Binder<TSettings> CreateComplex<TSettings, TFirst, TSecond, TThird, TFourth, TTarget>(
    ValueNodeBase<TFirst> first,
    ValueNodeBase<TSecond> second,
    ValueNodeBase<TThird> third,
    ValueNodeBase<TFourth> fourth,
    Setter<TSettings, TTarget> setter,
    Func<TFirst, TSecond, TThird, TFourth, TTarget> producer)
    where TSettings: class 
    => (settings, properties) => setter(settings, producer(
      properties.GetPropertyValue<TFirst>(first.Name), 
      properties.GetPropertyValue<TSecond>(second.Name),
      properties.GetPropertyValue<TThird>(third.Name),
      properties.GetPropertyValue<TFourth>(fourth.Name)));

  public static Binder<TSettings> CreateComplex<TSettings, TFirst, TSecond, TThird, TFourth, TFifth, TTarget>(
    ValueNodeBase<TFirst> first,
    ValueNodeBase<TSecond> second,
    ValueNodeBase<TThird> third,
    ValueNodeBase<TFourth> fourth,
    ValueNodeBase<TFifth> fifth,
    Setter<TSettings, TTarget> setter,
    Func<TFirst, TSecond, TThird, TFourth, TFifth, TTarget> producer)
    where TSettings: class 
    => (settings, properties) => setter(settings, producer(
      properties.GetPropertyValue<TFirst>(first.Name), 
      properties.GetPropertyValue<TSecond>(second.Name),
      properties.GetPropertyValue<TThird>(third.Name),
      properties.GetPropertyValue<TFourth>(fourth.Name),
      properties.GetPropertyValue<TFifth>(fifth.Name)));

  public static Binder<TSettings> CreateComplex<TSettings, TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TTarget>(
    ValueNodeBase<TFirst> first,
    ValueNodeBase<TSecond> second,
    ValueNodeBase<TThird> third,
    ValueNodeBase<TFourth> fourth,
    ValueNodeBase<TFifth> fifth,
    ValueNodeBase<TSixth> sixth,
    Setter<TSettings, TTarget> setter,
    Func<TFirst, TSecond, TThird, TFourth, TFifth, TSixth, TTarget> producer)
    where TSettings: class 
    => (settings, properties) => setter(settings, producer(
      properties.GetPropertyValue<TFirst>(first.Name), 
      properties.GetPropertyValue<TSecond>(second.Name),
      properties.GetPropertyValue<TThird>(third.Name),
      properties.GetPropertyValue<TFourth>(fourth.Name),
      properties.GetPropertyValue<TFifth>(fifth.Name),
      properties.GetPropertyValue<TSixth>(sixth.Name)));
}