using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns
{
    /****************************************************************************
    * 享元模式
    * 运用共享技术有效地支持“大量”细粒度对象
    ****************************************************************************/
    /// <summary>
    /// 享元类
    /// </summary>
    public abstract class Flyweight
    {
        public abstract void Operation(int extrinsicstate);
    }
    /// <summary>
    /// 具体享元类
    /// </summary>
    public class ConcreteFlyweight : Flyweight
    {
        public override void Operation(int extrinsicstate)
        {
            Console.WriteLine($"具体Flyweight:{extrinsicstate}");
        }
    }
    /// <summary>
    /// 不享元类
    /// </summary>
    public class UnSharedConcreteFlyweight : Flyweight
    {
        public override void Operation(int extrinsicstate)
        {
            Console.WriteLine($"不共享具体Flyweight:{extrinsicstate}");
        }
    }
    public class FlyweightFactory
    {
        readonly Dictionary<string, Flyweight> flyweights;
        public FlyweightFactory()
        {
            //把共享的对象提前实例化好，放在字典集合中，需要时按key取出使用，不用大量实例化
            flyweights = new Dictionary<string, Flyweight>
            {
                { "x", new ConcreteFlyweight() },
                { "y", new ConcreteFlyweight() },
                { "z", new ConcreteFlyweight() }
            };
        }
        public Flyweight GetFlyweight(string key)
        {
            return flyweights[key];
        }
    }
}
