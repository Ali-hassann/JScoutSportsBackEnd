using System.Data;

namespace AMNSystemsERP.CL.Enums
{
    public enum DB_TYPE
    {
        BigInt = SqlDbType.BigInt,
        Binary = SqlDbType.Binary,
        Bit = SqlDbType.Bit,
        Char = SqlDbType.Char,
        DateTime = SqlDbType.DateTime,
        //
        // Summary:
        //     System.Decimal. A fixed precision and scale numeric value between -10 38 -1 and
        //     10 38 -1.
        Decimal = SqlDbType.Decimal,
        //
        // Summary:
        //     System.Double. A floating point number within the range of -1.79E +308 through
        //     1.79E +308.
        Float = SqlDbType.Float,
        //
        // Summary:
        //     System.Array of type System.Byte. A variable-length stream of binary data ranging
        //     from 0 to 2 31 -1 (or 2,147,483,647) bytes.
        Image = SqlDbType.Image,
        //
        // Summary:
        //     System.Int32. A 32-bit signed integer.
        Int = SqlDbType.Int,
        //
        // Summary:
        //     System.Decimal. A currency value ranging from -2 63 (or -9,223,372,036,854,775,808)
        //     to 2 63 -1 (or +9,223,372,036,854,775,807) with an accuracy to a ten-thousandth
        //     of a currency unit.
        Money = SqlDbType.Money,
        //
        // Summary:
        //     System.String. A fixed-length stream of Unicode characters ranging between 1
        //     and 4,000 characters.
        NChar = SqlDbType.NChar,
        //
        // Summary:
        //     System.String. A variable-length stream of Unicode data with a maximum length
        //     of 2 30 - 1 (or 1,073,741,823) characters.
        NText = SqlDbType.NText,
        //
        // Summary:
        //     System.String. A variable-length stream of Unicode characters ranging between
        //     1 and 4,000 characters. Implicit conversion fails if the string is greater than
        //     4,000 characters. Explicitly set the object when working with strings longer
        //     than 4,000 characters. Use System.Data.SqlDbType.NVarChar when the database column
        //     is nvarchar(max).
        NVarChar = SqlDbType.NVarChar,
        //
        // Summary:
        //     System.Single. A floating point number within the range of -3.40E +38 through
        //     3.40E +38.
        Real = SqlDbType.Real,
        //
        // Summary:
        //     System.Guid. A globally unique identifier (or GUID).
        UniqueIdentifier = SqlDbType.UniqueIdentifier,
        //
        // Summary:
        //     System.DateTime. Date and time data ranging in value from January 1, 1900 to
        //     June 6, 2079 to an accuracy of one minute.
        SmallDateTime = SqlDbType.SmallDateTime,
        //
        // Summary:
        //     System.Int16. A 16-bit signed integer.
        SmallInt = SqlDbType.SmallInt,
        //
        // Summary:
        //     System.Decimal. A currency value ranging from -214,748.3648 to +214,748.3647
        //     with an accuracy to a ten-thousandth of a currency unit.
        SmallMoney = SqlDbType.SmallMoney,
        //
        // Summary:
        //     System.String. A variable-length stream of non-Unicode data with a maximum length
        //     of 2 31 -1 (or 2,147,483,647) characters.
        Text = SqlDbType.Text,
        //
        // Summary:
        //     System.Array of type System.Byte. Automatically generated binary numbers, which
        //     are guaranteed to be unique within a database. timestamp is used typically as
        //     a mechanism for version-stamping table rows. The storage size is 8 bytes.
        Timestamp = SqlDbType.Timestamp,
        //
        // Summary:
        //     System.Byte. An 8-bit unsigned integer.
        TinyInt = SqlDbType.TinyInt,
        //
        // Summary:
        //     System.Array of type System.Byte. A variable-length stream of binary data ranging
        //     between 1 and 8,000 bytes. Implicit conversion fails if the byte array is greater
        //     than 8,000 bytes. Explicitly set the object when working with byte arrays larger
        //     than 8,000 bytes.
        VarBinary = SqlDbType.VarBinary,
        //
        // Summary:
        //     System.String. A variable-length stream of non-Unicode characters ranging between
        //     1 and 8,000 characters. Use System.Data.SqlDbType.VarChar when the database column
        //     is varchar(max).
        VarChar = SqlDbType.VarChar,
        //
        // Summary:
        //     System.Object. A special data type that can contain numeric, string, binary,
        //     or date data as well as the SQL Server values Empty and Null, which is assumed
        //     if no other type is declared.
        Variant = SqlDbType.Variant,
        //
        // Summary:
        //     An XML value. Obtain the XML as a string using the System.Data.SqlClient.SqlDataReader.GetValue(System.Int32)
        //     method or System.Data.SqlTypes.SqlXml.Value property, or as an System.Xml.XmlReader
        //     by calling the System.Data.SqlTypes.SqlXml.CreateReader method.
        Xml = SqlDbType.Xml,
        //
        // Summary:
        //     A SQL Server user-defined type (UDT).
        Udt = SqlDbType.Udt,
        //
        // Summary:
        //     A special data type for specifying structured data contained in table-valued
        //     parameters.
        Structured = SqlDbType.Structured,
        //
        // Summary:
        //     Date data ranging in value from January 1,1 AD through December 31, 9999 AD.
        Date = SqlDbType.Date,
        //
        // Summary:
        //     Time data based on a 24-hour clock. Time value range is 00:00:00 through 23:59:59.9999999
        //     with an accuracy of 100 nanoseconds. Corresponds to a SQL Server time value.
        Time = SqlDbType.Time,
        //
        // Summary:
        //     Date and time data. Date value range is from January 1,1 AD through December
        //     31, 9999 AD. Time value range is 00:00:00 through 23:59:59.9999999 with an accuracy
        //     of 100 nanoseconds.
        DateTime2 = SqlDbType.DateTime2,
        //
        // Summary:
        //     Date and time data with time zone awareness. Date value range is from January
        //     1,1 AD through December 31, 9999 AD. Time value range is 00:00:00 through 23:59:59.9999999
        //     with an accuracy of 100 nanoseconds. Time zone value range is -14:00 through
        //     +14:00.
        DateTimeOffset = SqlDbType.DateTimeOffset
    }
}
