﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ILRuntime.Other;

namespace ILRuntimeTest.TestFramework
{

    public class BindableProperty<T>
    {
        public delegate void onChange(T val);
        public delegate void onChangeWithOldVal(T oldVal, T newVal);
        public onChange OnChange;
        public onChangeWithOldVal OnChangeWithOldVal;

        private T _value;
        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (!Equals(_value, value))
                {
                    T oldValue = _value;
                    _value = value;
                    OnChange?.Invoke(_value);
                    OnChangeWithOldVal?.Invoke(oldValue, _value);
                }
            }
        }

        public BindableProperty(T val)
        {
            _value = val;
        }

        public override string ToString()
        {
            return (Value != null ? Convert.ToString(Value) : "null");
        }

        public void Clear()
        {
            _value = default(T);
        }

        public static implicit operator T(BindableProperty<T> t)
        {
            return t.Value;
        }
    }

    [DelegateExport]
    public delegate void IntDelegate(int a);

    [DelegateExport]
    public delegate void Int2Delegate(int a, int b);

    [DelegateExport]
    public delegate void InitFloat(int a, float b);

    [DelegateExport]
    public delegate int IntDelegate2(int a);

    [DelegateExport]
    public delegate bool Int2Delegate2(int a, int b);

    [DelegateExport]
    public delegate string IntFloatDelegate2(int a, float b);

    public class DelegateTest
    {
        int val;

        public DelegateTest()
        {
            val = 666;
        }
        public static IntDelegate IntDelegateTest;
        public static event IntDelegate IntDelegateEventTest;
        public event IntDelegate IntDelegateEventTest2;
        public static Int2Delegate IntDelegateTest1;
        public static IntDelegate2 IntDelegateTest2;
        public static IntDelegate2 IntDelegateTestReturn;
        public static Int2Delegate2 IntDelegateTestReturn1;
        public static IntFloatDelegate2 IntDelegateTestReturn2;
        public static Action<BaseClassTest> GenericDelegateTest;
        public static Func<int, float, short, double> DelegatePerformanceTest;
        public static Action<TestCLREnum> EnumDelegateTest;
        public static Func<TestCLREnum> EnumDelegateTest2;
        public static event Action<float, double, int> OnIntEvent;

        public static void TestEvent()
        {
            IntDelegateEventTest(100);
        }

        public void TestEvent2()
        {
            IntDelegateEventTest2(22222);
        }

        public static void TestEvent3(float a, double b, int c)
        {
            OnIntEvent(a, b, c);
        }

        public static bool TestEvent4()
        {
            return OnIntEvent == null;
        }

        public static void TestEnumDelegate()
        {
            EnumDelegateTest(TestCLREnum.Test1);
            EnumDelegateTest(TestCLREnum.Test2);
            EnumDelegateTest(TestCLREnum.Test3);
        }

        public static void TestEnumDelegate2()
        {
            var e = EnumDelegateTest2();
            switch (e)
            {
                case TestCLREnum.Test2:
                    Console.WriteLine("Test2");
                    break;
                default:
                    throw new Exception("Shouldn't be here");
            }
        }

        public static int TestIntDelegate(int a)
        {
            return a + 233;
        }

        public int TestIntDelegateInstance(int a)
        {
            return a + 233 + val;
        }
    }

    public class GenericClassTest<T> : BaseClassTest
    {

    }
    public class BaseClassTest
    {
        public bool testField;

        public static void DoTest()
        {
            GenericClassTest<int> a = new GenericClassTest<int>();
            if (DelegateTest.GenericDelegateTest != null)
                DelegateTest.GenericDelegateTest(a);
        }
    }
}
