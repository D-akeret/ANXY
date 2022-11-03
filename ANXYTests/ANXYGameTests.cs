﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using ANXY;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ANXY.ECS.Components;
using ANXY.ECS;
using Microsoft.Xna.Framework;

namespace ANXY.Tests
{
    [TestClass()]
    public class ANXYGameTests
    {
        [TestMethod()]
        public void IsMouseVisible()
        {
            ANXYGame anxyGame = new ANXYGame();
            Assert.IsTrue(anxyGame.IsMouseVisible);
        }
        [TestMethod()]
        public void TestTransformSystemUpdate()
        {
            ANXYGame anxyGame = new ANXYGame();
            Vector2 vectorBefore = new Vector2(0, 0);
            Vector2 vectorAfter = new Vector2(2, 0);
            Entity testEntity = new Entity();
            testEntity.AddComponent(new Transform(vectorBefore,0,0));

            testEntity.GetComponent<Transform>().Translate(vectorAfter);
            Thread.Sleep(2000);

            Assert.AreEqual( vectorAfter, testEntity.GetComponent<Transform>().Position);
            Assert.AreEqual( vectorBefore, testEntity.GetComponent<Transform>().LastPosition);

        }
    }
}