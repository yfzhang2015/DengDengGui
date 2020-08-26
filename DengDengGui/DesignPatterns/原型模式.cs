using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns
{
    /****************************************************************************
    * 原型模式
    * 用原型实例指定创建对象的种类，并且通过拷贝这些原型创建新对象。
    ****************************************************************************/
    /// <summary>
    /// 抽象类
    /// </summary>
    public abstract class Prototype
    {       
        public Prototype(string id)
        {
            ID = id;
        }
        public string ID
        {
            get;
            private set;
        }
        public abstract Prototype Clone();
    }
    /// <summary>
    /// 子类A
    /// </summary>
    public class ConcretePrototypeA : Prototype
    {        
        public ConcretePrototypeA(string id) : base(id)
        {
        }
        public override Prototype Clone()
        {
            Console.WriteLine("调用ConcretePrototypeA.Clone方法");
            return (Prototype)this.MemberwiseClone();
        }
    }

    /// <summary>
    /// 子类B
    /// </summary>
    public class ConcretePrototypeB : Prototype
    {
        public ConcretePrototypeB(string id) : base(id)
        {
        }
        public override Prototype Clone()
        {
            Console.WriteLine("调用ConcretePrototypeB.Clone方法");
            return (Prototype)this.MemberwiseClone();
        }
    }
}
