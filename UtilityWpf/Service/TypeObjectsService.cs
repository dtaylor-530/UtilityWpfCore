﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilityInterface.NonGeneric;
using UtilityWpf.Abstract;
using UtilityWpf.Model;

namespace UtilityWpf.Service
{

    public class TypeObjectsService: ITypeObjectsService
    {
        public virtual TypeObject[] SelectTypeObjects(Type[] types)
        {
            var xs = types
                   .SelectMany(type =>
                   {
                       var arr = Splat.Locator.Current.GetServices(type).Select(a => (a, type)).ToArray();
                       if (arr.Length == 0 && type.GetConstructor(Type.EmptyTypes) != null)
                           return new[] { (Activator.CreateInstance(type), type) };
                       return arr;
                   }).ToArray();

            return xs.Select(st =>
            {
                var (service, type) = st;
                var name = typeof(IName).IsAssignableFrom(type) ?
                                                (service as IName).Name :
                                                 type.Name;
                return new TypeObject { TypeName = type.Name, Key = name, Object = service, Type = type };
            }).ToArray();
        }
    }
}
