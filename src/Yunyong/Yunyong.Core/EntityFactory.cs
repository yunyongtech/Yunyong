﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using Yunyong.Core.ViewModels;

namespace Yunyong.Core
{
    public static class EntityFactory
    {
        public static TEntity Create<TEntity>(Guid? id = null) where TEntity : Entity, new()
        {
            var tmp = new TEntity
            {
                Id = id ?? GuidUtil.NewSequentialId(),
                CreatedOn = DateTimeUtil.GetCurrentTime()
            };
            return tmp;
        }

        public static TEntity Create<TEntity, TVM>(TVM vm, Guid? id = null) where TEntity : Entity, new()
            where TVM : CreateVM
        {
            var tmp = new TEntity
            {
                Id = id ?? GuidUtil.NewSequentialId(),
                CreatedOn = DateTimeUtil.GetCurrentTime()
            };
            if (vm == null)
            {
                return tmp;
            }

            var sourceProps = vm.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                .ToDictionary(a => a.Name, a => a);
            var targetProps = typeof(TEntity).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                .ToDictionary(a => a.Name, a => a);

            var sourceNames = sourceProps.Keys;
            var targetNames = targetProps.Keys;
            var mixedNames = targetNames.Intersect(sourceNames);

            foreach (var name in mixedNames)
            {
                var sourceProp = sourceProps[name];

                try
                {
                    var sourceVal = sourceProp.GetValue(vm);
                    if (sourceVal != null)
                    {
                        var targetProp = targetProps[name];
                        targetProp.SetValue(tmp, sourceVal);
                    }
                }
                catch
                {
                }
            }

            return tmp;
        }

        public static IEnumerable<TEntity> Create<TEntity, TVM>(IEnumerable<TVM> vms)
            where TEntity : Entity, new()
            where TVM : CreateVM
        {
            return vms.Select(a => Create<TEntity, TVM>(a));
        }

        public static TEntity Create<TEntity>(this CreateVM vm, Guid? id = null) where TEntity : Entity, new()
        {
            var tmp = new TEntity
            {
                Id = id ?? GuidUtil.NewSequentialId(),
                CreatedOn = DateTimeUtil.GetCurrentTime()
            };
            if (vm == null)
            {
                return tmp;
            }

            var sourceProps = vm.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                .ToDictionary(a => a.Name, a => a);
            var targetProps = typeof(TEntity).GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                .ToDictionary(a => a.Name, a => a);

            var sourceNames = sourceProps.Keys;
            var targetNames = targetProps.Keys;
            var mixedNames = targetNames.Intersect(sourceNames);

            foreach (var name in mixedNames)
            {
                var sourceProp = sourceProps[name];

                try
                {
                    var sourceVal = sourceProp.GetValue(vm);
                    if (sourceVal != null)
                    {
                        var targetProp = targetProps[name];
                        targetProp.SetValue(tmp, sourceVal);
                    }
                }
                catch
                {
                }
            }

            return tmp;
        }

        public static IEnumerable<TEntity> Create<TEntity>(this IEnumerable<CreateVM> vms) where TEntity : Entity, new()
        {
            return vms.Select(a => a.Create<TEntity>());
        }
    }
}