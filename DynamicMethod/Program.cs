using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;

namespace DynamicMethodWP
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    /// <summary>
    /// IDataReader数据转换成List类
    /// </summary>
    public static class DataToList
    {
        /// <summary>
        /// IDataReader数据转换成List方法
        /// </summary>
        /// <typeparam name="TResult">返回Model类型</typeparam>
        /// <param name="dr">IDataReader数据</param>
        /// <param name="isClose">是否关闭 DataReader</param>
        /// <returns>转换结果</returns>
        public static List<TResult> ReadToList<TResult>(this IDataReader dr, bool isClose = true) where TResult : class, new()
        {

            IDataReaderDynamicEntityBuilder<TResult> eblist = IDataReaderDynamicEntityBuilder<TResult>.CreateBuilder(dr);
            List<TResult> list = new List<TResult>();
            if (dr == null)
            {
                return list;
            }
            while (dr.Read())
            {
                list.Add(eblist.Build(dr));
            }

            if (isClose)
            {
                dr.Close();
                dr.Dispose(); dr = null;
            }

            return list;

        }

    }

    /// <summary>
    ///创建实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IDataReaderDynamicEntityBuilder<T>
    {
        //私有构造函数
        private IDataReaderDynamicEntityBuilder() { }

        private static readonly MethodInfo getValueMethod = typeof(IDataRecord).GetMethod(
            "get_Item",
            new Type[] { typeof(int) }
            );
        private static readonly MethodInfo isDBNullMethod = typeof(IDataRecord).GetMethod(
            "IsDBNull",
            new Type[] { typeof(int) });

        //委托 
        private delegate T Load(IDataRecord dataRecord);

        //最终执行动态方法的一个委托 参数是IDataRecord接口
        private Load handler;


        /// <summary>
        /// 使用委托调用已经构造好的动态方法
        /// </summary>
        /// <param name="dataRecord"></param>
        /// <returns></returns>
        public T Build(IDataRecord dataRecord)
        {
            //执行CreateBuilder里创建的DynamicCreate动态方法的委托
            return handler(dataRecord);
        }


        /// <summary>
        /// 构造实现类的动态方法
        /// </summary>
        /// <param name="dataRecord"></param>
        /// <returns></returns>
        public static IDataReaderDynamicEntityBuilder<T> CreateBuilder(IDataRecord dataRecord)
        {
            IDataReaderDynamicEntityBuilder<T> dynamicBuilder = new IDataReaderDynamicEntityBuilder<T>();

            //定义一个名为DynamicCreate的动态方法，返回值typof(T)，参数typeof(IDataRecord)
            DynamicMethod method = new DynamicMethod("DynamicCreate",
                typeof(T), //返回值类型
                new Type[] { typeof(IDataRecord) },//参数
                typeof(T),
                true);

            //创建一个MSIL生成器，为动态方法生成代码
            ILGenerator generator = method.GetILGenerator();

            //声明指定类型的局部变量 可以 T result 这么理解
            LocalBuilder result = generator.DeclareLocal(typeof(T));
            //BindingFlags.IgnoreCase |
            //BindingFlags.Public |
            //BindingFlags.Instance);

            //实例化类型的对象,并将其存储在本地变量（result）. 可以 result=new T();这么理解
            generator.Emit(OpCodes.Newobj, typeof(T).GetConstructor(Type.EmptyTypes));
            generator.Emit(OpCodes.Stloc, result);

            //数据集合
            for (int i = 0; i < dataRecord.FieldCount; i++)
            {
                //根据列名取属性  原则上属性和列是一一对应的关系
                PropertyInfo propertyInfo = typeof(T).GetProperty(dataRecord.GetName(i));

                Label endIfLabel = generator.DefineLabel();

                if (propertyInfo != null && propertyInfo.GetSetMethod() != null)//实体存在该属性 且该属性有SetMethod方法
                {

                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);
                    //调用IsDBNull方法 如果IsDBNull == true contine
                    generator.Emit(OpCodes.Callvirt, isDBNullMethod);
                    generator.Emit(OpCodes.Brtrue, endIfLabel);

                    /*如果在read中此值非null,则在对象 中设置此值*/
                    generator.Emit(OpCodes.Ldloc, result);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldc_I4, i);

                    //调用get_Item方法
                    generator.Emit(OpCodes.Callvirt, getValueMethod);

                    //拆箱操作   <span style="color: #FF0000;">问题可能就在这里</span>
                    generator.Emit(OpCodes.Unbox_Any, dataRecord.GetFieldType(i));
                    // generator.Emit(OpCodes.Unbox_Any, propertyInfo.PropertyType);
                    //给该属性设置对应值
                    generator.Emit(OpCodes.Callvirt, propertyInfo.GetSetMethod());

                    generator.MarkLabel(endIfLabel);
                }
            }

            /*给本地变量（result）返回值*/
            generator.Emit(OpCodes.Ldloc, result);
            generator.Emit(OpCodes.Ret);//方法结束，返回

            //完成动态方法的创建，并且创建执行该动态方法的委托，赋值到全局变量handler,handler在Build方法里Invoke
            dynamicBuilder.handler = (Load)method.CreateDelegate(typeof(Load));
            return dynamicBuilder;
        }
    }
}
