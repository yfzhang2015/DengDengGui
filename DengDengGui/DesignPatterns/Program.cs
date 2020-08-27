using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace DesignPatterns
{
    class Program
    {
      

        static void Main()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //观察者模式
            //观察者模式.观察者模式_消息通知.Start();

            //装饰模式.装饰模式_Tax.Start();
            // return;


            // 职责链模式.职责链模式_挂号.Start();
            // return;

            while (true)
            {
                Console.WriteLine("=======================================================================================================");
                Console.WriteLine(@"1、简单工厂  2、策略模式  3、装饰模式  4、代理模式  5、工厂方法  6、原型模式  7、模板方法  8、外观模式 
9、建造者模式  10、观察者模式  11、抽象工厂  12、状态模式  13、适配模式  14、备忘录模式  15、组合模式  
16、迭代器模式  17、单例模式  18、桥接模式  19、命令模式  20、职责链模式  21、中介者模式  22、享元模式
23、解释器模式  24、访问者模式");
                Console.WriteLine("=======================================================================================================");
                Console.WriteLine("选择模式编号：");
                switch (Console.ReadLine())
                {
                    case "1":
                        Invock1();
                        break;
                    case "2":
                        Invock2();
                        break;
                    case "3":
                        Invock3();
                        break;
                    case "4":
                        Invock4();
                        break;
                    case "5":
                        Invock5();
                        break;
                    case "6":
                        Invock6();
                        break;
                    case "7":
                        Invock7();
                        break;
                    case "8":
                        Invock8();
                        break;
                    case "9":
                        Invock9();
                        break;
                    case "10":
                        Invock10();
                        break;
                    case "11":
                        Invock11();
                        break;
                    case "12":
                        Invock12();
                        break;
                    case "13":
                        Invock13();
                        break;
                    case "14":
                        Invock14();
                        break;
                    case "15":
                        Invock15();
                        break;
                    case "16":
                        Invock16();
                        break;
                    case "17":
                        Invock17();
                        break;
                    case "18":
                        Invock18();
                        break;
                    case "19":
                        Invock19();
                        break;
                    case "20":
                        Invock20();
                        break;
                    case "21":
                        Invock21();
                        break;
                    case "22":
                        Invock22();
                        break;
                    case "23":
                        Invock23();
                        break;
                    case "24":
                        Invock24();
                        break;
                }
            }
        }
        #region 简单工厂客户端      
        static void Invock1()
        {
            var opertion = OperationFactory.CreateOperate(1);
            opertion.GetResult();
        }
        #endregion
        #region 策略模式客户端    
        static void Invock2()
        {
            var contextA = new Context("1");
            contextA.GetCompute();

            var contextB = new Context("2");
            contextB.GetCompute();
        }
        #endregion
        #region 装饰模式客户端   
        static void Invock3()
        {
            //基础功能类
            var c = new ConcreteComponent();
            //装饰A类
            var dA = new ConcreteDecoratorA();
            //装饰B类
            var dB = new ConcreteDecoratorB();
            dA.Component = c;
            dB.Component = dA;
            dB.Operation();
        }
        #endregion
        #region 代理模式客户端  
        static void Invock4()
        {
            var proxy = new Proxy();
            proxy.Request();
        }
        #endregion
        #region 工厂方法客户端  
        static void Invock5()
        {
            IFactory funFactoryA = new FactoryA();
            Function functionA = funFactoryA.CreateFuntion();
            functionA.Operation();

            IFactory funFactoryB = new FactoryB();
            Function functionB = funFactoryB.CreateFuntion();
            functionB.Operation();
        }
        #endregion
        #region 原型模式客户端  
        static void Invock6()
        {
            //同类不同对象复制
            var pa1 = new ConcretePrototypeA("abc");
            var clone_pa1 = (ConcretePrototypeA)pa1.Clone();
            Console.WriteLine($"pa1.id=:{pa1.ID}");
            Console.WriteLine($"clone_pa1.id={clone_pa1.ID}");

            var pa2 = new ConcretePrototypeA("123");
            var clone_pa2 = (ConcretePrototypeA)pa2.Clone();
            Console.WriteLine($"pa2.id=:{pa2.ID}");
            Console.WriteLine($"clone_pa2.id={clone_pa2.ID}");


            //不同类复制
            var pb1 = new ConcretePrototypeB("!@#$%^&*()_+");
            var clone_pb1 = (ConcretePrototypeB)pb1.Clone();
            Console.WriteLine($"pb1.id=:{pb1.ID}");
            Console.WriteLine($"clone_pb1.id={clone_pb1.ID}");


        }
        #endregion
        #region 模板方法模式客户端  
        static void Invock7()
        {
            AbstractClass abC;
            abC = new ConcreteClassA();
            abC.TemplateMethod();

            abC = new ConcreteClassB();
            abC.TemplateMethod();
        }
        #endregion
        #region 外观模式客户端  
        static void Invock8()
        {
            var facade = new Facade();
            facade.FacadeOne();
            facade.FacadeTow();
        }
        #endregion
        #region 建造者模式客户端  
        static void Invock9()
        {
            var shapBuilder = new CircleBuilder();
            var shapeDirector = new ShapeDirector(shapBuilder);
            shapeDirector.DrawShape();
        }
        #endregion
        #region 观察者模式客户端  
        static void Invock10()
        {
            var aObsSub = new AObsSubject();
            aObsSub.Attach(new AObserver(aObsSub));
            aObsSub.Attach(new BObserver(aObsSub));
            aObsSub.Notify();
            Console.WriteLine("----------------------");
            var bObsSub = new BObsSubject();
            bObsSub.Attach(new AObserver(bObsSub));
            bObsSub.Attach(new BObserver(bObsSub));
            bObsSub.Attach(new AObserver(bObsSub));
            bObsSub.Notify();
        }
        #endregion
        #region 抽象工厂模式客户端  
        static void Invock11()
        {
            var department = new Department();
            IDataBaseFactory factory = new OracleFactory();// new SqlserverFactory();
            IDepartment departmentOpt = factory.CreateDepartment();
            departmentOpt.Insert(department);
            departmentOpt.GetDepartment(10);

        }
        #endregion
        #region 状态模式客户端  
        static void Invock12()
        {
            StateContext context = new StateContext(new StateA());
            context.Request();
            context.Request();
            context.Request();
            context.Request();
            context.Request();
            context.Request();
        }
        #endregion
        #region 适配模式客户端  
        static void Invock13()
        {
            Target target = new Adapter();
            target.Request();
        }
        #endregion
        #region 备忘录模式客户端  
        static void Invock14()
        {
            Console.WriteLine("原始状态数据：");
            var ori = new Originator
            {
                State = "开"
            };
            ori.Show();

            Console.WriteLine("备忘状态：");
            var car = new Caretaker
            {
                Memento = ori.CreateMemento()
            };
            Console.WriteLine("改变原对象状态后：");
            ori.State = "关";
            ori.Show();

            Console.WriteLine("恢复对象状态后：");
            ori.SetMemento(car.Memento);
            ori.Show();

        }
        #endregion
        #region 组合模式客户端  
        static void Invock15()
        {
            var root = new Composite("root");
            root.Add(new Leaf("Leaf A"));
            root.Add(new Leaf("Leaf B"));

            var comp1 = new Composite("Composite1");
            comp1.Add(new Leaf("Composite1 Leaf A"));
            comp1.Add(new Leaf("Composite1 Leaf B"));
            root.Add(comp1);

            var comp2 = new Composite("Composite2");
            comp2.Add(new Leaf("Composite2 Leaf A"));
            comp2.Add(new Leaf("Composite2 Leaf B"));
            comp1.Add(comp2);

            root.Add(new Leaf("Leaf C"));
            root.Add(new Leaf("Leaf D"));

            root.Display();
        }
        #endregion
        #region 迭代器模式客户端  
        static void Invock16()
        {
            var conAgg = new ConcreteAggregate();
            conAgg[0] = "aaaa";
            conAgg[1] = "bbbb";
            conAgg[2] = "cccc";
            conAgg[3] = "dddd";
            conAgg[4] = "eeee";
            conAgg[5] = "ffff";
            conAgg[6] = "hhhh";

            Iterator iterator = new ConcreteIterator(conAgg);
            object item = iterator.First();
            Console.WriteLine($"First:{item}");
            while (!iterator.IsDone())
            {
                Console.WriteLine($"{iterator.CurrentItem()}来了");
                iterator.Next();
            }
        }
        #endregion
        #region 单例模式客户端  
        static void Invock17()
        {

            Singleton s1 = Singleton.GetInstance();
            Singleton s2 = Singleton.GetInstance();
            if (s1 == s2)
            {
                Console.WriteLine("s1==s2");
            }
            else
            {
                Console.WriteLine("s1!=s2");
            }
        }
        #endregion
        #region 桥接模式客户端  
        static void Invock18()
        {

            Abstraction abs = new RefinedAbstraction();
            abs.SetImplementor(new ImplementorA());
            abs.Operation();
            abs.SetImplementor(new ImplementorB());
            abs.Operation();
        }
        #endregion
        #region 命令模式客户端  
        static void Invock19()
        {
            var rec = new Receiver();
            var cmd = new ConcreteCommand(rec);
            var inv = new Invoker();
            inv.SetCommand(cmd);
            inv.ExecuteCommand();
        }
        #endregion
        #region 职责链模式客户端  
        static void Invock20()
        {

            var h1 = new ConcreteHandler1();
            var h2 = new ConcreteHandler2();
            var h3 = new ConcreteHandler3();
            h1.SetSuccessor(h2);
            h2.SetSuccessor(h3);
            var requests = new int[] { 3, 5, 7, 12, 19, 22, 23, 29 };
            foreach (var i in requests)
            {
                h1.HandleRequest(i);
            }
        }
        #endregion
        #region 中介者模式客户端  
        static void Invock21()
        {
            ConcreteMediator mediator = new ConcreteMediator();
            var colleague1 = new ConcreteColleague1(mediator);
            var colleague2 = new ConcreteColleague2(mediator);

            mediator.Colleague1 = colleague1;
            mediator.Colleague2 = colleague2;

            colleague1.Send("colleague1 send了一个message");
            colleague2.Send("colleague1 send了一个message");
        }
        #endregion
        #region 享元模式客户端  
        static void Invock22()
        {
            int extrinsicstate = 22;
            var flyweightFactory = new FlyweightFactory();

            var flyweightX = flyweightFactory.GetFlyweight("x");
            flyweightX.Operation(--extrinsicstate);
            var flyweightY = flyweightFactory.GetFlyweight("y");
            flyweightY.Operation(--extrinsicstate);
            var flyweightZ = flyweightFactory.GetFlyweight("z");
            flyweightZ.Operation(--extrinsicstate);
            var unFlyweight = new UnSharedConcreteFlyweight();
            unFlyweight.Operation(--extrinsicstate);
        }
        #endregion
        #region 解释器模式客户端  
        static void Invock23()
        {

            ExtContext context = new ExtContext();
            var list = new List<AbstractExperssion>
            {
                new TerminalExpression(),
                new NoterminalExpression(),
                new TerminalExpression(),
                new TerminalExpression()
            };

            foreach (var exp in list)
            {
                exp.Interpret(context);
            }
        }
        #endregion
        #region 访问者模式客户端  
        static void Invock24()
        {
            ObjectStructure ostr = new ObjectStructure();
            ostr.Attach(new ConcreteElementA());
            ostr.Attach(new ConcreteElementB());
            ConcreteVisitor1 v1 = new ConcreteVisitor1();
            ConcreteVisitor2 v2 = new ConcreteVisitor2();

            ostr.Accept(v1);
            ostr.Accept(v2);
        }
        #endregion
    }
}