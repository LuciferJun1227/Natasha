﻿using Natasha;
using System;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
             *   在此之前，你需要右键，选择工程文件，在你的.csproj里面 
             *   
             *   写上这样一句浪漫的话： 
             *   
             *      <PreserveCompilationContext>true</PreserveCompilationContext>
             */
            /*  var delegateAction = MethodBuilder.NewMethod
                  .Using(typeof(Console))
                  .Param<string>("str1")
                  .Param<string>("str2")
                  .Body(@"
                      string result = str1 +"" ""+ str2;
                      Console.WriteLine(result);
                                                 ")
                  .Return()
                  .Create();

              ((Action<string, string>)delegateAction)("Hello", "World!");

              var delegateAction2 = MethodBuilder.NewMethod
                  .Using(typeof(Console))
                  .Param<string>("str1")
                  .Param<string>("str2")
                  .Body(@"
                      string result = str1 +"" ""+ str2;
                      Console.WriteLine(result);
                                                 ")
                  .Return()
                  .Create<Action<string, string>>();

              delegateAction2("Hello", "World!");

              string text = @"using System;
  using System.Collections;
  using System.Linq;
  using System.Text;

  namespace HelloWorld
  {
      public class Test
      {
          public Test(){
              Name=""111"";
          }

          public string Name;
          public int Age{get;set;}
      }
  }";
              //根据脚本创建动态类
              Type type = ClassBuilder.GetType(text);
              //创建动态类实例代理
              DynamicInstance instance = new DynamicInstance(type);

              if (instance["Name"].StringValue=="111")
              {
                  //调用动态委托赋值
                  instance["Name"].StringValue = "222";
              }
              //调用动态类
              Console.WriteLine(instance["Name"].StringValue);




              //创建动态类实例代理
              DynamicInstance<TestB> instance2 = new DynamicInstance<TestB>();

              if (instance2["Name"].StringValue == "111")
              {
                  //调用动态委托赋值
                  instance2["Name"].StringValue = "222";
              }
              //调用动态类
              Console.WriteLine(instance2["Name"].StringValue);


              OopModifier<ITestA> interfaceBuilder = new OopModifier<ITestA>();
              interfaceBuilder.ClassName("TestClass");
              interfaceBuilder["Abc"] = "Console.WriteLine(\"ITest\");";
              interfaceBuilder["Get"] = "return 12345;";
              interfaceBuilder["Set"] = "return abc;";
              interfaceBuilder.Compile();

              var test = interfaceBuilder.Create("TestClass");
              test = OopModifier<ITestA>.New("TestClass");
              Console.WriteLine(test.Get());
              test.Abc();
              Console.WriteLine(test.Set("hello world"));*/
            Console.ReadKey();
            for (int i = 0; i < 500; i++)
            {
                var delegateAction2 = FastMethod.New.UseBodyTemplate(t=>t
                 .Param<string>("str1")
                 .Param<string>("str2")
                 .Body(@"
                      string result = str1 +"" ""+ str2;
                      Console.WriteLine(result);
                                                 ")
                 .Return())
                 .Create<Action<string, string>>();

                delegateAction2("Hello", "World!");
            }
            Console.WriteLine("Complete!");
            Console.ReadKey();
        }
    }
    public interface ITestA
    {
        int Get();
        string Set(string abc);
        void Abc();
    }
    public class TestB
    {
        public TestB()
        {
            Name = "111";
        }
        public string Name { get; set; }
        public int Age;
    }

   
}
