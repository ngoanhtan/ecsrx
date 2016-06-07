﻿using EcsRx.Entities;
using EcsRx.Pools;
using EcsRx.Unity.Systems;
using UnityEngine;

namespace Assets.Examples.AutoRegisterSystems.Systems
{
    public class DefaultViewResolver : ViewResolverSystem
    {
        public DefaultViewResolver(IPoolManager poolManager) : base(poolManager)
        { }

        public override GameObject ResolveView(IEntity entity)
        {
            var view = GameObject.CreatePrimitive(PrimitiveType.Cube);
            view.name = "entity-" + entity.Id;
            return view;
        }
    }
}