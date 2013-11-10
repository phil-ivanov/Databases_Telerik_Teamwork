#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the ClassGenerator.ttinclude code generation file.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Common;
using System.Collections.Generic;
using Telerik.OpenAccess;
using Telerik.OpenAccess.Metadata;
using Telerik.OpenAccess.Data.Common;
using Telerik.OpenAccess.Metadata.Fluent;
using Telerik.OpenAccess.Metadata.Fluent.Advanced;
using MySQLSupermarket.Data;

namespace MySQLSupermarket.Data	
{
	[Table("products")]
	[ConcurrencyControl(OptimisticConcurrencyControlStrategy.Changed)]
	[KeyGenerator(KeyGenerator.Autoinc)]
	public partial class Product
	{
		private int _iD;
		[Column("ID", OpenAccessType = OpenAccessType.Int32, IsBackendCalculated = true, IsPrimaryKey = true, Length = 0, Scale = 0, SqlType = "integer")]
		[Storage("_iD")]
		public virtual int ID
		{
			get
			{
				return this._iD;
			}
			set
			{
				this._iD = value;
			}
		}
		
		private int _vendorID;
		[Column("VendorID", OpenAccessType = OpenAccessType.Int32, Length = 0, Scale = 0, SqlType = "integer")]
		[Storage("_vendorID")]
		public virtual int VendorID
		{
			get
			{
				return this._vendorID;
			}
			set
			{
				this._vendorID = value;
			}
		}
		
		private string _productName;
		[Column("ProductName", OpenAccessType = OpenAccessType.UnicodeStringVariableLength, Length = 100, Scale = 0, SqlType = "nvarchar")]
		[Storage("_productName")]
		public virtual string ProductName
		{
			get
			{
				return this._productName;
			}
			set
			{
				this._productName = value;
			}
		}
		
		private int _measureID;
		[Column("MeasureID", OpenAccessType = OpenAccessType.Int32, Length = 0, Scale = 0, SqlType = "integer")]
		[Storage("_measureID")]
		public virtual int MeasureID
		{
			get
			{
				return this._measureID;
			}
			set
			{
				this._measureID = value;
			}
		}
		
		private double _basicPrice;
		[Column("BasicPrice", OpenAccessType = OpenAccessType.Double, Length = 0, Scale = 0, SqlType = "double")]
		[Storage("_basicPrice")]
		public virtual double BasicPrice
		{
			get
			{
				return this._basicPrice;
			}
			set
			{
				this._basicPrice = value;
			}
		}
		
		private Vendor _vendor;
		[ForeignKeyAssociation(ConstraintName = "products_ibfk_1", SharedFields = "VendorID", TargetFields = "ID")]
		[Storage("_vendor")]
		public virtual Vendor Vendor
		{
			get
			{
				return this._vendor;
			}
			set
			{
				this._vendor = value;
			}
		}
		
		private Measure _measure;
		[ForeignKeyAssociation(ConstraintName = "products_ibfk_2", SharedFields = "MeasureID", TargetFields = "ID")]
		[Storage("_measure")]
		public virtual Measure Measure
		{
			get
			{
				return this._measure;
			}
			set
			{
				this._measure = value;
			}
		}
		
	}
}
#pragma warning restore 1591
