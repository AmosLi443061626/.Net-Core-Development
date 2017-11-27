using System;
using System.Linq;

using AntData.ORM;
using AntData.ORM.Linq;
using AntData.ORM.Mapping;

namespace DBContextEntity.Mysql
{
	/// <summary>
	/// Database       : mydemo
	/// Data Source    : 101.37.21.242
	/// Server Version : 5.7.13-log
	/// </summary>
	public partial class Entitys : IEntity
	{
		public IQueryable<Mytest> Mytests { get { return this.Get<Mytest>(); } }

		private readonly IDataContext con;

		public IQueryable<T> Get<T>()
			 where T : class
		{
			return this.con.GetTable<T>();
		}

		public Entitys(IDataContext con)
		{
			this.con = con;
		}
	}

	[Table("mytest")]
	public partial class Mytest : BaseEntity
	{
		#region Column

		[Column("name", DataType=DataType.VarChar, Length=255), Nullable]
		public string Name { get; set; } // varchar(255)

		#endregion
	}
}
