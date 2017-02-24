﻿using System;
using System.Linq;
using EcsRx.Json;
using EcsRx.Persistence.Data;
using EcsRx.Persistence.Transformers;
using EcsRx.Persistence.Types;
using NSubstitute;
using NUnit.Framework;

namespace Assets.Tests.Editor.Persistence
{
    [TestFixture]
    public class JsonTransformerTests
    {
        [Test]
        public void should_correctly_serialize_entity_data()
        {
            var entityData = new EntityData { EntityId = Guid.NewGuid() };
            entityData.Data.Add("npc.name", "Bob");
            entityData.Data.Add("stats.health", 20);
            entityData.Data.Add("stats.magic", 10);
            entityData.Data.Add("moveable.position", new[] {1.5f, 2.1f, 3.3f });
            entityData.Data.Add("pathfinder.route", new[]
            {
                new [] { 1.0f, 1.0f, 1.0f },
                new [] { 2.0f, 2.0f, 2.0f },
                new [] { 3.0f, 3.0f, 3.0f },
                new [] { 4.0f, 4.0f, 4.0f }
            });
            entityData.Data.Add("activequest.quests", new[]
            {
                new object[] { 1, "hello" }
            });

            var dummyComponentTypeRegistry = Substitute.For<IComponentTypeRegistry>();
            var dummyComponentDescriptorRegistry = Substitute.For<IComponentDescriptorRegistry>();
            var transformer = new JsonTransformer(dummyComponentTypeRegistry, dummyComponentDescriptorRegistry);
            var jsonEntity = transformer.TransformEntity(entityData);
            Console.WriteLine(jsonEntity.ToString());

            Assert.That(jsonEntity[transformer.EntityIdKey].Value, Is.EqualTo(entityData.EntityId.ToString()));

            var jsonData = jsonEntity[transformer.DataKey];
            Assert.That(jsonData.Count, Is.EqualTo(5));

            Assert.That(jsonData["npc.name"].Value, Is.EqualTo("Bob"));
            Assert.That(jsonData["stats.health"].Value, Is.EqualTo("20"));
            Assert.That(jsonData["stats.magic"].Value, Is.EqualTo("10"));
            Assert.That(jsonData["moveable.position"].AsArray.Count, Is.EqualTo(3));

            var positionArray = jsonData["moveable.position"].AsArray;
            Assert.That(positionArray[0].Value, Is.EqualTo("1.5"));
            Assert.That(positionArray[1].Value, Is.EqualTo("2.1"));
            Assert.That(positionArray[2].Value, Is.EqualTo("3.3"));

            var pathfinderArray = jsonData["pathfinder.route"].AsArray;
            Assert.That(pathfinderArray.Count, Is.EqualTo(4));

            var position1Array = pathfinderArray[0].AsArray;
            Assert.That(position1Array[0].Value, Is.EqualTo("1"));
            Assert.That(position1Array[1].Value, Is.EqualTo("1"));
            Assert.That(position1Array[2].Value, Is.EqualTo("1"));

            var position2Array = pathfinderArray[1].AsArray;
            Assert.That(position2Array[0].Value, Is.EqualTo("2"));
            Assert.That(position2Array[1].Value, Is.EqualTo("2"));
            Assert.That(position2Array[2].Value, Is.EqualTo("2"));

            var position3Array = pathfinderArray[2].AsArray;
            Assert.That(position3Array[0].Value, Is.EqualTo("3"));
            Assert.That(position3Array[1].Value, Is.EqualTo("3"));
            Assert.That(position3Array[2].Value, Is.EqualTo("3"));

            var position4Array = pathfinderArray[3].AsArray;
            Assert.That(position4Array[0].Value, Is.EqualTo("4"));
            Assert.That(position4Array[1].Value, Is.EqualTo("4"));
            Assert.That(position4Array[2].Value, Is.EqualTo("4"));
        }

        [Test]
        public void should_correctly_deserialize_entity_data()
        {
            var entityString = "{ \"entityId\":\"67c12ef1-5d0b-4eee-9432-838efb0c958b\", \"data\": { \"npc.name\":\"Bob\", \"stats.health\":20,	\"moveable.position\":[\"1.5\", \"2.1\", \"3.3\"] }}";
            var jsonEntity = JSON.Parse(entityString);
            Console.WriteLine(jsonEntity.ToString());

            var dummyComponentTypeRegistry = Substitute.For<IComponentTypeRegistry>();
            var dummyComponentDescriptorRegistry = Substitute.For<IComponentDescriptorRegistry>();
            var transformer = new JsonTransformer(dummyComponentTypeRegistry, dummyComponentDescriptorRegistry);
            var entityData = transformer.TransformEntity(jsonEntity);

            Console.WriteLine(entityData.ToString());
        }
    }
}