using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlTypes;
using Microsoft.SqlServer.Server;
using System.ComponentModel;

#region AggregateSubject

[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedAggregate(
    Format.UserDefined, /// Binary Serialization because of StringBuilder
    IsInvariantToOrder = false, /// order changes the result
    IsInvariantToNulls = true,  /// nulls don't change the result
    IsInvariantToDuplicates = false, /// duplicates change the result
    MaxByteSize = -1
)]
public struct AggregateSubject : IBinarySerialize
{
    private StringBuilder _accumulator;

    /// <summary>
    /// IsNull property
    /// </summary>
    public Boolean IsNull { get; private set; }

    public void Init()
    {
        _accumulator = new StringBuilder();
        this.IsNull = true;
    }

    /// <summary>
    /// cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, cm.HasAppeal, 13
    /// </summary>
    /// <param name="Value"></param>
    /// <param name="Delimiter"></param>
    public void Accumulate(SqlInt32 Mark, SqlInt32 SubjectCode, SqlInt32 GlobalStatusID, SqlInt32 SubjectCodeNeeded)
    {
        if (SubjectCode == SubjectCodeNeeded)
        {
            if (!GlobalStatusID.IsNull && GlobalStatusID.Value == (int)GlobalResultStatus.LowerThanMinimal)
                _accumulator.Append(string.Format("!{0}", Mark));
            else _accumulator.Append(Mark);
        }
    }

    /// <summary>
    /// Merge onto the end 
    /// </summary>
    /// <param name="Group"></param>
    public void Merge(AggregateSubject Group)
    {
        ///// add the delimiter between strings
        //if (_accumulator.Length > 0
        //    & Group._accumulator.Length > 0) _accumulator.Append(_delimiter);

        /////_accumulator += Group._accumulator;
        _accumulator.Append(Group._accumulator.ToString());
    }

    public SqlString Terminate()
    {
        // Put your code here
        return new SqlString(_accumulator.ToString());
    }

    /// <summary>
    /// deserialize from the reader to recreate the struct
    /// </summary>
    /// <param name="r">BinaryReader</param>
    void IBinarySerialize.Read(System.IO.BinaryReader r)
    {
        //_delimiter = r.ReadString();
        _accumulator = new StringBuilder(r.ReadString());
        if (_accumulator.Length != 0) this.IsNull = false;
    }

    /// <summary>
    /// searialize the struct.
    /// </summary>
    /// <param name="w">BinaryWriter</param>
    void IBinarySerialize.Write(System.IO.BinaryWriter w)
    {
        //w.Write(_delimiter);
        w.Write(_accumulator.ToString());
    }
}
#endregion

#region AggregateAppeal
[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedAggregate(
    Format.UserDefined, /// Binary Serialization because of StringBuilder
    IsInvariantToOrder = false, /// order changes the result
    IsInvariantToNulls = true,  /// nulls don't change the result
    IsInvariantToDuplicates = false, /// duplicates change the result
    MaxByteSize = -1
)]
public struct AggregateAppeal : IBinarySerialize
{
    private StringBuilder _accumulator;

    /// <summary>
    /// IsNull property
    /// </summary>
    public Boolean IsNull { get; private set; }

    public void Init()
    {
        _accumulator = new StringBuilder();
        this.IsNull = true;
    }

    /// <summary>
    /// cm.Mark, cm.SubjectCode, rsg.GlobalStatusID, cm.HasAppeal, 13
    /// </summary>
    /// <param name="Value"></param>
    /// <param name="Delimiter"></param>
    public void Accumulate(SqlInt32 HasAppeal, SqlInt32 SubjectCode, SqlInt32 SubjectCodeNeeded)
    {
        if (SubjectCode == SubjectCodeNeeded)
        {
            _accumulator.Append(HasAppeal.Value);
        }
    }

    /// <summary>
    /// Merge onto the end 
    /// </summary>
    /// <param name="Group"></param>
    public void Merge(AggregateAppeal Group)
    {
        ///// add the delimiter between strings
        //if (_accumulator.Length > 0
        //    & Group._accumulator.Length > 0) _accumulator.Append(_delimiter);

        /////_accumulator += Group._accumulator;
        _accumulator.Append(Group._accumulator.ToString());
    }

    public SqlString Terminate()
    {
        // Put your code here
        return new SqlString(_accumulator.ToString());
    }

    /// <summary>
    /// deserialize from the reader to recreate the struct
    /// </summary>
    /// <param name="r">BinaryReader</param>
    void IBinarySerialize.Read(System.IO.BinaryReader r)
    {
        //_delimiter = r.ReadString();
        _accumulator = new StringBuilder(r.ReadString());
        if (_accumulator.Length != 0) this.IsNull = false;
    }

    /// <summary>
    /// searialize the struct.
    /// </summary>
    /// <param name="w">BinaryWriter</param>
    void IBinarySerialize.Write(System.IO.BinaryWriter w)
    {
        //w.Write(_delimiter);
        w.Write(_accumulator.ToString());
    }
}
#endregion

public enum GlobalResultStatus
{
    [Description("Действующий")]
    Ready = 1,
    [Description("Аннулирован с правом пересдачи")]
    AnnulWithRetry = 2,
    [Description("Аннулирован без права передачи")]
    AnnulWithoutRetry = 3,
    [Description("Недействительный результат по причине нарушения порядка проведения ГИА")]
    Cancelled = 4,
    [Description("Ниже минимума")]
    LowerThanMinimal = 5
}