﻿using System;
using System.Collections.Generic;

namespace Machine.Core.ValueTypes
{
  public class ValueHelper
  {
    private static readonly ObjectEqualityFunctionFactory _objectEqualityFunctionFactory = new ObjectEqualityFunctionFactory();
    private static readonly CalculateHashCodeFunctionFactory _calculateHashCodeFunctionFactory = new CalculateHashCodeFunctionFactory();
    private static readonly Dictionary<Type, CacheEntry> _cache = new Dictionary<Type, CacheEntry>();

    private class CacheEntry
    {
      public ObjectEqualityFunction AreEqual;
      public CalculateHashCodeFunction CalculateHashCode;
    }

    public static bool AreEqual<TType>(TType a, TType b)
    {
      return Lookup(typeof(TType)).AreEqual(a, b);
    }

    public static bool AreEqual<TType>(object a, object b)
    {
      if (a is TType && b is TType)
      {
        return Lookup(typeof(TType)).AreEqual(a, b);
      }
      return false;
    }

    public static bool AreEqual(object a, object b)
    {
      throw new InvalidOperationException("You must use generic version of AreEqual!");
    }

    public static Int32 GetHashCode<TType>(TType a)
    {
      return Lookup(typeof(TType)).CalculateHashCode(a);
    }

    private static CacheEntry Lookup(Type type)
    {
      if (_cache.ContainsKey(type))
      {
        return _cache[type];
      }
      _cache[type] = new CacheEntry();
      _cache[type].AreEqual = _objectEqualityFunctionFactory.CreateObjectEqualityFunction(type);
      _cache[type].CalculateHashCode = _calculateHashCodeFunctionFactory.CreateCalculateHashCodeFunction(type);
      return _cache[type];
    }
  }
}
