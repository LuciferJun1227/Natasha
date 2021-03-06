﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Mathematics;
using BenchmarkDotNet.Order;
using Natasha;
using NatashaBenchmark.Model;
using System;
using System.Reflection.Emit;

namespace NatashaBenchmark
{
    [MemoryDiagnoser, MarkdownExporter, RPlotExporter]
    [MinColumn, MaxColumn, MeanColumn, MedianColumn]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn(NumeralSystem.Arabic)]
    [CategoriesColumn]
    public class DynamicCallFieldTest
    {
        public Func<CallModel, string> EmitGetString;
        public Func<CallModel, DateTime> EmitGetDateTime;
        public Action<CallModel, DateTime> EmitSetDateTime;
        public Action<CallModel, string> EmitSetString;


        public Func<CallModel, string> NatashaGetString;
        public Func<CallModel, DateTime> NatashaGetDateTime;
        public Action<CallModel, DateTime> NatashaSetDateTime;
        public Action<CallModel, string> NatashaSetString;

        public CallModel EmitModel;
        public CallModel OriginModel;
        public CallModel NatashaModel;
        public CallModel NatashaProxyModel;

        public DynamicInstance<CallModel> NatashaCaller;
        public DynamicCallFieldTest()
        {
            Precache();
            Preheating();
        }
        public void Precache()
        {
            Type type = typeof(CallModel);
            DynamicMethod method = new DynamicMethod("GetString", typeof(string), new Type[] { type });
            ILGenerator il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, type.GetField("Age"));
            il.Emit(OpCodes.Ret);
            EmitGetString = (Func<CallModel, string>)(method.CreateDelegate(typeof(Func<CallModel, string>)));

            method = new DynamicMethod("GetDateTime", typeof(DateTime), new Type[] { type });
            il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldfld, type.GetField("CreateTime"));
            il.Emit(OpCodes.Ret);
            EmitGetDateTime = (Func<CallModel, DateTime>)(method.CreateDelegate(typeof(Func<CallModel, DateTime>)));


            method = new DynamicMethod("SetDateString", null, new Type[] { type, typeof(string) });
            il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, type.GetField("Age"));
            il.Emit(OpCodes.Ret);
            EmitSetString = (Action<CallModel, string>)(method.CreateDelegate(typeof(Action<CallModel, string>)));

            method = new DynamicMethod("SetDateTime", null , new Type[] { type,typeof(DateTime) });
            il = method.GetILGenerator();
            il.Emit(OpCodes.Ldarg_0);
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Stfld, type.GetField("CreateTime"));
            il.Emit(OpCodes.Ret);
            EmitSetDateTime = (Action<CallModel, DateTime>)(method.CreateDelegate(typeof(Action<CallModel, DateTime>)));

            NatashaGetString = Natasha
                .FastMethod
                .New
                .UseBodyTemplate(t=>t
                .Param<CallModel>("obj")
                .Body("return obj.Age;")
                .Return<string>())
                .Create<Func<CallModel, string>>();

            NatashaGetDateTime = Natasha
                 .FastMethod
                .New
                .UseBodyTemplate(t => t
                .Param<CallModel>("obj")
                .Body("return obj.CreateTime;")
                .Return<DateTime>())
                .Create<Func<CallModel, DateTime>>();

            NatashaSetString = Natasha
                .FastMethod
                .New
                .UseBodyTemplate(t => t
                .Param<CallModel>("obj")
                .Param<string>("str")
                .Body("obj.Age=str;")
                .Return())
                .Create<Action<CallModel, string>>();

            NatashaSetDateTime = Natasha
                .FastMethod
                .New
                .UseBodyTemplate(t => t
                .Param<CallModel>("obj")
                .Param<DateTime>("time")
                .Body("obj.CreateTime=time;")
                .Return())
                .Create<Action<CallModel, DateTime>>();
        }
        public void Preheating()
        {
            OriginModel = new CallModel();
            EmitModel = new CallModel();
            NatashaModel = new CallModel();
            NatashaProxyModel = new CallModel();
            //NatashaCaller = NatashaProxyModel;
        }
        /*
      #region 字段写性能

      [BenchmarkCategory("Write", "String"),Benchmark(Description = "Emit")]
      public void EmitFieldSetStringTest()
      {
          EmitSetString(EmitModel, "Hello");
      }
      [BenchmarkCategory("Write", "String"), Benchmark(Baseline = true, Description = "Origin")]
      public void OriginFieldSetStringTest()
      {
          OriginModel.Age = "Hello";
      }
      //[BenchmarkCategory("Write", "String"), Benchmark(Description = "NatashaProxy")]
      public void DynamicFieldProxySetStringTest()
      {
          NatashaCaller["Age"].StringValue = "Hello";
      }

      //[BenchmarkCategory("Write", "String"), Benchmark(Description = "NatashaDirectly")]
      public void DynamicFieldSetStringTest()
      {
          NatashaSetString(NatashaModel, "Hello");
      }



      [BenchmarkCategory("Write", "Time"), Benchmark(Description = "Emit")]
      public void EmitFieldSetTimeTest()
      {
          EmitSetDateTime(EmitModel, DateTime.Now);
      }

      //[BenchmarkCategory("Write", "Time"), Benchmark(Baseline = true, Description = "Origin")]
      public void OriginFieldSetTimeTest()
      {
          OriginModel.CreateTime = DateTime.Now;
      }
      //[BenchmarkCategory("Write", "Time"), Benchmark(Description = "NatashaProxy")]
      public void DynamicFieldProxySetTimeTest()
      {
          NatashaCaller["CreateTime"].DateTimeValue = DateTime.Now;
      }

      //[BenchmarkCategory("Write", "DateTime"), Benchmark(Description = "NatashaDirectly")]
      public void DynamicFieldSetTimeTest()
      {
          NatashaSetDateTime(NatashaProxyModel, DateTime.Now);
      }
      #endregion
      */
        /*
        #region 字段读性能
        [BenchmarkCategory("Read", "String"), Benchmark(Description = "Emit")]
        public void EmitFieldGetStringTest()
        {
            string result = EmitGetString(EmitModel);
        }
        [BenchmarkCategory("Read", "String"), Benchmark(Baseline = true, Description = "Origin")]
        public void OriginFieldGetStringTest()
        {
            string result = OriginModel.Age;
        }
        //[BenchmarkCategory("Read", "String"), Benchmark(Description = "NatashaProxy")]
        public void DynamicFieldProxyGetStringTest()
        {
            string result = NatashaCaller["Age"].StringValue;
        }
        //[BenchmarkCategory("Read", "String"), Benchmark(Description = "NatashaDirectly")]
        public void DynamicFieldGetStringTest()
        {
            string result = NatashaGetString(NatashaModel);
        }


        [BenchmarkCategory("Read", "Time"), Benchmark(Description = "Emit")]
        public void EmitFieldGetTimeTest()
        {
            DateTime result = EmitGetDateTime(EmitModel);
        }
        [BenchmarkCategory("Read", "Time"), Benchmark(Baseline = true, Description = "Origin")]
        public void OriginFieldGetTimeTest()
        {
            DateTime result = OriginModel.CreateTime;
        }
        //[BenchmarkCategory("Read", "Time"), Benchmark(Description = "NatashaProxy")]
        public void DynamicFieldProxyGetTimeTest()
        {
            DateTime result = NatashaCaller["CreateTime"].DateTimeValue;
        }
        //[BenchmarkCategory("Read", "Time"), Benchmark(Description = "NatashaDirectly")]
        public void DynamicFieldGetTimeTest()
        {
            DateTime result = NatashaGetDateTime(NatashaModel);
        }
        #endregion*/
        [BenchmarkCategory("Read", "Time"), Benchmark(Baseline = true, Description = "Origin")]
        public void OriginFieldGetTimeTest()
        {
            for (int i = 0; i < 1000; i++)
            {
                DateTime result = OriginModel.CreateTime;
            }
        }
    }
}
