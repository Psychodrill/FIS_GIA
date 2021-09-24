namespace Ege.Dal.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Ege.Check.Common.Extensions;
    using JetBrains.Annotations;

    internal class DbTypeFactory : IDbTypeFactory
    {
        [NotNull] private readonly Dictionary<Type, DbType> _typeMap = new Dictionary<Type, DbType>
            {
                {typeof (byte), DbType.Byte},
                {typeof (sbyte), DbType.SByte},
                {typeof (short), DbType.Int16},
                {typeof (ushort), DbType.UInt16},
                {typeof (int), DbType.Int32},
                {typeof (uint), DbType.UInt32},
                {typeof (long), DbType.Int64},
                {typeof (ulong), DbType.UInt64},
                {typeof (float), DbType.Single},
                {typeof (double), DbType.Double},
                {typeof (decimal), DbType.Decimal},
                {typeof (bool), DbType.Boolean},
                {typeof (string), DbType.String},
                {typeof (char), DbType.StringFixedLength},
                {typeof (Guid), DbType.Guid},
                {typeof (DateTime), DbType.DateTime},
                {typeof (DateTimeOffset), DbType.DateTimeOffset},
                {typeof (byte[]), DbType.Binary},
                {typeof (byte?), DbType.Byte},
                {typeof (sbyte?), DbType.SByte},
                {typeof (short?), DbType.Int16},
                {typeof (ushort?), DbType.UInt16},
                {typeof (int?), DbType.Int32},
                {typeof (uint?), DbType.UInt32},
                {typeof (long?), DbType.Int64},
                {typeof (ulong?), DbType.UInt64},
                {typeof (float?), DbType.Single},
                {typeof (double?), DbType.Double},
                {typeof (decimal?), DbType.Decimal},
                {typeof (bool?), DbType.Boolean},
                {typeof (char?), DbType.StringFixedLength},
                {typeof (Guid?), DbType.Guid},
                {typeof (DateTime?), DbType.DateTime},
                {typeof (DateTimeOffset?), DbType.DateTimeOffset},
            };

        [NotNull]
        private Type UnwrapIfEnum([NotNull] Type type)
        {
            return type.IsEnum ? type.GetEnumUnderlyingType() : type;
        }

        [NotNull]
        private Type UnwrapIfEnumOrNullableEnum([NotNull] Type type)
        {
            Type unwrappedNullable;
            return type.TryGetNullableParameter(out unwrappedNullable)
                ? typeof(Nullable<>).MakeGenericType(UnwrapIfEnum(unwrappedNullable))
                : UnwrapIfEnum(type);
        }

        public DbType Create(Type cSharpType)
        {
            DbType dbType;
            cSharpType = UnwrapIfEnumOrNullableEnum(cSharpType);
            if (!_typeMap.TryGetValue(cSharpType, out dbType))
            {
                throw new ArgumentOutOfRangeException("cSharpType",
                                                      string.Format("Type {0} can not be converted to DbType",
                                                                    cSharpType));
            }
            return dbType;
        }
    }
}