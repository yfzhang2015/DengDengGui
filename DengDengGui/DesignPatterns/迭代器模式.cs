using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns
{
    /****************************************************************************
     * 迭代器模式
     * 提供一种方法顺序访问一个聚合对象中各个元素，而又不是暴露访对象内部表示
     ****************************************************************************/

    /// <summary>
    /// 迭代器抽象类
    /// </summary>
    public abstract class Iterator
    {
        public abstract object First();
        public abstract object Next();

        public abstract bool IsDone();
        public abstract object CurrentItem();
    }
    public class ConcreteIterator : Iterator
    {
        readonly ConcreteAggregate _aggregate;
        int current;
        public ConcreteIterator(ConcreteAggregate aggregate)
        {
            _aggregate = aggregate;
        }
        public override object CurrentItem()
        {
            return _aggregate[current];
        }

        public override object First()
        {
            return _aggregate[0];
        }

        public override bool IsDone()
        {
            return current >= _aggregate.Count;
        }

        public override object Next()
        {
            current++;
            if (current < _aggregate.Count)
            {
                return _aggregate[current];
            }
            else
            {
                return null;
            }
        }
    }

    /// <summary>
    /// 聚集抽象类
    /// </summary>
    public abstract class Aggregate
    {
        public abstract Iterator CreateIterator();
    }

    public class ConcreteAggregate : Aggregate
    {
        readonly IList<object> _items;
        public ConcreteAggregate()
        {
            _items = new List<object>();
        }
        public override Iterator CreateIterator()
        {
            return new ConcreteIterator(this);
        }

        public int Count
        {
            get
            {
                return _items.Count;
            }
        }
        public object this[int index]
        {
            get
            {
                return _items[index];
            }
            set
            {
                _items.Insert(index, value);
            }
        }
    }


}
